using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyExt
{
	public class ObjectDatabaseEntry
	{
		//Always check if properties or description are null 
		[Newtonsoft.Json.JsonProperty("P")]
		public Dictionary<string, string> Properties;

		[Newtonsoft.Json.JsonProperty("D")]
		public string Description;

		[Newtonsoft.Json.JsonProperty("C")]
		public string ParameterConfigName;

		[Newtonsoft.Json.JsonProperty("M")]
		public string[] ModelNames;
	}

	public class ObjectDatabase : Dictionary<string, ObjectDatabaseEntry>
	{
		public string Serialize()
		{
			return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.None,
							new JsonSerializerSettings
							{
								NullValueHandling = NullValueHandling.Ignore
							});
		}

		public static ObjectDatabase Deserialize(string json)
		{
			return JsonConvert.DeserializeObject<ObjectDatabase>(json);
		}

		public LevelObj MakeObject(string name)
		{
			if (!this.ContainsKey(name)) return null;
			LevelObj o = new LevelObj();
			o.Name = name;
			if (this[name].Properties == null) return o;

			foreach (var prop in this[name].Properties)
			{
				o.Prop.Add(prop.Key, GetValue(prop.Value));
			}
			return o;
		}

		dynamic GetValue(string type)
		{
			switch (type)
			{
				case "String":
					return "";
				case "Boolean":
					return false;
				case "Int32":
					return (int)0;
				case "Single":
					return 0f;
				default:
					throw new Exception("unexpected type");
			}
		}
	}
}
