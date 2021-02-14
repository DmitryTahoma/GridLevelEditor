using Catel.Data;
using Catel.MVVM;
using GridLevelEditor.Controls;
using GridLevelEditor.Models;
using GridLevelEditor.Objects;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GridLevelEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Grid grid;
        private StackPanel mgElemsStack;

        private LevelEditor model;
        private List<MgElemControl> imagesSelect;

        private DialogManager dialogManager;
        private ControlCreator controlCreator;

        public MainWindowViewModel()
        {
            dialogManager = new DialogManager();
            controlCreator = new ControlCreator();

            grid = null;
            mgElemsStack = null;

            try
            {
                model = new LevelEditor();
            }
            catch(Exception e)
            {
                dialogManager.LoadLevelError(e.ToString());
            }
            imagesSelect = new List<MgElemControl>();

            CreateLoadedControls();

            string levelName = "";
            if (model.LevelName != "")
            {
                levelName = " | " + model.LevelName;
            }
            TitleText = "Grid Level Editor v1.0" + levelName;
            SelectedMgElem = null;

            CreateGrid = new Command(OnCreateGridExecute);
            Closing = new Command(OnClosingExecute);
            AddMgElem = new Command(OnAddMgElemExecute);
            RemoveMgElem = new Command(OnRemoveMgElemExecute);
            BindStackPanel = new Command<StackPanel>(OnBindStackPanelExecute);
            BindGrid = new Command<Grid>(OnBindGridExecute);
        }

        #region Properties

        public string TitleText
        {
            get => GetValue<string>(TitleTextProperty);
            set => SetValue(TitleTextProperty, value);
        }
        public static readonly PropertyData TitleTextProperty = RegisterProperty(nameof(TitleText), typeof(string), "");

        public string WidthText
        {
            get => GetValue<string>(WidthTextProperty);
            set => SetValue(WidthTextProperty, ValidateToInt(GetValue<string>(WidthTextProperty), value));
        }
        public static readonly PropertyData WidthTextProperty = RegisterProperty(nameof(WidthText), typeof(string), "");

        public string HeightText
        {
            get => GetValue<string>(HeightTextProperty);
            set => SetValue(HeightTextProperty, ValidateToInt(GetValue<string>(HeightTextProperty), value));
        }
        public static readonly PropertyData HeightTextProperty = RegisterProperty(nameof(HeightText), typeof(string), "");

        public string SizeText
        {
            get => GetValue<string>(SizeTextProperty);
            set
            {
                SetValue(SizeTextProperty, ValidateToInt(GetValue<string>(SizeTextProperty), value));
                UpdateSize();
            }
        }
        public static readonly PropertyData SizeTextProperty = RegisterProperty(nameof(SizeText), typeof(string), "50");

        #endregion

        #region Commands

        public Command CreateGrid { get; private set; }
        private void OnCreateGridExecute()
        {
            int rows = Parse(HeightText),
                columns = Parse(WidthText);
            model.SetLevelSize(rows, columns);
            controlCreator.FillLevelGrid(grid, rows, columns, Parse(SizeText), ChangeImage);
        }

        public Command Closing { get; private set; }
        private void OnClosingExecute()
        {
            try
            {
                model.UpdateData(GridDataTransformer.GetLevelDataFromGrid(grid, model.GetLevelSize(), model.GetElems()));
                model.Save();
            }
            catch (Exception e)
            {
                dialogManager.SavingError(e.ToString());
            }
        }

        public Command AddMgElem { get; private set; }
        private void OnAddMgElemExecute()
        {
            string filename = dialogManager.GetImageFilepath();

            if (filename != "")
            {
                BitmapImage image = new BitmapImage(new Uri(filename));
                if (image.PixelWidth != image.PixelHeight && dialogManager.PictureNot1to1() == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                MgElemControl control = controlCreator.CreateMgElemControl(Parse(SizeText), image, SelectImage);
                mgElemsStack.Children.Add(control);
                imagesSelect.Add(control);
                model.AddMgElem(control.ViewModel.GetModel());

                if (imagesSelect.Count == 1)
                {
                    control.ViewModel.SelectVisibility = Visibility.Visible;
                }
            }
        }

        public Command RemoveMgElem { get; private set; }
        private void OnRemoveMgElemExecute()
        {
            var selected = SelectedMgElem;
            if (selected != null)
            {
                mgElemsStack.Children.Remove(selected);
                imagesSelect.Remove(selected);
                model.RemoveMgElem(selected.ViewModel.GetModel());

                if (imagesSelect.Count != 0)
                {
                    imagesSelect[0].ViewModel.SelectVisibility = Visibility.Visible;
                    SelectedMgElem = imagesSelect[0];
                }
            }
        }

        public Command<StackPanel> BindStackPanel { get; private set; }
        private void OnBindStackPanelExecute(StackPanel stackPanel)
        {
            mgElemsStack = stackPanel;

            model.ClearElems();
            foreach (MgElemControl control in imagesSelect)
            {
                stackPanel.Children.Add(control);
                model.AddMgElem(control.ViewModel.GetModel());
            }
        }

        public Command<Grid> BindGrid { get; private set; }
        private void OnBindGridExecute(Grid grid)
        {
            try
            {
                GridDataTransformer.FillGridFromLevelData(grid, new GridLength(Parse(SizeText)), model.GetLevelData(), model.GetLevelSize(), model.GetElems(), ChangeImage);
            }
            catch(Exception e)
            {
                dialogManager.GridCreationError(e.ToString());
            }
            this.grid = grid;
        }

        private MgElemControl SelectedMgElem { set; get; }

        #endregion

        public string ValidateToInt(string curValue, string nextValue)
        {
            if (int.TryParse(nextValue, out int _) || nextValue == "")
                return nextValue;
            return curValue;
        }

        public int Parse(string text)
        {
            if (int.TryParse(text, out int res))
                return res;
            return 0;
        }

        public void UpdateSize()
        {
            if(grid != null)
            {
                GridLength size = new GridLength(Parse(SizeText));

                foreach(ColumnDefinition it in grid.ColumnDefinitions)
                {
                    it.Width = size;
                }

                foreach(RowDefinition it in grid.RowDefinitions)
                {
                    it.Height = size;
                }
            }
        }

        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            if(sender != null && sender is Image snd)
            {
                MgElemControl sndr = null;

                foreach (MgElemControl img in imagesSelect)
                {
                    img.ViewModel.SelectVisibility = Visibility.Hidden;
                    if(snd == img.ImgControl && sndr == null)
                    {
                        sndr = img;
                    }
                }

                if(sndr != null)
                {
                    sndr.ViewModel.SelectVisibility = Visibility.Visible;
                    SelectedMgElem = sndr;
                }
            }
        }

        private void ChangeImage(object sender, MouseEventArgs e)
        {
            if(sender != null && sender is Image img)
            {
                if(SelectedMgElem != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    img.Source = SelectedMgElem.ViewModel.ImageSource;
                }
                else if(e.RightButton == MouseButtonState.Pressed)
                {
                    img.Source = ResourceDriver.GetVoidBmp();
                }
            }
        }

        private void CreateLoadedControls()
        {
            List<MgElem> elems = model.GetElems();
            foreach(MgElem elem in elems)
            {
                MgElemControl control = controlCreator.CreateMgElemControl(Parse(SizeText), elem.Image, SelectImage);
                control.ViewModel.TextIndex = elem.Id;
                imagesSelect.Add(control);
            }
        }
    }
}
