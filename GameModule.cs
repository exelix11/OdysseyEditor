using BfresLib;
using ByamlExt;
using EditorCore;
using EditorCore.EditorFroms;
using EditorCore.Interfaces;
using EveryFileExplorer;
using SARCExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace OdysseyExt
{
	public class OdysseyModule : IGameModule, IEditingOptionsModule
	{
		public string ModuleName => "Odyssey level";

		public Tuple<Type, Type>[] GetClassConverters { get; } =
		new Tuple<Type, Type>[] {
			new Tuple<Type, Type>(typeof(LinksNode), typeof(LinksConveter))
		};

		public string[] ReservedPropNames => LevelObj.CantRemoveNames;
		public string[] ModelFieldPropNames => LevelObj.ModelFieldNames;

		public bool IsAddListSupported => true;
		public bool IsPropertyEditingSupported => true;
		public string[] AutoHideList => new string[] { "AreaList", "SkyList" };

		public IEditorFormContext ViewForm { get; set; } = null;
		public string GameFolder => ViewForm.GameFolder;

		public string ModelsFolder => "OdysseyModels";

		public bool ConvertModelFile(string ObjName, string path) => BfresConverter.Convert(BfresFromSzs(ObjName), path);

		public string GetPlaceholderModel(string name, string listName)
		{
			string PlaceholderModel = "UnkBlue.obj";
			if (listName == "AreaList") PlaceholderModel = "UnkYellow.obj";
			else if (listName == "DebugList") PlaceholderModel = "UnkRed.obj";
			else if (listName == "CameraAreaInfo") PlaceholderModel = "UnkGreen.obj";

			if (name == "PointRailCollision")
				PlaceholderModel = "UnkRed.obj";
			return PlaceholderModel;
		}

		OdysseyMenuExt MenuExt;
		bool UseKclCollisions = false;
		public void InitModule(IEditorFormContext currentView)
		{
			ViewForm = currentView;
			MenuExt = new OdysseyMenuExt();
			ToggleKclCollisions(false);
			MenuExt.KCLCollisions.Click += delegate (object o, EventArgs a) { ToggleKclCollisions(); };
			MenuExt.KCLConverter.Click += delegate (object o, EventArgs a) { KclExport.ExportCollisionFromObject(GameFolder + "\\ObjectData"); };
			MenuExt.FileMenuExtensions[0].Click += SelectStage;
			ViewForm.RegisterMenuExtension(MenuExt);
		}

		void SelectStage(object o, EventArgs a)
		{
			var f = new StageSelectionForm();
			f.ShowDialog();
			if (f.Result == null) return;
			if (File.Exists($"{GameFolder}StageData\\{f.Result}Map.szs")) 
				ViewForm.LoadLevel($"{GameFolder}StageData\\{f.Result}Map.szs"); //Also it's possible to load a specific scenario with ViewForm.LoadLevel(new Level(f.result, id))
		}

        void ToggleKclCollisions(bool? val = null)
		{
			UseKclCollisions = val.HasValue ? val.Value : !UseKclCollisions;
			MenuExt.KCLCollisions.Text = $"Use collisions as stage models : {(UseKclCollisions ? "ON" : "OFF")}";
		}

		public void ParseArgs(string[] Args)
		{
			foreach (string file in Args)
			{
				if (File.Exists(file))
				{
					if (file.EndsWith("byml") || file.EndsWith("byaml"))
					{
						ByamlViewer.OpenByml(file);
					}
					else if (file.EndsWith(".szs"))
					{
						ViewForm.LoadLevel(file);
						return;
					}
				}
			}
		}

		string LevelFormatFilter => "szs file | *.szs";
		public ILevel LoadLevel(string file = null)
		{
			if (file == null)
			{
				var opn = new OpenFileDialog()
				{
					Filter = LevelFormatFilter,
					Title = "Select a level",
					//FileName = "RailCollisionExStageMap.szs"
				};
				if (opn.ShowDialog() != DialogResult.OK)
					return null;
				file = opn.FileName;
			}
#if DEBUG
			return new Level(file, 0);
#else
			return new Level(file, -1);
#endif
		}

		public ILevel NewLevel(string file = null)
		{
			if (file == null)
			{
				var sav = new SaveFileDialog()
				{
					Filter = LevelFormatFilter
				};
				if (sav.ShowDialog() != DialogResult.OK)
					return null;
				file = sav.FileName;
			}
			return new Level(true, file);
		}

		public void SaveLevel(ILevel level) => File.WriteAllBytes(level.FilePath, ((Level)level).SaveSzs());
		public void SaveLevelAs(ILevel level)
		{
			var sav = new SaveFileDialog() { Filter = LevelFormatFilter, FileName = level.FilePath };
			if (sav.ShowDialog() != DialogResult.OK)
				return;
			File.WriteAllBytes(sav.FileName, ((Level)level).SaveSzs(sav.FileName));
		}

		public IObjList CreateObjList(string name, IList<dynamic> baseList) =>
			 new ObjList(name, baseList);

		public ILevelObj NewObject() => new LevelObj(); //TODO : show a dialog

		public bool OpenLevelFile(string name, Stream file) => false;

		public string AddObjList(ILevel level)
		{
			var f = new EditorFroms.AddObjList();
			f.ShowDialog();
			if (f.Result == null || f.Result.Trim() == "") return null;
			level.objs.Add(f.Result, new ObjList(f.Result, null));
			return f.Result;
		}

		public void EditChildrenNode(ILevelObj obj)
		{
			if (obj is Rail)
				ViewForm.EditPath((IPathObj)obj);
			else if (obj[LevelObj.N_Links] != null)
			{
				var BakLinks = ((LinksNode)obj[LevelObj.N_Links]).Clone();

				ViewForm.AddToUndo((dynamic arg) =>
				{
					((ILevelObj)arg[0])[LevelObj.N_Links] = arg[1];
				},
					$"Edited links of {obj.ToString()}",
					new dynamic[] { obj, BakLinks });

				new EditorFroms.LinksEditor(obj[LevelObj.N_Links], ViewForm).ShowDialog();
			}
		}

		//This is static so it can be used by multiple instances
		public static ObjectDatabase ObjsDB = null;

		public void FormLoaded()
		{
			if (!Directory.Exists(ModelsFolder))
			{
				Directory.CreateDirectory(ModelsFolder);
				ZipArchive z = new ZipArchive(new MemoryStream(Properties.Resources.baseModels));
				z.ExtractToDirectory(ModelsFolder);				
			}
			if (!Directory.Exists($"{ModelsFolder}/GameTextures"))
			{
				if (GameFolder == "" || !Directory.Exists(GameFolder))
					MessageBox.Show("The game path is not set or not valid, can't extract texture archives");
				else
				{
					MessageBox.Show($"The game texture archives will be extracted in {ModelsFolder}/GameTextures, this might take a while");
					LoadingForm.ShowLoading(ViewForm as Form, "Extracting textures...\r\nThis might take a while");
					foreach (var a in Directory.GetFiles($"{GameFolder}ObjectData\\").Where(x => x.EndsWith("Texture.szs")))
					{
						BfresLib.BfresConverter.GetTextures(
							BfresFromSzs(Path.GetFileNameWithoutExtension(a)),
							ModelsFolder);
					}
					LoadingForm.EndLoading();
				}
			}

			if (ObjsDB == null)
			{
				if (!File.Exists(ModelsFolder + "/OdysseyDB.json"))
					ObjsDB = ObjectDatabase.Deserialize(File.ReadAllText(ModelsFolder + "/OdysseyDB.json"));
				else
					ObjsDB = ObjectDatabase.Deserialize(Properties.Resources.OdysseyDB);
			}
		}

		byte[] BfresFromSzs(string fileName)
		{
			if (File.Exists($"{GameFolder}ObjectData\\{fileName}.szs"))
			{
				var SzsFiles = SARC.UnpackRam(YAZ0.Decompress($"{GameFolder}ObjectData\\{fileName}.szs"));
				if (SzsFiles.ContainsKey(fileName + ".bfres"))
				{
					return SzsFiles[fileName + ".bfres"];
				}
			}
			return null;
		}

		public Tuple<string, dynamic> GetNewProperty(dynamic target) => AddBymlPropertyDialog.newProperty(target is IDictionary<string, dynamic>);


		#region IEditingOptionsModule

		ContextMenuStrip optionsMenu;

		IDictionary<string, dynamic> editableLinks;

		void IEditingOptionsModule.InitOptionsMenu(ref ContextMenuStrip baseMenu)
		{
			optionsMenu = baseMenu;
			optionsMenu.Items["ObjectRightClickMenu_EditChildren"].Text = "Edit Links";
		}

		ContextMenuStrip IEditingOptionsModule.GetOptionsMenu(ILevelObj clickedObj)
		{
			if (clickedObj != null&&!(clickedObj is IPathObj))
			{
				ToolStripMenuItem linksMenu = optionsMenu.Items["ObjectRightClickMenu_EditChildren"] as ToolStripMenuItem;
				editableLinks = clickedObj[LevelObj.N_Links];
				linksMenu.DropDownItems.Clear();
				foreach (string k in editableLinks.Keys)
				{
					var item = linksMenu.DropDownItems.Add(k);
					item.Text = k;
					item.Click += LinkMenuItem_Click;
				}
			}
			return optionsMenu;
		}

		private void LinkMenuItem_Click(object sender, EventArgs e)
		{
			ViewForm.EditList(editableLinks[(sender as ToolStripItem).Text]);
		}

		void IEditingOptionsModule.InitActionButtons(ref ToolStrip baseButtonStrip)
		{
			
		}

		ToolStrip IEditingOptionsModule.GetActionButtons(IObjList activeObjList, ILevelObj selectedObj)
		{
			return null;
		}
		#endregion
	}

	public class LinksConveter : System.ComponentModel.TypeConverter
	{
		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
		{
			return false;
		}

		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			return "<Links>";
		}
	}

	class OdysseyMenuExt : IMenuExtension
	{
		public ToolStripMenuItem[] FileMenuExtensions { get; internal set; }
		public ToolStripMenuItem[] ToolsMenuExtensions => null;
		public ToolStripMenuItem[] TitleBarExtensions { get; internal set; }

		internal ToolStripItem KCLCollisions;
		internal ToolStripItem KCLConverter;

		internal OdysseyMenuExt()
		{
			FileMenuExtensions = new ToolStripMenuItem[]
				{
					new ToolStripMenuItem("Select stage")
				};
			TitleBarExtensions = new ToolStripMenuItem[]
				{
					new ToolStripMenuItem("Odyssey")
				};

			KCLCollisions = TitleBarExtensions[0].DropDownItems.Add("");
			KCLConverter = TitleBarExtensions[0].DropDownItems.Add("Export collisions from object");

#if DEBUG
			var a  = TitleBarExtensions[0].DropDownItems.Add("CreateObjDb");
			a.Click += delegate (object o, EventArgs e)
			{
				string s = new DebugStuff.ObjectDatabaseGenerator().Generate(Directory.GetFiles(@"D:\E\Desktop\HAX\Odyssey\StageData", "*Map.szs")).Serialize();
				File.WriteAllText("db.json", s);
			};

			var b = TitleBarExtensions[0].DropDownItems.Add("TestObjDB");
			b.Click += delegate (object o, EventArgs e)
			{
				ObjectDatabase obj = ObjectDatabase.Deserialize(File.ReadAllText("db.json"));
				foreach (var k in obj.Keys)
				{
					var gameObj = obj.MakeObject(k);
				}
				MessageBox.Show("All objs have been instantiated");
			};			
#endif
		}
	}
}
