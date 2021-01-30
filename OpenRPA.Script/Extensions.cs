using OpenRPA.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Script
{
    public static class Extensions
    {
        public static string ReplaceEnvironmentVariable(this string filename)
        {
            var USERPROFILE = Environment.GetEnvironmentVariable("USERPROFILE");
            var windir = Environment.GetEnvironmentVariable("windir");
            var SystemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            var PUBLIC = Environment.GetEnvironmentVariable("PUBLIC");

            if (!string.IsNullOrEmpty(USERPROFILE)) filename = filename.Replace(USERPROFILE, "%USERPROFILE%");
            if (!string.IsNullOrEmpty(windir)) filename = filename.Replace(windir, "%windir%");
            if (!string.IsNullOrEmpty(SystemRoot)) filename = filename.Replace(SystemRoot, "%SystemRoot%");
            if (!string.IsNullOrEmpty(PUBLIC)) filename = filename.Replace(PUBLIC, "%PUBLIC%");

            var ProgramData = Environment.GetEnvironmentVariable("ProgramData");
            if (!string.IsNullOrEmpty(ProgramData)) filename = filename.Replace(ProgramData, "%ProgramData%");
            var ProgramFilesx86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            if (!string.IsNullOrEmpty(ProgramFilesx86)) filename = filename.Replace(ProgramFilesx86, "%ProgramFiles(x86)%");
            var ProgramFiles = Environment.GetEnvironmentVariable("ProgramFiles");
            if (!string.IsNullOrEmpty(ProgramFiles)) filename = filename.Replace(ProgramFiles, "%ProgramFiles%");
            var LOCALAPPDATA = Environment.GetEnvironmentVariable("LOCALAPPDATA");
            if (!string.IsNullOrEmpty(LOCALAPPDATA)) filename = filename.Replace(LOCALAPPDATA, "%LOCALAPPDATA%");
            var APPDATA = Environment.GetEnvironmentVariable("APPDATA");
            if (!string.IsNullOrEmpty(APPDATA)) filename = filename.Replace(APPDATA, "%APPDATA%");


            //var = Environment.GetEnvironmentVariable("");
            //if (!string.IsNullOrEmpty()) filename = filename.Replace(, "%%");

            return filename;
        }
        static public string ResourceAsString(this Type type, string resourceName)
        {
            // string[] names = typeof(Extensions).Assembly.GetManifestResourceNames();
            string[] names = type.Assembly.GetManifestResourceNames();
            foreach (var name in names)
            {
                if (name.EndsWith(resourceName))
                {
                    using (var s = type.Assembly.GetManifestResourceStream(name))
                    {
                        using (var reader = new System.IO.StreamReader(s))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    try
                    {
                        var set = new System.Resources.ResourceSet(type.Assembly.GetManifestResourceStream(names[0]));
                        foreach (System.Collections.DictionaryEntry resource in set)
                        {
                            // Log.Information("\n[{0}] \t{1}", resource.Key, resource.Value);
                            if (((string)resource.Key).EndsWith(resourceName.ToLower()))
                            {
                                using (var reader = new System.IO.StreamReader(resource.Value as System.IO.Stream))
                                {
                                    return reader.ReadToEnd();
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex.ToString());
                    }
                }
            }
            return null;
        }
        static public string ResourceAsString(string resourceName)
        {
            return ResourceAsString(typeof(Extensions), resourceName);
        }
        public static void AddCacheArgument(System.Activities.NativeActivityMetadata metadata, string name, System.Activities.Argument argument)
        {
            try
            {
                if (argument == null) return;
                Type ttype = argument.GetType().GetGenericArguments()[0];
                System.Activities.ArgumentDirection direction = System.Activities.ArgumentDirection.In;
                if (argument is System.Activities.InArgument) direction = System.Activities.ArgumentDirection.In;
                if (argument is System.Activities.InOutArgument) direction = System.Activities.ArgumentDirection.InOut;
                if (argument is System.Activities.OutArgument) direction = System.Activities.ArgumentDirection.Out;
                var ra = new System.Activities.RuntimeArgument(name, ttype, direction);
                metadata.Bind(argument, ra);
                metadata.AddArgument(ra);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool TryCast<T>(this object obj, out T result)
        {
            if (obj is T)
            {
                result = (T)obj;
                return true;
            }
            if (obj is System.Activities.Expressions.Literal<T>)
            {
                result = ((System.Activities.Expressions.Literal<T>)obj).Value;
                return true;
            }

            result = default;
            return false;
        }
        public static T TryCast<T>(this object obj)
        {
            if (TryCast(obj, out T result))
                return result;
            return result;
        }
        public static T GetValue<T>(this System.Activities.Presentation.Model.ModelItem model, string name)
        {
            T result = default;
            if (model.Properties[name] != null)
            {
                if (model.Properties[name].Value == null) return result;
                if (model.Properties[name].Value.Properties["Expression"] != null)
                {
                    result = model.Properties[name].Value.Properties["Expression"].ComputedValue.TryCast<T>();
                    return result;
                }
                result = model.Properties[name].ComputedValue.TryCast<T>();
                return result;
            }
            return result;
        }
        public static void SetValue<T>(this System.Activities.Presentation.Model.ModelItem model, string name, T value)
        {
            if (model.Properties[name] != null)
            {
                model.Properties[name].SetValue(value);
            }
        }
        public static void SetValueInArg<T>(this System.Activities.Presentation.Model.ModelItem model, string name, T value)
        {
            model.SetValue(name, new System.Activities.InArgument<T>() { Expression = new System.Activities.Expressions.Literal<T>(value) });
        }
        public static void SetValueOutArg<T>(this System.Activities.Presentation.Model.ModelItem model, string name, string value)
        {
            model.SetValue(name, new System.Activities.OutArgument<T>() { Expression = new Microsoft.VisualBasic.Activities.VisualBasicReference<T>(value) });
            // model.SetValue(name, new System.Activities.OutArgument<T>() { Expression = new Microsoft.VisualBasic.Activities.VisualBasicValue<T>(value) });
        }
    }
}
