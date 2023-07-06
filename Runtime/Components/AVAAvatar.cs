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
	public class AVAAvatar : ASTFComponent
	{
		public static string _TYPE = "AVA.avatar";
		public string avatar_name;
		public string avatar_version;
		public Texture2D icon;
		public string author;
		public string license;
		public string license_link;
		public GameObject viewport_parent;
		public Vector3 viewport_position;
	}

	public class AVAAvatarImporter : ASTFComponentImporter
	{
		override public void ParseFromJson(ISTFImporter state, ISTFAsset asset, JToken json, string id, GameObject go)
		{
			var c = go.AddComponent<AVAAvatar>();
			state.AddComponent(id, c);
			this.ParseRelationships(json, c);
			c.id = id;
			c.avatar_name = (string)json["avatar_name"];
			c.avatar_version = (string)json["avatar_version"];
			if((string)json["icon"] != null && ((string)json["icon"]).Length > 0)
				c.icon = (Texture2D)state.GetResource((string)json["icon"]);
			c.author = (string)json["author"];
			c.license = (string)json["license"];
			c.license_link = (string)json["license_link"];
			c.viewport_parent = state.GetNode((string)json["viewport_parent"]);
			var viewport_position_array = (JArray)json["viewport_position"];
			c.viewport_position = new Vector3((float)viewport_position_array[0], (float)viewport_position_array[1], (float)viewport_position_array[2]);
		}
	}

	public class AVAAvatarExporter : ASTFComponentExporter
	{
		override public List<GameObject> GatherNodes(Component component)
		{
			var c = (AVAAvatar)component;
			var ret = new List<GameObject>();
			if(c.viewport_parent) ret.Add(c.viewport_parent);
			return ret;
		}

		override public List<KeyValuePair<UnityEngine.Object, Dictionary<string, System.Object>>> GatherResources(Component component)
		{
			var c = (AVAAvatar)component;
			var ret = new List<KeyValuePair<UnityEngine.Object, Dictionary<string, System.Object>>>();
			if(c.icon != null) ret.Add(new KeyValuePair<UnityEngine.Object, Dictionary<string, System.Object>> (c.icon, null));
			return ret;
		}

		override public JToken SerializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAAvatar)component;
			var ret = new JObject();
			ret.Add("type", AVAAvatar._TYPE);
			this.SerializeRelationships(c, ret);
			ret.Add("avatar_name", c.avatar_name);
			ret.Add("avatar_version", c.avatar_version);
			if(c.icon != null)
				ret.Add("icon", state.GetResourceId(c.icon));
			ret.Add("author", c.author);
			ret.Add("license", c.license);
			ret.Add("license_link", c.license_link);
			ret.Add("viewport_parent", state.GetNodeId(c.viewport_parent));
			ret.Add("viewport_position", new JArray() {c.viewport_position.x, c.viewport_position.y, c.viewport_position.z});
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAAvatar
	{
		static Register_AVAAvatar()
		{
			STFRegistry.RegisterComponentImporter(AVAAvatar._TYPE, new AVAAvatarImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAAvatar), new AVAAvatarExporter());
		}
	}
#endif

}