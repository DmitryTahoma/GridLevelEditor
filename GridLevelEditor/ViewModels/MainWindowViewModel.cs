using Catel.Data;
using Catel.MVVM;
using GridLevelEditor.Controls;
using GridLevelEditor.Models;
using GridLevelEditor.Objects;
using System;
using System.Windows;

namespace GridLevelEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private StartWindow startWindow;
        private LevelContent levelContent;
        private OpenLevelForm openLevelForm;

        private DialogManager dialogManager;

        private LevelEditor model;

        public MainWindowViewModel()
        {
            startWindow = null;
            levelContent = null;
            openLevelForm = null;

            dialogManager = new DialogManager();

            IsLevelOpen = false;

            model = new LevelEditor();

            UpdateTitle();

            Closing = new Command(OnClosingExecute);
            BindStartWindow = new Command<StartWindow>(OnBindStartWindowExecute);
            BindLevelContent = new Command<LevelContent>(OnBindLevelContentExecute);
            BindOpenLevelForm = new Command<OpenLevelForm>(OnBindOpenLevelFormExecute);
            CreateNewLevel = new Command(OnCreateNewLevelExecute);
            OpenExistsLevel = new Command(OnOpenExistsLevelExecute);
            DeleteCurrentLevel = new Command(OnDeleteCurrentLevelExecute);
            CopyMgElemsFromExistsLevel = new Command(OnCopyMgElemsFromExistsLevelExecute);
            ExportToXml = new Command(OnExportToXmlExecute);
            ExportToXmlLL8Extras = new Command(OnExportToXmlLL8ExtrasExecute);
            TryLoadLastLevel = new Command(OnTryLoadLastLevelExecute);
        }

        #region Properties

        public string TitleText
        {
            get => GetValue<string>(TitleTextProperty);
            set => SetValue(TitleTextProperty, value);
        }
        public static readonly PropertyData TitleTextProperty = RegisterProperty(nameof(TitleText), typeof(string), "");

        public int SelectedTabId
        {
            get => GetValue<int>(SelectedTabIdProperty);
            set => SetValue(SelectedTabIdProperty, value);
        }
        public static readonly PropertyData SelectedTabIdProperty = RegisterProperty(nameof(SelectedTabId), typeof(int), 0);

        public bool IsLevelOpen { get; set; }

        #endregion

        #region Commands

        public Command Closing { get; private set; }
        private void OnClosingExecute()
        {
            if (IsLevelOpen)
            {
                TrySaveLevel();
                IsLevelOpen = false;
            }
        }

        public Command<StartWindow> BindStartWindow { get; private set; }
        private void OnBindStartWindowExecute(StartWindow startWindow)
        {
            this.startWindow = startWindow;
        }

        public Command<LevelContent> BindLevelContent { get; private set; }
        private void OnBindLevelContentExecute(LevelContent levelContent)
        {
            this.levelContent = levelContent;
            startWindow.ViewModel.SetCreationGrid((name, rows, cols) => {
                levelContent.ViewModel.CreateGrid(rows, cols, 50);
                model.SetLevelSize(rows, cols);
                model.LevelName = name;
                model.UpdateData(GridDataTransformer.GetLevelDataFromGrid(levelContent.GridLevel, model.GetLevelSize(), model.GetElems()));
                model.Save();
                SelectedTabId = 1;
                UpdateTitle();
                IsLevelOpen = true;
            });
            levelContent.ViewModel.SetFillingGrid((grid, changer) => { GridDataTransformer.FillGridFromLevelData(grid, new GridLength(50), model.GetLevelData(), model.GetLevelSize(), model.GetElems(), changer); });
            levelContent.ViewModel.SetAdditionMgElem(model.AddMgElem);
            levelContent.ViewModel.SetDeletionMgElem(model.RemoveMgElem);
            levelContent.ViewModel.SetCleansingMgElems(model.ClearElems);
            levelContent.ViewModel.CreateLoadedLevel(model.GetElems());
        }

        public Command<OpenLevelForm> BindOpenLevelForm { get; private set; }
        private void OnBindOpenLevelFormExecute(OpenLevelForm openLevelForm)
        {
            this.openLevelForm = openLevelForm;
            this.openLevelForm.ViewModel.SetLevelSelected((lvl, mode) => {
                if (mode == Controls.OpenLevelFormViewModel.OpenType.Open)
                {
                    model.Load(lvl);
                    levelContent.ViewModel.CreateLoadedLevel(model.GetElems());
                }
                else if(mode == Controls.OpenLevelFormViewModel.OpenType.CopyMgElems)
                {
                    foreach(MgElem elem in lvl.Elems)
                    {
                        model.AddMgElem(elem);
                    }
                    levelContent.ViewModel.CreateLoadedLevel(lvl.Elems);
                }
                SelectedTabId = 1;
                IsLevelOpen = true;
                UpdateTitle();
            });
        }

        public Command TryLoadLastLevel { get; private set; }
        private void OnTryLoadLastLevelExecute()
        {
            try
            {
                model.Load();
                if (model.IsLoaded)
                {
                    SelectedTabId = 1;
                    IsLevelOpen = true;
                }
            }
            catch (Exception e)
            {
                dialogManager.LoadLevelError(e.ToString());
            }
        }

        public Command CreateNewLevel { get; private set; }
        private void OnCreateNewLevelExecute()
        {
            if(SelectedTabId == 1)
            {
                OnClosingExecute();
            }
            SelectedTabId = 0;
            model.ReInit();
            levelContent.ViewModel.ReInit();
            UpdateTitle();
        }

        public Command OpenExistsLevel { get; private set; }
        private void OnOpenExistsLevelExecute()
        {
            if (SelectedTabId == 1)
            {
                OnClosingExecute();
            }
            model.ReInit();
            levelContent.ViewModel.ReInit();
            UpdateTitle();

            openLevelForm.ViewModel.LoadLevels();
            openLevelForm.ViewModel.Mode = Controls.OpenLevelFormViewModel.OpenType.Open;
            SelectedTabId = 2;
        }

        public Command DeleteCurrentLevel { get; private set; }
        private void OnDeleteCurrentLevelExecute()
        {
            if(SelectedTabId == 1 && dialogManager.AreYouSureDialog("удалить текущий уровень?") == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    model.DeleteLevel();
                    SelectedTabId = 0;
                    IsLevelOpen = false;
                }
                catch(Exception e)
                {
                    dialogManager.DeletionError(e.ToString());
                }
            }
        }

        public Command CopyMgElemsFromExistsLevel { get; private set; }
        private void OnCopyMgElemsFromExistsLevelExecute()
        {
            if(SelectedTabId == 1)
            {
                openLevelForm.ViewModel.LoadLevels();
                openLevelForm.ViewModel.Mode = Controls.OpenLevelFormViewModel.OpenType.CopyMgElems;
                SelectedTabId = 2;
            }
        }

        public Command ExportToXml { get; private set; }
        private void OnExportToXmlExecute()
        {
            if(SelectedTabId == 1)
            {
                string path = dialogManager.GetFileExportXml();
                if(path != "")
                {
                    try
                    {
                        TrySaveLevel();
                        model.ExportToXml(path);
                    }
                    catch(Exception e)
                    {
                        dialogManager.ExportError(e.ToString());
                    }
                }
            }
        }

        public Command ExportToXmlLL8Extras { get; private set; }
        private void OnExportToXmlLL8ExtrasExecute()
        {
            if (SelectedTabId == 1)
            {
                string path = dialogManager.GetFileExportXml();
                if (path != "")
                {
                    try
                    {
                        TrySaveLevel();
                        model.ExportToXmlLL8Extras(path);
                    }
                    catch (Exception e)
                    {
                        dialogManager.ExportError(e.ToString());
                    }
                }
            }
        }

        #endregion

        private void UpdateTitle()
        {
            string levelName = "";
            if (model.LevelName != "")
            {
                levelName = " | " + model.LevelName;
            }
            TitleText = "Grid Level Editor v1.0" + levelName;
        }

        private void TrySaveLevel()
        {
            try
            {
                levelContent.ViewModel.SetIdsUnique();
                model.UpdateData(GridDataTransformer.GetLevelDataFromGrid(levelContent.GridLevel, model.GetLevelSize(), model.GetElems()));
                model.Save();
            }
            catch (Exception e)
            {
                dialogManager.SavingError(e.ToString());
            }
        }
    }
}
