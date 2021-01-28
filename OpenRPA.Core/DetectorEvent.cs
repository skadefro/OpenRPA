using OpenRPA.Interfaces;
using OpenRPA.Interfaces.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Core
{
    public class DetectorEvent : IDetectorEvent
    {
        public ITokenUser user { get; set; }
        public IElement element { get; set; }
        public string host { get; set; }
        public string fqdn { get; set; }
        public string result { get; set; }
        public DetectorEvent()
        {
            //this.element = element;
            host = Environment.MachineName.ToLower();
            fqdn = System.Net.Dns.GetHostEntry(Environment.MachineName).HostName.ToLower();
        }

    }
}
