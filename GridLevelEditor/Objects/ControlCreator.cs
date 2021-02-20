using GridLevelEditor.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GridLevelEditor.Objects
{
    class ControlCreator
    {
        public MgElemControl CreateMgElemControl(int size, BitmapImage imageSource, MouseButtonEventHandler onBtDown)
        {
            MgElemControl control = new MgElemControl();
            control.ViewModel.ImageSource = imageSource;
            control.ViewModel.ImageSize = size;
            control.ImgControl.MouseDown += onBtDown;
            return control;
        }

        public Image CreateVoidImage(MouseEventHandler imageChanger)
        {
            Image control = new Image();
            control.Source = ResourceDriver.GetVoidBmp();
            control.MouseEnter += imageChanger;
            control.MouseDown += (s, e) => { imageChanger(s, e); };

            return control;
        }

        public void FillLevelGrid(ref Grid levelGrid, int rows, int columns, int size, MouseEventHandler imageChanger)
        {
            levelGrid.RowDefinitions.Clear();
            levelGrid.ColumnDefinitions.Clear();
            levelGrid.Children.Clear();

            GridLength gridSize = new GridLength(size);

            for (int i = 0; i < columns; ++i)
            {
                levelGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = gridSize });
            }
            if (columns != 0)
            {
                for (int i = 0; i < rows; ++i)
                {
                    levelGrid.RowDefinitions.Add(new RowDefinition() { Height = gridSize });
                }
            }

            for (int i = 0; i < columns; ++i)
            {
                for (int j = 0; j < rows; ++j)
                {
                    Image imageControl = CreateVoidImage(imageChanger);
                    levelGrid.Children.Add(imageControl);
                    Grid.SetColumn(imageControl, i);
                    Grid.SetRow(imageControl, j);
                }
            }
        }
    }
}
