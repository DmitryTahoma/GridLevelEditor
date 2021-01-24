using Catel.Data;
using Catel.MVVM;

namespace GridLevelEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            TitleText = "Grid Level Editor v1.0";
        }

        public string TitleText
        {
            get => GetValue<string>(TitleTextProperty);
            set => SetValue(TitleTextProperty, value);
        }
        public static readonly PropertyData TitleTextProperty = RegisterProperty(nameof(TitleText), typeof(string), "");
    }
}
