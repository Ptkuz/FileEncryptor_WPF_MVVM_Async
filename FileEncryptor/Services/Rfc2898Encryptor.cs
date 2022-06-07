using FileEncryptor.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

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


        public async Task EncryptAsync(
            string sourcePath,
            string destinationPath,
            string password,
            int bufferLength = 104200,
            IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("Файл-источник для процесса шифрования не найден", sourcePath);

            if (bufferLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(bufferLength), bufferLength, "Размер буфера чтения должен быть больше 0");



            var encryptor = GetEncryptor(password);

            try
            {
                await using (var destination_encrypted = File.Create(destinationPath, bufferLength))
                await using (var destination = new CryptoStream(destination_encrypted, encryptor, CryptoStreamMode.Write))
                await using (var source = File.OpenRead(sourcePath))
                {
                    var fileLength = source.Length;

                    var buffer = new byte[bufferLength];
                    int readed;
                    var lastpercent = 0.0;
                    do
                    {
                        readed = await source.ReadAsync(buffer, 0, buffer.Length, cancel).ConfigureAwait(false);
                        await destination.WriteAsync(buffer, 0, readed, cancel).ConfigureAwait(false);

                        var position = source.Position;
                        var percent = (double)position / fileLength;
                        if (percent - lastpercent >= 0.001)
                        {
                            progress?.Report(percent);
                            lastpercent = percent;
                        }



                        if (cancel.IsCancellationRequested)
                            cancel.ThrowIfCancellationRequested();
                    }
                    while (readed > 0);
                    destination.FlushFinalBlock();

                    progress?.Report(1);

                }
            }
            catch (OperationCanceledException)
            {
                File.Delete(destinationPath);
                progress?.Report(0);
                //throw;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
                throw;
            }


        }

        public async Task<bool> DescryptAsync(
            string sourcePath,
            string destinationPath,
            string password,
            int bufferLength = 104200,
            IProgress<double> progress = null,
            CancellationToken cancel = default)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("Файл-источник для процесса дешифрования не найден", sourcePath);

            if (bufferLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(bufferLength), bufferLength, "Размер буфера чтения должен быть больше 0");


            var descryptor = GetDecryptor(password);


            try
            {
                await using (var destination_descrypted = File.Create(destinationPath, bufferLength))
                await using (var destination = new CryptoStream(destination_descrypted, descryptor, CryptoStreamMode.Write))
                await using (var encrypted_source = File.OpenRead(sourcePath))
                {
                    var fileLength = encrypted_source.Length;

                    var buffer = new byte[bufferLength];
                    int readed;
                    var lastpercent = 0.0;
                   
                        do
                        {
                            //Thread.Sleep(1000);
                            readed = await encrypted_source.ReadAsync(buffer, 0, buffer.Length, cancel).ConfigureAwait(false);
                            await destination.WriteAsync(buffer, 0, readed, cancel).ConfigureAwait(false);



                            var position = encrypted_source.Position;
                            var percent = (double)position / fileLength;
                            if (percent - lastpercent >= 0.001)
                            {
                                progress?.Report(percent);
                                lastpercent = percent;
                            }

                            if (cancel.IsCancellationRequested)
                                cancel.ThrowIfCancellationRequested();
                        }
                        while (readed > 0);

                        destination.FlushFinalBlock();
                    
                    

                    progress?.Report(1);

                }
            }
            catch (OperationCanceledException)
            {
                File.Delete(destinationPath);
                progress?.Report(0);
                throw;
            }
            catch (CryptographicException)
            {
                File.Delete(destinationPath);
                progress?.Report(0);
                return false;
            }
            catch (Exception)
            {
                //Debug.WriteLine(error.ToString());
                File.Delete(destinationPath);
                progress?.Report(0);
                //throw;
            }

            return true;
        }


    }
}
