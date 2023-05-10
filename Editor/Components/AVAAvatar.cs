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

namespace stf.ava.Components
{
	public class AVAAvatar : MonoBehaviour, ISTFComponent
	{
		public string _id;
		public string id {get => _id; set => _id = value;}
		public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		public List<string> _targets;
		public List<string> targets {get => _targets; set => _targets = value;}
		
		public string avatar_name;
		public string avatar_version;
		public Texture2D icon;
		public string author;
		public string license;
		public string license_link;
		public GameObject viewport_parent;
		public Vector3 viewport_position;
		/*public GameObject voice_parent;
		public Vector3 voice_position;*/
	}

	public class AVAAvatarImporter : ASTFComponentImporter
	{
		override public void parseFromJson(ISTFImporter state, JToken json, string id, GameObject go)
		{
			var component = go.AddComponent<AVAAvatar>();
			component.id = id;
			component.avatar_name = (string)json["avatar_name"];
			component.avatar_version = (string)json["avatar_version"];
			if((string)json["icon"] != null && ((string)json["icon"]).Length > 0)
				component.icon = (Texture2D)state.GetResource((string)json["icon"]);
			component.author = (string)json["author"];
			component.license = (string)json["license"];
			component.license_link = (string)json["license_link"];
			component.viewport_parent = state.GetNode((string)json["viewport_parent"]);
			var viewport_position_array = (JArray)json["viewport_position"];
			component.viewport_position = new Vector3((float)viewport_position_array[0], (float)viewport_position_array[1], (float)viewport_position_array[2]);
			/*component.voice_parent = state.GetNode((string)json["voice_parent"]);
			var voice_position_array = (JArray)json["voice_position"];
			component.voice_position = new Vector3((float)voice_position_array[0], (float)voice_position_array[1], (float)voice_position_array[2]);*/
		}
	}

	public class AVAAvatarExporter : ASTFComponentExporter
	{

		override public List<GameObject> gatherNodes(Component component)
		{
			var c = (AVAAvatar)component;
			var ret = new List<GameObject>();
			if(c.viewport_parent) ret.Add(c.viewport_parent);
			//if(c.voice_parent) ret.Add(c.voice_parent);
			return ret;
		}

		override public List<UnityEngine.Object> gatherResources(Component component)
		{
			var c = (AVAAvatar)component;
			var ret = new List<UnityEngine.Object>();
			if(c.icon != null) ret.Add(c.icon);
			return ret;
		}

		override public JToken serializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAAvatar)component;
			var ret = new JObject();
			string viewport_parent_node = state.GetNodeId(c.viewport_parent);
			//string voice_parent_node = state.GetNodeId(c.voice_parent);
			ret.Add("type", "AVA.avatar");
			ret.Add("avatar_name", c.avatar_name);
			ret.Add("avatar_version", c.avatar_version);
			if(c.icon != null)
				ret.Add("icon", state.GetResourceId(c.icon));
			ret.Add("author", c.author);
			ret.Add("license", c.license);
			ret.Add("license_link", c.license_link);
			ret.Add("viewport_parent", viewport_parent_node);
			ret.Add("viewport_position", new JArray() {c.viewport_position.x, c.viewport_position.y, c.viewport_position.z});
			/*ret.Add("voice_parent", voice_parent_node);
			ret.Add("voice_position", new JArray() {c.voice_position.x, c.voice_position.y, c.voice_position.z});*/
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAAvatar
	{
		static Register_AVAAvatar()
		{
			STFRegistry.RegisterComponentImporter("AVA.avatar", new AVAAvatarImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAAvatar), new AVAAvatarExporter());
		}
	}
#endif
}