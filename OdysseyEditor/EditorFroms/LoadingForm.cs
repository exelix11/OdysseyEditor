using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyEditor.EditorFroms
{
    public partial class LoadingForm : Form
    {
        public static LoadingForm LoadingFormInstance;

        public static void ShowLoading(Form owner, string text = "Loading...")
        {
            if (LoadingFormInstance != null)
                throw new Exception("Unexpected loadingForm state");
            LoadingFormInstance = new LoadingForm();
            LoadingFormInstance.Show(owner);
            LoadingFormInstance.label1.Text = text;
            LoadingFormInstance.Focus();
            LoadingFormInstance.Refresh();
        }

        public static void EndLoading()
        {
            if (LoadingFormInstance == null) return;
            LoadingFormInstance.Close();
            LoadingFormInstance = null;
        }

        public LoadingForm()
        {
            InitializeComponent();
        }        
    }
}
