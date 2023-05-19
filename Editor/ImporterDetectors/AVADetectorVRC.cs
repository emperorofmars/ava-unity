
#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace ava
{
	[InitializeOnLoad]
	public class AVADetectorVRC
	{
		const string VRCSDK3_FOUND = "VRCSDK3_FOUND";
		static AVADetectorVRC()
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
		}
	}
}

#endif
