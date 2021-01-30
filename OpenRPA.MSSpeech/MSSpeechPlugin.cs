using OpenRPA.Interfaces;
using OpenRPA.Interfaces.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenRPA.MSSpeech
{
    public class MSSpeechPlugin : IDetectorPlugin, IDisposable
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        public IDetector Entity { get; set; }
        public string Name
        {
            get
            {
                if (Entity != null && !string.IsNullOrEmpty(Entity.name)) return Entity.name;
                return "MSSpeech";
            }
        }
        private Views.MSSpeechView view;
        public UserControl editor
        {
            get
            {
                if (view == null)
                {
                    view = new Views.MSSpeechView(this);
                    view.PropertyChanged += (s, e) =>
                    {
                        NotifyPropertyChanged("Entity");
                        NotifyPropertyChanged("Name");
                    };
                }
                return view;
            }
        }
        public event DetectorDelegate OnDetector;
        System.Speech.Recognition.SpeechRecognitionEngine recEngine = null;
        private bool disposedValue;

        public void Initialize(IOpenRPAClient client, IDetector InEntity)
        {
            Entity = InEntity;
            Start();
        }
        public string Commands
        {
            get
            {
                if (Entity == null) return null;
                if (!Entity.Properties.ContainsKey("Commands")) return null;
                var _val = Entity.Properties["Commands"];
                if (_val == null) return null;
                return _val.ToString();
            }
            set
            {
                if (Entity == null) return;
                Entity.Properties["Commands"] = value;
            }
        }
        public bool IncludeCommonWords
        {
            get
            {
                if (Entity == null) return false;
                if (!Entity.Properties.ContainsKey("IncludeCommonWords")) return false;
                var _val = Entity.Properties["IncludeCommonWords"];
                if (_val == null) return false;
                return bool.Parse(_val.ToString());
            }
            set
            {
                if (Entity == null) return;
                Entity.Properties["IncludeCommonWords"] = value;
            }
        }
        public void Start()
        {
            var _commands = Commands;
            //if (IncludeCommonWords)
            //{
            //    if (string.IsNullOrEmpty(_commands)) _commands = Extensions.GetStringFromResource(typeof(MSSpeechPlugin), "commands.txt");
            //    if (!string.IsNullOrEmpty(_commands)) _commands += "\n" + Extensions.GetStringFromResource(typeof(MSSpeechPlugin), "commands.txt");
            //}
            if (!string.IsNullOrEmpty(_commands) || IncludeCommonWords)
            {
                var gBuilder = new System.Speech.Recognition.GrammarBuilder();
                if (IncludeCommonWords)
                {
                    gBuilder.AppendDictation();
                }
                if (!string.IsNullOrEmpty(_commands))
                {
                    var commands = new System.Speech.Recognition.Choices();
                    var array = _commands.Split(new char[] { '\n', '\r' }).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    commands.Add(array);
                    gBuilder.Append(commands);
                }
                var grammer = new System.Speech.Recognition.Grammar(gBuilder);
                Task.Run(() => {
                    if (recEngine == null)
                    {
                        recEngine = new System.Speech.Recognition.SpeechRecognitionEngine();
                        recEngine.SetInputToDefaultAudioDevice();
                        recEngine.SpeechRecognized += SpeechRecognized;
                    }
                    recEngine.UnloadAllGrammars();
                    recEngine.LoadGrammarAsync(grammer);
                    recEngine.RecognizeAsync(System.Speech.Recognition.RecognizeMode.Multiple);
                });
            }
        }
        public void Stop()
        {
            Task.Run(() => {
                if(recEngine != null)
                {
                    recEngine.SpeechRecognized -= SpeechRecognized;
                    recEngine.RecognizeAsyncStop();
                    recEngine.Dispose();
                    recEngine = null;
                }
            });
        }
        public void Initialize(IOpenRPAClient client)
        {
        }
        private void SpeechRecognized(object sender, System.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            var _e = new SpeachEvent(e.Result.Text);
            OnDetector?.Invoke(this, _e, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                Stop();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MSSpeechPlugin()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
    public class SpeachEvent : IDetectorEvent
    {
        public ITokenUser user { get; set; }
        public IElement element { get; set; }
        public string host { get; set; }
        public string fqdn { get; set; }
        public string result { get; set; }
        public SpeachEvent(string text)
        {
            host = Environment.MachineName.ToLower();
            fqdn = System.Net.Dns.GetHostEntry(Environment.MachineName).HostName.ToLower();
            this.result = text;
        }

    }
}
