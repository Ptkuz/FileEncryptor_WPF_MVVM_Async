using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileEncryptor.Infrastucture.Commands.Base;
using System.Windows;

namespace FileEncryptor.Infrastucture.Commands
{
    internal class CloseWindow : Command
    {
        protected override bool CanExecute(object parametr) => (parametr as Window ?? App.FocusedWindow ?? App.ActiveWindow) != null;



        protected override void Execute(object parametr) => (parametr as Window ?? App.FocusedWindow ?? App.ActiveWindow)?.Close(); 
       
    }
}
