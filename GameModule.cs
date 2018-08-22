﻿using BfresLib;
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
			string PlaceholderModel ="UnkBlue.obj";
			if (listName == "AreaList") PlaceholderModel = "UnkYellow.obj";
			else if (listName == "DebugList") PlaceholderModel = "UnkRed.obj";
			else if (listName == "CameraAreaInfo") PlaceholderModel = "UnkGreen.obj";

			if (name == "PointRailCollision")
				PlaceholderModel = "UnkRed.obj";
			return PlaceholderModel;
		}

		ToolStripMenuItem MenuExtension = new ToolStripMenuItem("Odyssey extension");
		ToolStripItem KCLModelItem;
        ToolStripItem KCLObjectExport;
        bool UseKclCollisions = false;
		public void InitModule(IEditorFormContext currentView)
		{
			ViewForm = currentView;
			ViewForm.RegisterMenuStripExt(MenuExtension);
			KCLModelItem = MenuExtension.DropDownItems.Add("");
            KCLObjectExport = MenuExtension.DropDownItems.Add("Export collision from Object");
            ToggleKclCollisions(false);
			KCLModelItem.Click += delegate (object o, EventArgs a) { ToggleKclCollisions(); };
            KCLObjectExport.Click += delegate (object o, EventArgs a) { KclExport.ExportCollisionFromObject(GameFolder + "\\ObjectData");} ;
			
		}

        void ToggleKclCollisions(bool? val = null)
		{
			UseKclCollisions = val.HasValue ? val.Value : !UseKclCollisions;
			KCLModelItem.Text = $"Use collisions as stage models : {(UseKclCollisions ? "ON" : "OFF")}";
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

		public ILevelObj NewObject()
		{
			string name = "";
			//InputDialog.Show("Object Creation Wizard 2000", "Enter the name of the object", ref name);

			var dlg = new EditorForms.AddObjectDialog();

			if(dlg.ShowDialog()==DialogResult.Cancel) return null;

			return new LevelObj(dlg.FileName, dlg.ModelName, ViewForm.CurList.name, "Map", dlg.ObjectName, "Undefined");
		}

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

		ToolStripItem addMoveKeyMenu;
		ToolStripItem removeMoveKeyMenu;
		ToolStripItem resetControlPointsMenu;

		IDictionary<string, dynamic> editableLinks;
		

		void IEditingOptionsModule.InitOptionsMenu(ref ContextMenuStrip baseMenu)
		{
			optionsMenu = baseMenu;
			optionsMenu.Items["ObjectRightClickMenu_EditChildren"].Text = "Edit Links";
			addMoveKeyMenu = optionsMenu.Items.Add("Add MoveKey");
			addMoveKeyMenu.Click += AddMoveKeyMenu_Click;

			removeMoveKeyMenu = optionsMenu.Items.Add("Remove MoveKey");
			removeMoveKeyMenu.Click += RemoveMoveKeyMenu_Click; ;

			resetControlPointsMenu = optionsMenu.Items.Add("Make PathPoint Sharp");
			resetControlPointsMenu.Click += ResetControlPointsMenu_Click; ;
			//addMoveKeyMenu.Visible = false;
		}

		ContextMenuStrip IEditingOptionsModule.GetOptionsMenu(ILevelObj clickedObj)
		{
			addMoveKeyMenu.Visible = clickedObj != null && clickedObj.Properties.ContainsKey("MoveType") && !clickedObj.Properties[LevelObj.N_Links].ContainsKey("KeyMoveNext");
			removeMoveKeyMenu.Visible = clickedObj != null && clickedObj.Properties.ContainsKey("MoveType") && clickedObj.Properties[LevelObj.N_Links].ContainsKey("KeyMoveNext");
			resetControlPointsMenu.Visible = clickedObj != null && clickedObj is RailPoint;
			if (clickedObj != null)
			{
				ToolStripMenuItem linksMenu = optionsMenu.Items["ObjectRightClickMenu_EditChildren"] as ToolStripMenuItem;
				linksMenu.DropDownItems.Clear();

				if (clickedObj is IPathObj)
				{
					linksMenu.Text = "Edit PathPoints";
				}
				else
				{
					linksMenu.Text = "Edit Links";
					editableLinks = clickedObj[LevelObj.N_Links];

					foreach (string k in editableLinks.Keys)
					{
						var item = linksMenu.DropDownItems.Add(k);
						item.Text = k;
						item.Click += LinkMenuItem_Click;
					}
				}
			}
			return optionsMenu;
		}

		private void LinkMenuItem_Click(object sender, EventArgs e)
		{
			ViewForm.EditList(editableLinks[(sender as ToolStripItem).Text]);
		}

		private void AddMoveKeyMenu_Click(object sender, EventArgs e)
		{
			ILevelObj obj = optionsMenu.Tag as ILevelObj;
			Dictionary<string, dynamic> copy = (obj.Clone() as ILevelObj).Properties;

			obj[LevelObj.N_Links].Add("KeyMoveNext", new List<dynamic>());
			obj[LevelObj.N_Links]["KeyMoveNext"].Add(copy);
			ViewForm.EditList(editableLinks["KeyMoveNext"]);
		}

		private void RemoveMoveKeyMenu_Click(object sender, EventArgs e)
		{
			ILevelObj obj = optionsMenu.Tag as ILevelObj;
			(obj[LevelObj.N_Links] as Dictionary<string, dynamic>).Remove("KeyMoveNext");
		}

		private void ResetControlPointsMenu_Click(object sender, EventArgs e)
		{
			ILevelObj obj = optionsMenu.Tag as ILevelObj;
			obj.Properties[Rail.N_CtrlPoints][0]["X"] = obj.Properties[LevelObj.N_Translate]["X"];
			obj.Properties[Rail.N_CtrlPoints][0]["Y"] = obj.Properties[LevelObj.N_Translate]["Y"];
			obj.Properties[Rail.N_CtrlPoints][0]["Z"] = obj.Properties[LevelObj.N_Translate]["Z"];

			obj.Properties[Rail.N_CtrlPoints][1]["X"] = obj.Properties[LevelObj.N_Translate]["X"];
			obj.Properties[Rail.N_CtrlPoints][1]["Y"] = obj.Properties[LevelObj.N_Translate]["Y"];
			obj.Properties[Rail.N_CtrlPoints][1]["Z"] = obj.Properties[LevelObj.N_Translate]["Z"];
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
}
