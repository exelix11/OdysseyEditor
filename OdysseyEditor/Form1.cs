using EveryFileExplorer;
using ModelViewer;
using RedCarpet;
using Syroot.NintenTools.Byaml.Dynamic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.IO.Compression;
using BfresLib;
using OdysseyEditor.EditorFroms;
using System.Diagnostics;

namespace OdysseyEditor
{
    public partial class Form1 : Form
    {
#if RELEASE
        public static string GameFolder = "";
#else 
        public static string GameFolder = @"D:\E\Desktop\HAX\Odyssey\"; //Set the correct path on your pc
#endif
        const string ModelsFolder = "Models";

        public RendererControl render = new RendererControl();

        public Level LoadedLevel;
        public CustomStack<ObjList> ListEditingStack = new CustomStack<ObjList>();

        public ClipBoardItem StoredValue = null;
        public CustomStack<UndoAction> UndoList = new CustomStack<UndoAction>();

        string CurListName
        {
            get {
                if (EditingList) return RendererControl.C0ListName;
                return comboBox1.Text;
            }
            set {
                if (EditingList) return;
                comboBox1.Text = value;
            }
        }

        ObjList CurList
        {
            get
            {
                if (EditingList) return ListEditingStack.Peek(); ;
                return LoadedLevel.objs[CurListName];
            }
        }

        int SelectionCount
        {
            get { return ObjectsListBox.SelectedItems.Count; }
        }

        LevelObj SelectedObj
        {
            get {
                return SelectionCount == 0 ? null : CurList[ObjectsListBox.SelectedIndex];
            }
        }

        LevelObj[] SelectedObjs
        {
            get
            {
                if (SelectionCount > 0)
                {
                    var res = new LevelObj[ObjectsListBox.SelectedIndices.Count];
                    for (int i = 0; i < res.Length; i++) res[i] = CurList[ObjectsListBox.SelectedIndices[i]];
                    return res;
                }
                else return new LevelObj[0];
            }
        }

        bool EditingList { get { return ListEditingStack.Count != 0; } }

        public Form1(string[] args)
        {
            InitializeComponent();
            KeyPreview = true;
            RenderingCanvas.Child = render;
            render.MouseLeftButtonDown += render_LeftClick;
            render.MouseMove += render_MouseMove;
            render.MouseLeftButtonDown += render_MouseLeftButtonDown;
            render.MouseLeftButtonUp += render_MouseLeftButtonUp;
            render.KeyDown += render_KeyDown;
            render.KeyUp += render_KeyUP;
            render.CameraInertiaFactor = Properties.Settings.Default.CameraInertia;
            render.ShowFps = Properties.Settings.Default.ShowFps;
            render.ShowTriangleCount = Properties.Settings.Default.ShowTriCount;
            render.ShowDebugInfo = Properties.Settings.Default.ShowDbgInfo;
            render.CamMode = Properties.Settings.Default.CameraMode == 0 ? HelixToolkit.Wpf.CameraMode.Inspect : HelixToolkit.Wpf.CameraMode.WalkAround;
            render.ZoomSensitivity = Properties.Settings.Default.ZoomSen;
            render.RotationSensitivity = Properties.Settings.Default.RotSen;
            GameFolder = Properties.Settings.Default.GamePath;

#if DEBUG
            if (Debugger.IsAttached) this.Text += " - Debugger.IsAttached";
            else this.Text += " - DEBUG BUILD";            
#endif

            foreach (string file in args)
            {
                if (File.Exists(file))
                {
                    if (file.EndsWith("byml") || file.EndsWith("byaml"))
                    {
                        ByamlViewer.OpenByml(file);
                    }
                    else if (file.EndsWith(".szs"))
                    {
                        FileOpenArgs = file;
                        break;
                    }
                }
            }

        }

#if DEBUG
        void TestModelLoader(string name)
        {
            if (File.Exists($"{ModelsFolder}/{name}.obj"))
                File.Delete($"{ModelsFolder}/{name}.obj");
            if (File.Exists($"{ModelsFolder}/{name}.mtl"))
                File.Delete($"{ModelsFolder}/{name}.mtl");
            render.addModel(GetModelName(name), this, new Vector3D(), new Vector3D(1, 1, 1), new Vector3D());
            return;

        }
#endif
        string FileOpenArgs = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            GamePathAndModelCheck();
            if (Properties.Settings.Default.CheckUpdates)
                UpdateCheck.CheckForUpdatesAsync();

