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
    /// OneTaskHandler MethodNameOneTaskHandler; //Class level
    /// 
    /// async Task MethodName() {
    ///        if (MethodNameOneTaskHandler is null) MethodNameOneTaskHandler = new OneTaskHandler();
    ///        CancellationTokenSource cancellationTokenSource = null;
    ///        lock (MethodNameOneTaskHandler)
    ///        {
    ///            MethodNameOneTaskHandler.CancelRunningTasks();
    ///            //Add your code to check to run a new operation.
    ///            //if (!File.Exists(FilePath)) return;
    ///            cancellationTokenSource = MethodNameOneTaskHandler.PrepareNewTask();
    ///        }
    ///        
    ///        //Await for the task
    ///        var TaskResult = await Method.Run(()=>{/*Task func*/}, cancellationTokenSource.Token);
    ///        
    ///        // Remove cancellationTokenSource from running tasks
    ///        MethodNameOneTaskHandler.CleanUp(cancellationTokenSource);
    ///        //If the operation is not Canceled 
    ///         if (!cancellationTokenSource.IsCancellationRequested)
    ///         {
    ///             //Set Result Code from TaskResult
    ///         }
    ///         else if (cancellationTokenSource.IsCancellationRequested && MethodNameOneTaskHandler.ContainsTask())
    ///         {
    ///             //Let next task to reset props
    ///             return;
    ///         }
    ///         //Reset props code here like Mouse.OverrideCursor = null;
    /// }
    /// 
    /// </example>
    public class OneTaskHandler
    {

        //public string Name { get; set; }
        //public bool JustOneTask { get; set; } = true;
        public bool ThrowOnCancel { get; set; }

        public HashSet<CancellationTokenSource> RunningTasksCancellationTokenSources { get; } = new HashSet<CancellationTokenSource>();

        public void CancelRunningTasks()
        {
            lock (RunningTasksCancellationTokenSources)
            {
                foreach (var item in RunningTasksCancellationTokenSources)
                {
                    item.Cancel(ThrowOnCancel);
                }
                RunningTasksCancellationTokenSources.Clear();
            }
        }

        //public TaskInfo CurrentPreparedTask { get; private set; }

        public CancellationTokenSource PrepareNewTask(CancellationTokenSource cancellationTokenSource = default)
        {
            var res = cancellationTokenSource ?? new CancellationTokenSource();
            lock (RunningTasksCancellationTokenSources)
            {
                this.RunningTasksCancellationTokenSources.Add(res);
            }
            return res;
        }

        /// <summary>
        /// Removes the specified <see cref="CancellationTokenSource"/> from <see cref="RunningTasksCancellationTokenSources"/>
        /// Note: Use this after await <see cref="Task"/> 
        /// </summary>
        /// <param name="cancellationTokenSource">The element to remove.</param>
        /// <returns></returns>
        public bool AfterTask(CancellationTokenSource cancellationTokenSource)
        {
            lock (RunningTasksCancellationTokenSources)
            {
                //if (!RunningTasksCancellationTokenSources.Contains(cancellationTokenSource))
                //{
                //    throw new KeyNotFoundException("CancellationTokenSource not ins the list!");
                //}
                return RunningTasksCancellationTokenSources.Remove(cancellationTokenSource);
            }
        }
        public bool ContainsAnyTask
        {
            get
            {
                lock (RunningTasksCancellationTokenSources)
                {
                    return RunningTasksCancellationTokenSources.Any();
                }
            }
        }
    }
}
