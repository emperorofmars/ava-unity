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
	public class AVAAvatarVoice : MonoBehaviour, ISTFComponent
	{
		public static string _TYPE = "AVA.avatar_voice";
		public string _id = Guid.NewGuid().ToString();
		public string id {get => _id; set => _id = value;}
		public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		public List<string> _targets;
		public List<string> targets {get => _targets; set => _targets = value;}

		public GameObject voice_parent;
		public Vector3 voice_position;
	}

	public class AVAAvatarVoiceImporter : ASTFComponentImporter
	{
		override public void parseFromJson(ISTFImporter state, JToken json, string id, GameObject go)
		{
			var component = go.AddComponent<AVAAvatarVoice>();
			component.id = id;
			component.extends = json["extends"]?.ToObject<List<string>>();
			component.voice_parent = state.GetNode((string)json["voice_parent"]);
			var voice_position_array = (JArray)json["voice_position"];
			component.voice_position = new Vector3((float)voice_position_array[0], (float)voice_position_array[1], (float)voice_position_array[2]);
		}
	}

	public class AVAAvatarVoiceExporter : ASTFComponentExporter
	{
		override public List<GameObject> gatherNodes(Component component)
		{
			var c = (AVAAvatarVoice)component;
			var ret = new List<GameObject>();
			if(c.voice_parent) ret.Add(c.voice_parent);
			return ret;
		}

		override public JToken serializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAAvatarVoice)component;
			var ret = new JObject();
			string voice_parent_node = state.GetNodeId(c.voice_parent);
			ret.Add("type", AVAAvatarVoice._TYPE);
			ret.Add("extends", new JArray(c.extends));
			ret.Add("voice_parent", voice_parent_node);
			ret.Add("voice_position", new JArray() {c.voice_position.x, c.voice_position.y, c.voice_position.z});
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAAvatarVoice
	{
		static Register_AVAAvatarVoice()
		{
			STFRegistry.RegisterComponentImporter(AVAAvatarVoice._TYPE, new AVAAvatarVoiceImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAAvatarVoice), new AVAAvatarVoiceExporter());
		}
	}
#endif
}