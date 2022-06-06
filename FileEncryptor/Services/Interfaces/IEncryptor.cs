using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptor.Services.Interfaces
{
    internal interface IEncryptor
    {
        void Encrypt(string sourcePath, string destinationPath, string password, int bufferLength = 104200);
        bool Descrypt(string sourcePath, string destinationPath, string password, int bufferLength = 104200);

    }
}
