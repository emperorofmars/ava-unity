
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
	[CustomEditor(typeof(AVAFacialTrackingSimple))]
	public class AVAFacialTrackingSimpleInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			//base.DrawDefaultInspector();
			var c = (AVAFacialTrackingSimple)target;

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Id");
			c.id = EditorGUILayout.TextField(c.id);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Mesh Instance");
			c.TargetMeshInstance = (SkinnedMeshRenderer)EditorGUILayout.ObjectField(c.TargetMeshInstance, typeof(SkinnedMeshRenderer), true);
			EditorGUILayout.EndHorizontal();

			if(c.extends != null && c.extends.Count == 0 && c.GetComponent<AVAAvatar>() && GUILayout.Button("Setup Extends", GUILayout.ExpandWidth(false)))
			{
				c.extends.Add(c.GetComponent<AVAAvatar>()?.id);
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Extends");
			EditorGUILayout.LabelField(c.extends?.Count == 1 ? c.extends[0] : "-");
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10f);
			if(c.TargetMeshInstance != null && c.extends?.Count == 1)
			{
				if(GUILayout.Button("Map Visemes & Expressions", GUILayout.ExpandWidth(false))) {
					c.Map();
				}
			}

			GUILayout.Space(10f);
			EditorGUILayout.LabelField("Mapped Visemes & Expressions");
			foreach(var m in c.Mappings)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel(m.VisemeName);
				m.BlendshapeName = EditorGUILayout.TextField(m.BlendshapeName);
				EditorGUILayout.EndHorizontal();
			}

			if(EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(c);
			}
		}
	}
}

#endif
