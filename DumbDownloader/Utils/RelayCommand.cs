using System;
using System.Diagnostics;
using System.Windows.Input;

namespace DumbDownloader.Utils
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is 'true'.
    /// 
    /// 사용례 : ICommand _saveCommand = new RelayCommand(param => this.Save(), param => this.CanSave);
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        // Action : 리턴값이 없는(void) 함수를 파라메터로 주고 받고 싶을 때.
        readonly Action<object?>? _execute;
        // Predicate : 리턴타입이 bool 이고, 입력 매개변수가 무조건 1개일 경우 사용
        readonly Predicate<object?>? _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object?>? execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object?>? execute, Predicate<object?>? canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members
        public event EventHandler? CanExecuteChanged;
        /*{
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }*/
        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        [DebuggerStepThrough]
        public virtual bool CanExecute(object? parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public virtual void Execute(object? parameter)
        {
            _execute(parameter);
        }
        #endregion // ICommand Members
    }
}