            if (FileOpenArgs != null)
                LoadLevel(FileOpenArgs);
            //openToolStripMenuItem_Click(null, null);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "szs file | *.szs";
            if (openFile.ShowDialog() != DialogResult.OK) return;
            LoadLevel(openFile.FileName);
        }

        void UnloadLevel()
        {
            List<Form> ToClose = new List<Form>();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is SearchResult ||
                    frm is AddObjList ||
                    frm is LinksEditor)
                    ToClose.Add(frm);
            }
            for (int i = 0; i < ToClose.Count; i++)
                ToClose[i].Close();
            ToClose = null;

            saveAsToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;

            splitContainer2.Enabled = false;
            findToolStripMenuItem.Visible = false;

            HideGroup_CB.CheckedChanged -= HideGroup_CB_CheckedChanged;
            OtherLevelDataMenu.DropDownItems.Clear();
            render.UnloadLevel();
            LoadedLevel = null;

            ListEditingStack.Clear();
            //SelectionIndex = new Stack<int>();
            //InitialAllInfosSection = -1;

            //AllInfos = new Dictionary<string, AllInfoSection>();
            //AllRailInfos = new List<Rail>();
            //higestID = new Dictionary<string, int>();
            UndoList = new CustomStack<UndoAction>();
            comboBox1.Items.Clear();
            ObjectsListBox.Items.Clear();
            propertyGrid1.SelectedObject = null;

            if (SkipModels == null)
            {
                if (File.Exists($"{ModelsFolder}/SkipModels.txt"))
                    SkipModels = new List<string>(File.ReadAllLines($"{ModelsFolder}/SkipModels.txt"));
                else
                    SkipModels = new List<string>();
            }
            else File.WriteAllLines($"{ModelsFolder}/SkipModels.txt", SkipModels.ToArray());
        }

        public void LoadLevel(string path)
        {
            UnloadLevel();
#if DEBUG
            LoadedLevel = new Level(path, 0);
#else
            LoadedLevel = new Level(path, -1);
#endif
            //Populate szs file list
            int index = 0;
            List<ToolStripMenuItem> Files = new List<ToolStripMenuItem>();
            foreach (var f in LoadedLevel.SzsFiles)
            {
                ToolStripMenuItem btn = new ToolStripMenuItem();
                btn.Name = "LoadFile" + index.ToString();
                btn.Text = f.Key;
                btn.Click += OpenSzsFile_click;
                Files.Add(btn);
                index++;
            }
            OtherLevelDataMenu.DropDownItems.AddRange(Files.ToArray());
            //LoadedLevel.OpenBymlViewer();
            //Load models
            LoadingForm.ShowLoading(this, "Loading models...\r\nOpening a level for the first time will take longer");
            foreach (string k in LoadedLevel.objs.Keys.ToArray())
            {
                LoadObjListModels(LoadedLevel.objs[k],k);
            }
            if (LoadedLevel.HasList("AreaList")) HideList(LoadedLevel.objs["AreaList"], true);
            if (LoadedLevel.HasList("SkyList")) HideList(LoadedLevel.objs["SkyList"], true);
            HideGroup_CB.CheckedChanged += HideGroup_CB_CheckedChanged;
            //Populate combobox
            comboBox1.Items.AddRange(LoadedLevel.objs.Keys.ToArray());
            comboBox1.SelectedIndex = 0;
            splitContainer2.Enabled = true;
            findToolStripMenuItem.Visible = true;
            saveAsToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            LoadingForm.EndLoading();
        }

        //NewLevel
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnloadLevel();
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "szs file | *.szs";
            if (sav.ShowDialog() != DialogResult.OK) return;
            LoadedLevel = new Level(true, sav.FileName);
            //Populate combobox
            comboBox1.Items.AddRange(LoadedLevel.objs.Keys.ToArray());
            comboBox1.SelectedIndex = 0;
            splitContainer2.Enabled = true;
            findToolStripMenuItem.Visible = true;
        }

        bool NoModels = true; //Debug only
        List<string> SkipModels = null;
        string GetModelName(string ObjName) //convert bfres to obj and cache in models folder
        {
            if (NoModels && Debugger.IsAttached)
                return null;

            if (SkipModels?.Contains(ObjName) ?? false) return null;

            string CachedModelPath = $"{ModelsFolder}\\{ObjName}.obj";
            if (File.Exists(CachedModelPath))
                return CachedModelPath;
            else if (BfresConverter.Convert(BfresFromSzs(ObjName), CachedModelPath)) 
                return CachedModelPath;

            SkipModels?.Add(ObjName);
            return null;
        }

        public void LoadObjListModels(List<LevelObj> list, string listName)
        {
            foreach (var obj in list)
            {
                AddModel(obj, listName);
            }
        }

        public void AddModel(LevelObj obj, string listName)
        {
            string PlaceholderModel = $"{ModelsFolder}/UnkBlue.obj";
            if (listName == "AreaList") PlaceholderModel = $"{ModelsFolder}/UnkYellow.obj";
            else if (listName == "DebugList") PlaceholderModel = $"{ModelsFolder}/UnkRed.obj";
            else if (listName == "CameraAreaInfo") PlaceholderModel = $"{ModelsFolder}/UnkGreen.obj";

            string ModelFile = GetModelName(obj.ModelName);
            if (ModelFile == null) ModelFile = PlaceholderModel;
            render.addModel(ModelFile, obj, obj.ModelView_Pos, obj.ModelView_Scale, obj.ModelView_Rot);
        }

        public void UpdateModelPosition(LevelObj o)
        {
            render.ChangeTransform(o, o.ModelView_Pos, o.ModelView_Scale, o.ModelView_Rot);
        }

        void PopulateListBox()
        {
            ObjectsListBox.Items.Clear();
            propertyGrid1.SelectedObject = null;
            foreach (var o in CurList) ObjectsListBox.Items.Add(o.ToString());
            ListEditingPanel.Visible = EditingList;
        }

        public void EditList(IList<dynamic> objList)
        {
            ObjList list = new ObjList(RendererControl.C0ListName, objList);
            ListEditingStack.Push(list);
            foreach (var o in list) ObjectsListBox.Items.Add(o.ToString());
            LoadObjListModels(list, RendererControl.C0ListName);
            PopulateListBox();
        }

        public void PreviousList()
        {
            if (!EditingList) return;
            foreach (var obj in CurList) render.RemoveModel(obj);
            ListEditingStack.Pop().ApplyToNode();
            PopulateListBox();
        }

        //Open,save find etc
