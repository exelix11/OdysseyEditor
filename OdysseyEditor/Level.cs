using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Syroot.NintenTools.Byaml.Dynamic;
using RedCarpet;
using EveryFileExplorer;

namespace OdysseyEditor
{
    public class ObjList : List<LevelObj>
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
                if (Level.HighestID < objID) Level.HighestID = objID;
                this.Add(obj);
            }
        }

        public void ApplyToNode()
        {
            bymlNode.Clear();
            foreach (var o in this) bymlNode.Add(o.Prop);
        }    

        public bool IsHidden = false;
        public string name = "";
    }

    public class Level
    {
        public Dictionary<string, byte[]> SzsFiles;
        public Dictionary<string, ObjList> objs = new Dictionary<string, ObjList>();
        public dynamic LoadedByml = null;
        public string FilePath = "";
        int _ScenarioIndex = -1;

        public static int HighestID = 0;

        public Level(bool empty, string levelN)
        {
            if (!empty) throw new Exception();
            SzsFiles = new Dictionary<string, byte[]>();
            FilePath = levelN;
            LoadedByml = new dynamic[15];
            for (int i = 0; i < 15; i++)
                LoadedByml[i] = new Dictionary<string, dynamic>();
            SzsFiles.Add(Path.GetFileNameWithoutExtension(FilePath) + ".byml", ByamlFile.Save(LoadedByml,false, Syroot.BinaryData.ByteOrder.LittleEndian));
            LoadObjects();
        }

        public Level (string path, int scenarioIndex = -1)
        {
            FilePath = path;
            Load(File.ReadAllBytes(path), scenarioIndex);
        }        

        void Load(byte[] file, int scenarioIndex = -1)
        {
            SzsFiles = new SARC().unpackRam(YAZ0.Decompress(file));
            LoadObjects(scenarioIndex);
        }

        void LoadObjects(int scenarioIndex = -1)
        {
            Stream s = new MemoryStream(SzsFiles[Path.GetFileNameWithoutExtension(FilePath) + ".byml"]);
            LoadedByml = ByamlFile.Load(s,false, Syroot.BinaryData.ByteOrder.LittleEndian);

            if (scenarioIndex == -1)
            {
                string res = "0";
                InputDialog.Show("Select scenario", $"enter scenario value [0,{LoadedByml.Count- 1}]", ref res);
                if (!int.TryParse(res, out scenarioIndex)) scenarioIndex = 0;
            }

            _ScenarioIndex = scenarioIndex;
            var Scenario = (Dictionary<string, dynamic>)LoadedByml[scenarioIndex];
            if (Scenario.Keys.Count == 0)
                Scenario.Add("ObjectList", new List<dynamic>());
            foreach (string k in Scenario.Keys)
            {
                objs.Add(k, new ObjList(k,Scenario[k]));
            }
        }

        public void OpenBymlViewer()
        {
            RedCarpet.ByamlViewer v = new RedCarpet.ByamlViewer(LoadedByml);
            v.Show();
        }

        void ApplyChangesToByml() //this makes sure new objects are added
        {
            objs.OrderBy(k => k.Key);
            for (int i = 0; i < objs.Count; i++)
            {
                var values = objs.Values.ToArray();
                if (values[i].Count == 0) objs.Remove(objs.Keys.ToArray()[i--]);
                else values[i].ApplyToNode();
            }
        }

        public byte[] ToByaml()
        {
            ApplyChangesToByml();
            MemoryStream mem = new MemoryStream();
            ByamlFile.Save(mem, LoadedByml, false, Syroot.BinaryData.ByteOrder.LittleEndian);
            var res = mem.ToArray();
            return res;
        }

        public byte[] SaveSzs(string newPath = null)
        {
            if (newPath != null)
            {
                SzsFiles.Remove(Path.GetFileNameWithoutExtension(FilePath) + ".byml");
                FilePath = newPath;
                SzsFiles.Add(Path.GetFileNameWithoutExtension(FilePath) + ".byml",ToByaml());
            }
            else
                SzsFiles[Path.GetFileNameWithoutExtension(FilePath) + ".byml"] = ToByaml();
            return SARC.pack(SzsFiles);
        }

        public bool HasList(string name) { return objs.ContainsKey(name); }

        public struct SearchResult
        {
            public LevelObj obj;
            public int Index;
            public string ListName;
        }

        public SearchResult FindObjById(string ID)
        {
            foreach (string k in objs.Keys)
            {
                for (int i = 0; i < objs[k].Count; i++)
                {
                    if (objs[k][i].ID == ID)
                        return new SearchResult
                        {
                            obj = objs[k][i],
                            Index = i,
                            ListName = k
                        };
                }
            }
            return new SearchResult
            {
                obj = null,
                Index = -1,
                ListName = ""
            };
        }

        public ObjList FindListByObj(LevelObj o)
        {
            foreach (string k in objs.Keys)
            {
                if (objs[k].Contains(o)) return objs[k];
            }
            return null;
        }
    }
}
