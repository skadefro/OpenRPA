using OpenRPA.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Net
{
    class SaveFileMessage : SocketCommand
    {
        public SaveFileMessage() : base()
        {
            msg.command = "savefile";
            metadata = new Core.entity.metadata();
        }
        public string filename { get; set; }
        public string file { get; set; }
        public string mimeType { get; set; }
        public Interfaces.entity.Imetadata metadata { get; set; }
        public string id { get; set; }

    }

}
