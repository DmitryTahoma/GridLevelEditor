using System.Windows.Media.Imaging;

namespace GridLevelEditor.Objects
{
    class ResourceDriver
    {
        public static BitmapImage GetVoidBmp()
        {
            return new BitmapImage(new System.Uri("pack://application:,,,/Resources/void.png"));
        }
    }
}
