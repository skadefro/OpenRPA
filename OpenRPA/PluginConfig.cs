using OpenRPA.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA
{
    class PluginConfig
    {
        private static string pluginname => null;
        private static Config _globallocal = null;
        public static Config globallocal
        {
            get
            {
                if (_globallocal == null)
                {
                    _globallocal = Config.local;
                    _ = remote_allow_multiple_running;
                    _ = remote_allow_multiple_running_max;
                    _ = remote_allowed;
                    _ = cef_useragent;
                    _ = show_getting_started;
                    _ = getting_started_url;
                    _ = notify_on_workflow_remote_start;
                    _ = notify_on_workflow_end;
                    _ = notify_on_workflow_remote_end;
                    _ = log_busy_warning;
                }
                return _globallocal;
            }
        }
        public static bool log_busy_warning { get { return globallocal.GetProperty(null, true); } set { globallocal.SetProperty(null, value); } }
        public static bool fix_xaml_1_2_17 { get { return globallocal.GetProperty(null, true); } set { globallocal.SetProperty(null, value); } }
        public static bool remote_allow_multiple_running { get { return globallocal.GetProperty(null, false); } set { globallocal.SetProperty(null, value); } }
        public static bool remote_allowed { get { return globallocal.GetProperty(null, true); } set { globallocal.SetProperty(null, value); } }
        public static int remote_allow_multiple_running_max { get { return globallocal.GetProperty(null, 2); } set { globallocal.SetProperty(null, value); } }
        public static string cef_useragent { get { return globallocal.GetProperty(null, ""); } set { globallocal.SetProperty(null, value); } }
        public static bool show_getting_started { get { return globallocal.GetProperty(null, true); } set { globallocal.SetProperty(null, value); } }
        public static string getting_started_url { get { return globallocal.GetProperty(null, ""); } set { globallocal.SetProperty(null, value); } }
        public static bool notify_on_workflow_remote_start { get { return globallocal.GetProperty(null, true); } set { globallocal.SetProperty(null, value); } }
        public static bool notify_on_workflow_end { get { return globallocal.GetProperty(null, true); } set { globallocal.SetProperty(null, value); } }
        public static bool notify_on_workflow_remote_end { get { return globallocal.GetProperty(null, false); } set { globallocal.SetProperty(null, value); } }

    }
}
