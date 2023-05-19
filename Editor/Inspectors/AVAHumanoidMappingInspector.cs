
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using stf.serialisation;
using stf;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Linq;
using UnityEditor;

namespace ava.Components
{
	[InitializeOnLoad]
	public class Register_AVAHumanoidMapping
	{
		static Register_AVAHumanoidMapping()
		{
			STFRegistry.RegisterComponentImporter(AVAHumanoidMapping._TYPE, new AVAHumanoidMappingImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAHumanoidMapping), new AVAHumanoidMappingExporter());
			AVASecondStage.RegisterPreStageConverter(typeof(AVAHumanoidMapping), new AVAHumanoidMappingConverter());
		}
	}

	[CustomEditor(typeof(AVAHumanoidMapping))]
	public class AVAHumanoidMappingInspector : Editor
	{
		private int locomotionSelection = 0;
		private string[] locomotionOptions = new string[] { "plantigrade", "digitigrade" };
		private string[] locomotionDisplayOptions = new string[] { "Plantigrade", "Digitigrade" };
		private bool _foldoutMappings = true;

		void OnEnable()
		{
			var c = (AVAHumanoidMapping)target;
			if(c.locomotion_type != null)
			{
				for(int i = 0; i < locomotionOptions.Length; i++)
				{
					if(c.locomotion_type == locomotionOptions[i])
					{
						locomotionSelection = i;
						break;
					}
				}
			}
		}

		public override void OnInspectorGUI()
		{
			//base.DrawDefaultInspector();
			var c = (AVAHumanoidMapping)target;

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Id");
			c.id = EditorGUILayout.TextField(c.id);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Armature Instance");
			c.armatureInstance = (STFArmatureInstance)EditorGUILayout.ObjectField(c.armatureInstance, typeof(STFArmatureInstance), true);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Locomotion Type");

			var locomotionSelectionNew = EditorGUILayout.Popup(locomotionSelection, locomotionDisplayOptions);
			if(locomotionSelectionNew != locomotionSelection || c.locomotion_type == null || c.locomotion_type.Length == 0)
			{
				c.locomotion_type = locomotionOptions[locomotionSelectionNew];
				locomotionSelection = locomotionSelectionNew;
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10f);
			
			EditorGUILayout.PrefixLabel("Mapped " + (c.mappings != null ? c.mappings.Count : 0) + " bones.");
			if(c.armatureInstance != null)
			{
				if(GUILayout.Button("Map Humanoid Bones", GUILayout.ExpandWidth(false))) {
					c.Map(c.armatureInstance.bones);
				}
			}
			else
			{
				if(GUILayout.Button("Map Humanoid Bones From Root", GUILayout.ExpandWidth(false))) {
					c.Map(Utils.getRoot(c.transform).GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray());
				}
			}

			if(c.mappings != null && c.mappings.Count > 0)
			{
				GUILayout.Space(10f);
				if(GUILayout.Button("Create Unity Avatar", GUILayout.ExpandWidth(false))) {
					var path = EditorUtility.SaveFilePanel("Save Avatar", "Assets", "avatar", "asset");
					var avatar = HumanoidMappingConverter.convert(c, Utils.getRoot(c.transform).gameObject);
					if (path.StartsWith(Application.dataPath)) {
						path = "Assets" + path.Substring(Application.dataPath.Length);
					}
					AssetDatabase.CreateAsset(avatar, path);
				}
			}

			GUILayout.Space(10f);

			_foldoutMappings = EditorGUILayout.Foldout(_foldoutMappings, "Mappings", true, EditorStyles.foldoutHeader);
			if(_foldoutMappings)
			{
				GUILayout.Space(5f);
				//base.DrawDefaultInspector();

				foreach(var mapping in AVAHumanoidMapping.NameMappings)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel(mapping.Key);
					var bone = c.mappings.Find(m => m.humanoidName == mapping.Key);
					if(bone != null)
					{
						bone.bone = (GameObject)EditorGUILayout.ObjectField(bone.bone, typeof(GameObject), true);
					}
					else
					{
						if(GUILayout.Button("Add Bone Mapping", GUILayout.ExpandWidth(false))) {
							c.mappings.Add(new BoneMappingPair(mapping.Key, null));
						}
					}
					EditorGUILayout.EndHorizontal();
				}
			}

			if(EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(c);
			}
			
		}
	}
}

#endif
