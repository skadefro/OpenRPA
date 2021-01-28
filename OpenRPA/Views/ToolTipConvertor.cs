using System;
using System.Activities.Presentation.Toolbox;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using OpenRPA.Core;
using OpenRPA.Interfaces;

namespace OpenRPA.Views
{
    [ValueConversion(typeof(ToolboxItemWrapper), typeof(string))]
    public class ToolTipConvertor : IValueConverter
    {
        public static Dictionary<ToolboxItemWrapper, string> ToolTipDic = new Dictionary<ToolboxItemWrapper, string>();
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                ToolboxItemWrapper itemWrapper = (ToolboxItemWrapper)value;
                if(itemWrapper.Type != null)
                {
                    var attr = Core.Extensions.GetMyCustomAttributes<ToolboxTooltipAttribute>(itemWrapper.Type, false).FirstOrDefault();
                    if (attr != null)
                    {
                        return attr.Text;
                    }
                    if (ToolTipDic.ContainsKey(itemWrapper))
                    {
                        return ToolTipDic[itemWrapper];
                    }
                    else
                        return null;
                } else
                {
                    Console.WriteLine(itemWrapper.DisplayName + " has no type!");
                }
            }
            catch (Exception)
            {
            }
            return "Unknown";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
