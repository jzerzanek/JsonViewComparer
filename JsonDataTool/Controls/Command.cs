using System;
using System.Windows.Input;

namespace JsonDataTool.Controls
{
    public class Command : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public Command(Action<object> execute, Func<bool> canExecute)
            : this(execute, o => canExecute())
        {
        }

        public Command(Action execute, Func<bool> canExecute)
            : this(p => execute.Invoke(), canExecute)
        {
        }

        public Command(Action execute)
            : this(execute, () => true)
        {
        }

        public Command(Action<object> execute)
            : this(execute, () => true)
        {
        }

        public bool CanExecute(object parameter)
        {
            return canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            execute.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
