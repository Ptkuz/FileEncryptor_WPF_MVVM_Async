using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileEncryptor.ViewModels.Base;

namespace FileEncryptor.ViewModels
{
    internal class EncryptorWindowViewModel : ViewModel
    {

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
    }
}
