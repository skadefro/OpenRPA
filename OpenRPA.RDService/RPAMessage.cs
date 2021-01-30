using OpenRPA.NamedPipeWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.RDService
{
    [Serializable]
    public class RPAMessage : PipeMessage
    {
        public RPAMessage() : base()
        {
        }
        public RPAMessage(string command) : base()
        {
            this.command = command;
        }
        public RPAMessage(string command, string windowsusername, Core.entity.TokenUser user, string openrpapath) : base()
        {
            this.command = command;
            this.windowsusername = windowsusername;
            this.user = user;
            this.openrpapath = openrpapath;
        }
        public string command { get; set; }
        public string windowsusername { get; set; }
        public Core.entity.TokenUser user { get; set; }
        public string openrpapath { get; set; }

    }
}
