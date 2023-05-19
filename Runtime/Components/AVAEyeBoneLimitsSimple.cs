using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using stf.Components;
using stf.serialisation;
using UnityEngine;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava.Components
{
	public class AVAEyeBoneLimitsSimple : MonoBehaviour, ISTFComponent
	{
		public static string _TYPE = "AVA.eye_bone_limits_simple";
		public string _id = Guid.NewGuid().ToString();
		public string id {get => _id; set => _id = value;}
		public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		public List<string> _targets;
		public List<string> targets {get => _targets; set => _targets = value;}

		public float up = 15;
		public float down = 12;
		public float inner = 15;
		public float outer = 18;
	}

	public class AVAEyeBoneLimitsSimpleImporter : ASTFComponentImporter
	{
		override public void parseFromJson(ISTFImporter state, JToken json, string id, GameObject go)
		{
			var component = go.AddComponent<AVAEyeBoneLimitsSimple>();
			component.id = id;
			component.extends = json["extends"]?.ToObject<List<string>>();
			component.overrides = json["overrides"]?.ToObject<List<string>>();
			component.targets = json["targets"]?.ToObject<List<string>>();

			component.up = (float)json["up"];
			component.down = (float)json["down"];
			component.inner = (float)json["inner"];
			component.outer = (float)json["outer"];
		}
	}

	public class AVAEyeBoneLimitsSimpleExporter : ASTFComponentExporter
	{
		override public JToken serializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAEyeBoneLimitsSimple)component;
			var ret = new JObject();
			ret.Add("type", AVAEyeBoneLimitsSimple._TYPE);
			ret.Add("extends", new JArray(c.extends));
			ret.Add("overrides", new JArray(c.overrides));
			ret.Add("targets", new JArray(c.targets));

			ret.Add("up", c.up);
			ret.Add("down", c.down);
			ret.Add("inner", c.inner);
			ret.Add("outer", c.outer);
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAEyeBoneLimitsSimple
	{
		static Register_AVAEyeBoneLimitsSimple()
		{
			STFRegistry.RegisterComponentImporter(AVAEyeBoneLimitsSimple._TYPE, new AVAEyeBoneLimitsSimpleImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAEyeBoneLimitsSimple), new AVAEyeBoneLimitsSimpleExporter());
		}
	}
#endif
}