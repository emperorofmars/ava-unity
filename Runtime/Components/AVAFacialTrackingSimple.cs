using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using stf.Components;
using stf.serialisation;
using UnityEngine;
using System.Threading.Tasks;
using stf;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava.Components
{
	public class AVAFacialTrackingSimple : ASTFComponent
	{
		[Serializable]
		public class BlendshapeMapping
		{
			public string VisemeName;
			public string BlendshapeName;
		}
		public static string _TYPE = "AVA.facial_tracking_simple";
		public static readonly List<string> VoiceVisemes15 = new List<string> {
			"sil", "aa", "ch", "dd", "e", "ff", "ih", "kk", "nn", "oh", "ou", "pp", "rr", "ss", "th"
		};

		public static readonly List<string> FacialExpressions = new List<string> {
			"blink", "look_up", "look_down" // add all the facial tracking ones
		};

		public SkinnedMeshRenderer TargetMeshInstance;
		public List<BlendshapeMapping> Mappings = new List<BlendshapeMapping>();

		public void Map()
		{
			if(TargetMeshInstance == null ||TargetMeshInstance.sharedMesh == null) throw new Exception("Meshinstance must be mapped!");
			Mappings = new List<BlendshapeMapping>();
			foreach(var v in VoiceVisemes15)
			{
				string match = null;
				for(int i = 0; i < TargetMeshInstance.sharedMesh.blendShapeCount; i++)
				{
					var bName = TargetMeshInstance.sharedMesh.GetBlendShapeName(i);
					if(bName.ToLower().Contains("vrc." + v)) { match = bName; break; }
					else if(bName.ToLower().Contains("vrc.v_" + v)) { match = bName; break; }
					else if(bName.ToLower().Contains("vis." + v)) { match = bName; break; }
					else if(bName.ToLower().Contains("vis_" + v)) { match = bName; break; }
				}
				Mappings.Add(new BlendshapeMapping{VisemeName = v, BlendshapeName = match});
			}
			foreach(var v in FacialExpressions)
			{
				var compare = v.Split('_');
				string match = null;
				for(int i = 0; i < TargetMeshInstance.sharedMesh.blendShapeCount; i++)
				{
					var bName = TargetMeshInstance.sharedMesh.GetBlendShapeName(i);
					bool matchedAll = true;
					foreach(var c in compare)
					{
						if(!bName.ToLower().Contains(c)) { matchedAll = false; break; }
					}
					if(matchedAll && (match == null || bName.Length < match.Length)) match = bName;
				}
				Mappings.Add(new BlendshapeMapping{VisemeName = v, BlendshapeName = match});
			}
		}
	}

	public class AVAFacialTrackingSimpleImporter : ASTFComponentImporter
	{
		override public void ParseFromJson(ISTFImporter state, ISTFAsset asset, JToken json, string id, GameObject go)
		{
			var c = go.AddComponent<AVAFacialTrackingSimple>();
			state.AddComponent(id, c);
			c.id = id;
			this.ParseRelationships(json, c);

			state.AddTask(new Task(() => {
				c.TargetMeshInstance = (SkinnedMeshRenderer)state.GetComponent((string)json["target_mesh_instance"]);
			}));
			foreach(var vis in AVAFacialTrackingSimple.VoiceVisemes15)
			{
				c.Mappings.Add(new AVAFacialTrackingSimple.BlendshapeMapping {VisemeName = vis, BlendshapeName = (string)json[vis]});
			}
			foreach(var vis in AVAFacialTrackingSimple.FacialExpressions)
			{
				c.Mappings.Add(new AVAFacialTrackingSimple.BlendshapeMapping {VisemeName = vis, BlendshapeName = (string)json[vis]});
			}
		}
	}

	public class AVAFacialTrackingSimpleExporter : ASTFComponentExporter
	{
		override public JToken SerializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAFacialTrackingSimple)component;
			var ret = new JObject();
			ret.Add("type", AVAFacialTrackingSimple._TYPE);
			this.SerializeRelationships(c, ret);

			ret.Add("target_mesh_instance", c.TargetMeshInstance?.GetComponent<STFUUID>().GetIdByComponent(c.TargetMeshInstance));
			foreach(var m in c.Mappings)
			{
				ret.Add(m.VisemeName, m.BlendshapeName);
			}
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAFacialTrackingSimple
	{
		static Register_AVAFacialTrackingSimple()
		{
			STFRegistry.RegisterComponentImporter(AVAFacialTrackingSimple._TYPE, new AVAFacialTrackingSimpleImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAFacialTrackingSimple), new AVAFacialTrackingSimpleExporter());
		}
	}
#endif
}