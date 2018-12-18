using System;
using System.Windows.Input;

namespace FolderIconChangerWPF.ViewModels
{
    /// <summary>
    /// A basic command that runs an Action
    /// </summary>
    public class DelegateCommand : ICommand
    {
        readonly Predicate<object> _canExecute;
        readonly Action _execute;
        readonly Action<object> _executeWithParam;

        public event EventHandler CanExecuteChanged;

        //public DelegateCommand(Func<Task> _executeAsync, Predicate<object> canExecute)
        //{
        //    _canExecute = canExecute;
        //    _execute = async ()=> await _executeAsync();
        //}
        //public DelegateCommand(Func<Task> _executeAsync) : this(_executeAsync, null)
        //{
        //}

        public DelegateCommand(Action execute) : this(execute, null)
        {
        }

        public DelegateCommand(Action execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public DelegateCommand(Action<object> executeWithParam) : this(executeWithParam, null)
        {
        }

        public DelegateCommand(Action<object> executeWithParam, Predicate<object> canExecute)
        {
            _executeWithParam = executeWithParam;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null ? true : _canExecute(parameter);

        public void Execute(object parameter)
        {
            _executeWithParam?.Invoke(parameter);
            _execute?.Invoke();
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
