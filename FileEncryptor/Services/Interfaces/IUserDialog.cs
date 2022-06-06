using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptor.Services.Interfaces
{
    internal interface IUserDialog
    {
        bool OpenFile(string title, out string selectedFile, string filter = "Все файлы (*.*)|(*.*)");
        bool OpenFiles(string title, out IEnumerable<string> selectedFiles, string filter = "Все файлы (*.*)|(*.*)");
    }
}
