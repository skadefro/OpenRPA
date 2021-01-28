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
    public static class OpenRPAServiceUtil
    {
        public static string ApplicationIdentifier(string uniqueName, bool ChildSession)
        {
            if (ChildSession)
            {
                uint SessionId = win32.ChildSession.GetChildSessionId();
                return uniqueName + Environment.UserName + SessionId;
            }
            else
            {
                var p = System.Diagnostics.Process.GetCurrentProcess();
                return uniqueName + Environment.UserName + p.SessionId;
            }
        }
        private const string Delimiter = ":";
        private const string ChannelNameSuffix = "OpenRPAIPCChannel";
        private const string RemoteServiceName = "OpenRPAService";
        private const string IpcProtocol = "ipc://";
        private static IpcServerChannel channel;
        private static System.Threading.Mutex OpenRPAMutex;
        /// <summary>
        /// Cleans up single-instance code, clearing shared resources, mutexes, etc.
        /// </summary>
        public static void Cleanup()
        {
            if (OpenRPAMutex != null)
            {
                OpenRPAMutex.Close();
                OpenRPAMutex = null;
            }

            if (channel != null)
            {
                ChannelServices.UnregisterChannel(channel);
                channel = null;
            }
        }
        private static string GetUsersGroupName()
        {
            const string builtInUsersGroup = "S-1-5-32-545";
            var sid = new System.Security.Principal.SecurityIdentifier(builtInUsersGroup);
            var ntAccount = (System.Security.Principal.NTAccount)sid.Translate(typeof(System.Security.Principal.NTAccount));
            return ntAccount.Value;
        }
        private static void CreateRemoteService(string channelName)
        {
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Dictionary<string, string>();

            props["authorizedGroup"] = GetUsersGroupName();
            props["name"] = channelName;
            props["portName"] = channelName;
            props["exclusiveAddressUse"] = "false";

            // Create the IPC Server channel with the channel properties
            channel = new IpcServerChannel(props, serverProvider);

            // Register the channel with the channel services
            ChannelServices.RegisterChannel(channel, true);

            // Expose the remote service with the REMOTE_SERVICE_NAME
            RemoteInstance = new OpenRPAService();
            RemotingServices.Marshal(RemoteInstance, RemoteServiceName);
            RemoteInstance.Ping();
        }
        /// <summary>
        /// Checks if the instance of the application attempting to start is the first instance. 
        /// If not, activates the first instance.
        /// </summary>
        /// <returns>True if this is the first instance of the application.</returns>
        public static bool InitializeService(string uniqueName = "OpenRPAService")
        {
            // Build unique application Id and the IPC channel name.
            string channelName = String.Concat(ApplicationIdentifier(uniqueName, false), Delimiter, ChannelNameSuffix);
            // Create mutex based on unique application Id to check if this is the first instance of the application. 
            bool firstInstance;
            OpenRPAMutex = new Mutex(true, ApplicationIdentifier(uniqueName, false), out firstInstance);
            if (firstInstance)
            {
                CreateRemoteService(channelName);
            }
            return firstInstance;
        }
        public static OpenRPAService RemoteInstance;
        public static bool _ChildSession = false;
        public static IpcClientChannel secondInstanceChannel;
        public static bool GetInstance(string uniqueName = "OpenRPAService", bool ChildSession = false)
        {
            try
            {
                if (RemoteInstance != null && _ChildSession == ChildSession)
                {
                    try
                    {
                        RemoteInstance.Ping();
                        return true;
                    }
                    catch (Exception)
                    {
                    }
                }
                _ChildSession = ChildSession;
                try
                {
                    if (ChannelServices.RegisteredChannels.Length > 0 && secondInstanceChannel != null)
                    {
                        try
                        {
                            ChannelServices.UnregisterChannel(secondInstanceChannel);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    secondInstanceChannel = new IpcClientChannel();
                    ChannelServices.RegisterChannel(secondInstanceChannel, true);
                }
                catch (Exception ex)
                {
                    Log.Debug(ex.ToString());
                }
                string channelName = String.Concat(ApplicationIdentifier(uniqueName, ChildSession), Delimiter, ChannelNameSuffix);
                string remotingServiceUrl = IpcProtocol + channelName + "/" + RemoteServiceName;
                // Obtain a reference to the remoting service exposed by the server i.e the first instance of the application
                RemoteInstance = (OpenRPAService)RemotingServices.Connect(typeof(OpenRPAService), remotingServiceUrl);
                RemoteInstance.Ping();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
