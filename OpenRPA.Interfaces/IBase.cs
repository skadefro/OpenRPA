using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces.entity
{
    public interface IBase
    {
        string _id { get; set; }
        string _type { get; set; }
        string name { get; set; }
        DateTime _modified { get; set; }
        string _modifiedby { get; set; }
        string _modifiedbyid { get; set; }
        DateTime _created { get; set; }
        string _createdby { get; set; }
        string _createdbyid { get; set; }
        Iace[] _acl { get; set; }
        string[] _encrypt { get; set; }
        long _version { get; set; }
        bool hasRight(Iapiuser user, ace_right bit);
        bool hasRight(ITokenUser user, ace_right bit);
        void AddRight(ITokenUser user, ace_right[] rights);
        void AddRight(string _id, string name, ace_right[] rights);

    }
}
