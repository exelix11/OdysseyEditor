using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyEditor
{
    class UpdateCheck
    {
        public const int ReleaseID = 2;

        public static async Task CheckForUpdates()
        {
            var githubClient = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("OdysseyEditor"));
            var ver = await githubClient.Repository.Release.GetAll("exelix11", "OdysseyEditor");
            if (ver.Count > ReleaseID)
            {                
                if (MessageBox.Show($"There is a new version of the editor, do you want to open the github page ?\r\n\r\nDetails:\r\n{ver[0].Body}", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    System.Diagnostics.Process.Start("https://github.com/exelix11/OdysseyEditor/releases");
            }
        }

        public static void CheckForUpdatesAsync()
        {
            Task.Run(async () => await CheckForUpdates());
        }
    }
}
