using System.Windows.Forms;

namespace GridLevelEditor.Objects
{
    class DialogManager
    {
        public string GetImageFilepath()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Выберите изображение";
            dialog.Filter = "Изображение (*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png";
            dialog.ShowDialog();
            return dialog.FileName;
        }

        public DialogResult PictureNot1to1()
        {
            return MessageBox.Show("Соотношение сторон картинки не 1:1!\nВы уверенны, что хотите продолжить?",
                                   "Не стандартное соотношение сторон",
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Error,
                                   MessageBoxDefaultButton.Button2);
        }

        public DialogResult SavingError(string errorMessage)
        {
            return ErrorMessageBox("Во время сохранения произошла ошибка!\n" + errorMessage,
                                   "Ошибка сохранения");
        }

        public DialogResult GridCreationError(string errorMessage)
        {
            return ErrorMessageBox("Во время создания уровня произошла ошибка!\n" + errorMessage,
                                   "Ошибка создания уровня");
        }

        public DialogResult LoadLevelError(string errorMessage)
        {
            return ErrorMessageBox("Во время загрузки уровня из файла произошла ошибка!\n" + errorMessage,
                                   "Ошибка чтения уровня из файла");
        }

        private DialogResult ErrorMessageBox(string text, string caption)
        {
            return MessageBox.Show(text, 
                                   caption,
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error,
                                   MessageBoxDefaultButton.Button1);
        }
    }
}
