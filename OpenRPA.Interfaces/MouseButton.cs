using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces
{
    [Flags]
    public enum MouseButton
    {
        None,
        Left,
        Right,
        Middle = 4
    }
}
