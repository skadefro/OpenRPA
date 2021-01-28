using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces.entity
{
    public interface ITokenUser
    {
        string _id { get; set; }
        string name { get; set; }
        string username { get; set; }
        IRolemember[] roles { get; set; }
        bool hasRole(string role);
    }
}
