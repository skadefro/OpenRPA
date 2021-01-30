using OpenRPA.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenRPA.Forms
{
    class RunPlugin : IRunPlugin
    {
        public UserControl editor
        {
            get
            {
                return null;
            }
        }
        public static string PluginName => "ForgeForms";
        public string Name => PluginName;
        public static IOpenRPAClient client;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Initialize(IOpenRPAClient client)
        {
            RunPlugin.client = client;
        }
        public void onWorkflowAborted(ref IWorkflowInstance e)
        {
        }
        public void onWorkflowCompleted(ref IWorkflowInstance e)
        {
        }
        public void onWorkflowIdle(ref IWorkflowInstance e)
        {
        }
        public bool onWorkflowResumeBookmark(ref IWorkflowInstance e, string bookmarkName, object value)
        {
            return true;
        }
        public bool onWorkflowStarting(ref IWorkflowInstance e, bool resumed)
        {
            return true;
        }
    }
}
