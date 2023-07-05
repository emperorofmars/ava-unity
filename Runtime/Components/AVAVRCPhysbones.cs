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
	public class AVAVRCPhysbones : MonoBehaviour, ISTFComponent // partial implementation, to be completed whenever
	{
		public static string _TYPE = "AVA.VRC.physbones";
		public string _id = Guid.NewGuid().ToString();
		public string id {get => _id; set => _id = value;}
		public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		public List<string> _targets = new List<string> {"vrchat"};
		public List<string> targets {get => _targets; set => _targets = value;}

		public GameObject target;
		public string version = "1.1";
		public string integration_type = "simplified";
		public float pull = 0.2f; // support curves for each appropriate parameter
		public float stiffness = 0.2f;
		public float spring = 0.2f;
		public float gravity;
		public float gravity_falloff;
		public string immobile_type = "all_motion";
		public float immobile;
	}

	public class AVAVRCPhysbonesImporter : ASTFComponentImporter
	{
		override public void ParseFromJson(ISTFImporter state, ISTFAsset asset, JToken json, string id, GameObject go)
		{
			var c = go.AddComponent<AVAVRCPhysbones>();
			state.AddComponent(id, c);
			c.id = id;
			this.ParseRelationships(json, c);
			c.extends = json["extends"]?.ToObject<List<string>>();
			c.target = state.GetNode((string)json["target"]);
			c.version = (string)json["version"];
			c.integration_type = (string)json["integration_type"];
			c.pull = (float)json["pull"];
			c.stiffness = (float)json["stiffness"];
			c.spring = (float)json["spring"];
			c.gravity = (float)json["gravity"];
			c.gravity_falloff = (float)json["gravity_falloff"];
			c.immobile_type = (string)json["immobile_type"];
			c.immobile = (float)json["immobile"];
		}
	}

	public class AVAVRCPhysbonesExporter : ASTFComponentExporter
	{
		override public List<GameObject> GatherNodes(Component component)
		{
			var c = (AVAVRCPhysbones)component;
			var ret = new List<GameObject>();
			if(c.target) ret.Add(c.target);
			return ret;
		}

		override public JToken SerializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAVRCPhysbones)component;
			var ret = new JObject();
			this.SerializeRelationships(c, ret);
			string voice_parent_node = state.GetNodeId(c.target);
			ret.Add("type", AVAVRCPhysbones._TYPE);
			ret.Add("target", state.GetNodeId(c.target));
			ret.Add("version", c.version);
			ret.Add("integration_type", c.integration_type);
			ret.Add("pull", c.pull);
			ret.Add("stiffness", c.stiffness);
			ret.Add("spring", c.spring);
			ret.Add("gravity", c.gravity);
			ret.Add("gravity_falloff", c.gravity_falloff);
			ret.Add("immobile_type", c.immobile_type);
			ret.Add("immobile", c.immobile);
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAVRCPhysbones
	{
		static Register_AVAVRCPhysbones()
		{
			STFRegistry.RegisterComponentImporter(AVAVRCPhysbones._TYPE, new AVAVRCPhysbonesImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAVRCPhysbones), new AVAVRCPhysbonesExporter());
		}
	}
#endif
}