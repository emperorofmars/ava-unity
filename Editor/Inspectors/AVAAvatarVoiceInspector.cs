
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
	[CustomEditor(typeof(AVAAvatarVoice))]
	public class AVAAvatarVoiceInspector : Editor
	{
		private bool editPosition = false;

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			var c = (AVAAvatarVoice)target;

			EditorGUI.BeginChangeCheck();

			var humanoid = c.GetComponent<AVAHumanoidMapping>();
			if(humanoid != null && humanoid.mappings != null)
			{
				var voice_parent = c.voice_parent != null ? c.voice_parent : humanoid.mappings.Find(m => m.humanoidName == "Head")?.bone;
				if(voice_parent != null)
				{
					GUILayout.Space(10f);
					if(GUILayout.Button("Setup voice position", GUILayout.ExpandWidth(false)))
					{
						if(c.voice_parent == null) c.voice_parent = voice_parent;

						c.voice_position = new Vector3(0f, voice_parent.transform.position.y * 0.02f, voice_parent.transform.position.y * 0.04f);
						c.voice_position.y = Math.Abs(c.voice_position.y) < 0.0001 ? 0 : c.voice_position.y;
						c.voice_position.z = Math.Abs(c.voice_position.z) < 0.0001 ? 0 : c.voice_position.z;
					}
				}
				else
				{
					EditorGUILayout.LabelField("Humanoid mappings for the head and/or eyes not found!");
				}
				
				GUILayout.Space(10f);
				if(!editPosition && GUILayout.Button("Edit voice position", GUILayout.ExpandWidth(false))) editPosition = true;
				else if(editPosition && GUILayout.Button("Stop editing voice position", GUILayout.ExpandWidth(false))) editPosition = false;
			}
			else
			{
				EditorGUILayout.LabelField("Humanoid mappings not set up!");
			}

			if(EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(c);
			}
		}

		public void OnSceneGUI()
		{
			var c = (AVAAvatarVoice)target;
			
			if(c.voice_parent && editPosition)
			{
				Handles.Label(c.voice_parent.transform.position + c.voice_position, "Voice");
				c.voice_position = Handles.DoPositionHandle(c.voice_parent.transform.position + c.voice_position, Quaternion.identity) - c.voice_parent.transform.position;
			}
		}
		
		[DrawGizmo(GizmoType.Selected)]
		public static void OnDrawGizmo(AVAAvatarVoice target, GizmoType gizmoType)
		{
			var c = (AVAAvatarVoice)target;

			if(c && c.voice_parent)
			{
				Gizmos.DrawSphere(c.voice_parent.transform.position + c.voice_position, 0.01f);
			}
		}
	}
}

#endif
