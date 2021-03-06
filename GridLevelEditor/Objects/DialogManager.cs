﻿using System.Windows.Forms;

namespace GridLevelEditor.Objects
{
    class DialogManager
    {
        public string GetImageFilepath()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "GridLevelEditor | Выберите изображение";
            dialog.Filter = "Изображение (*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png";
            dialog.ShowDialog();
            return dialog.FileName;
        }

        public string GetFileExportXml()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "GridLevelEditor | Экспорт в XML";
            dialog.Filter = "Файл XML (*.xml)|*.xml";
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

        public DialogResult ExportError(string errorMessage)
        {
            return ErrorMessageBox("Во время экспорта уровня произошла ошибка!\n" + errorMessage,
                                   "GridLevelEditor | Ошибка экспорта уровня");
        }

        public DialogResult LevelIsAlreadyExists(string levelName)
        {
            return ErrorMessageBox("Уровень с таким именем уже существует!", "GridLevelEditor | Уровень уже существует");
        }

        public DialogResult FileNotExist(string path)
        {
            return ErrorMessageBox("Не найден файл по пути:\n" + path, "GridLevelEditor | Не найден файл по пути");
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
