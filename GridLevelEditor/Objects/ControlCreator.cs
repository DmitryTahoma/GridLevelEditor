using GridLevelEditor.Controls;
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
    }
}
