using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Core.entity
{
    public class metadata : apibase, Interfaces.entity.Imetadata
    {
        public string filename { get { return GetProperty<string>(); } set { SetProperty(value); } }
        public string path { get { return GetProperty<string>(); } set { SetProperty(value); } }
        public string workflow { get { return GetProperty<string>(); } set { SetProperty(value); } }
    }
    public class metadataitem : apibase
    {
        public metadata metadata { get { return GetProperty<metadata>(); } set { SetProperty(value); } }
        public string filename { get { return GetProperty<string>(); } set { SetProperty(value); } }
    }
}
