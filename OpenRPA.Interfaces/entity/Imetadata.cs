using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces.entity
{
    public interface Imetadata : IBase
    {
        string filename { get ; set; }
        string path { get; set; }
        string workflow { get; set; }
    }
}
