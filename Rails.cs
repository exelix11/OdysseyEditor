using EditorCore;
using EditorCore.Interfaces;
using ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace OdysseyExt
{
	class RailPoint : LevelObj
	{
		public RailPoint(Dictionary<string, dynamic> baseNode) : base(baseNode) { }

		public override Vector3D Pos
		{
			get => base.Pos;
			set
			{
				if (Properties != null && Properties.ContainsKey(Rail.N_CtrlPoints))
				{
					//move the controlPoints along as they aren't relative

					float deltaX = (Single)value.X - this[N_Translate]["X"];
					float deltaY = (Single)value.Y - this[N_Translate]["Y"];
					float deltaZ = (Single)value.Z - this[N_Translate]["Z"];

					Properties[Rail.N_CtrlPoints][0]["X"] += deltaX;
					Properties[Rail.N_CtrlPoints][0]["Y"] += deltaY;
					Properties[Rail.N_CtrlPoints][0]["Z"] += deltaZ;

					Properties[Rail.N_CtrlPoints][1]["X"] += deltaX;
					Properties[Rail.N_CtrlPoints][1]["Y"] += deltaY;
					Properties[Rail.N_CtrlPoints][1]["Z"] += deltaZ;
				}
				base.Pos = value;
			}
		}
	}

	class Rail : LevelObj, IPathObj
	{
		public List<ILevelObj> ChildrenObjects { get; set; } = new List<ILevelObj>();
		public const string N_RailPoints = "RailPoints";
		public const string N_CtrlPoints = "ControlPoints";

		public bool IsClosed
		{
			get => this["IsClosed"] ?? false;
			set => this["IsClosed"] = value;
		}
		
		public bool IsBezier => (this["RailType"] ?? "") == "Bezier";

		Point3D[] RawPoints
		{
			get
			{
				var res = new Point3D[ChildrenObjects.Count];
				for (int i = 0; i < res.Length; i++)
					res[i] = (ChildrenObjects[i].ModelView_Pos).ToPoint3D();
				return res;
			}
		}

		Point3D[] _cachedPositions = new Point3D[0];
		Point3D[] _cachedPoints = null;
		[Browsable(false)]
		public Point3D[] Points
		{
			get
			{
				if (!IsBezier)
					return RawPoints;
				
				var curPoints = RawPoints;
				if (!curPoints.SequenceEqual(_cachedPositions))
				{
					_cachedPositions = curPoints;
					_cachedPoints = GetBeizerPoints();
				}
				return _cachedPoints;
			}
		}

		Point3D[] GetBeizerPoints()
		{
			var res = new Point3D[1 + (ChildrenObjects.Count - 1) * 4 + (IsClosed ? 4 : 0)];

			res[0] = (ChildrenObjects[0].ModelView_Pos).ToPoint3D();

			for (int i = 0; i < ChildrenObjects.Count; i++)
			{
				if (i < ChildrenObjects.Count - 1)
				{
					int subPoint = 0;
					Vector3D p0 = ChildrenObjects[i].ModelView_Pos;
					Vector3D p1 = vectorFromDict(ChildrenObjects[i].Prop[N_CtrlPoints][1]);
					Vector3D p2, p3;
					if (i < ChildrenObjects.Count - 1)
					{
						p2 = vectorFromDict(ChildrenObjects[i + 1].Prop[N_CtrlPoints][0]);
						p3 = ChildrenObjects[i + 1].ModelView_Pos;
					}
					else if (IsClosed)
					{
						p2 = vectorFromDict(ChildrenObjects[0].Prop[N_CtrlPoints][0]);
						p3 = ChildrenObjects[0].ModelView_Pos;
					}
					else continue;
					for (float time = 0.25f; subPoint <= 3; time += 0.25f)
					{
						res[i * 4 + subPoint + 1] = (GetPointAtTime(time, p0, p1, p2, p3)).ToPoint3D();
						subPoint++;
					}
				}
			}
			return res;
		}

		public Rail(Dictionary<string, dynamic> bymlNode) : base(bymlNode)
		{
			if (Prop.ContainsKey(N_RailPoints))
			{
				foreach (var o in Prop[N_RailPoints])
					ChildrenObjects.Add(new RailPoint(o));
			}
		}

		[DisplayName("Position")]
		[TypeConverter(typeof(PropertyGridTypes.Vector3DConverter))]
		[Category(" Transform")]
		override public Vector3D Pos
        {
            get => base.Pos;
            set
			{
				_cachedPositions = new Point3D[0]; //invalidate cache;

				if (ChildrenObjects.Count>0)
                {
					float deltaX = (Single)value.X - this[N_Translate]["X"];
                    float deltaY = (Single)value.Y - this[N_Translate]["Y"];
                    float deltaZ = (Single)value.Z - this[N_Translate]["Z"];

                    foreach (ILevelObj obj in ChildrenObjects)
                    {
                        Vector3D pos = obj.Pos;
                        pos.X += deltaX;
                        pos.Y += deltaY;
                        pos.Z += deltaZ;
                        obj.Pos = pos;
                    }
                }
				base.Pos = value;
            }
        }        

        [Browsable(false)]
        int ILevelObj.ID_int
		{
			get
			{
				int res = -1;
				if (ID.StartsWith("rail"))
					int.TryParse(ID.Substring(4), out res);
				return res;
			}
			set
			{
				ID = "rail" + value.ToString();
			}
		}

		[Browsable(false)]
		public int Count => ChildrenObjects.Count;
		[Browsable(false)]
		public bool IsReadOnly => false;
		[Browsable(false)]
		public ILevelObj this[int index] { get => ChildrenObjects[index]; set => ChildrenObjects[index] = value; }
		public int IndexOf(ILevelObj item) => ChildrenObjects.IndexOf(item);
		public void Insert(int index, ILevelObj item) => ChildrenObjects.Insert(index, item);
		public void RemoveAt(int index) => ChildrenObjects.RemoveAt(index);
		public void Add(ILevelObj item) => ChildrenObjects.Add(item);
		public void Clear() => ChildrenObjects.Clear();
		public bool Contains(ILevelObj item) => ChildrenObjects.Contains(item);
		public void CopyTo(ILevelObj[] array, int arrayIndex) => ChildrenObjects.CopyTo(array,arrayIndex);
		public bool Remove(ILevelObj item) => ChildrenObjects.Remove(item);
		public IEnumerator<ILevelObj> GetEnumerator() => ChildrenObjects.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ChildrenObjects.GetEnumerator();

		[Browsable(false)]
		public bool IsHidden { get; set; } = false;
		[Browsable(false)]
		public string name { get => "PathList"; set { } }
		public void ApplyChanges()
		{
			Prop[N_RailPoints].Clear();
			foreach (var o in this) Prop[N_RailPoints].Add(o.Prop);
		}

        static public Vector3D GetPointAtTime(float t, Vector3D p0, Vector3D p1, Vector3D p2, Vector3D p3)
        {
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            return       uuu    * p0 +
                     3 * uu * t * p1 +
                     3 * u  *tt * p2 +
                         ttt    * p3;
        }

        static public Vector3D vectorFromDict(dynamic dict) => new Vector3D(dict["X"], -dict["Z"], dict["Y"]);
	}
}
