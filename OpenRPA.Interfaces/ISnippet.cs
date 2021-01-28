using OpenRPA.Interfaces.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces
{
    public interface ISnippet : IBase
    {
        string Name { get; }
        string Category { get; }
        string Xaml { get; }
        ISnippet Snippet { get; set; }
    }
}
