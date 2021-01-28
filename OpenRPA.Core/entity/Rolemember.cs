using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Core.entity
{
    public class Rolemember : Interfaces.entity.IRolemember
    {
        public string _id { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return name;
        }
    }
}
