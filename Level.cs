using ByamlExt;
using EditorCore;
using EditorCore.Interfaces;
using EveryFileExplorer;
using SARCExt;
using Syroot.NintenTools.Byaml.Dynamic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OdysseyExt
{
    public class ObjList : List<ILevelObj>, IObjList
	{
        IList<dynamic> bymlNode;
        public ObjList(string _name, IList<dynamic> _bymlNode)
        {
            name = _name;
            if (_bymlNode == null)
            {
                bymlNode = new List<dynamic>();
                return;
            }
            bymlNode = _bymlNode;
            foreach (var o in bymlNode)
            {
                var obj = new LevelObj(o);
                int objID = obj.ID_int;
                if (Level._HighestID < objID) Level._HighestID = objID;
                this.Add(obj);
            }
        }

        public void ApplyChanges()
        {
            bymlNode.Clear();
            foreach (var o in this) bymlNode.Add(o.Prop);
        }

		public bool IsHidden { get; set; } = false;
		public string name { get; set; } = "";
	}

    public class Level : ILevel
    {
        public Dictionary<string, byte[]> LevelFiles { get; set; }
        public Dictionary<string, IObjList> objs { get; set; }
        public dynamic LoadedLevelData { get; set; }
		public string FilePath { get; set; } = "";
        int _ScenarioIndex = -1;
		public int ScenarioCount => LoadedLevelData.Count;

		public int HighestID { get => _HighestID; set => _HighestID = value; }
		public static int _HighestID = 0;

		public Level(bool empty, string levelN)
        {
            if (!empty) throw new Exception();
            LevelFiles = new Dictionary<string, byte[]>();
            FilePath = levelN;
            LoadedLevelData = new List<dynamic>();
            for (int i = 0; i < 15; i++)
                LoadedLevelData[i] = new Dictionary<string, dynamic>();
            LevelFiles.Add(Path.GetFileNameWithoutExtension(FilePath) + ".byml", ByamlFile.Save(LoadedLevelData,false, Syroot.BinaryData.ByteOrder.LittleEndian));
			LoadByml();
        }

        public Level (string path, int scenarioIndex = -1)
        {
            FilePath = path;
            Load(File.ReadAllBytes(path), scenarioIndex);
        }        

        void Load(byte[] file, int scenarioIndex = -1)
        {
            LevelFiles = SARC.UnpackRam(YAZ0.Decompress(file));
			LoadByml(scenarioIndex);
        }

		void LoadByml(int scenarioIndex = -1)
		{
			Stream s = new MemoryStream(LevelFiles[Path.GetFileNameWithoutExtension(FilePath) + ".byml"]);
			LoadedLevelData = ByamlFile.Load(s, false, Syroot.BinaryData.ByteOrder.LittleEndian);

			LoadObjects(scenarioIndex);
		}

		void LoadObjects(int scenarioIndex = -1)
        {
			objs = new Dictionary<string, IObjList>();

			if (scenarioIndex == -1)
            {
                string res = "0";
                InputDialog.Show("Select scenario", $"enter scenario value [0,{ScenarioCount - 1}]", ref res);
                if (!int.TryParse(res, out scenarioIndex)) scenarioIndex = 0;
            }

            _ScenarioIndex = scenarioIndex;
            var Scenario = (Dictionary<string, dynamic>)LoadedLevelData[scenarioIndex];
            if (Scenario.Keys.Count == 0)
                Scenario.Add("ObjectList", new List<dynamic>());
            foreach (string k in Scenario.Keys)
            {
                objs.Add(k, new ObjList(k,Scenario[k]));
            }
        }

		public void SwitchScenario(int newScenario = -1)
		{
			if (newScenario == -1)
			{
				string res = "0";
				var dialogResult = InputDialog.Show("Select scenario", $"enter scenario value [0,{LoadedLevelData.Count - 1}]", ref res);
				if (dialogResult != System.Windows.Forms.DialogResult.OK) return;
				if (!int.TryParse(res, out newScenario)) newScenario = 0;
			}
			if (_ScenarioIndex == newScenario) return;
			_ScenarioIndex = newScenario;
			LoadObjects(newScenario);
		}

        void ApplyChangesToByml() //this makes sure new objects are added
        {
            objs.OrderBy(k => k.Key);
            for (int i = 0; i < objs.Count; i++)
            {
                var values = objs.Values.ToArray();
                if (values[i].Count == 0) objs.Remove(objs.Keys.ToArray()[i--]);
                else values[i].ApplyChanges();
            }
        }


        public byte[] ToByaml()
        {
            ApplyChangesToByml();
            MemoryStream mem = new MemoryStream();
            ByamlFile.Save(mem, LoadedLevelData, false, Syroot.BinaryData.ByteOrder.LittleEndian);
            var res = mem.ToArray();
            return res;
        }
		
		public byte[] SaveSzs(string newPath = null)
        {
            if (newPath != null)
            {
                LevelFiles.Remove(Path.GetFileNameWithoutExtension(FilePath) + ".byml");
                FilePath = newPath;
                LevelFiles.Add(Path.GetFileNameWithoutExtension(FilePath) + ".byml",ToByaml());
            }
            else
                LevelFiles[Path.GetFileNameWithoutExtension(FilePath) + ".byml"] = ToByaml();
            return YAZ0.Compress(SARC.pack(LevelFiles));
        }

        public bool HasList(string name) { return objs.ContainsKey(name); }
		
        //public SearchResult FindObjById(string ID)
        //{
        //    foreach (string k in objs.Keys)
        //    {
        //        for (int i = 0; i < objs[k].Count; i++)
        //        {
        //            if (objs[k][i].ID == ID)
        //                return new SearchResult
        //                {
        //                    obj = objs[k][i],
        //                    Index = i,
        //                    ListName = k
        //                };
        //        }
        //    }
        //    return new SearchResult
        //    {
        //        obj = null,
        //        Index = -1,
        //        ListName = ""
        //    };
        //}

        public IObjList FindListByObj(ILevelObj o)
        {
            foreach (string k in objs.Keys)
            {
                if (objs[k].Contains(o)) return objs[k];
            }
            return null;
        }
    }
}
