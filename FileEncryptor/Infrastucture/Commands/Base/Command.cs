using System;
using System.Windows.Input;

namespace FileEncryptor.Infrastucture.Commands.Base
{
    internal abstract class Command : ICommand
    {
        private bool executable =true;
        public bool Executable 
        {
            get { return executable; }
            set 
            {
                if(executable == value) return;
                executable = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        bool ICommand.CanExecute(object parametr) => executable && CanExecute(parametr);


        void ICommand.Execute(object parametr) 
        {
            if (CanExecute(parametr))
                Execute(parametr);
        }
       

        protected virtual bool CanExecute(object parametr) => true;

        protected abstract void Execute(object parametr);
    }
}
