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
	public class OdysseyModule : IGameModule, IEditingOptionsModule, IActionButtonsModule
	{
		public string ModuleName => "Odyssey level editor";

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

		public bool ConvertModelFile(string ObjName, string path) => BfresConverter.Convert(BfresFromSzs(ObjName), path) != null;

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
		//bool UseKclCollisions = false;
		public void InitModule(IEditorFormContext currentView)
		{
			ViewForm = currentView;
			MenuExt = new OdysseyMenuExt();
			//ToggleKclCollisions(false);
			//MenuExt.KCLCollisions.Click += delegate (object o, EventArgs a) { ToggleKclCollisions(); };
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

       // void ToggleKclCollisions(bool? val = null)
		//{
		//	UseKclCollisions = val.HasValue ? val.Value : !UseKclCollisions;
		//	MenuExt.KCLCollisions.Text = $"Use collisions as stage models : {(UseKclCollisions ? "ON" : "OFF")}";
		//}

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
			try
			{
#if DEBUG
				return new Level(file, -1);
#else
				return new Level(file, -1);
#endif
			}
			catch (OperationCanceledException)
			{
				return null;
			}
		}

		public ILevel NewLevel(string file = null)
		{
			MessageBox.Show("This function is not implemented yet.");
			return null;
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
			var dlg = new EditorForms.AddObjectDialog();
			dlg.ShowDialog();
			if (dlg.Result == null) return null;
			bool IsLink = ViewForm.CurList.name == Constants.LinkedListName;
			dlg.Result[LevelObj.N_LinkDest] = IsLink;
			dlg.Result[LevelObj.N_UnitConfig][LevelObj.N_UnitConfigGenList] = IsLink ? "ObjectList" : ViewForm.CurList.name;
			return dlg.Result;
		}

		public bool OpenLevelFile(string name, Stream file)
		{
			if (name.EndsWith(".byml"))
			{
				ByamlViewer.OpenByml(file, name, file,true);
				return true;
			}
			return false;
		}

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
				if (File.Exists(ModelsFolder + "/OdysseyDB.json"))
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

		ToolStripMenuItem LinksMenu;

		ToolStripItem addMoveKeyMenu;
		ToolStripItem removeMoveKeyMenu;
		ToolStripItem resetControlPointsMenu;
		
		void IEditingOptionsModule.InitOptionsMenu(ref ContextMenuStrip baseMenu)
		{
			LinksMenu = baseMenu.Items.Add("Edit links") as ToolStripMenuItem;
			addMoveKeyMenu = baseMenu.Items.Add("Add MoveKey");
			addMoveKeyMenu.Click += AddMoveKeyMenu_Click;

			removeMoveKeyMenu = baseMenu.Items.Add("Remove MoveKey");
			removeMoveKeyMenu.Click += RemoveMoveKeyMenu_Click; ;

			resetControlPointsMenu = baseMenu.Items.Add("Make PathPoint Sharp");
			resetControlPointsMenu.Click += ResetControlPointsMenu_Click; ;
			//addMoveKeyMenu.Visible = false;
		}

		LinksNode selectedLinksNode;
		void IEditingOptionsModule.OptionsMenuOpening(ILevelObj clickedObj)
		{
			addMoveKeyMenu.Visible = clickedObj != null && clickedObj.Prop[LevelObj.N_UnitConfig][LevelObj.N_UnitConfigBaseClass] == "KeyMoveMapParts" && !clickedObj.Prop[LevelObj.N_Links].ContainsKey("KeyMoveNext");
			removeMoveKeyMenu.Visible = clickedObj != null && clickedObj.Prop[LevelObj.N_UnitConfig][LevelObj.N_UnitConfigBaseClass] == "KeyMoveMapParts" && clickedObj.Prop[LevelObj.N_Links].ContainsKey("KeyMoveNext");
			resetControlPointsMenu.Visible = clickedObj != null && clickedObj is RailPoint;
			if (clickedObj != null)
			{
				selectedLinksNode = clickedObj[LevelObj.N_Links] as LinksNode;
				if (selectedLinksNode.Count == 0)
				{
					LinksMenu.Visible = false;
					return;
				}
				LinksMenu.Visible = true;
				LinksMenu.DropDownItems.Clear();

				foreach (string k in selectedLinksNode.Keys)
				{
					var item = LinksMenu.DropDownItems.Add(k);
					item.Text = k;
					item.Click += 
						delegate (object sender, EventArgs e) { ViewForm.EditList(selectedLinksNode[(sender as ToolStripItem).Text]); };
				}

			}
		}

		private void AddMoveKeyMenu_Click(object sender, EventArgs e)
		{
			var optionsMenu = (sender as ToolStripItem).GetCurrentParent() as ContextMenuStrip;
			ILevelObj obj = optionsMenu.Tag as ILevelObj;
			Dictionary<string, dynamic> copy = (obj.Clone() as ILevelObj).Prop;

			obj[LevelObj.N_Links].Add("KeyMoveNext", new List<dynamic>());
			obj[LevelObj.N_Links]["KeyMoveNext"].Add(copy);
			ViewForm.EditList(selectedLinksNode["KeyMoveNext"]);
		}

		private void RemoveMoveKeyMenu_Click(object sender, EventArgs e)
		{
			var optionsMenu = (sender as ToolStripItem).GetCurrentParent() as ContextMenuStrip;
			ILevelObj obj = optionsMenu.Tag as ILevelObj;
			(obj[LevelObj.N_Links] as Dictionary<string, dynamic>).Remove("KeyMoveNext");
		}

		private void ResetControlPointsMenu_Click(object sender, EventArgs e)
		{
			var optionsMenu = (sender as ToolStripItem).GetCurrentParent() as ContextMenuStrip;
			ILevelObj obj = optionsMenu.Tag as ILevelObj;
			obj.Prop[Rail.N_CtrlPoints][0]["X"] = obj.Prop[LevelObj.N_Translate]["X"];
			obj.Prop[Rail.N_CtrlPoints][0]["Y"] = obj.Prop[LevelObj.N_Translate]["Y"];
			obj.Prop[Rail.N_CtrlPoints][0]["Z"] = obj.Prop[LevelObj.N_Translate]["Z"];

			obj.Prop[Rail.N_CtrlPoints][1]["X"] = obj.Prop[LevelObj.N_Translate]["X"];
			obj.Prop[Rail.N_CtrlPoints][1]["Y"] = obj.Prop[LevelObj.N_Translate]["Y"];
			obj.Prop[Rail.N_CtrlPoints][1]["Z"] = obj.Prop[LevelObj.N_Translate]["Z"];
		}
		#endregion

		#region CustomActionButtons
		ToolStripDropDownButton OdysseyDropDown;
		public void InitActionButtons(ref ToolStrip baseButtonStrip)
		{
			OdysseyDropDown = new ToolStripDropDownButton() { Text = "Odyssey" };
			baseButtonStrip.Items.Add(OdysseyDropDown);
			var AddViewBtn = new ToolStripMenuItem("Add GroupView items");
			AddViewBtn.Click += AddViewBtn_Click;
			var CloneScenarioBtn = new ToolStripMenuItem("Clone scenario");
			CloneScenarioBtn.Click += CloneScenarioBtn_Click;
			OdysseyDropDown.DropDownItems.Add(AddViewBtn);
			OdysseyDropDown.DropDownItems.Add(CloneScenarioBtn);
		}

		private void CloneScenarioBtn_Click(object sender, EventArgs e)
		{
			var lev = (Level)ViewForm.LoadedLevel;
			var s = new EditorForms.CloneScenarioDialog(lev.CurScenario, lev.ScenarioCount);			
			s.ShowDialog();
			if (s.result == null)
				return;
			foreach (int i in s.result)
				lev.LoadedLevelData[i] = s.clone ?
					DeepCloneDictArr.DeepClone((Dictionary<string, dynamic>)lev.LoadedLevelData[lev.CurScenario]) :
					lev.LoadedLevelData[lev.CurScenario];
		}

		private void AddViewBtn_Click(object sender, EventArgs e)
		{
			if (ViewForm.SelectedObjs.Length == 0)
			{
				MessageBox.Show("Select at least an object to use this function");
				return;
			}
			var res = MessageBox.Show(
				"This will add a ViewGroup to all the selected objects. The children list will be LINKED among all the objects, this means that when editing the GroupView for one object it will automatically change for all the objects you selected when creating it.\r\n" +
				"This only works for the current session (aka if you save and then reload the level the linked editing will be lost).\r\n" +
				"This action cannot be undone, do you want to continue ?", "", MessageBoxButtons.YesNo);
			if (res != DialogResult.Yes) return;
			var list = new List<dynamic>();
			var obj = OdysseyModule.ObjsDB.MakeObject("GroupView");
			obj[LevelObj.N_UnitConfig][LevelObj.N_UnitConfigBaseClass] = "GroupView";
			obj.ID_int = ++ViewForm.LoadedLevel.HighestID;			
			list.Add(obj.Prop);
			bool? KeepCurrent = null;
			foreach (var o in ViewForm.SelectedObjs)
			{
				var linkNode = o[LevelObj.N_Links] as LinksNode;
				if (linkNode == null)
				{
					linkNode = new LinksNode();
					o[LevelObj.N_Links] = linkNode;
				}
				if (linkNode.ContainsKey("ViewGroup"))
				{
					if (KeepCurrent == null)
					{
						res = MessageBox.Show("One or more objects already have a ViewGroup list, do you want to replace it ?\r\n" +
										"Clicking No will add the GroupView object but will not link the list to keep the already existing GroupViews, the object will be linked anyway", "", MessageBoxButtons.YesNo);
						KeepCurrent = res != DialogResult.Yes;
					}
					else if (KeepCurrent.Value)
						((List<dynamic>)linkNode["ViewGroup"]).Add(obj.Prop);
					else
						linkNode["ViewGroup"] = list;
				}
				else 
					linkNode.Add("ViewGroup", list);
			}
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

		//internal ToolStripItem KCLCollisions;
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

			//KCLCollisions = TitleBarExtensions[0].DropDownItems.Add("");
			KCLConverter = TitleBarExtensions[0].DropDownItems.Add("Export collisions from object");

#if DEBUG
			var a  = TitleBarExtensions[0].DropDownItems.Add("CreateObjDb");
			a.Click += delegate (object o, EventArgs e)
			{
				string s = new DebugStuff.ObjectDatabaseGenerator().Generate(Directory.GetFiles(@"D:\E\Desktop\HAX\Odyssey\StageData", "*Map.szs")).Serialize();
				File.WriteAllText("OdysseyDB.json", s);
			};

			var b = TitleBarExtensions[0].DropDownItems.Add("TestObjDB");
			b.Click += delegate (object o, EventArgs e)
			{
				ObjectDatabase obj = ObjectDatabase.Deserialize(File.ReadAllText("OdysseyDB.json"));
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
