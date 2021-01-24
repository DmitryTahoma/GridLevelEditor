using Catel.Data;
using Catel.MVVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GridLevelEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            CreateGrid = new Command<Grid>(OnCreateGridExecute);

            TitleText = "Grid Level Editor v1.0";
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

        public Command<Grid> CreateGrid { get; private set; }
        private void OnCreateGridExecute(Grid levelGrid)
        {
            levelGrid.RowDefinitions.Clear();
            levelGrid.ColumnDefinitions.Clear();
            levelGrid.Children.Clear();

            int rows = Parse(HeightText),
                columns = Parse(WidthText);

            for(int i = 0; i < columns; ++i)
            {
                levelGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            if(columns != 0)
            {
                for(int i = 0; i < rows; ++i)
                {
                    levelGrid.RowDefinitions.Add(new RowDefinition());
                }
            }

            for(int i = 0; i < columns; ++i)
            {
                for (int j = 0; j < rows; ++j) 
                {
                    TextBlock tb = new TextBlock() { Text = $"{i}:{j}" };
                    levelGrid.Children.Add(tb);
                    Grid.SetColumn(tb, i);
                    Grid.SetRow(tb, j);
                }
            }
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
    }
}
