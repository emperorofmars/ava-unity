
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
	[CustomEditor(typeof(AVAAvatar))]
	public class AVAAvatarInspector : Editor
	{
		private bool editPosition = false;

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			var c = (AVAAvatar)target;

			EditorGUI.BeginChangeCheck();

			var humanoid = c.GetComponent<AVAHumanoidMapping>();
			if(humanoid != null && humanoid.mappings != null)
			{
				var eyeLeft = humanoid.mappings.Find(m => m.humanoidName == "EyeLeft");
				var eyeRight = humanoid.mappings.Find(m => m.humanoidName == "EyeRight");
				var viewport_parent = c.viewport_parent != null ? c.viewport_parent : humanoid.mappings.Find(m => m.humanoidName == "Head")?.bone;
				if(eyeLeft != null && eyeRight != null && viewport_parent != null)
				{
					GUILayout.Space(10f);
					if(GUILayout.Button("Set viewport between the eyes", GUILayout.ExpandWidth(false)))
					{
						if(c.viewport_parent == null) c.viewport_parent = viewport_parent;

						c.viewport_position = ((eyeLeft.bone.transform.position + eyeRight.bone.transform.position) / 2) - c.viewport_parent.transform.position;
						c.viewport_position.x = Math.Abs(c.viewport_position.x) < 0.0001 ? 0 : c.viewport_position.x;
						c.viewport_position.y = Math.Abs(c.viewport_position.y) < 0.0001 ? 0 : c.viewport_position.y;
						c.viewport_position.z = Math.Abs(c.viewport_position.z) < 0.0001 ? 0 : c.viewport_position.z;
					}
				}
				else
				{
					EditorGUILayout.LabelField("Humanoid mappings for the head and/or eyes not found!");
				}
				
				GUILayout.Space(10f);
				if(!editPosition && GUILayout.Button("Edit viewport", GUILayout.ExpandWidth(false))) editPosition = true;
				else if(editPosition && GUILayout.Button("Stop editing viewport", GUILayout.ExpandWidth(false))) editPosition = false;
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
			var c = (AVAAvatar)target;
			
			if(c.viewport_parent && editPosition)
			{
				Handles.Label(c.viewport_parent.transform.position + c.viewport_position, "Viewport");
				c.viewport_position = Handles.DoPositionHandle(c.viewport_parent.transform.position + c.viewport_position, Quaternion.identity) - c.viewport_parent.transform.position;
			}
		}
		
		[DrawGizmo(GizmoType.Selected)]
		public static void OnDrawGizmo(AVAAvatar target, GizmoType gizmoType)
		{
			var c = (AVAAvatar)target;

			if(c && c.viewport_parent)
			{
				Gizmos.DrawSphere(c.viewport_parent.transform.position + c.viewport_position, 0.01f);
			}
		}
	}
}

#endif
