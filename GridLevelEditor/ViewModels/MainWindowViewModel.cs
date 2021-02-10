using Catel.Data;
using Catel.MVVM;
using GridLevelEditor.Controls;
using GridLevelEditor.Models;
using GridLevelEditor.Objects;
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
        private LevelEditor model;
        private List<MgElemControl> imagesSelect;

        private DialogManager dialogManager;
        private ControlCreator controlCreator;

        public MainWindowViewModel()
        {
            grid = null;
            model = new LevelEditor();
            imagesSelect = new List<MgElemControl>();

            dialogManager = new DialogManager();
            controlCreator = new ControlCreator();

            CreateGrid = new Command<Grid>(OnCreateGridExecute);
            Closing = new Command(OnClosingExecute);
            AddMgElem = new Command<StackPanel>(OnAddMgElemExecute);
            RemoveMgElem = new Command<StackPanel>(OnRemoveMgElemExecute);

            string levelName = "";
            if (model.LevelName != "")
            {
                levelName = " | " + model.LevelName;
            }
            TitleText = "Grid Level Editor v1.0" + levelName;
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

        public Command<Grid> CreateGrid { get; private set; }
        private void OnCreateGridExecute(Grid levelGrid)
        {
            grid = levelGrid;

            levelGrid.RowDefinitions.Clear();
            levelGrid.ColumnDefinitions.Clear();
            levelGrid.Children.Clear();

            int rows = Parse(HeightText),
                columns = Parse(WidthText);

            GridLength size = new GridLength(Parse(SizeText));

            for (int i = 0; i < columns; ++i)
            {
                levelGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = size });
            }
            if(columns != 0)
            {
                for(int i = 0; i < rows; ++i)
                {
                    levelGrid.RowDefinitions.Add(new RowDefinition() { Height = size });
                }
            }

            for(int i = 0; i < columns; ++i)
            {
                for (int j = 0; j < rows; ++j) 
                {
                    Image imageControl = controlCreator.CreateVoidImage(ChangeImage);
                    levelGrid.Children.Add(imageControl);
                    Grid.SetColumn(imageControl, i);
                    Grid.SetRow(imageControl, j);
                }
            }
        }

        public Command Closing { get; private set; }
        private void OnClosingExecute()
        {
            model.Save();
        }

        public Command<StackPanel> AddMgElem { get; private set; }
        private void OnAddMgElemExecute(StackPanel stackPanel)
        {
            string filename = dialogManager.GetImageFilepath();

            if (filename != "")
            {
                BitmapImage image = new BitmapImage(new System.Uri(filename));
                if (image.PixelWidth != image.PixelHeight && dialogManager.PictureNot1to1() == System.Windows.Forms.DialogResult.No)
                {
                    return;                    
                }

                MgElemControl control = controlCreator.CreateMgElemControl(Parse(SizeText), image, SelectImage);
                stackPanel.Children.Add(control);
                imagesSelect.Add(control);

                if(imagesSelect.Count == 1)
                {
                    control.ViewModel.SelectVisibility = Visibility.Visible;
                }
            }
        }

        public Command<StackPanel> RemoveMgElem { get; private set; }
        private void OnRemoveMgElemExecute(StackPanel stackPanel)
        {
            var selected = GetSelected();
            if (selected != null)
            {
                stackPanel.Children.Remove(selected);
                imagesSelect.Remove(selected);
                if(imagesSelect.Count != 0)
                {
                    imagesSelect[0].ViewModel.SelectVisibility = Visibility.Visible;
                }
            }
        }

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
                }
            }
        }

        private MgElemControl GetSelected()
        {
            if(imagesSelect.Count != 0)
            {
                foreach(MgElemControl control in imagesSelect)
                {
                    if(control.ViewModel.SelectVisibility == Visibility.Visible)
                    {
                        return control;
                    }
                }
            }

            return null;
        }

        private void ChangeImage(object sender, MouseEventArgs e)
        {
            if(sender != null && sender is Image img)
            {
                MgElemControl selected = GetSelected();
                if(selected != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    img.Source = selected.ViewModel.ImageSource;
                }
                else if(e.RightButton == MouseButtonState.Pressed)
                {
                    img.Source = ResourceDriver.GetVoidBmp();
                }
            }
        }
    }
}
