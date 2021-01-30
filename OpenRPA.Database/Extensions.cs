using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRPA.Database
{
    public static class Extensions
    {
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
    }
}
