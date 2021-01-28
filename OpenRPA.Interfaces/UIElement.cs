using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces
{
    public class UIElement : IElement
    {
        public UIElement()
        {
            var b = true;
        }
        public object RawElement { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle Rectangle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IElement[] Items => throw new NotImplementedException();

        public void Click(bool VirtualClick, MouseButton Button, int OffsetX, int OffsetY, bool DoubleClick, bool AnimateMouse)
        {
            throw new NotImplementedException();
        }

        public void Focus()
        {
            throw new NotImplementedException();
        }

        public Task Highlight(bool Blocking, Color Color, TimeSpan Duration)
        {
            throw new NotImplementedException();
        }

        public string ImageString()
        {
            throw new NotImplementedException();
        }
    }
}
