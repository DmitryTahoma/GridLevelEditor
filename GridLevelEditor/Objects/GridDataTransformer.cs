using GridLevelEditor.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GridLevelEditor.Objects
{
    class GridDataTransformer
    {
        public static string[][] GetLevelDataFromGrid(Grid grid, KeyValuePair<int, int> levelSize, List<MgElem> keys)
        {
            string[][] data = new string[levelSize.Key][];

            for(int i = 0; i < levelSize.Key; ++i)
            {
                data[i] = new string[levelSize.Value];
            }

            foreach(UIElement elem in grid.Children)
            {
                if(elem != null && elem is Image img && img.Source is BitmapImage bmp)
                {
                    int x = Grid.GetRow(elem);
                    int y = Grid.GetColumn(elem);

                    foreach(MgElem mgElem in keys)
                    {
                        if (mgElem.Image.UriSource.LocalPath == bmp.UriSource.LocalPath)
                        {
                            data[x][y] = mgElem.Id;
                            break;
                        }
                    }
                }
            }

            return data;
        }

        public static void FillGridFromLevelData(Grid grid, GridLength gridSize, string[][] data, KeyValuePair<int, int> levelSize, List<MgElem> keys, MouseEventHandler imageChanger)
        {
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();

            ControlCreator creator = new ControlCreator();

            for (int i = 0; i < levelSize.Value; ++i)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = gridSize });
            }
            if (levelSize.Value != 0)
            {
                for (int i = 0; i < levelSize.Key; ++i)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = gridSize });
                }
            }

            for (int i = 0; i < levelSize.Key; ++i)
            {
                for(int j = 0; j < levelSize.Value; ++j)
                {
                    Image image = creator.CreateVoidImage(imageChanger);

                    foreach(MgElem mgElem in keys)
                    {
                        if(mgElem.Id == data[i][j])
                        {
                            image.Source = mgElem.Image;
                            break;
                        }
                    }

                    grid.Children.Add(image);
                    Grid.SetRow(image, i);
                    Grid.SetColumn(image, j);
                }
            }
        }
    }
}
