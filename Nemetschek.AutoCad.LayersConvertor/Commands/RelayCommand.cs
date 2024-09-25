using System.Windows.Input;

namespace Nemetschek.AutoCad.LayersConvertor.Commands
{
    class RelayCommand : ICommand
    {
        private Action<object> _execute { get; set; }

        private Predicate<object> _canExecute { get; set; }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        
        public bool CanExecute(object? parameter=null) => _canExecute(parameter!);

        public void Execute(object? parameter=null) => _execute(parameter!);

    }
}

