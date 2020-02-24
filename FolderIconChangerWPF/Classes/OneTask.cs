using System;
using System.Threading;
using System.Threading.Tasks;

namespace FolderIconChangerWPF.Classes
{
    /// <summary>
    /// Handles Multiple Task requests for one operation. by Canceling old running tasks. using <see cref="Tasks.OneTaskHandler"/>
    /// </summary>
    public class OneTask : IDisposable
    {

        private OneTaskHandler _OneTaskHandler;

        public OneTaskHandler OneTaskHandler
        {
            get
            {
                if (this._OneTaskHandler is null) this._OneTaskHandler = new OneTaskHandler();
                return this._OneTaskHandler;
            }
            //set { _OneTaskHandler = value; }
        }

        public Action ResetIsWorkingPropsAction { get; set; }
        public Action IsWorkingPropsAction { get; set; }
        public Func<bool> CanRunNewTask { get; set; }
        //public Task TaskToRun { get; set; }

        CancellationTokenSource PrepareNewTask()
        {
            //Add Waiting props Here Like
            //Mouse.OverrideCursor = Cursors.Wait;
            this.IsWorkingPropsAction?.Invoke();

            this.OneTaskHandler.CancelRunningTasks();

            //Add your code to check to run a new operation. Like
            if (!(this.CanRunNewTask is null) && !(this.CanRunNewTask.Invoke()))
            {
                this.ResetIsWorkingPropsAction?.Invoke();
                return null;
            }

            //A CancellationTokenSource for the new Task
            //var cancellationTokenSource = this.OneTaskHandler.PrepareNewTask();
            return this.OneTaskHandler.PrepareNewTask();
        }
        bool AfterTask(CancellationTokenSource cancellationTokenSource)
        {
            // Remove cancellationTokenSource from running tasks
            this.OneTaskHandler.AfterTask(cancellationTokenSource);

            //If the task is Canceled by a newer task or other things
            //If the operation is Canceled and there is an other task running Let next task to reset props
            if (cancellationTokenSource.IsCancellationRequested && this.OneTaskHandler.ContainsAnyTask) return false;
            return true;
        }

        public async Task Run(Action<CancellationToken> action, Action<TaskResult> OnTaskResult = null, bool throwOperationCanceledException = false)
        {
            var cancel = PrepareNewTask(); if (cancel is null) return;

            //Await for the task
            var taskResult = await TaskResult.RunAsync(action, cancel.Token, throwOperationCanceledException: throwOperationCanceledException);
            if (!AfterTask(cancel)) return;

            //Set Result Code from TaskResult
            OnTaskResult?.Invoke(taskResult);

            //Reset props code here
            this.ResetIsWorkingPropsAction?.Invoke();
        }
        public async Task Run<O>(Func<CancellationToken, O> func, Action<TaskResult<O>> OnTaskResult = null, bool throwOperationCanceledException = false)
        {
            var cancel = PrepareNewTask(); if (cancel is null) return;

            //Await for the task
            var taskResult = await TaskResult.RunAsync(func, cancel.Token, throwOperationCanceledException: throwOperationCanceledException);
            if (!AfterTask(cancel)) return;

            //Set Result Code from TaskResult
            OnTaskResult?.Invoke(taskResult);

            //Reset props code here
            this.ResetIsWorkingPropsAction?.Invoke();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //  dispose managed state (managed objects).
                    if (this.OneTaskHandler.ContainsAnyTask) this.OneTaskHandler.CancelRunningTasks();
                    _OneTaskHandler = null;
                }

                //  free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.

                disposedValue = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OneTask()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

    }
}