#region UIControlsEvents
        private void objectByIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ID = "obj0";
            if (InputDialog.Show("Search by ID", "Write the ID to search for", ref ID) == DialogResult.Cancel) return;
            SearchObject(o => o.ID == ID, null, "Object ID =" + ID);
        }

        private void objectByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ObjName = "Coin";
            if (InputDialog.Show("Search by object name", "Write the name to search for", ref ObjName) == DialogResult.Cancel) return;
            ObjName = ObjName.ToLower();
            SearchObject(o => o.Name.ToLower() == ObjName, null, "Object name =" + ObjName);
        }

        private void objectByModelNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ObjName = "Coin";
            if (InputDialog.Show("Search by ModelName", "Write the name to search for", ref ObjName) == DialogResult.Cancel) return;
            ObjName = ObjName.ToLower();
            SearchObject(o => o.ModelName.ToLower() == ObjName, null, "Object ModelName =" + ObjName);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            new FrmCredits().ShowDialog();
        }

        private void propertyGridChange(object s, PropertyValueChangedEventArgs e)
        {
            if (SelectionCount < 0) { MessageBox.Show("No object selected in the list"); return; }
            {
                string name = e.ChangedItem.Label;
                if (name == LevelObj.N_Name || name == LevelObj.N_ModelName || name == "Name")
                {
                    string path = GetModelName(SelectedObj.ModelName);
                    if (path == null) path = $"{ModelsFolder}/UnkBlue.obj";
                    foreach (var i in SelectedObjs) render.ChangeModel(i, path);
                }
                foreach (var i in SelectedObjs)
                {
                    UpdateModelPosition(i);
                }
            }
        }

        private void Btn_AddObj_Click(object sender, EventArgs e)
        {
            string name = "";
            InputDialog.Show("", "Enter a name for the object", ref name);
            if (name.Trim() == "") return;
            var o = new LevelObj();
            o.ID = "obj" + Level.HighestID++;
            o.Name = name;
            o.ModelView_Pos = render.GetPositionInView();
            AddObj(o, CurList);
            render.LookAt(o.ModelView_Pos);
        }
        
        private void compressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            if (opn.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(opn.FileName + ".yaz0"))
                    if (MessageBox.Show($"{opn.FileName}.yaz0 already exists, overwrite it ?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                File.WriteAllBytes(opn.FileName + ".yaz0",YAZ0.Compress(File.ReadAllBytes(opn.FileName)));
            }
        }

        private void decompressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            if (opn.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(opn.FileName + ".bin"))
                    if (MessageBox.Show($"{opn.FileName}.bin already exists, overwrite it ?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                File.WriteAllBytes(opn.FileName + ".bin", YAZ0.Decompress(File.ReadAllBytes(opn.FileName)));
            }
        }

        #region ClipBoard
        private void ClipBoardMenu_Opening(object sender, CancelEventArgs e)
        {
            ClipBoardMenu_Paste.Enabled = StoredValue != null;
            {
                bool SingleObjectSelected = SelectionCount == 1;
                ClipBoardMenu_CopyPos.Visible = SingleObjectSelected;
                ClipBoardMenu_CopyRot.Visible = SingleObjectSelected;
                ClipBoardMenu_CopyScale.Visible = SingleObjectSelected;
                ClipBoardMenu_CopyTransform.Visible = SingleObjectSelected;
            }
            if (SelectionCount > 1) ClipBoardMenu_CopyFull.Text = "Copy objects";
            else ClipBoardMenu_CopyFull.Text = "Copy object";
            ClipBoardMenu_CopyFull.Visible = SelectionCount != 0;
        }

        private void ClipBoardMenu_CopyTransform_Click(object sender, EventArgs e) =>
            StoredValue = new ClipBoardItem() { Type = ClipBoardItem.ClipboardType.Transform, transform = SelectedObj.transform };

        private void ClipBoardMenu_CopyPos_Click(object sender, EventArgs e) =>
            StoredValue = new ClipBoardItem() { Type = ClipBoardItem.ClipboardType.Position, transform = SelectedObj.transform };

        private void ClipBoardMenu_CopyRot_Click(object sender, EventArgs e) =>
            StoredValue = new ClipBoardItem() { Type = ClipBoardItem.ClipboardType.Rotation, transform = SelectedObj.transform };

        private void ClipBoardMenu_CopyScale_Click(object sender, EventArgs e) =>
            StoredValue = new ClipBoardItem() { Type = ClipBoardItem.ClipboardType.Scale, transform = SelectedObj.transform };

        private void Btn_CopyObjs_Click(object sender, EventArgs e) => ClipBoardMenu_CopyFull_Click(null, null);
        private void ClipBoardMenu_CopyFull_Click(object sender, EventArgs e)
        {
            LevelObj[] objs = new LevelObj[SelectionCount];
            for (int i = 0; i < objs.Length; i++) objs[i] = SelectedObjs[i].Clone();
            StoredValue = new ClipBoardItem() { Type = ClipBoardItem.ClipboardType.Objects, Objs = objs };
        }

        private void ClipBoardMenu_Paste_Click(object sender, EventArgs e)
        {
            if (StoredValue.Type == ClipBoardItem.ClipboardType.Objects)
            {
                foreach (var o in StoredValue.Objs)
                    AddObj(o.Clone(), CurList);
            }
            else if (SelectionCount != 0)
            {

                Tuple<LevelObj, Transform>[] args = new Tuple<LevelObj, Transform>[SelectionCount];
                for (int i = 0; i < SelectionCount; i++) args[i] = new Tuple<LevelObj, Transform>(SelectedObjs[i], SelectedObjs[i].transform);
                AddToUndo((dynamic arg) =>
               {
                   var _args = (Tuple<LevelObj, Transform>[])arg;
                   foreach (var a in _args)
                   {
                       a.Item1.transform = a.Item2;
                       UpdateModelPosition(a.Item1);
                   }
               }, $"Pasted value to {SelectionCount} Object(s)");

                foreach (var o in SelectedObjs)
                {
                    switch (StoredValue.Type)
                    {
                        case ClipBoardItem.ClipboardType.Position:
                            o.Pos = StoredValue.transform.Pos;
                            break;
                        case ClipBoardItem.ClipboardType.Rotation:
                            o.Rot = StoredValue.transform.Rot;
                            break;
                        case ClipBoardItem.ClipboardType.Scale:
                            o.Scale = StoredValue.transform.Scale;
                            break;
                        case ClipBoardItem.ClipboardType.Transform:
                            o.transform = StoredValue.transform;
                            break;
                    }
                    UpdateModelPosition(o);
                }
            }
        }
