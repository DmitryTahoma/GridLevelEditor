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
                                   "GridLevelEditor | Не стандартное соотношение сторон",
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Error,
                                   MessageBoxDefaultButton.Button2);
        }

        public DialogResult SavingError(string errorMessage)
        {
            return ErrorMessageBox("Во время сохранения произошла ошибка!\n" + errorMessage,
                                   "GridLevelEditor | Ошибка сохранения");
        }

        public DialogResult GridCreationError(string errorMessage)
        {
            return ErrorMessageBox("Во время создания уровня произошла ошибка!\n" + errorMessage,
                                   "GridLevelEditor | Ошибка создания уровня");
        }

        public DialogResult LoadLevelError(string errorMessage)
        {
            return ErrorMessageBox("Во время загрузки уровня из файла произошла ошибка!\n" + errorMessage,
                                   "GridLevelEditor | Ошибка чтения уровня из файла");
        }

        public DialogResult AreYouSureDialog(string what)
        {
            return MessageBox.Show("Вы уверенны, что хотите " + what,
                                   "GridLevelEditor", 
                                   MessageBoxButtons.YesNo, 
                                   MessageBoxIcon.Question,
                                   MessageBoxDefaultButton.Button1);
        }

        public DialogResult DeletionError(string errorMessage)
        {
            return ErrorMessageBox("Во время удаления уровня произошла ошибка!\n" + errorMessage,
                                   "GridLevelEditor | Ошибка удаления уровня");
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
