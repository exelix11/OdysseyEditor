using EveryFileExplorer;
using KCLExt;
using SARCExt;
using Syroot.NintenTools.Byaml.Dynamic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdysseyExt
{
	class KclExport
	{
		public static void ExportCollisionFromObject(string ObjectDataFolder)
		{
			var opn = new OpenFileDialog()
			{
				InitialDirectory = Directory.Exists(ObjectDataFolder) ? ObjectDataFolder : null,
				Filter = "szs file | *.szs",
				Title = "Select an Object which has a collision model"
			};

			if (opn.ShowDialog() != DialogResult.OK)
				return;
			var szs = SARC.UnpackRam(YAZ0.Decompress(opn.FileName));

			foreach (string name in szs.Keys)
			{
				if (name.EndsWith(".kcl"))
				{
					List<Color> typeColors = null;

					string attributeFileName = Path.GetFileNameWithoutExtension(name) + "Attribute.byml";
					if (szs.ContainsKey(attributeFileName))
						typeColors = GetKCLColors(szs[attributeFileName]);

					var sav = new SaveFileDialog()
					{
						FileName = name + ".obj",
						Filter = "obj file|*.obj"
					};
					if (sav.ShowDialog() != DialogResult.OK)
						return;

					var mod = new MarioKart.MK7.KCL(szs[name]).ToOBJ().toWritableObj();

					if (typeColors != null)
						for (int i = 0; i < mod.Materials.Count; i++)
						{
							if (i >= typeColors.Count) break;
							mod.Materials[i].Colors.normal.X = typeColors[i].R / 255f;
							mod.Materials[i].Colors.normal.Y = typeColors[i].G / 255f;
							mod.Materials[i].Colors.normal.Z = typeColors[i].B / 255f;
						}

					mod.WriteObj(sav.FileName);
				}
			}
		}

		static List<Color> GetKCLColors(byte[] byml)
		{
			var attributeFile = ByamlFile.FastLoad(new MemoryStream(byml));

			var typeColors = new List<Color>();
			foreach (dynamic attrib in attributeFile)
			{
				Console.WriteLine(attrib["FloorCode"]);
				switch (attrib["FloorCode"])
				{
					case "Ground":
						typeColors.Add(Color.FromArgb(255, 200, 200, 200)); break;
					case "DamageFire":
					case "DamageFire2D":
						typeColors.Add(Color.FromArgb(255, 200, 50, 0)); break;
					case "Poison":
					case "Poison2D":
						typeColors.Add(Color.FromArgb(255, 255, 0, 200)); break;
					case "SandSink":
						typeColors.Add(Color.FromArgb(255, 10, 30, 0)); break;
					case "Skate":
						typeColors.Add(Color.FromArgb(255, 0, 220, 255)); break;
					default:
						typeColors.Add(Color.FromArgb(255, 255, 255, 255)); break;
				}
			}
			return typeColors;
		}
	}
}
