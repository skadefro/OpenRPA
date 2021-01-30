using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Interfaces
{
    public class Log
    {
        public static ILogger Logger { get; set; }
        public static void FunctionIndent(string cls, string func, string message = "") => Logger.FunctionIndent(cls, func, message);
        public static void FunctionOutdent(string cls, string func, string message = "") => Logger.FunctionOutdent(cls, func, message);
        public static void Function(string cls, string func, string message = "") => Logger.Function(cls, func, message);
        public static void ResetLogPath(string folder) => Logger.ResetLogPath(folder);
        public static void LogLine(string message, string category) => Logger.LogLine(message, category);
        public static void Verbose(string message) => Logger.Verbose(message);
        public static void Network(string message) => Logger.Network(message);
        public static void Activity(string message) => Logger.Activity(message);
        public static void Debug(string message) => Logger.Debug(message);
        public static void Selector(string message) => Logger.Selector(message);
        public static void SelectorVerbose(string message) => Logger.SelectorVerbose(message);
        public static void Information(string message) => Logger.Information(message);
        public static void Output(string message) => Logger.Output(message);
        public static void Warning(string message) => Logger.Warning(message);
        public static void Error(object obj, string message) => Logger.Error(obj, message);
        public static void Error(string message) => Logger.Error(message);
    }
    public interface ILogger
    {
        void FunctionIndent(string cls, string func, string message = "");
        void FunctionOutdent(string cls, string func, string message = "");
        void Function(string cls, string func, string message = "");
        void ResetLogPath(string folder);
        void LogLine(string message, string category);
        void Verbose(string message);
        void Network(string message);
        void Activity(string message);
        void Debug(string message);
        void Selector(string message);
        void SelectorVerbose(string message);
        void Information(string message);
        void Output(string message);
        void Warning(string message);
        void Error(object obj, string message);
        void Error(string message);
    }
}
