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

namespace OpenRPA.AviRecorder
{
    public class CustomSelectEditor : PropertyValueEditor
    {
        public CustomSelectEditor()
        {
            InlineEditorTemplate = new DataTemplate();
            FrameworkElementFactory combo = new FrameworkElementFactory(typeof(ComboBox));
            Binding b = new Binding("Value");
            b.Mode = BindingMode.TwoWay;
            b.Converter = new StateConverter();
            combo.SetValue(ComboBox.SelectedValueProperty, b);
            Binding items = new Binding("options");
            items.Source = this;
            combo.SetValue(ComboBox.ItemsSourceProperty, items);
            combo.SetValue(ComboBox.SelectedValuePathProperty, "ID");
            DataTemplate itemTemplate = new DataTemplate();
            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            Binding text = new Binding();
            text.Converter = new DataTableConverter(); textBlock.SetValue(TextBlock.TextProperty, text);
            itemTemplate.VisualTree = textBlock; combo.SetValue(ComboBox.ItemTemplateProperty, itemTemplate);
            this.InlineEditorTemplate.VisualTree = combo;
        }
        public virtual DataTable options
        {
            get
            {
                DataTable lst = new DataTable();
                lst.Columns.Add("ID", typeof(string));
                lst.Columns.Add("TEXT", typeof(string));
                lst.Rows.Add("1", "field 1");
                lst.Rows.Add("2", "field 2");
                lst.Rows.Add("3", "field 3");
                lst.Rows.Add("4", "field 4");
                return lst;
            }
        }
    }
}
