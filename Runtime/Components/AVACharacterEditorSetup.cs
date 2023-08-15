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

// This exists for now purely to showcase how data could be stored to populate the ui of a character editor application
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
		}
	}

	public class AVACharacterEditorSetupExporter : ASTFComponentExporter
	{
		override public List<GameObject> GatherNodes(Component component)
		{
			var c = (AVACharacterEditorSetup)component;
			var ret = new List<GameObject>();
			return ret;
		}

		override public JToken SerializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVACharacterEditorSetup)component;
			var ret = new JObject();
			string voice_parent_node = state.GetNodeId(c.voice_parent);
			ret.Add("type", AVACharacterEditorSetup._TYPE);
			this.SerializeRelationships(c, ret);
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