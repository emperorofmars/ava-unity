using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using stf.Components;
using stf.serialisation;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor.Animations;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava.Components
{
	public abstract class ACharacterEditorEntry
	{
		public string display_name;
		public string description;
		public string tooltip;
		public Texture2D icon;
	}
	public class CharacterEditorSelectionEntry : ACharacterEditorEntry
	{
		public List<AnimationClip> options;
	}
	public class CharacterEditorToggleEntry : ACharacterEditorEntry
	{
		public AnimationClip animation_on;
		public AnimationClip animation_off;
	}
	public class CharacterEditorSliderEntry : ACharacterEditorEntry
	{
		public BlendTree blendtree;
	}
	public class CharacterEditorCategory
	{
		public string display_name;
		public List<ACharacterEditorEntry> entries = new List<ACharacterEditorEntry>();
	}

	public class AVACharacterEditorSetup : ASTFComponent
	{
		public static string _TYPE = "AVA.character-editor-setup";
		
		public string model_description;
		public string help_text;

		public List<CharacterEditorCategory> categories = new List<CharacterEditorCategory>();
	}

	/*public class AVACharacterEditorSetupImporter : ASTFComponentImporter
	{
		override public void ParseFromJson(ISTFImporter state, ISTFAsset asset, JToken json, string id, GameObject go)
		{
			var c = go.AddComponent<AVACharacterEditorSetup>();
			state.AddComponent(id, c);
			c.id = id;
			this.ParseRelationships(json, c);
			c.voice_parent = state.GetNode((string)json["voice_parent"]);
			var voice_position_array = (JArray)json["voice_position"];
			c.voice_position = new Vector3((float)voice_position_array[0], (float)voice_position_array[1], (float)voice_position_array[2]);
		}
	}

	public class AVACharacterEditorSetupExporter : ASTFComponentExporter
	{
		override public List<GameObject> GatherNodes(Component component)
		{
			var c = (AVACharacterEditorSetup)component;
			var ret = new List<GameObject>();
			if(c.voice_parent) ret.Add(c.voice_parent);
			return ret;
		}

		override public JToken SerializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVACharacterEditorSetup)component;
			var ret = new JObject();
			string voice_parent_node = state.GetNodeId(c.voice_parent);
			ret.Add("type", AVACharacterEditorSetup._TYPE);
			this.SerializeRelationships(c, ret);
			ret.Add("voice_parent", voice_parent_node);
			ret.Add("voice_position", new JArray() {c.voice_position.x, c.voice_position.y, c.voice_position.z});
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVACharacterEditorSetup
	{
		static Register_AVACharacterEditorSetup()
		{
			STFRegistry.RegisterComponentImporter(AVACharacterEditorSetup._TYPE, new AVACharacterEditorSetupImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVACharacterEditorSetup), new AVACharacterEditorSetupExporter());
		}
	}
#endif*/
}