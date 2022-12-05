using System;
using System.Windows.Input;
using System.Windows;

namespace PersonnelSystem.Configuration
{
    public class CommandHandler : ICommand
    {
        private Action<object> ExecuteDelegate { get; set; }

        private Func<bool> CanExecuteDelegate { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public CommandHandler(Action executeDelegate)
        {
            ExecuteDelegate = delegate
            {
                executeDelegate();
            };
            CanExecuteDelegate = () => true;
        }

        public CommandHandler(Action executeDelegate, Func<bool> canExecuteDelegate)
        {
            ExecuteDelegate = delegate
            {
                executeDelegate();
            };
            CanExecuteDelegate = canExecuteDelegate;
        }

        public CommandHandler(Action<object> executeDelegate)
        {
            ExecuteDelegate = executeDelegate;
            CanExecuteDelegate = () => true;
        }

        public CommandHandler(Action<object> executeDelegate, Func<bool> canExecuteDelegate)
        {
            ExecuteDelegate = executeDelegate;
            CanExecuteDelegate = canExecuteDelegate;
        }

        public static void UpdateCommandHandlers()
        {
            Application.Current?.Dispatcher?.Invoke(CommandManager.InvalidateRequerySuggested);
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteDelegate();
        }

        public void Execute(object parameter)
        {
            ExecuteDelegate(parameter);
        }
    }
}
