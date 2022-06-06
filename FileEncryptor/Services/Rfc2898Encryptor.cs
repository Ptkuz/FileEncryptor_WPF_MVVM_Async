using FileEncryptor.Services.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;

namespace FileEncryptor.Services
{
    internal class Rfc2898Encryptor : IEncryptor
    {
        private static readonly byte[] __Salt =
        {
            0x26, 0xdc, 0xff, 0x00,
            0xad, 0xed, 0x7a, 0xee,
            0xc5, 0xfe, 0x07, 0xaf,
            0x4d, 0x08, 0x22, 0x3c
        };

        private static byte[] CreateRandomSalt(int length = 16) => RandomNumberGenerator.GetBytes(Math.Max(1, length));


        private static ICryptoTransform GetEncryptor(string password, byte[] Slat = null)
        {
            var pdb = new Rfc2898DeriveBytes(password, Slat ?? __Salt);
            var algorithm = Aes.Create();
            algorithm.Key = pdb.GetBytes(32);
            algorithm.IV = pdb.GetBytes(16);
            return algorithm.CreateEncryptor();
        }

        private static ICryptoTransform GetDecryptor(string password, byte[] Slat = null)
        {
            var pdb = new Rfc2898DeriveBytes(password, Slat ?? __Salt);
            var algorithm = Aes.Create();
            algorithm.Key = pdb.GetBytes(32);
            algorithm.IV = pdb.GetBytes(16);
            return algorithm.CreateDecryptor();
        }

        public void Encrypt(string sourcePath, string destinationPath, string password, int bufferLength = 104200)
        {
            var encryptor = GetEncryptor(password);

            using (var destination_encrypted = File.Create(destinationPath, bufferLength))
            using (var destination = new CryptoStream(destination_encrypted, encryptor, CryptoStreamMode.Write))
            using (var source = File.OpenRead(sourcePath))
            {
                var buffer = new byte[bufferLength];
                int readed;
                do 
                {
                    readed = source.Read(buffer, 0, buffer.Length); 
                    destination.Write(buffer, 0, readed);
                }
                while (readed>0);
                destination.FlushFinalBlock();

            }


        }

        public bool Descrypt(string sourcePath, string destinationPath, string password, int bufferLength = 104200)
        {
            var descryptor = GetDecryptor(password);


            using (var destination_descrypted = File.Create(destinationPath, bufferLength))
            using (var destination = new CryptoStream(destination_descrypted, descryptor, CryptoStreamMode.Write))
            using (var encrypted_source = File.OpenRead(sourcePath))
            {
                var buffer = new byte[bufferLength];
                int readed;
                do
                {
                    readed = encrypted_source.Read(buffer, 0, buffer.Length);
                    destination.Write(buffer, 0, readed);
                }
                while (readed > 0);
                try
                {
                    destination.FlushFinalBlock();
                }
                catch (CryptographicException)
                {

                    return false;
                }

            }

            return true;
        }


    }
}
