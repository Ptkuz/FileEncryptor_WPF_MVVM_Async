using FileEncryptor.Infrastucture.Commands;
using FileEncryptor.Services.Interfaces;
using FileEncryptor.ViewModels.Base;
using System.IO;
using System.Windows.Input;

namespace FileEncryptor.ViewModels
{
    internal class EncryptorWindowViewModel : ViewModel
    {
        private const string encryptedFileSuffix = ".encrypted";

        private readonly IUserDialog userDialog;
        private readonly IEncryptor encryptor;

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
            set {  Set(ref password, value); }
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
        #region SelectFileCommand
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

        #region EncryptCommand
        private ICommand encryptCommand;
        public ICommand EncryptCommand => encryptCommand ??= new LambdaCommand(OnEncryptCommandExecuted, CanEncryptCommandExecute);

        private bool CanEncryptCommandExecute(object p) => (p is FileInfo file && file.Exists || SelectedFile != null) && !string.IsNullOrEmpty(Password);


        private void OnEncryptCommandExecuted(object p)
        {
            var file = p as FileInfo ?? SelectedFile;
            if (file is null) return;


            var defaultFileName = file.FullName + encryptedFileSuffix;
            if(!userDialog.SaveFile("Выбор файла для сохранения", out var destination_path, defaultFileName)) return;

            encryptor.Encrypt(file.FullName, destination_path, Password);

            userDialog.Information("Шифрование", "Шифрование выполнено успешно!");
        }


        #endregion DecryptCommand

        #region
        private ICommand descryptCommand;
        public ICommand DescryptCommand => descryptCommand ??= new LambdaCommand(OnDescryptCommandExecuted, CanDescryptCommandExecute);

        private bool CanDescryptCommandExecute(object p) => (p is FileInfo file && file.Exists || SelectedFile != null) && !string.IsNullOrEmpty(Password);
       

        private void OnDescryptCommandExecuted(object p)
        {
            var file = p as FileInfo ?? SelectedFile;
            if (file is null) return;

            var defaultFileName = file.FullName.EndsWith(encryptedFileSuffix) ?
                file.FullName.Substring(0, file.FullName.Length - encryptedFileSuffix.Length) :
                file.FullName;
            if (!userDialog.SaveFile("Выбор файла для сохранения", out var destination_path, defaultFileName)) return;

          bool success = encryptor.Descrypt(file.FullName, destination_path, Password);
            if (success)
                userDialog.Information("Шифрование", "Дешифрование файла выполнено успешно!");
            else
                userDialog.Warning("Шифрование", "Дешифрование файла не выполнено: указан неверный пароль.");
        }


        #endregion
        #endregion

        public EncryptorWindowViewModel(IUserDialog userDialog, IEncryptor encryptor)
        {
            this.userDialog = userDialog;
            this.encryptor = encryptor;
            
        }

    }
}
