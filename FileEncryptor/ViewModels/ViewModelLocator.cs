using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileEncryptor.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FileEncryptor.ViewModels
{
    internal class ViewModelLocator
    {
        public EncryptorWindowViewModel EncryptorWindowModel =>
            App.Services.GetRequiredService<EncryptorWindowViewModel>();
    }
}
