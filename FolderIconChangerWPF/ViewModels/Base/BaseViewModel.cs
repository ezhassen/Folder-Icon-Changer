using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FolderIconChangerWPF.ViewModels
{
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Protected Members

        /// <summary>
        /// A global lock for property checks so prevent locking on different instances of expressions.
        /// Considering how fast this check will always be it isn't an issue to globally lock all callers.
        /// </summary>
        protected object mPropertyValueCheckLock = new object();

        #endregion

        #region NotifyPropertyChanged
       
        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the value and calls OnPropertyChanged if it not equal the storage value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                OnPropertyChanged(propertyName);
            }
        }
        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnAllPropertiesChanged<T>(System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.Public)
        {
            foreach (var props in typeof(T).GetProperties(bindingFlags))
            {
                OnPropertyChanged(props.Name);
            }
        }

        #endregion

        #region Props

        private bool _IsWorking;

        public bool IsWorking
        {
            get { return _IsWorking; }
            set
            {
                if (_IsWorking != value)
                {
                    _IsWorking = value;
                    OnPropertyChanged(); //uses CallerMemberName
                }
                if (value)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Calls While Window closing Event when canExcut is false then handle the closing event. Must Add the AttachedProperty <see cref="AttachedProperties.WindowHelper.OnClosingCommandProperty"/>
        /// </summary>
        public ICommand OnClosingCommand { get; set; }

        /// <summary>
        /// Calls While Window Close Event. Must Add the AttachedProperty <see cref="AttachedProperties.WindowHelper.ClosedCommandProperty"/>
        /// </summary>
        public ICommand OnClosedCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Runs a command if the updating flag is not set.
        /// If the flag is true (indicating the function is already running) then the action is not run.
        /// If the flag is false (indicating no running function) then the action is run.
        /// Once the action is finished if it was run, then the flag is reset to false
        /// </summary>
        /// <param name="updatingFlag">The boolean property flag defining if the command is already running</param>
        /// <param name="action">The action to run if the command is not already running</param>
        /// <returns></returns>
        protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            // Lock to ensure single access to check
            lock (mPropertyValueCheckLock)
            {
                // Check if the flag property is true (meaning the function is already running)
                if (updatingFlag.GetPropertyValue())
                    return;

                // Set the property flag to true to indicate we are running
                updatingFlag.SetPropertyValue(true);
            }

            try
            {
                // Run the passed in action
                await action();
            }
            finally
            {
                // Set the property flag back to false now it's finished
                updatingFlag.SetPropertyValue(false);
            }
        }

        /// <summary>
        /// Runs a command if the updating flag is not set.
        /// If the flag is true (indicating the function is already running) then the action is not run.
        /// If the flag is false (indicating no running function) then the action is run.
        /// Once the action is finished if it was run, then the flag is reset to false
        /// </summary>
        /// <param name="updatingFlag">The boolean property flag defining if the command is already running</param>
        /// <param name="action">The action to run if the command is not already running</param>
        /// <typeparam name="T">The type the action returns</typeparam>
        /// <returns></returns>
        protected async Task<T> RunCommandAsync<T>(Expression<Func<bool>> updatingFlag, Func<Task<T>> action, T defaultValue = default(T))
        {
            // Lock to ensure single access to check
            lock (mPropertyValueCheckLock)
            {
                // Check if the flag property is true (meaning the function is already running)
                if (updatingFlag.GetPropertyValue())
                    return defaultValue;

                // Set the property flag to true to indicate we are running
                updatingFlag.SetPropertyValue(true);
            }

            try
            {
                // Run the passed in action
                return await action();
            }
            finally
            {
                // Set the property flag back to false now it's finished
                updatingFlag.SetPropertyValue(false);
            }
        }

        #endregion
    }
}
