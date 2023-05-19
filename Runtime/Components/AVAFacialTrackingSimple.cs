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
	[Serializable]
	public class BlendshapeMapping
	{
		public string VisemeName;
		public string BlendshapeName;
	}
	
	public class AVAFacialTrackingSimple : MonoBehaviour, ISTFComponent
	{

		public static string _TYPE = "AVA.facial_tracking_simple";
		public string _id;
		public string id {get => _id; set => _id = value;}
		public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		public List<string> _targets;
		public List<string> targets {get => _targets; set => _targets = value;}

		public static List<String> VoiceVisemes15 = new List<string> {
			"sil", "aa", "ch", "dd", "e", "ff", "ih", "kk", "nn", "oh", "ou", "pp", "rr", "ss", "th"
		};

		public SkinnedMeshRenderer TargetMeshInstance;
		public List<BlendshapeMapping> Mappings = new List<BlendshapeMapping>();
	}

	public class AVAFacialTrackingSimpleImporter : ASTFComponentImporter
	{
		override public void parseFromJson(ISTFImporter state, JToken json, string id, GameObject go)
		{
			var c = go.AddComponent<AVAFacialTrackingSimple>();
			c.id = id;
			c.extends = json["extends"]?.ToObject<List<string>>();
			c.overrides = json["overrides"]?.ToObject<List<string>>();
			c.targets = json["targets"]?.ToObject<List<string>>();

			state.AddTask(new Task(() => {
				c.TargetMeshInstance = state.GetNode((string)json["target_mesh_instance"]).GetComponent<SkinnedMeshRenderer>();
			}));
			foreach(var vis in AVAFacialTrackingSimple.VoiceVisemes15)
			{
				c.Mappings.Add(new BlendshapeMapping {VisemeName = vis, BlendshapeName = (string)json[vis]});
			}
		}
	}

	public class AVAFacialTrackingSimpleExporter : ASTFComponentExporter
	{
		override public JToken serializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAFacialTrackingSimple)component;
			var ret = new JObject();
			ret.Add("type", AVAFacialTrackingSimple._TYPE);
			ret.Add("extends", new JArray(c.extends));
			ret.Add("overrides", new JArray(c.overrides));
			ret.Add("targets", new JArray(c.targets));

			ret.Add("target_mesh_instance", c.TargetMeshInstance?.GetComponent<STFUUID>().id); // actually retrieve mesh renderer component id
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