#endregion

        private void UndoMenu_Open(object sender, EventArgs e)
        {
            UndoMenu.DropDownItems.Clear();
            List<ToolStripMenuItem> Items = new List<ToolStripMenuItem>();
            int count = 0;
            foreach (UndoAction act in UndoList.ToArray().Reverse())
            {
                ToolStripMenuItem btn = new ToolStripMenuItem();
                btn.Name = "Undo" + count.ToString();
                btn.Text = act.ToString();
                btn.Click += UndoListItem_Click;
                btn.MouseEnter += UndoListItem_MouseEnter;
                Items.Add(btn);
                count++;
            }
            UndoMenu.DropDownItems.AddRange(Items.ToArray());
        }

        private void UndoListItem_MouseEnter(object sender, EventArgs e)
        {
            string SenderName = ((ToolStripMenuItem)sender).Name;
            int index = int.Parse(SenderName.Substring("Undo".Length));
            for (int i = 0; i < UndoMenu.DropDownItems.Count; i++)
            {
                if (i < index) UndoMenu.DropDownItems[i].BackColor = Color.LightBlue;
                else UndoMenu.DropDownItems[i].BackColor = SystemColors.Control;
            }
        }

        private void UndoListItem_Click(object sender, EventArgs e)
        {
            string SenderName = ((ToolStripMenuItem)sender).Name;
            int index = int.Parse(SenderName.Substring("Undo".Length));
            for (int i = 0; i <= index; i++)
            {
                UndoList.Pop().Undo();
            }
            UndoMenu.HideDropDown();
        }

        private void OpenSzsFile_click(object sender, EventArgs e)
        {
            string name = ((ToolStripMenuItem)sender).Text;
            var byml = ByamlFile.Load(new MemoryStream(LoadedLevel.SzsFiles[name]), false, Syroot.BinaryData.ByteOrder.LittleEndian);
            new RedCarpet.ByamlViewer(byml).Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if DEBUG
            saveAsSZSToolStripMenuItem_Click(sender, e); //Let's not risk modifing our precious dump
#else
            File.WriteAllBytes(LoadedLevel.FilePath, LoadedLevel.SaveSzs());
#endif
        }

        private void saveAsSZSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadedLevel == null) return;
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "szs file | *.szs";
            sav.FileName = Path.GetFileNameWithoutExtension(LoadedLevel.FilePath);
            if (sav.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sav.FileName, LoadedLevel.SaveSzs(sav.FileName));
            }
        }

        private void saveAsBymlToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (LoadedLevel == null) return;
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "byml file | *.byml";
            sav.FileName = Path.GetFileNameWithoutExtension(LoadedLevel.FilePath);
            if (sav.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sav.FileName, LoadedLevel.ToByaml());
            }
        }

        private void Btn_addType_Click(object sender, EventArgs e)
        {
            var f = new EditorFroms.AddObjList();
            f.ShowDialog();
            if (f.Result == null || f.Result.Trim() == "") return;
            LoadedLevel.objs.Add(f.Result, new ObjList(f.Result, null));
            comboBox1.SelectedIndex = comboBox1.Items.Add(f.Result);
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                GameFolder = dlg.SelectedPath;
                if (!GameFolder.EndsWith("\\")) GameFolder += "\\";
                Properties.Settings.Default.GamePath = GameFolder;
                Properties.Settings.Default.Save();
                gamePathToolStripItem.Text = "Game path: " + GameFolder;
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e) => new Settings(render).ShowDialog();

        private void DuplicateSelectedObj_btn(object sender, EventArgs e) => DuplicateObj(SelectedObj, CurList);
        private void btn_delObj_Click(object sender, EventArgs e)
        {
            var list = SelectedObjs.ToArray();
            foreach (var o in list) DeleteObj(o, CurList);
        }

        private void openBymlToolStripMenuItem_Click(object sender, EventArgs e) => ByamlViewer.OpenByml();
        private void importFromJsonToolStripMenuItem_Click(object sender, EventArgs e) => ByamlViewer.ImportFromJson();
