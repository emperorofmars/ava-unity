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
	public class AVAJankyFallbackPhysics : MonoBehaviour, ISTFComponent
	{
		public static string _TYPE = "AVA.janky_fallback_physics";
		public string _id = Guid.NewGuid().ToString();
		public string id {get => _id; set => _id = value;}
		public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		public List<string> _targets;
		public List<string> targets {get => _targets; set => _targets = value;}

		public GameObject target;
		public float pull = 0.2f;
		public float spring = 0.2f;
		public float stiffness = 0.2f;
	}

	public class AVAJankyFallbackPhysicsImporter : ASTFComponentImporter
	{
		override public void ParseFromJson(ISTFImporter state, ISTFAsset asset, JToken json, string id, GameObject go)
		{
			var c = go.AddComponent<AVAJankyFallbackPhysics>();
			state.AddComponent(id, c);
			c.id = id;
			this.ParseRelationships(json, c);
			c.extends = json["extends"]?.ToObject<List<string>>();
			c.target = state.GetNode((string)json["target"]);
			c.pull = (float)json["pull"];
			c.spring = (float)json["spring"];
			c.stiffness = (float)json["stiffness"];
		}
	}

	public class AVAJankyFallbackPhysicsExporter : ASTFComponentExporter
	{
		override public List<GameObject> GatherNodes(Component component)
		{
			var c = (AVAJankyFallbackPhysics)component;
			var ret = new List<GameObject>();
			if(c.target) ret.Add(c.target);
			return ret;
		}

		override public JToken SerializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAJankyFallbackPhysics)component;
			var ret = new JObject();
			this.SerializeRelationships(c, ret);
			string voice_parent_node = state.GetNodeId(c.target);
			ret.Add("type", AVAJankyFallbackPhysics._TYPE);
			ret.Add("target", state.GetNodeId(c.target));
			ret.Add("pull", c.pull);
			ret.Add("spring", c.spring);
			ret.Add("stiffness", c.stiffness);
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAJankyFallbackPhysics
	{
		static Register_AVAJankyFallbackPhysics()
		{
			STFRegistry.RegisterComponentImporter(AVAJankyFallbackPhysics._TYPE, new AVAJankyFallbackPhysicsImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAJankyFallbackPhysics), new AVAJankyFallbackPhysicsExporter());
		}
	}
#endif
}