using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileEncryptor.Infrastucture.Commands.Base;

namespace FileEncryptor.Infrastucture.Commands
{
    internal class LambdaCommand : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        public LambdaCommand(Action Execute, Func<bool> CanExecute = null) :
             this(
                Execute: p => Execute(),
                CanExecute: CanExecute is null ? (Func<object, bool>)null : p => CanExecute())
        { 
        
        }

        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null) 
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }

        protected override void Execute(object parametr) => _Execute(parametr);

        protected override bool CanExecute(object parametr) => _CanExecute?.Invoke(parametr) ?? true;

    }
}
