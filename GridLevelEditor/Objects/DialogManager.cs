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
            return MessageBox.Show("Во время сохранения произошла ошибка!\n" + errorMessage,
                                   "Ошибка сохранения",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error,
                                   MessageBoxDefaultButton.Button1);
        }
    }
}
