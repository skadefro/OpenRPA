using System;
using System.Activities;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OpenRPA.Core.Activity
{
    public class BookmarkResumptionHelper : IWorkflowInstanceExtension
    {
        private WorkflowInstanceProxy instance;
        public void ResumeBookmark(Bookmark bookmark, object value)

        {

            this.instance.EndResumeBookmark(

                this.instance.BeginResumeBookmark(bookmark, value, null, null));

        }
        IEnumerable<object> IWorkflowInstanceExtension.GetAdditionalExtensions()

        {

            yield break;

        }
        void IWorkflowInstanceExtension.SetInstance(WorkflowInstanceProxy instance)

        {

            this.instance = instance;

        }
    }
}
