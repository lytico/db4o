using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TasksSample
{
    public partial class NewTaskDialog : Form
    {
        public NewTaskDialog()
        {
            InitializeComponent();
        }

        public String TaskLabel
        {
            get { return txtLabel.Text; }
        }

        public String TaskDescription
        {
            get { return txtDescription.Text; }
        }	
    }
}