using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces.entity
{
    public interface Iapiuser : IBase
    {
        string username { get; set; }
        IRolemember[] roles { get; set; }
        bool hasRole(string role);
    }
}
