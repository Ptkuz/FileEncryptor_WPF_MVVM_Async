using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FileEncryptor.Services.Interfaces;

namespace FileEncryptor.Services
{
    internal static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient<IUserDialog, UserDialogService>()
            .AddTransient<IEncryptor, Rfc2898Encryptor>();


        
    }
}
