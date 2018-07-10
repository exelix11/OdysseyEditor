using EditorCore;
using EditorCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyExt
{
	class OdysseyExt : ExtensionManifest
	{
		public string ModuleName => "OdysseyEditor";
		public string Author => "Exelix11";
		public string ThanksTo => "KillzXGaming for the C# BFRES loader\r\ngdkchan for Bn" +
								  "Txx\r\nEveryone from masterf0x/RedCarpet";

		public Version TargetVersion => new Version(1, 0, 0, 0);
		
		public IMenuExtension MenuExt => null;

		public IClipboardExtension ClipboardExt => null;

		public bool HasGameModule => true;
		OdysseyModule _module = null;
		public IGameModule GameModule
		{
			get
			{
				if (_module == null) _module = new OdysseyModule();
				return _module;
			}
		}

		public IFileHander[] Handlers => null;

		public void CheckForUpdates()
		{
			return;
			//var res = await GitHubUpdateCheck.CheckForUpdates("Exelix11", "OdysseyEditor");
		}
	}
}
