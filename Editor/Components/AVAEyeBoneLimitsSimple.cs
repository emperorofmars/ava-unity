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
		public string _id;
		public string id {get => _id; set => _id = value;}
		public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		public List<string> _targets;
		public List<string> targets {get => _targets; set => _targets = value;}

		// limit vector: up, down, inner, outer
		public Vector4 limits;
		//public Vector4 limitLeft;
		//public Vector4 limitRight;
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

			component.limits = new Vector4((float)json["limits"][0], (float)json["limits"][1], (float)json["limits"][2], (float)json["limits"][3]);
			//component.limitLeft = new Vector4((float)json["limit_left"][0], (float)json["limit_left"][1], (float)json["limit_left"][2], (float)json["limit_left"][3]);
			//component.limitRight = new Vector4((float)json["limit_right"][0], (float)json["limit_right"][1], (float)json["limit_right"][2], (float)json["limit_right"][3]);
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

			ret.Add("limits", new JArray() {c.limits[0], c.limits[1], c.limits[2], c.limits[3]});
			//ret.Add("limit_left", new JArray() {c.limitLeft[0], c.limitLeft[1], c.limitLeft[2], c.limitLeft[3]});
			//ret.Add("limit_right", new JArray() {c.limitRight[0], c.limitRight[1], c.limitRight[2], c.limitRight[3]});
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