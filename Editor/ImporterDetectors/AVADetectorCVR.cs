
#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ava
{
	[InitializeOnLoad]
	public class AVADetectorCVR
	{
		const string CVRCCK3_FOUND = "CVRCCK3_FOUND";
		static AVADetectorCVR()
		{
			if(Directory.GetFiles(Path.GetDirectoryName(Application.dataPath), "CVRAvatar.cs", SearchOption.AllDirectories).Length > 0)
			//if(Type.GetType("CVRAvatar") != null)
			{
				Debug.Log("Found CVR CCK 3");
				ScriptDefinesManager.AddDefinesIfMissing(BuildTargetGroup.Standalone, CVRCCK3_FOUND);
			}
			else
			{
				Debug.Log("Didn't find CVR CCK 3");
				ScriptDefinesManager.RemoveDefines(CVRCCK3_FOUND);
			}
		}
	}
}

#endif