#endregion

        //Property grid change, listbox, combobox, show/hide
#region EditorControlsEvents

        private void lnk_hideSelectedObjs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CurList.IsHidden) return;
            foreach (var o in SelectedObjs)
                render.RemoveModel(o);            
        }

        private void lnk_ShowHiddenObjs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (var list in ListEditingStack)
            {
                if (list.IsHidden) continue;
                foreach (var o in list)
                    AddModel(o, list.name);
            }
            foreach (var list in LoadedLevel.objs.Values)
            {
                if (list.IsHidden) continue;
                foreach (var o in list)
                    AddModel(o, list.name);
            }
        }

        private void ListEditGoBack(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PreviousList();
        }

        private void SelectedListChanged(object sender, EventArgs e) //comboBox1
        {
            if (EditingList) return;
            ObjectsListBox.Items.Clear();
            propertyGrid1.SelectedObject = null;
            if (CurListName == null || CurListName == "" || CurList.Count == 0) return;
            HideGroup_CB.CheckedChanged -= HideGroup_CB_CheckedChanged; //Do not trigger event, unneeded (?)
            HideGroup_CB.Checked = CurList.IsHidden;
            HideGroup_CB.CheckedChanged += HideGroup_CB_CheckedChanged;
            foreach (var o in CurList) ObjectsListBox.Items.Add(o.ToString());
            ObjectsListBox.SelectedIndex = 0;
        }

        private void SelectedObjectChanged(object sender, EventArgs e) //ObjectsListBox
        {
            if (SelectionCount > 1)
            {
                Btn_CopyObjs.Visible = true;
                Btn_Duplicate.Visible = false;
            }
            else
            {
                Btn_CopyObjs.Visible = false;
                Btn_Duplicate.Visible = true;
            }

            if (ObjectsListBox.SelectedIndex == -1 || SelectedObj == null)
            {
                propertyGrid1.SelectedObject = null;
                return;
            }

            if (SelectionCount == 1) propertyGrid1.SelectedObject = ((LevelObj)SelectedObj);
            else propertyGrid1.SelectedObject = null;

            if (CurList.IsHidden)
            {
                foreach (var o in CurList)
                    render.RemoveModel(o);
                foreach (var o in SelectedObjs)
                    AddModel(o, CurList.name);
            }

            render.SelectObjs(SelectedObjs.Cast<dynamic>().ToList());
        }        

        private void ObjectsList_DoubleClick(object sender, EventArgs e)
        {
            if (ObjectsListBox.SelectedIndex == -1 || SelectionCount > 1) return;
            render.LookAt(SelectedObj.ModelView_Pos);
        }

        private void HideGroup_CB_CheckedChanged(object sender, EventArgs e)
        {
            HideList(CurList, HideGroup_CB.Checked);
        }
