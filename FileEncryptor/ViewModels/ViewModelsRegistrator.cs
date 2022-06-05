using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FileEncryptor.ViewModels
{
    internal static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModel(this IServiceCollection services) => services
            .AddSingleton<EncryptorWindowViewModel>();
    }
}
