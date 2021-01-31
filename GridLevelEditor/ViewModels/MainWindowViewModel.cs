using Catel.Data;
using Catel.MVVM;
using GridLevelEditor.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GridLevelEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Grid grid;
        private LevelEditor model;

        public MainWindowViewModel()
        {
            grid = null;
            model = new LevelEditor();

            CreateGrid = new Command<Grid>(OnCreateGridExecute);
            Closing = new Command(OnClosingExecute);
            AddImage = new Command<StackPanel>(OnAddImageExecute);

            string levelName = "";
            if (model.LevelName != "")
            {
                levelName = " | " + model.LevelName;
            }
            TitleText = "Grid Level Editor v1.0" + levelName;
        }

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
                    Image imageControl = new Image();
                    BitmapImage img = new BitmapImage(new System.Uri("pack://application:,,,/Resources/void.png"));
                    imageControl.Source = img;
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

        public Command<StackPanel> AddImage { get; private set; }
        private void OnAddImageExecute(StackPanel stackPanel)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Выберите изображение";
            dialog.Filter = "Изображение (*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png";
            dialog.ShowDialog();
            string filename = dialog.FileName;

            BitmapImage image = new BitmapImage(new System.Uri(filename));
            if(image.PixelWidth != image.PixelHeight)
            {
                System.Windows.Forms.DialogResult res =
                    System.Windows.Forms.MessageBox.Show("Соотношение сторон картинки не 1:1!\nВы уверенны, что хотите продолжить?", 
                                                         "Не стандартное соотношение сторон", 
                                                         System.Windows.Forms.MessageBoxButtons.YesNo,
                                                         System.Windows.Forms.MessageBoxIcon.Error, 
                                                         System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if(res == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            Image control = new Image();
            control.Source = image;
            stackPanel.Children.Add(control);
        }

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
    }
}