#endregion

        //Dragging, click
#region RendererEvents
            
        DragArgs DraggingArgs = null;
        bool RenderIsDragging { get { return DraggingArgs != null && Mouse.LeftButton == MouseButtonState.Pressed && (ModifierKeys & Keys.Control) == Keys.Control; } }        

        private void render_LeftClick(object sender, MouseButtonEventArgs e)
        {
            if (RenderIsDragging) return;
            var result = render.GetOBJ(sender, e);
            if (result == null) return;
            if ((ModifierKeys & Keys.Shift) == Keys.Shift && CurList.Contains(result))
            {
                ObjectsListBox.SelectedIndices.Add(CurList.IndexOf(result));
            }
            else
            {
                ObjList list = null;
                if (EditingList) list = CurList.Contains(result) ? CurList : null;
                else list = LoadedLevel.FindListByObj(result);
                if (list != null)
                {
                    CurListName = list.name;
                    ObjectsListBox.ClearSelected();
                    ObjectsListBox.SelectedIndex = list.IndexOf(result);
                }
            }
        }

        private void render_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) //Render hotkeys
        {
            if (RenderIsDragging) return;
            HandleHotKey(e);
        }
        
        private void render_KeyUP(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                if (DraggingArgs != null) endDragging();
            }
        }        

        private void render_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!RenderIsDragging) return;
            int RoundTo = (ModifierKeys & Keys.Alt) == Keys.Alt ? 100 : ((ModifierKeys & Keys.Shift) == Keys.Shift ? 50 : 0);

            Vector3D NewPos = render.Drag(DraggingArgs, e, RoundTo);
            if (NewPos == null) return;
            ((LevelObj)DraggingArgs.obj).ModelView_Pos = NewPos;
            UpdateModelPosition(DraggingArgs.obj);
            DraggingArgs.position = NewPos;
        }

        void endDragging()
        {
            AddToUndo((dynamic args) => 
            {
                var a = (DragArgs)args;
                ((LevelObj)a.obj).ModelView_Pos = a.StartPos;
                UpdateModelPosition(a.obj);
            }, $"Moved object {SelectedObj}", DraggingArgs);
            DraggingArgs = null;
            propertyGrid1.Refresh();
        }

        private void render_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DraggingArgs != null) endDragging();
        }

        private void render_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) != Keys.Control || RenderIsDragging) return;
            var obj = render.GetOBJ(sender, e);
            if (obj == null)
            {
                return;
            }
            DraggingArgs = new DragArgs();
            DraggingArgs.obj = obj;
            DraggingArgs.StartPos = ((LevelObj)DraggingArgs.obj).ModelView_Pos;
            DraggingArgs.position = DraggingArgs.StartPos;
        }

