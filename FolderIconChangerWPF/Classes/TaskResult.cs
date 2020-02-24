using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FolderIconChangerWPF.Classes
{
    public class TaskResult<T> : TaskResult
    {
        public T Result { get; set; }
    }

    public class TaskResult
    {
        private static object CheckLockObj = new object();

        public bool OperationWasSuccessful { get; protected set; }
        public Exception Exception { get; set; }

        public bool IsCanceled { get; set; }

        public static TaskResult<O> Run<O>(Func<O> func, Action<TaskResult<O>> Finally = null)
        {
            var res = new TaskResult<O>();
            try
            {
                if (func is null) return res;
                res.Result = func.Invoke();
                res.OperationWasSuccessful = true;
            }
            catch (Exception ex)
            {
                res.Exception = ex;
                //throw;
            }
            Finally?.Invoke(res);
            return res;
        }

        public static TaskResult Run(Action action, Action<TaskResult> Finally = null)
        {
            var res = new TaskResult();
            try
            {
                action?.Invoke();
                res.OperationWasSuccessful = true;
            }
            catch (Exception ex)
            {
                res.Exception = ex;
                //throw;
            }
            Finally?.Invoke(res);
            return res;
        }

        //
        public async static Task<TaskResult<O>> RunAsync<O>(Func<O> func, Action<TaskResult<O>> Finally = null, bool throwOperationCanceledException = false)
        {
            var res = new TaskResult<O>();
            if (func is null) return res;
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        res.Result = func.Invoke();
                        res.OperationWasSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        res.Exception = ex;
                        //throw;
                    }
                });
            }
            catch (OperationCanceledException)
            {
                if (throwOperationCanceledException) throw;
            }
            Finally?.Invoke(res);
            return res;
        }

        public async static Task<TaskResult> RunAsync(Action action, Action<TaskResult> Finally = null, bool throwOperationCanceledException = false)
        {
            var res = new TaskResult();
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        action?.Invoke();
                        res.OperationWasSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        res.Exception = ex;
                        //throw;
                    }
                });
            }
            catch (OperationCanceledException)
            {
                if (throwOperationCanceledException) throw;
            }
            Finally?.Invoke(res);
            return res;
        }

        //
        public async static Task<TaskResult<O>> RunAsync<O>(Func<CancellationToken, O> func,
            CancellationToken cancellationToken, Action<TaskResult<O>> Finally = null, bool throwOperationCanceledException = false)
        {
            var res = new TaskResult<O>();
            if (func is null) return res;
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        res.Result = func.Invoke(cancellationToken);
                        res.OperationWasSuccessful = !cancellationToken.IsCancellationRequested;
                    }
                    catch (Exception ex)
                    {
                        res.Exception = ex;
                        //throw;
                    }
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                if (throwOperationCanceledException) throw;
            }
            res.IsCanceled = cancellationToken.IsCancellationRequested;
            Finally?.Invoke(res);
            return res;
        }

        public async static Task<TaskResult> RunAsync(Action<CancellationToken> action,
            CancellationToken cancellationToken, Action<TaskResult> Finally = null, bool throwOperationCanceledException = false)
        {
            var res = new TaskResult();
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        action?.Invoke(cancellationToken);
                        res.OperationWasSuccessful = !cancellationToken.IsCancellationRequested;
                    }
                    catch (Exception ex)
                    {
                        res.Exception = ex;
                        //throw;
                    }
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                if (throwOperationCanceledException) throw;
            }
            res.IsCanceled = cancellationToken.IsCancellationRequested;
            Finally?.Invoke(res);
            return res;
        }

        //

        public async static Task<TaskResult<O>> RunAsync<O>(Expression<Func<bool>> updatingFlag, Func<O> func, Action<TaskResult<O>> Finally = null
            , bool throwOperationCanceledException = false)
        {
            // Lock to ensure single access to check
            var res = new TaskResult<O>();
            lock (CheckLockObj)
            {
                // Check if the flag property is true (meaning the function is already running)
                if (updatingFlag.GetPropertyValue())
                {
                    res.IsCanceled = true;
                    return res;
                }

                // Set the property flag to true to indicate we are running
                updatingFlag.SetPropertyValue(true);
            }
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        res.Result = func.Invoke();
                        res.OperationWasSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        res.Exception = ex;
                        //throw;
                    }
                });
            }
            catch (OperationCanceledException)
            {
                if (throwOperationCanceledException) throw;
            }
            // Set the property flag back to false now it's finished
            updatingFlag.SetPropertyValue(false);
            Finally?.Invoke(res);
            return res;
        }

        public async static Task<TaskResult> RunAsync(Expression<Func<bool>> updatingFlag, Action action, Action<TaskResult> Finally = null, bool throwOperationCanceledException = false)
        {
            // Lock to ensure single access to check
            var res = new TaskResult();
            lock (CheckLockObj)
            {
                // Check if the flag property is true (meaning the function is already running)
                if (updatingFlag.GetPropertyValue())
                {
                    res.IsCanceled = true;
                    return res;
                }

                // Set the property flag to true to indicate we are running
                updatingFlag.SetPropertyValue(true);
            }
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        action?.Invoke();
                        res.OperationWasSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        res.Exception = ex;
                        //throw;
                    }
                });
            }
            catch (OperationCanceledException)
            {
                if (throwOperationCanceledException) throw;
            }
            // Set the property flag back to false now it's finished
            updatingFlag.SetPropertyValue(false);
            Finally?.Invoke(res);
            return res;
        }

        //

        public async static Task<TaskResult<O>> RunAsync<O>(Expression<Func<bool>> updatingFlag, Func<CancellationToken, O> func,
            CancellationToken cancellationToken, Action<TaskResult<O>> Finally = null, bool throwOperationCanceledException = false)
        {
            // Lock to ensure single access to check
            var res = new TaskResult<O>();
            lock (CheckLockObj)
            {
                // Check if the flag property is true (meaning the function is already running)
                if (updatingFlag.GetPropertyValue())
                {
                    res.IsCanceled = true;
                    return res;
                }

                // Set the property flag to true to indicate we are running
                updatingFlag.SetPropertyValue(true);
            }
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        res.Result = func.Invoke(cancellationToken);
                        res.OperationWasSuccessful = !cancellationToken.IsCancellationRequested;
                    }
                    catch (Exception ex)
                    {
                        res.Exception = ex;
                        //throw;
                    }
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                if (throwOperationCanceledException) throw;
            }
            // Set the property flag back to false now it's finished
            res.IsCanceled = cancellationToken.IsCancellationRequested;
            updatingFlag.SetPropertyValue(false);
            Finally?.Invoke(res);
            return res;
        }

        public async static Task<TaskResult> RunAsync(Expression<Func<bool>> updatingFlag, Action<CancellationToken> action,
            CancellationToken cancellationToken, Action<TaskResult> Finally = null, bool throwOperationCanceledException = false)
        {
            // Lock to ensure single access to check
            var res = new TaskResult();
            lock (CheckLockObj)
            {
                // Check if the flag property is true (meaning the function is already running)
                if (updatingFlag.GetPropertyValue())
                {
                    res.IsCanceled = true;
                    return res;
                }

                // Set the property flag to true to indicate we are running
                updatingFlag.SetPropertyValue(true);
            }
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        action?.Invoke(cancellationToken);
                        res.OperationWasSuccessful = !cancellationToken.IsCancellationRequested;
                    }
                    catch (Exception ex)
                    {
                        res.Exception = ex;
                        //throw;
                    }
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                if (throwOperationCanceledException) throw;
            }
            // Set the property flag back to false now it's finished
            res.IsCanceled = cancellationToken.IsCancellationRequested;
            updatingFlag.SetPropertyValue(false);
            Finally?.Invoke(res);
            return res;
        }
    }
}