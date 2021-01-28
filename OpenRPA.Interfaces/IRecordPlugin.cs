using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces
{
    public interface IRecordPlugin : INotifyPropertyChanged, IPlugin
    {
        string Status { get; }
        void Start();
        void Stop();
        event Action<IRecordPlugin, IRecordEvent> OnUserAction;
        event Action<IRecordPlugin, IRecordEvent> OnMouseMove;
        bool ParseUserAction(ref IRecordEvent e);
        bool ParseMouseMoveAction(ref IRecordEvent e);
        // Selector.Itreeelement[] GetRootElements(Selector.ISelector anchor);
        object[] GetRootElements(object anchor);
        // Selector.ISelector GetSelector(Selector.ISelector anchor, Selector.Itreeelement item);
        object GetSelector(object anchor, object item);
        // IElement[] GetElementsWithSelector(Selector.ISelector selector, IElement fromElement = null, int maxresults = 1);
        IElement[] GetElementsWithSelector(object selector, IElement fromElement = null, int maxresults = 1);
        // bool Match(Selector.ISelectorItem item, IElement m);
        bool Match(object item, IElement m);
        // IElement LaunchBySelector(Selector.ISelector selector, bool CheckRunning, TimeSpan timeout);
        IElement LaunchBySelector(object selector, bool CheckRunning, TimeSpan timeout);
        // void CloseBySelector(Selector.ISelector selector, TimeSpan timeout, bool Force);
        void CloseBySelector(object selector, TimeSpan timeout, bool Force);
    }
}