#endregion

        void HandleHotKey(System.Windows.Input.KeyEventArgs e)
        {
            if (RenderIsDragging) return;
            if (e.Key == Key.B && EditingList) PreviousList();
            if (SelectionCount == 0) return;
            if (e.Key == Key.Space) render.LookAt(SelectedObj.ModelView_Pos);
            else if (e.Key == Key.OemPlus && Btn_AddObj.Enabled) Btn_AddObj_Click(null, null);
            else if (e.Key == Key.D && SelectionCount == 1) DuplicateObj(SelectedObj, CurList);
            else if (e.Key == Key.Delete) btn_delObj_Click(null, null);
            else if (e.Key == Key.F) findToolStripMenuItem.ShowDropDown();
            else if (e.Key == Key.H) lnk_hideSelectedObjs_LinkClicked(null, null);
            else if (e.Key == Key.Z && UndoList.Count > 0) UndoList.Pop().Undo();
            else if (e.Key == Key.C && SelectionCount == 1)
            {
                if (SelectedObj[LevelObj.N_Links] != null)
                {
                    var BakLinks = ((LinksNode)SelectedObj[LevelObj.N_Links]).Clone();

                    AddToUndo((dynamic arg) =>
                    {
                        ((LevelObj)arg[0])[LevelObj.N_Links] = arg[1];
                    },
                        $"Edited links of {SelectedObj.ToString()}",
                        new dynamic[] { SelectedObj, BakLinks });

                    new EditorFroms.LinksEditor(SelectedObj[LevelObj.N_Links]).ShowDialog();
                }
            }
#if DEBUG
            else if (e.Key == Key.P)
                foreach (ObjList l in LoadedLevel.objs.Values)
                    foreach (LevelObj o in l) UpdateModelPosition(o);
#endif
            else return;
        }

        const int UndoMax = 30;
        void AddToUndo(Action<dynamic> act, string desc, dynamic arg = null)
        {
            UndoList.Push(new UndoAction(desc, act, arg));
            if (UndoList.Count > UndoMax) UndoList.RemoveAt(0);
        }

        public void HideList(ObjList list, bool hide)
        {
            if (LoadedLevel == null || list.IsHidden == hide) return;

            list.IsHidden = hide;
            if (hide)
            {
                foreach (var o in list)
                    render.RemoveModel(o);
            }
            else
            {
                foreach (var o in list)
                    AddModel(o, list.name);
            }
        }

        public void AddObj(LevelObj o, ObjList list)
        {
            AddToUndo((dynamic) => InternalDeleteObj(o, list), "Added object: " + o.ToString());
            InternalAddObj(o, list);
        }

        void InternalAddObj(LevelObj o, ObjList list)
        {
            list.Add(o);
            if (list == CurList)
            {
                ObjectsListBox.Items.Add(o.ToString());
            }
            if (!(list.name == RendererControl.C0ListName && EditingList))
                AddModel(o, list.name);
        }

        public void DuplicateObj(LevelObj o, ObjList list)
        {
            if (o == null) return;
            var newobj = o.Clone();
            newobj.ID = "obj" + Level.HighestID++;
            AddObj(newobj, list);
        }

        public void DeleteObj(LevelObj o, ObjList list)
        {
            if (o == null) return;
            AddToUndo((dynamic) =>
            InternalAddObj(o, list), "Deleted object: " + o.ToString());
            InternalDeleteObj(o, list);
        }

        public void InternalDeleteObj(LevelObj o, ObjList list)
        {
            ObjectsListBox.SelectedIndex = -1;
            if (list == CurList)
            {
                ObjectsListBox.Items.RemoveAt(CurList.IndexOf(o));
                render.RemoveModel(o);
            }
            list.Remove(o);
        }

        public void SearchObject(Func<LevelObj,bool> seachFn, ObjList list = null, string QueryDescription = "")
        {
            List<Tuple<ObjList, LevelObj>> Result = new List<Tuple<ObjList, LevelObj>>();
            if (list != null)
            {
                foreach (var o in list)
                {
                    if (seachFn(o)) Result.Add(new Tuple<ObjList, LevelObj>(list, o));
                }
            }
            else if (EditingList)
            {
                foreach (var o in CurList)
                {
                    if (seachFn(o)) Result.Add(new Tuple<ObjList, LevelObj>(CurList, o));
                }
            }
            else
            {
                foreach (var k in LoadedLevel.objs.Values)
                {
                    foreach (var o in k)
                    {
                        if (seachFn(o)) Result.Add(new Tuple<ObjList, LevelObj>(k, o));
                    }
                }
            }
            new EditorFroms.SearchResult(Result.ToArray(), QueryDescription, this).Show();
        }

        public void SelectObject(ObjList List, LevelObj obj)
        {
            if (EditingList)
            {
                if (List != CurList) return;
            }
            else
                comboBox1.Text = List.name;
            ObjectsListBox.ClearSelected();
            ObjectsListBox.SelectedIndex = List.IndexOf(obj);
        }

        byte[] BfresFromSzs(string fileName)
        {
            if (File.Exists($"{GameFolder}ObjectData\\{fileName}.szs"))
            {
                var SzsFiles = new SARC().unpackRam(YAZ0.Decompress($"{GameFolder}ObjectData\\{fileName}.szs"));
                if (SzsFiles.ContainsKey(fileName + ".bfres"))
                {
                    return SzsFiles[fileName + ".bfres"];
                }
            }
            return null;
        }

        void GamePathAndModelCheck()
        {
            gamePathToolStripItem.Text = "Game path: " + GameFolder;
            if (GameFolder == "" || !Directory.Exists(GameFolder))
            {
                MessageBox.Show("Select the path of the game, it will be used to display the models from the game");
                changeToolStripMenuItem_Click(null, null);
                MessageBox.Show("You can change it from the tools menu later");
                this.Focus();
            }
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
                    LoadingForm.ShowLoading(this, "Extracting textures...\r\nThis might take a while");
                    foreach (var a in Directory.GetFiles($"{GameFolder}ObjectData\\").Where(x => x.EndsWith("Texture.szs")))
                    {
                        BfresLib.BfresConverter.GetTextures(
                            BfresFromSzs(Path.GetFileNameWithoutExtension(a)), 
                            ModelsFolder);
                    }
                    LoadingForm.EndLoading();
                }
            }
            if (Properties.Settings.Default.FirstStart)
            {
                Properties.Settings.Default.FirstStart = false;
                Properties.Settings.Default.Save();
                MessageBox.Show("You can now custmize the settings (to open this window again click on File -> Settings)");
                new Settings(render).ShowDialog();
                this.Focus();
            }
        }
    }
}
