using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces
{
    public class QueueMessageEventArgs : EventArgs
    {
        public bool isBusy { get; set; }
        public QueueMessageEventArgs()
        {
            isBusy = false;
        }
    }
}
