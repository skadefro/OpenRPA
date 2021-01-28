using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Activities.Hosting;
namespace OpenRPA.Core.Activity
{
    public abstract class AsyncNativeActivity : NativeActivity
    {
        private Variable<NoPersistHandle> NoPersistHandle { get; set; }
        private Variable<Bookmark> Bookmark { get; set; }
        protected override bool CanInduceIdle
        {
            get
            {
                return true; // we create bookmarks
            }
        }
        protected abstract IAsyncResult BeginExecute(NativeActivityContext context, AsyncCallback callback, object state);
        protected abstract void EndExecute(NativeActivityContext context, IAsyncResult result);
        protected override void Execute(NativeActivityContext context)
        {
            var noPersistHandle = NoPersistHandle.Get(context);
            noPersistHandle.Enter(context);
            var bookmark = context.CreateBookmark(BookmarkResumptionCallback);
            this.Bookmark.Set(context, bookmark);
            BookmarkResumptionHelper helper = context.GetExtension<BookmarkResumptionHelper>();
            Action<IAsyncResult> resumeBookmarkAction = (result) =>
            {
                helper.ResumeBookmark(bookmark, result);
            };
            IAsyncResult asyncResult = this.BeginExecute(context, AsyncCompletionCallback, resumeBookmarkAction);
            if (asyncResult.CompletedSynchronously)
            {
                noPersistHandle.Exit(context);
                context.RemoveBookmark(bookmark);
                EndExecute(context, asyncResult);
            }
        }
        private void AsyncCompletionCallback(IAsyncResult asyncResult)
        {
            if (!asyncResult.CompletedSynchronously)
            {
                Action<IAsyncResult> resumeBookmark = asyncResult.AsyncState as Action<IAsyncResult>;
                resumeBookmark.Invoke(asyncResult);
            }
        }
        private void BookmarkResumptionCallback(NativeActivityContext context, Bookmark bookmark, object value)
        {
            var noPersistHandle = NoPersistHandle.Get(context);
            noPersistHandle.Exit(context);
            // unnecessary since it's not multiple resume:
            // context.RemoveBookmark(bookmark);
            IAsyncResult asyncResult = value as IAsyncResult;
            EndExecute(context, asyncResult);
        }
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            NoPersistHandle = new Variable<NoPersistHandle>();
            Bookmark = new Variable<Bookmark>();
            metadata.AddImplementationVariable(this.NoPersistHandle);
            metadata.AddImplementationVariable(this.Bookmark);
            metadata.RequireExtension<BookmarkResumptionHelper>();
            metadata.AddDefaultExtensionProvider<BookmarkResumptionHelper>(() => new BookmarkResumptionHelper());
        }
    }
}
