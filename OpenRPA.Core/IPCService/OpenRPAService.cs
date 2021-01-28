using OpenRPA.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenRPA.Core.IPCService
{
    //public interface IOpenRPAService
    //{
    //    void ParseCommandLineArgs(IList<string> args);
    //}
    public class OpenRPAService : MarshalByRefObject //, IOpenRPAService
    {
        public void ParseCommandLineArgs(IList<string> args)
        {
            global.OpenRPAClient.ParseCommandLineArgs(args);
        }
        public static Dictionary<string, RunWorkflowInstance> RunWorkflowInstances = new Dictionary<string, RunWorkflowInstance>();
        private System.Timers.Timer pendingTimer = null;
        public void StartWorkflowInstances()
        {
            if(global.OpenRPAClient== null || !global.OpenRPAClient.isReadyForAction)
            {
                if(pendingTimer == null)
                {
                    pendingTimer = new System.Timers.Timer(500);
                    pendingTimer.Elapsed += (e, r) =>
                    {
                        pendingTimer.Stop();
                        if (global.OpenRPAClient == null || !global.OpenRPAClient.isReadyForAction)
                        {
                            pendingTimer.Start();
                            return;
                        }
                        StartWorkflowInstances();
                        pendingTimer = null;
                    };
                    pendingTimer.AutoReset = false;
                    pendingTimer.Start();
                }
                return;
            }
            foreach(var _instance in RunWorkflowInstances.ToList())
            {
                if(!_instance.Value.Started)
                {
                    try
                    {
                        _instance.Value.Started = true;
                        var workflow = global.OpenRPAClient.GetWorkflowByIDOrRelativeFilename(_instance.Value.IDOrRelativeFilename);
                        IWorkflowInstance instance = null;
                        IDesigner designer = null;
                        GenericTools.RunUI(() =>
                        {
                            try
                            {
                                designer = global.OpenRPAClient.GetWorkflowDesignerByIDOrRelativeFilename(_instance.Value.IDOrRelativeFilename);
                                if (designer != null)
                                {
                                    designer.BreakpointLocations = null;
                                    // instance = workflow.CreateInstance(Arguments, null, null, designer.IdleOrComplete, designer.OnVisualTracking);
                                    instance = workflow.CreateInstance(_instance.Value.Arguments, null, null, IdleOrComplete, designer.OnVisualTracking);
                                }
                                else
                                {
                                    instance = workflow.CreateInstance(_instance.Value.Arguments, null, null, IdleOrComplete, null);
                                }
                                instance.caller = _instance.Value.UniqueId;

                            }
                            catch (Exception ex)
                            {
                                _instance.Value.Error = ex;
                                if (_instance.Value.Pending != null) _instance.Value.Pending.Set();
                            }
                            if (designer != null)
                            {
                                designer.Run(designer.VisualTracking, designer.SlowMotion, instance);
                            }
                            else
                            {
                                if (instance != null) instance.Run();
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        _instance.Value.Error = ex;
                        if (_instance.Value.Pending != null) _instance.Value.Pending.Set();
                    }

                }
            }
        }
        public Dictionary<string, object> RunWorkflowByIDOrRelativeFilename(string IDOrRelativeFilename, bool WaitForCompleted, Dictionary<string, object> Arguments)
        {
            string uniqueid = Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", "");
            if (Arguments == null) Arguments = new Dictionary<string, object>();
            var _instance = new RunWorkflowInstance(uniqueid, IDOrRelativeFilename, WaitForCompleted, Arguments);
            RunWorkflowInstances.Add(uniqueid, _instance);
            StartWorkflowInstances();
            if (WaitForCompleted) _instance.Pending.WaitOne();
            if (_instance.Error != null) throw _instance.Error;
            return _instance.Result;
        }
        public void IdleOrComplete(Interfaces.IWorkflowInstance instance, EventArgs e)
        {
            if (string.IsNullOrEmpty(instance.caller)) return;
            if(!RunWorkflowInstances.ContainsKey(instance.caller)) return;
            if (instance.isCompleted || instance.Exception != null)
            {
                var _instance = RunWorkflowInstances[instance.caller];
                if (instance.Parameters != null)
                {
                    _instance.Result = instance.Parameters;
                } else { _instance.Result = new Dictionary<string, object>(); }
                RunWorkflowInstances.Remove(instance.caller);
                if (!string.IsNullOrEmpty(instance.errormessage)) _instance.Error = new Exception(instance.errormessage);
                if (instance.Exception != null) _instance.Error = instance.Exception;
                _instance.Pending.Set();
                GenericTools.RunUI(() =>
                {
                    // if ran in designer, call IdleOrComplete to break out of debugging and make designer not readonly
                    var designer = global.OpenRPAClient.GetWorkflowDesignerByIDOrRelativeFilename(_instance.IDOrRelativeFilename);
                    try
                    {
                        if (designer != null) designer.IdleOrComplete(instance, e);
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
        public string Ping() { return "pong"; }
        /// <summary>
        /// Remoting Object's ease expires after every 5 minutes by default. We need to override the InitializeLifetimeService class
        /// to ensure that lease never expires.
        /// </summary>
        /// <returns>Always null.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
