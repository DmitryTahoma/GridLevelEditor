using Catel.Data;
using Catel.MVVM;
using GridLevelEditor.Objects;

namespace GridLevelEditor.ViewModels.Controls
{
    class StartWindowViewModel : ViewModelBase
    {
        public delegate void CreationGridHandler(int height, int width, int cellSize);

        private Validator v;
        private CreationGridHandler creationGrid;

        public StartWindowViewModel()
        {
            v = new Validator();
            creationGrid = null;

            CreateLevel = new Command(OnCreateLevelExecute);
        }

        #region Properties

        public string LevelName
        {
            get => GetValue<string>(LevelNameProperty);
            set => SetValue(LevelNameProperty, value);
        }
        public static readonly PropertyData LevelNameProperty = RegisterProperty(nameof(LevelName), typeof(string), "");

        public string LevelWidthText
        {
            get => GetValue<string>(LevelWidthTextProperty);
            set => SetValue(LevelWidthTextProperty, v.IntString(value, GetValue<string>(LevelWidthTextProperty)));
        }
        public static readonly PropertyData LevelWidthTextProperty = RegisterProperty(nameof(LevelWidthText), typeof(string), "");

        public string LevelHeightText
        {
            get => GetValue<string>(LevelHeightTextProperty);
            set => SetValue(LevelHeightTextProperty, v.IntString(value, GetValue<string>(LevelHeightTextProperty)));
        }
        public static readonly PropertyData LevelHeightTextProperty = RegisterProperty(nameof(LevelHeightText), typeof(string), "");

        public string LevelCellSizeText
        {
            get => GetValue<string>(LevelCellSizeTextProperty);
            set => SetValue(LevelCellSizeTextProperty, v.IntString(value, GetValue<string>(LevelCellSizeTextProperty)));
        }
        public static readonly PropertyData LevelCellSizeTextProperty = RegisterProperty(nameof(LevelCellSizeText), typeof(string), "");

        #endregion

        #region Commands

        public Command CreateLevel { get; private set; }
        private void OnCreateLevelExecute()
        {
            if(creationGrid != null)
            {
                int rows = 1;
                if(int.TryParse(LevelHeightText, out int pRows))
                {
                    rows = pRows;
                }

                int cols = 1;
                if(int.TryParse(LevelWidthText, out int pCols))
                {
                    cols = pCols;
                }

                int size = 50;
                if(int.TryParse(LevelCellSizeText, out int pSize))
                {
                    size = pSize;
                }

                creationGrid.Invoke(rows, cols, size);
            }
        }

        #endregion

        public void SetCreationGrid(CreationGridHandler handler)
        {
            if(creationGrid == null)
            {
                creationGrid = handler;
            }
        }
    }
}
