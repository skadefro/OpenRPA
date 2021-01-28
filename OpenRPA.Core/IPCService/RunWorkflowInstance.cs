using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Core.IPCService
{
    public class RunWorkflowInstance
    {
        public RunWorkflowInstance() { }
        public RunWorkflowInstance(string UniqueId, string IDOrRelativeFilename, bool WaitForCompleted, Dictionary<string, object> Arguments)
        {
            this.UniqueId = UniqueId;
            this.IDOrRelativeFilename = IDOrRelativeFilename;
            this.WaitForCompleted = WaitForCompleted;
            this.Arguments = Arguments;
            Started = false;
            Pending = new System.Threading.AutoResetEvent(false);
        }
        public string UniqueId { get; set; }
        public bool Started { get; set; }
        public string IDOrRelativeFilename { get; set; }
        public bool WaitForCompleted { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
        public System.Threading.AutoResetEvent Pending { get; set; }
        public Dictionary<string, object> Result { get; set; }
        public Exception Error { get; set; }
    }
}
