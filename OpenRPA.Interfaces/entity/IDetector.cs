using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces.entity
{
    public interface IDetector : IBase
    {
        Dictionary<string, object> Properties { get; set; }
    }
}
