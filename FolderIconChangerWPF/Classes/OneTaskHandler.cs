using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FolderIconChangerWPF.Classes
{
    /// <summary>
    /// Handles Multiple Task requests for one operation. by Canceling old running tasks.
    /// </summary>
    /// <example>
    /// OneTaskHandler MethodNameOneTaskHandler;
    ///
    /// async Task MethodName()
    /// {
    ///     if (MethodNameOneTaskHandler is null) MethodNameOneTaskHandler = new OneTaskHandler();
    ///
    ///     void ResetPropsMethod()
    ///     {
    ///         //Reset props code here like
    ///         Mouse.OverrideCursor = null;
    ///     }
    ///
    ///     //Add Waiting props Here Like
    ///     //Mouse.OverrideCursor = Cursors.Wait;
    ///
    ///     MethodNameOneTaskHandler.CancelRunningTasks();
    ///
    ///     //Add your code to check to run a new operation. Like
    ///     //if (!File.Exists(FilePath))
    ///     //{
    ///     //    ResetPropsMethod();
    ///     //    return;
    ///     //}
    ///
    ///     //A CancellationTokenSource for the new Task
    ///     var cancellationTokenSource = MethodNameOneTaskHandler.PrepareNewTask();
    ///
    ///     //Await for the task
    ///     var TaskResult = await Task.Run(() => {/*Task func*/ return true; }, cancellationTokenSource.Token);
    ///
    ///     // Remove cancellationTokenSource from running tasks
    ///     MethodNameOneTaskHandler.AfterTask(cancellationTokenSource);
    ///
    ///
    ///      //If the operation is Canceled and there is an other task running Let next task to reset props
    ///      if (cancellationTokenSource.IsCancellationRequested && $MethodName$OneTaskHandler.ContainsAnyTask) return;
    ///     
    ///      //Set Result Code from taskResult
    ///		
    ///     //Reset props code here
    ///     ResetPropsMethod();
    /// }
    /// </example>
    public class OneTaskHandler
    {
        //public string Name { get; set; }
        //public bool JustOneTask { get; set; } = true;
        public bool ThrowOnCancel { get; set; }

        /// <summary>
        /// Running Tasks <see cref="CancellationTokenSource"/>s
        /// </summary>
        public HashSet<CancellationTokenSource> RunningTasksCTS { get; } = new HashSet<CancellationTokenSource>();

        public void CancelRunningTasks()
        {
            lock (this.RunningTasksCTS)
            {
                foreach (var item in this.RunningTasksCTS)
                {
                    item.Cancel(this.ThrowOnCancel);
                }
                this.RunningTasksCTS.Clear();
            }
        }

        //public TaskInfo CurrentPreparedTask { get; private set; }

        public CancellationTokenSource PrepareNewTask(CancellationTokenSource cancellationTokenSource = default)
        {
            var res = cancellationTokenSource ?? new CancellationTokenSource();
            lock (this.RunningTasksCTS)
            {
                this.RunningTasksCTS.Add(res);
            }
            return res;
        }

        /// <summary>
        /// Removes the specified <see cref="CancellationTokenSource"/> from <see cref="RunningTasksCTS"/>.
        /// Note: Use this after await <see cref="Task"/>
        /// </summary>
        /// <param name="cancellationTokenSource">The element to remove.</param>
        /// <returns></returns>
        public bool AfterTask(CancellationTokenSource cancellationTokenSource)
        {
            lock (this.RunningTasksCTS)
            {
                return this.RunningTasksCTS.Remove(cancellationTokenSource);
            }
        }

        public bool ContainsAnyTask
        {
            get
            {
                lock (this.RunningTasksCTS)
                {
                    return this.RunningTasksCTS.Any();
                }
            }
        }

    }
}
