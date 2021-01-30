﻿using OpenRPA.Core;
using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenRPA.Interfaces;

namespace OpenRPA.Utilities
{
    public partial class WriteExcelDesigner
    {
        public WriteExcelDesigner()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.Filter = "OpenXml Excel files (2007 format)|*.xlsx|Binary Excel files (2.0-2003 format)|*.xls|Comma-separated values (csv format)|*.csv|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            ModelItem.Properties["Filename"].SetValue(
                new System.Activities.InArgument<string>()
                {
                    Expression = new Microsoft.VisualBasic.Activities.VisualBasicValue<string>("\"" + saveFileDialog1.FileName.ReplaceEnvironmentVariable() + "\"")
                });
        }

    }
}