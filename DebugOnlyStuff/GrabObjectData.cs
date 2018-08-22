using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyExt.DebugStuff
{
	//USAGE: new Debug.GrabObjectData().run(Directory.GetFiles(GameFolder + "StageData","*Map.szs").ToList());
	//To use this consider enabling FASTLOAD in ByamlFile.cs
	//class GrabObjectData
	//{
	//	class ObjData
	//	{
	//		public class Field
	//		{
	//			public string name;
	//			public string type;
	//			public string level;
	//		}
	//		public List<Field> Fields = new List<Field>();
	//		public List<string> LinkGroups = new List<string>();

	//		[System.Web.Script.Serialization.ScriptIgnore]
	//		public string ListName = "Unclassified or Links";

	//		public bool ContainsField(string name)
	//		{
	//			foreach (var f in Fields) if (f.name == name) return true;
	//			return false;
	//		}
	//	}

	//	Dictionary<string, List<string>> LinkGroupsObjects = new Dictionary<string, List<string>>(); //Name, object inside
	//	Dictionary<string, ObjData> objs = new Dictionary<string, ObjData>(); //Name, data
	//	Dictionary<string, Dictionary<string, ObjData>> objsSorted = new Dictionary<string, Dictionary<string, ObjData>>(); //ListName, objs entries
	//	List<dynamic> AlreadyParsedObjs = new List<dynamic>();

	//	public void run(List<string> levels)
	//	{
	//		LinkGroupsObjects.Clear();
	//		objs.Clear();
	//		AlreadyParsedObjs.Clear();
	//		objsSorted.Clear();

	//		int index = 0;
	//		foreach (var l in levels)
	//		{
	//			ParseLevel(l);
	//			Console.WriteLine($"Parsing {index++}\\{levels.Count - 1}{l}");
	//		}

	//		foreach (var v in objs)
	//		{
	//			if (!objsSorted.ContainsKey(v.Value.ListName))
	//				objsSorted.Add(v.Value.ListName, new Dictionary<string, ObjData>());
	//			objsSorted[v.Value.ListName].Add(v.Key, v.Value);
	//		}
	//		objs.Clear();

	//		export();
	//	}

	//	void ParseLevel(string levelname)
	//	{
	//		Level level = new Level(levelname, 0);
	//		string name = Path.GetFileNameWithoutExtension(levelname);
	//		for (int i = 0; i < level.ScenarioCount; i++)
	//		{
	//			if (i != 0) level.SwitchScenario(i);
	//			foreach (string k in level.objs.Keys)
	//			{
	//				for (int j = 0; j < level.objs[k].Count; j++)
	//				{
	//					ParseObject((LevelObj)level.objs[k][j], name + $" - Scenario {i}", k);
	//				}
	//			}
	//		}
	//	}

	//	void ParseObject(LevelObj o, string levName, string listName)
	//	{
	//		if (AlreadyParsedObjs.Contains(o.Prop)) return;
	//		AlreadyParsedObjs.Add(o.Prop);
	//		ObjData data;
	//		if (objs.ContainsKey(o.Name)) data = objs[o.Name];
	//		else
	//		{
	//			data = new ObjData();
	//			objs.Add(o.Name, data);
	//		}
	//		data.ListName = listName;
	//		foreach (var prop in (Dictionary<string, dynamic>)o.Prop)
	//		{
	//			if (data.ContainsField(prop.Key)) continue;
	//			if (prop.Key == LevelObj.N_Links ||
	//			prop.Key == LevelObj.N_ModelName ||
	//			prop.Key == LevelObj.N_Name ||
	//			prop.Key == LevelObj.N_Rotate ||
	//			prop.Key == LevelObj.N_Scale ||
	//			prop.Key == LevelObj.N_Translate ||
	//			prop.Key == LevelObj.N_Id) continue;
	//			if (prop.Key == "UnitConfig")
	//			{
	//				foreach (var k in (Dictionary<string, dynamic>)prop.Value)
	//					if (k.Key != "DisplayRotate" && k.Key != "DisplayScale" && k.Key != "DisplayTranslate" && !data.ContainsField("UNITCFG_" + k.Key))
	//						data.Fields.Add(new ObjData.Field()
	//						{
	//							name = "UNITCFG_" + k.Key,
	//							type = k.Value == null ? "NULL" :
	//							(k.Value is string ? "STR:" + k.Value : k.Value.GetType().Name),
	//							level = levName
	//						});
	//			}
	//			else
	//				data.Fields.Add(new ObjData.Field()
	//				{
	//					name = prop.Key,
	//					type = prop.Value == null ? "NULL" :
	//					(prop.Value is string ? "STR:" + prop.Value : prop.Value.GetType().Name),
	//					level = levName
	//				});
	//		}

	//		if (o.ContainsKey(LevelObj.N_Links))
	//		{
	//			LinksNode n = o[LevelObj.N_Links];
	//			foreach (var k in n.Keys)
	//			{
	//				List<string> objNames = null;
	//				if (LinkGroupsObjects.ContainsKey(k)) objNames = LinkGroupsObjects[k];
	//				else
	//				{
	//					objNames = new List<string>();
	//					LinkGroupsObjects.Add(k, objNames);
	//				}
	//				if (!data.LinkGroups.Contains(k)) data.LinkGroups.Add(k);
	//				foreach (dynamic d in n[k])
	//				{
	//					var g = new LevelObj(d);
	//					if (!objNames.Contains(g.Name))
	//						objNames.Add(g.Name);
	//					ParseObject(g, levName + " - Links", listName);
	//				}
	//			}
	//		}

	//	}

	//	void export()
	//	{
	//		;
	//		try
	//		{
	//			var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
	//			var JSON = new { Objects = objsSorted, Links = LinkGroupsObjects };
	//			jss.MaxJsonLength = int.MaxValue;
	//			var tbyml = jss.Serialize(JSON);
	//			System.IO.File.WriteAllText("out.json", tbyml);
	//		}
	//		catch
	//		{
	//			Debugger.Break();
	//		}
	//	}
	//}

	class ObjectDatabaseGenerator
	{
		class internalObjectDatabaseEntry
		{
			public Dictionary<string,string> Properties = new Dictionary<string, string>();
			public Dictionary<string, List<string>> LinkedObjs = null;
			public List<string> DictWarn;
			public List<string> ArrWarn;
			public List<string> ModelNames = new List<string>();
			public string ParameterConfigName = null;
		}

		Dictionary<string, internalObjectDatabaseEntry> Objs = new Dictionary<string, internalObjectDatabaseEntry>();

		public ObjectDatabase Generate(string[] levels)
		{
			int index = 0;
			foreach (var l in levels)
			{
				ParseLevel(l);
				Console.WriteLine($"Parsing {index++}\\{levels.Length - 1} {l}");
			}

			return ToObjDb();
		}

		void ParseLevel(string levelname)
		{
			Level level = new Level(levelname, 0,true);
			string name = Path.GetFileNameWithoutExtension(levelname);
			for (int i = 0; i < level.ScenarioCount; i++)
			{
				if (i != 0) level.SwitchScenario(i);
				foreach (string k in level.objs.Keys)
				{
					for (int j = 0; j < level.objs[k].Count; j++)
					{
						ParseObject((LevelObj)level.objs[k][j]);
					}
				}
			}
		}

		void ParseObject(LevelObj o)
		{
			internalObjectDatabaseEntry data;
			if (Objs.ContainsKey(o.Name)) data = Objs[o.Name];
			else
			{
				data = new internalObjectDatabaseEntry();
				Objs.Add(o.Name, data);
			}
			
			foreach (var prop in (Dictionary<string, dynamic>)o.Prop)
			{
				if (prop.Value == null || data.Properties.ContainsKey(prop.Key)) continue;

				if (prop.Key == LevelObj.N_Links ||
				prop.Key == LevelObj.N_ModelName ||
				prop.Key == LevelObj.N_Name ||
				prop.Key == LevelObj.N_Rotate ||
				prop.Key == LevelObj.N_Scale ||
				prop.Key == LevelObj.N_Translate ||
				prop.Key == LevelObj.N_Id ||
				prop.Key == LevelObj.N_UnitConfig ||
				prop.Key == LevelObj.N_LinkDest ||
				prop.Key == LevelObj.N_LayerConfigName ||
				prop.Key == LevelObj.N_PlacementFileName ||
				prop.Key == LevelObj.N_Comment || 
				prop.Key == LevelObj.N_LinkReferenceList ||
				prop.Key == LevelObj.N_ResourceCategory) continue;

				if (prop.Value is IList<dynamic>)
				{
					if (data.ArrWarn == null) data.ArrWarn = new List<string>();
					if (!data.ArrWarn.Contains(prop.Key)) data.ArrWarn.Add(prop.Key);
				}
				else if (prop.Value is IDictionary<string, dynamic>)
				{
					if (data.DictWarn == null) data.DictWarn = new List<string>();
					if (!data.DictWarn.Contains(prop.Key)) data.DictWarn.Add(prop.Key);
				}
				else
					data.Properties.Add(prop.Key, ((Type)prop.Value.GetType()).Name);
			}

			if (o.ContainsKey(LevelObj.N_ModelName) && o[LevelObj.N_ModelName] != null && !data.ModelNames.Contains(o[LevelObj.N_ModelName]))
				data.ModelNames.Add(o[LevelObj.N_ModelName]);

			if (o.ContainsKey(LevelObj.N_UnitConfig) && o[LevelObj.N_UnitConfig].ContainsKey(LevelObj.N_UnitConfigBaseClass))
			{
				if (data.ParameterConfigName != null)
					Debug.Assert(data.ParameterConfigName == ((string)o[LevelObj.N_UnitConfig][LevelObj.N_UnitConfigBaseClass]));
				data.ParameterConfigName = o[LevelObj.N_UnitConfig][LevelObj.N_UnitConfigBaseClass];

				if (data.ParameterConfigName == o.Name)
					data.ParameterConfigName = null;
			}

			if (o.ContainsKey(LevelObj.N_Links))
			{
				LinksNode n = o[LevelObj.N_Links];
				foreach (var k in n.Keys)
				{
					if (data.LinkedObjs == null)
						data.LinkedObjs = new Dictionary<string, List<string>>();

					List<string> objNames = null;
					if (data.LinkedObjs.ContainsKey(k)) objNames = data.LinkedObjs[k];
					else
					{
						objNames = new List<string>();
						data.LinkedObjs.Add(k, objNames);
					}

					foreach (dynamic d in n[k])
					{
						var g = new LevelObj(d);
						if (!objNames.Contains(g.Name))
							objNames.Add(g.Name);
						ParseObject(g);
					}
				}
			}

		}

		ObjectDatabase ToObjDb()
		{
			ObjectDatabase res = new ObjectDatabase();
			foreach (var entry in Objs)
			{
				string desc = "";

				if (entry.Value.LinkedObjs != null)
				{
					desc = "can contain links ";
					foreach (var e in entry.Value.LinkedObjs)
					{
						desc += e.Key + " [" + string.Join(";",e.Value) + "] "; 
					}
				}

				if (entry.Value.ArrWarn != null)
				{
					desc += "Can contain array nodes [" + string.Join(";", entry.Value.ArrWarn) + "] ";
				}

				if (entry.Value.DictWarn != null)
				{
					desc += "Can contain dictionary nodes [" + string.Join(";", entry.Value.DictWarn) + "] ";
				}

				desc = desc.Trim();
				if (desc == "") desc = null;

				res.Add(entry.Key,
					new ObjectDatabaseEntry()
					{
						Properties = entry.Value.Properties.Count == 0 ? null : entry.Value.Properties,
						Description = desc,
						ParameterConfigName = entry.Value.ParameterConfigName,
						ModelNames = entry.Value.ModelNames.Count == 0 ? null : entry.Value.ModelNames.ToArray()
					});
			}
			return res;
		}
	}
}
