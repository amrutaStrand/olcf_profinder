using System;
using System.Windows.Input;

namespace Agilent.OpenLab.SampleGrouping
{
    internal class CommandHandler : ICommand
    {
        private Action _action;
        private Func<bool> _canExecute;

        public CommandHandler(Action p1, Func<bool> p2)
        {
            this._action = p1;
            this._canExecute = p2;
            
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            
            _action.Invoke();
        }
        
        
        
    }
}