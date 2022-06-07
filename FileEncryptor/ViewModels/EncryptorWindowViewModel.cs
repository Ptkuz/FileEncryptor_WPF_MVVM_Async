using FileEncryptor.Infrastucture.Commands;
using FileEncryptor.Infrastucture.Commands.Base;
using FileEncryptor.Services.Interfaces;
using FileEncryptor.ViewModels.Base;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Input;

namespace FileEncryptor.ViewModels
{
    internal class EncryptorWindowViewModel : ViewModel
    {
        private const string encryptedFileSuffix = ".encrypted";

        private readonly IUserDialog userDialog;
        private readonly IEncryptor encryptor;

        private CancellationTokenSource ProcessCancellation;



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

        #region Длина файла

        private string fileLength;
        public string FileLength
        {
            get { return fileLength; }
            set { Set(ref fileLength, value); }

        }
        #endregion

        #region Значение прогресса

        private double progressValue;
        public double ProgressValue
        {
            get { return progressValue; }
            set { Set(ref progressValue, value); }

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




        private async void OnEncryptCommandExecuted(object p)
        {
            var file = p as FileInfo ?? SelectedFile;
            if (file is null) return;


            var defaultFileName = file.FullName + encryptedFileSuffix;
            if (!userDialog.SaveFile("Выбор файла для сохранения", out var destination_path, defaultFileName)) return;

            var progressValue = new Progress<double>(p => ProgressValue = p);

            ProcessCancellation = new CancellationTokenSource();


            ((Command)encryptCommand).Executable = false;
            ((Command)descryptCommand).Executable = false;
            ((Command)selectFileCommand).Executable = false;
            try
            {
                await encryptor.EncryptAsync(file.FullName, destination_path, Password, progress: progressValue, Cancel: ProcessCancellation.Token);
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {

                ((Command)encryptCommand).Executable = true;
                ((Command)descryptCommand).Executable = true;
                ((Command)selectFileCommand).Executable = true;

                if (ProcessCancellation.IsCancellationRequested)
                    userDialog.Warning("Шифрование", "Шифрование прервано!");
                else
                    userDialog.Information("Шифрование", "Шифрование выполнено успешно!");

                ProcessCancellation.Dispose();
                ProcessCancellation = null;
            }


        }


        #endregion DecryptCommand

        #region DescryptCommand
        private ICommand descryptCommand;
        public ICommand DescryptCommand => descryptCommand ??= new LambdaCommand(OnDescryptCommandExecuted, CanDescryptCommandExecute);

        private bool CanDescryptCommandExecute(object p) => (p is FileInfo file && file.Exists || SelectedFile != null) && !string.IsNullOrEmpty(Password);


        private async void OnDescryptCommandExecuted(object p)
        {
            var file = p as FileInfo ?? SelectedFile;
            if (file is null) return;

            var defaultFileName = file.FullName.EndsWith(encryptedFileSuffix) ?
                file.FullName.Substring(0, file.FullName.Length - encryptedFileSuffix.Length) :
                file.FullName;
            if (!userDialog.SaveFile("Выбор файла для сохранения", out var destination_path, defaultFileName)) return;

            var progressValue = new Progress<double>(p => ProgressValue = p);

            ProcessCancellation = new CancellationTokenSource();


            ((Command)encryptCommand).Executable = false;
            ((Command)descryptCommand).Executable = false;
            ((Command)selectFileCommand).Executable = false;
            var decryptionTask = encryptor.DescryptAsync(file.FullName, destination_path, Password, progress: progressValue, Cancel: ProcessCancellation.Token);
            bool success = false;
            try
            {
                success = await decryptionTask;
            }
            catch (OperationCanceledException)
            {

            }
            //catch (CryptographicException ex) 
            //{
            //    userDialog.Warning("Шифрование", ex.Message.ToString());
            //}
            finally
            {
                ((Command)encryptCommand).Executable = true;
                ((Command)descryptCommand).Executable = true;
                ((Command)selectFileCommand).Executable = true;

                if (success)
                    userDialog.Information("Шифрование", "Дешифрование файла выполнено успешно!");
                else if (ProcessCancellation.IsCancellationRequested)
                    userDialog.Warning("Шифрование", "Шифрование прервано!");
                else
                    userDialog.Warning("Шифрование", "дешиифрованиее не удалось!");

                ProcessCancellation.Dispose();
                ProcessCancellation = null;

            }

        }


        #endregion
        #endregion

        #region CancelCommand

        private ICommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new LambdaCommand(OnCancelCommandExecuted, CanCancelCommandExecute);

        private bool CanCancelCommandExecute() => ProcessCancellation != null && !ProcessCancellation.IsCancellationRequested;


        private void OnCancelCommandExecuted() => ProcessCancellation?.Cancel();


        #endregion

        public EncryptorWindowViewModel(IUserDialog userDialog, IEncryptor encryptor)
        {
            this.userDialog = userDialog;
            this.encryptor = encryptor;

        }

    }
}
