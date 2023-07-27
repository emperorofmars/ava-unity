
#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ava
{
	[InitializeOnLoad]
	public class AVATargetDetector
	{
		const string VRCSDK3_FOUND = "VRCSDK3_FOUND";
		const string CVRCCK3_FOUND = "CVRCCK3_FOUND";
		const string UNIVRM_FOUND = "UNIVRM_FOUND";
		const string DynamicBones_FOUND = "DynamicBones_FOUND";

		static AVATargetDetector()
		{
			if(Directory.GetFiles(Path.GetDirectoryName(Application.dataPath), "VRCAvatarDescriptorEditor3.cs", SearchOption.AllDirectories).Length > 0)
			{
				Debug.Log("Found VRC SDK 3");
				ScriptDefinesManager.AddDefinesIfMissing(BuildTargetGroup.Standalone, VRCSDK3_FOUND);
			}
			else
			{
				Debug.Log("Didn't find VRC SDK 3");
				ScriptDefinesManager.RemoveDefines(VRCSDK3_FOUND);
			}
			
			if(Directory.GetFiles(Path.GetDirectoryName(Application.dataPath), "CVRAvatar.cs", SearchOption.AllDirectories).Length > 0)
			{
				Debug.Log("Found CVR CCK 3");
				ScriptDefinesManager.AddDefinesIfMissing(BuildTargetGroup.Standalone, CVRCCK3_FOUND);
			}
			else
			{
				Debug.Log("Didn't find CVR CCK 3");
				ScriptDefinesManager.RemoveDefines(CVRCCK3_FOUND);
			}

			if(Directory.GetFiles(Path.GetDirectoryName(Application.dataPath), "IVRMComponent.cs", SearchOption.AllDirectories).Length > 0)
			{
				Debug.Log("Found UNIVRM");
				ScriptDefinesManager.AddDefinesIfMissing(BuildTargetGroup.Standalone, UNIVRM_FOUND);
			}
			else
			{
				Debug.Log("Didn't find UNIVRM");
				ScriptDefinesManager.RemoveDefines(UNIVRM_FOUND);
			}
			
			if(Directory.GetFiles(Path.GetDirectoryName(Application.dataPath), "DynamicBone.cs", SearchOption.AllDirectories).Length > 0)
			{
				Debug.Log("Found Dynamic Bones");
				ScriptDefinesManager.AddDefinesIfMissing(BuildTargetGroup.Standalone, DynamicBones_FOUND);
			}
			else
			{
				Debug.Log("Didn't find Dynamic Bones");
				ScriptDefinesManager.RemoveDefines(DynamicBones_FOUND);
			}
		}
	}
}
#endif
