using OpenRPA.Input;
using OpenRPA.Interfaces;
using OpenRPA.Script.Activities;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenRPA.Script
{
    public class Plugin : INotifyPropertyChanged, IRecordPlugin
    {
        public event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChanged += value;
            }

            remove
            {
                PropertyChanged -= value;
            }
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        public static object[] _GetRootElements(object anchor)
        {
            var result = new List<object>();
            return result.ToArray();
        }
        public object[] GetRootElements(object anchor)
        {
            return Plugin._GetRootElements(anchor);
        }
        public object GetSelector(object anchor, object item)
        {
            return null;
        }
        public event Action<IRecordPlugin, IRecordEvent> OnUserAction
        {
            add { }
            remove { }
        }
        public event Action<IRecordPlugin, IRecordEvent> OnMouseMove
        {
            add { }
            remove { }
        }
        private Views.RecordPluginView view;
        public System.Windows.Controls.UserControl editor
        {
            get
            {
                if (view == null)
                {
                    view = new Views.RecordPluginView();
                }
                return view;
            }
        }
        public string Name { get => "Script"; }
        public string Status { get => ""; }
        public void Start()
        {
        }
        public void Stop()
        {
        }
        public bool ParseUserAction(ref IRecordEvent e)
        {
            return false;
        }
        //private static PyObject InstallAndImport(bool force = false)
        //{
        //    //var installer = new Installer();
        //    Installer.SetupPython(force).Wait();
        //    //Installer.InstallWheel(typeof(NumPy).Assembly, "numpy-1.16.3-cp37-cp37m-win_amd64.whl").Wait();
        //    PythonEngine.Initialize();
        //    var mod = Py.Import("numpy");
        //    return mod;
        //}
        public static IOpenRPAClient client = null;
        public void Initialize(IOpenRPAClient client)
        {
            Plugin.client = client;
            _ = PluginConfig.csharp_intellisense;
            _ = PluginConfig.vb_intellisense;
            _ = PluginConfig.use_embedded_python;
            _ = PluginConfig.py_create_no_window;

            //System.Diagnostics.Debugger.Launch();
            //System.Diagnostics.Debugger.Break();
            if (PluginConfig.use_embedded_python)
            {
                if(!Python.Included.Installer.IsPythonInstalled())
                {
                    Python.Included.Installer.SetupPython(true).Wait();
                }
                else
                {
                    Python.Included.Installer.SetupPython(false).Wait();
                }
                var path = Python.Included.Installer.EmbeddedPythonHome;
                PythonUtil.Setup.SetPythonPath(path);
                // Python.Runtime.PythonEngine.Initialize();
            } else
            {
                PythonUtil.Setup.Run();
            }


            // PythonUtil.Setup.Run();
            //Python.Runtime.PythonEngine.Initialize();
            _ = Python.Runtime.PythonEngine.BeginAllowThreads();
            //if (InvokeCode.pool == null)
            //{
            //    InvokeCode.pool = RunspaceFactory.CreateRunspacePool(1, 5);
            //    InvokeCode.pool.ApartmentState = System.Threading.ApartmentState.MTA;
            //    InvokeCode.pool.Open();
            //}

        }
        public IElement[] GetElementsWithSelector(object selector, IElement fromElement = null, int maxresults = 1)
        {
            return null;
        }
        public IElement LaunchBySelector(object selector, bool CheckRunning, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
        public void CloseBySelector(object selector, TimeSpan timeout, bool Force)
        {
            throw new NotImplementedException();
        }
        public bool Match(object item, IElement m)
        {
            return false;
        }
        public bool ParseMouseMoveAction(ref IRecordEvent e)
        {
            return false;
        }
    }

}
