using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FileEncryptor.Services.Interfaces;
using Microsoft.Win32;


namespace FileEncryptor.Services
{
    internal class UserDialogService : IUserDialog
    {
        

        public bool OpenFile(string title, out string selectedFile, string filter = "Все файлы(*.*)|*.*")
        {
            OpenFileDialog fileDialog = new OpenFileDialog() 
            {
                Title = title,
                Filter = filter, 
            };

            if (fileDialog.ShowDialog() != true) 
            {
                selectedFile = null;
                return false;
            }
            selectedFile = fileDialog.FileName;
            return true;
        }

        public bool OpenFiles(string title, out IEnumerable<string> selectedFiles, string filter = "Все файлы(*.*)|*.*")
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Title = title,
                Filter = filter,
            };

            if (fileDialog.ShowDialog() != true)
            {
                selectedFiles = Enumerable.Empty<string>();
                return false;
            }
            selectedFiles = fileDialog.FileNames;
            return true;
        }

        public bool SaveFile(string title, out string selectedFile, string defaultFileName = null, string filter = "Все файлы(*.*)|*.*")
        {
            SaveFileDialog fileDialog = new SaveFileDialog()
            {
                Title = title,
                Filter = filter,
            };

            if(!string.IsNullOrEmpty(defaultFileName))
                fileDialog.FileName = defaultFileName;

            if (fileDialog.ShowDialog() != true)
            {
                selectedFile = null;
                return false;
            }
            selectedFile = fileDialog.FileName;
            return true;
        }

        public void Warning(string Title, string Message) => MessageBox.Show(Message, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
       
        public void Eror(string Title, string Message) => MessageBox.Show(Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);


        public void Information(string Title, string Message) => MessageBox.Show(Message, Title, MessageBoxButton.OK, MessageBoxImage.Information);

    }
}
