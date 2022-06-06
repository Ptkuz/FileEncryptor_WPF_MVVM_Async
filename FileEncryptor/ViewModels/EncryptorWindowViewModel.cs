using FileEncryptor.Infrastucture.Commands;
using FileEncryptor.Services.Interfaces;
using FileEncryptor.ViewModels.Base;
using System.IO;
using System.Windows.Input;

namespace FileEncryptor.ViewModels
{
    internal class EncryptorWindowViewModel : ViewModel
    {
        private readonly IUserDialog userDialog;

        #region Заголовок окна

        private string title = "Шифратор";
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }
        #endregion

        #region Пароль
        private string password = "123";
        public string Password
        {
            get { return password; }
            set { Set(ref password, value); }
        }
        #endregion

        #region Выбранный файл
        private FileInfo selectedFile;
        public FileInfo SelectedFile
        {
            get { return selectedFile; }
            set { Set(ref selectedFile, value); }
        }
        #endregion

        #region

        private string fileLength;
        public string FileLength 
        {
            get { return fileLength; }
            set { Set(ref fileLength, value); }
            
        }
        #endregion

        #region Команды
        private ICommand selectFileCommand;
        public ICommand SelectFileCommand => selectFileCommand ??= new LambdaCommand(OnSelectFileCommandExecuted);

        private void OnSelectFileCommandExecuted()
        {
            if (!userDialog.OpenFile("Выбор файла для шифрования", out var filePath)) 
                return;

            if (!string.IsNullOrEmpty(filePath))
            {
                SelectedFile = new FileInfo(filePath);
                decimal file = (decimal)SelectedFile.Length / (decimal)1048576;
                FileLength = (file.ToString("0.00") + " МБ");

            }



        }

        #endregion

        public EncryptorWindowViewModel(IUserDialog userDialog)
        {
            this.userDialog = userDialog;
        }

    }
}
