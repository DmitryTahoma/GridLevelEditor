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

        private DialogManager dialogManager;

        private LevelEditor model;

        public MainWindowViewModel()
        {
            startWindow = null;

            dialogManager = new DialogManager();

            try
            {
                model = new LevelEditor();
            }
            catch (Exception e)
            {
                dialogManager.LoadLevelError(e.ToString());
            }

            string levelName = "";
            if (model.LevelName != "")
            {
                levelName = " | " + model.LevelName;
            }
            TitleText = "Grid Level Editor v1.0" + levelName;

            Closing = new Command(OnClosingExecute);
            BindStartWindow = new Command<StartWindow>(OnBindStartWindowExecute);
            BindLevelContent = new Command<LevelContent>(OnBindLevelContentExecute);
        }

        #region Properties

        public string TitleText
        {
            get => GetValue<string>(TitleTextProperty);
            set => SetValue(TitleTextProperty, value);
        }
        public static readonly PropertyData TitleTextProperty = RegisterProperty(nameof(TitleText), typeof(string), "");

        public int SelectedTapId
        {
            get => GetValue<int>(SelectedTapIdProperty);
            set => SetValue(SelectedTapIdProperty, value);
        }
        public static readonly PropertyData SelectedTapIdProperty = RegisterProperty(nameof(SelectedTapId), typeof(int), 0);

        #endregion

        #region Commands

        public Command Closing { get; private set; }
        private void OnClosingExecute()
        {
            try
            {
                model.UpdateData(GridDataTransformer.GetLevelDataFromGrid(levelContent.GridLevel, model.GetLevelSize(), model.GetElems()));
                model.Save();
            }
            catch (Exception e)
            {
                dialogManager.SavingError(e.ToString());
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
            startWindow.ViewModel.SetCreationGrid((rows, cols, size) => {
                levelContent.ViewModel.CreateGrid(rows, cols, size);
                model.SetLevelSize(rows, cols);
                model.UpdateData(GridDataTransformer.GetLevelDataFromGrid(levelContent.GridLevel, model.GetLevelSize(), model.GetElems()));
                model.Save();
                SelectedTapId = 1;
            });
            levelContent.ViewModel.SetFillingGrid((grid, changer) => { GridDataTransformer.FillGridFromLevelData(grid, new GridLength(50), model.GetLevelData(), model.GetLevelSize(), model.GetElems(), changer); });
            levelContent.ViewModel.SetAdditionMgElem(model.AddMgElem);
            levelContent.ViewModel.SetDeletionMgElem(model.RemoveMgElem);
            levelContent.ViewModel.SetCleansingMgElems(model.ClearElems);
            levelContent.ViewModel.CreateLoadedLevel(model.GetElems());
        }

        #endregion
    }
}
