using Catel.Data;
using Catel.MVVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GridLevelEditor.ViewModels.Controls
{
    class MgElemControlViewModel : ViewModelBase
    {
        private Image imageControl;

        public MgElemControlViewModel()
        {
            imageControl = null;

            BindImageSource = new Command<Image>(OnBindImageSourceExecute);
        }

        #region Properties

        public Visibility SelectVisibility
        {
            get => GetValue<Visibility>(SelectVisibilityProperty);
            set => SetValue(SelectVisibilityProperty, value);
        }
        public static readonly PropertyData SelectVisibilityProperty = RegisterProperty(nameof(SelectVisibility), typeof(Visibility), Visibility.Hidden);

        public BitmapImage ImageSource
        {
            get => GetValue<BitmapImage>(ImageSourceProperty);
            set 
            {
                SetValue(ImageSourceProperty, value); 
                if(imageControl != null && value != null)
                {
                    SetValue(ImageSourceProperty, value);
                }
            }
        }
        public static readonly PropertyData ImageSourceProperty = RegisterProperty(nameof(ImageSource), typeof(BitmapImage));

        public string TextIndex
        {
            get => GetValue<string>(TextIndexProperty);
            set => SetValue(TextIndexProperty, value);
        }
        public static readonly PropertyData TextIndexProperty = RegisterProperty(nameof(TextIndex), typeof(string));

        public double ImageSize
        {
            get => GetValue<double>(ImageSizeProperty);
            set => SetValue(ImageSizeProperty, value);
        }
        public static readonly PropertyData ImageSizeProperty = RegisterProperty(nameof(ImageSize), typeof(double));

        #endregion

        #region Commands

        public Command<Image> BindImageSource { get; private set; }
        private void OnBindImageSourceExecute(Image imgControl)
        {
            imageControl = imgControl;
            imageControl.Source = ImageSource;
        }

        #endregion
    }
}
