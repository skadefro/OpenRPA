using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace OpenRPA.Forms
{
    public class StateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                Activity<string> expression = (value as InArgument<string>).Expression;
                VisualBasicValue<string> vbexpression = expression as VisualBasicValue<string>;
                Literal<string> literal = expression as Literal<string>;
                if (literal != null) { return literal.Value.ToString(); }
                else if (vbexpression != null) { return vbexpression.ExpressionText; }
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            InArgument<string> inArgument = value.ToString();
            // Convert combo box value to InArgument<string>
            return inArgument;
        }
    }
}
