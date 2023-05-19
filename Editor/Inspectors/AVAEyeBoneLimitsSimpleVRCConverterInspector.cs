
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
using ava.Converters;

namespace ava.Components
{
	[CustomEditor(typeof(AVAEyeBoneLimitsSimple))]
	public class AVAEyeBoneLimitsSimpleInspector : Editor
	{
		private bool editPosition = false;

		public override void OnInspectorGUI()
		{
			//base.DrawDefaultInspector();
			var c = (AVAEyeBoneLimitsSimple)target;

			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Id");
			c.id = EditorGUILayout.TextField(c.id);
			EditorGUILayout.EndHorizontal();

			var humanoid = c.GetComponent<AVAHumanoidMapping>();
			var avatar = c.GetComponent<AVAAvatar>();

			if(humanoid != null && humanoid.mappings != null)
			{
				var eyeLeft = humanoid.mappings.Find(m => m.humanoidName == "EyeLeft");
				var eyeRight = humanoid.mappings.Find(m => m.humanoidName == "EyeRight");
				if(eyeLeft == null && eyeRight == null)
				{
					EditorGUILayout.LabelField("Warning: Eyes not mapped!");
				}
			}
			else
			{
				EditorGUILayout.LabelField("Humanoid mappings not set up!");
			}
			if(!avatar)
			{
				EditorGUILayout.LabelField("Avatar not set up!");
			}

			if(avatar && humanoid)
			{
				if(c.extends == null || c.extends.Count != 2 || String.IsNullOrWhiteSpace(humanoid.id) || String.IsNullOrWhiteSpace(avatar.id))
				{
					if(GUILayout.Button("Setup Extends", GUILayout.ExpandWidth(false)))
					{
						if(String.IsNullOrWhiteSpace(humanoid.id)) humanoid.id = Guid.NewGuid().ToString();
						if(String.IsNullOrWhiteSpace(avatar.id)) avatar.id = Guid.NewGuid().ToString();
						c.extends = new List<string>() {humanoid.id, avatar.id};
					}
				}
				else
				{
					EditorGUILayout.LabelField("Extends set up correctly");
					EditorGUILayout.LabelField($"Aavatar: {avatar.id}");
					EditorGUILayout.LabelField($"Humanoid: {humanoid.id}");
				}
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Limit Up");
			c.up = EditorGUILayout.FloatField(c.up);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Limit Down");
			c.down = EditorGUILayout.FloatField(c.down);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Limit Inner");
			c.inner = EditorGUILayout.FloatField(c.inner);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Limit Outer");
			c.outer = EditorGUILayout.FloatField(c.outer);
			EditorGUILayout.EndHorizontal();

			if(EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(c);
			}
		}
	}
}

#endif
