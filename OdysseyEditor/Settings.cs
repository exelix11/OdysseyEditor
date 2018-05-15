using ModelViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyEditor
{
    public partial class Settings : Form
    {
        RendererControl render;

        public Settings(RendererControl _MainFormrender)
        {
            InitializeComponent();
            render = _MainFormrender;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            SettingsPanel.Visible = true;
            CamInertiaUpDown.Value = (decimal)render.CameraInertiaFactor;
            ChbFps.Checked = render.ShowFps;
            ChbTriCount.Checked = render.ShowTriangleCount;
            ChbDebugInfo.Checked = render.ShowDebugInfo;
            cbCameraMode.SelectedIndex = render.CamMode == HelixToolkit.Wpf.CameraMode.Inspect ? 0 : 1;
            ZoomSenUpDown.Value = (decimal)render.ZoomSensitivity;
            RotSenUpDown.Value = (decimal)render.RotationSensitivity;
            ChbStartupUpdate.Checked = Properties.Settings.Default.CheckUpdates;
            ChbStartupDb.Checked = Properties.Settings.Default.DownloadDb;
            tbUrl.Text = Properties.Settings.Default.DownloadDbLink;
            SettingsPanel.Focus();
        }

        private void Form_closing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.CameraInertia = (double)CamInertiaUpDown.Value;
            render.CameraInertiaFactor = (double)CamInertiaUpDown.Value;
            Properties.Settings.Default.ShowFps = ChbFps.Checked;
            render.ShowFps = ChbFps.Checked;
            Properties.Settings.Default.ShowTriCount = ChbTriCount.Checked;
            render.ShowTriangleCount = ChbTriCount.Checked;
            Properties.Settings.Default.ShowDbgInfo = ChbDebugInfo.Checked;
            render.ShowDebugInfo = ChbDebugInfo.Checked;
            Properties.Settings.Default.CameraMode = cbCameraMode.SelectedIndex;
            render.CamMode = cbCameraMode.SelectedIndex == 0 ? HelixToolkit.Wpf.CameraMode.Inspect : HelixToolkit.Wpf.CameraMode.WalkAround;
            Properties.Settings.Default.ZoomSen = (double)ZoomSenUpDown.Value;
            render.ZoomSensitivity = (double)ZoomSenUpDown.Value;
            Properties.Settings.Default.RotSen = (double)RotSenUpDown.Value;
            render.RotationSensitivity = (double)RotSenUpDown.Value;
            Properties.Settings.Default.CheckUpdates = ChbStartupUpdate.Checked;
            Properties.Settings.Default.DownloadDb = ChbStartupDb.Checked;
            Properties.Settings.Default.DownloadDbLink = tbUrl.Text;
            Properties.Settings.Default.Save();
        }
    }
}
