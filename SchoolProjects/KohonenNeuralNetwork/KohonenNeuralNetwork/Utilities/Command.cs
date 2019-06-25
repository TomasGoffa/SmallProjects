/// <summary>
/// This file is part of application
/// which implements Kohonen neural network.
/// 
/// Author:     Tomas Goffa
/// Created:    2018
/// </summary>

namespace KohonenNeuralNetwork
{
    using System;
    using System.Windows.Input;

    internal class SimpleCommand : ICommand
    {
        private readonly Action executeMethod;
        public event EventHandler CanExecuteChanged;

        public SimpleCommand (Action executeMethod)
        {
            this.executeMethod = executeMethod;
        }

        public bool CanExecute (object parameter)
        {
            return true;
        }

        public void Execute (object parameter)
        {
            this.executeMethod?.Invoke ();
        }
    }

    internal class Command<T> : ICommand
    {
        readonly Action<T> _execute = null;
        readonly Predicate<T> _canExecute = null;

        public Command (Action<T> execute)
            : this (execute, null)
        {
        }

        public Command (Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException ("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute (object parameter)
        {
            return _canExecute == null ? true : _canExecute ((T)parameter);
        }

        public void Execute (object parameter)
        {
            _execute ((T)parameter);
        }
    }
}
