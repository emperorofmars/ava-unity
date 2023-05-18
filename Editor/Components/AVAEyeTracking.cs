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
	public class AVAEyeTracking : MonoBehaviour, ISTFComponent
	{
		public static string _TYPE = "AVA.eye_tracking";
		public string _id;
		public string id {get => _id; set => _id = value;}
		public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		public List<string> _targets;
		public List<string> targets {get => _targets; set => _targets = value;}

		// limit vector: up, down, left, right
		public Vector4 limitLeft;
		public Vector4 limitRight;
	}

	public class AVAEyeTrackingImporter : ASTFComponentImporter
	{
		override public void parseFromJson(ISTFImporter state, JToken json, string id, GameObject go)
		{
			var component = go.AddComponent<AVAEyeTracking>();
			component.id = id;
			component.extends = json["extends"]?.ToObject<List<string>>();
			component.overrides = json["overrides"]?.ToObject<List<string>>();
			component.targets = json["targets"]?.ToObject<List<string>>();

			component.limitLeft = new Vector4((float)json["limit_left"][0], (float)json["limit_left"][1], (float)json["limit_left"][2], (float)json["limit_left"][3]);
			component.limitRight = new Vector4((float)json["limit_right"][0], (float)json["limit_right"][1], (float)json["limit_right"][2], (float)json["limit_right"][3]);
		}
	}

	public class AVAEyeTrackingExporter : ASTFComponentExporter
	{
		override public JToken serializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAEyeTracking)component;
			var ret = new JObject();
			ret.Add("type", AVAEyeTracking._TYPE);
			ret.Add("extends", new JArray(c.extends));
			ret.Add("overrides", new JArray(c.overrides));
			ret.Add("targets", new JArray(c.targets));

			ret.Add("limit_left", new JArray() {c.limitLeft[0], c.limitLeft[1], c.limitLeft[2], c.limitLeft[3]});
			ret.Add("limit_right", new JArray() {c.limitRight[0], c.limitRight[1], c.limitRight[2], c.limitRight[3]});
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAEyeTracking
	{
		static Register_AVAEyeTracking()
		{
			STFRegistry.RegisterComponentImporter(AVAEyeTracking._TYPE, new AVAEyeTrackingImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAEyeTracking), new AVAEyeTrackingExporter());
		}
	}
#endif
}