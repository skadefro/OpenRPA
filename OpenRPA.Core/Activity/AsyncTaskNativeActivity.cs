using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OpenRPA.Core.Activity
{
    public abstract class AsyncTaskNativeActivity : AsyncNativeActivity
    {
        protected sealed override IAsyncResult BeginExecute(NativeActivityContext context, AsyncCallback callback, object state)
        {
            var task = ExecuteAsync(context);
            var tcs = new TaskCompletionSource<object>(state);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);
                callback?.Invoke(tcs.Task);
            });
            return tcs.Task;
        }
        protected sealed override void EndExecute(NativeActivityContext context, IAsyncResult result)
        {
            var task = (Task<object>)result;
            try
            {
                AfterExecute(context, task.Result);
                return;
            }
            catch (AggregateException ex)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
        protected abstract Task<object> ExecuteAsync(NativeActivityContext context);
        protected abstract void AfterExecute(NativeActivityContext context, object result);
    }
}
