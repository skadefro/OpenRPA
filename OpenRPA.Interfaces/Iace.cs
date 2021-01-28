using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces
{
    public interface Iace
    {
        bool deny { get; set; }
        string rights { get; set; }
        string _id { get; set; }
        string name { get; set; }
        void setBit(decimal bit);
        void unsetBit(decimal bit);
        bool getBit(decimal bit);
    }
}
