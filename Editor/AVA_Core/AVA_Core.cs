using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace stf.ava
{
	public interface IAVAImporter
	{
		void run(GameObject go);
	}

	public class RegisteredImporters
	{
		private static List<IAVAImporter> importerList = new List<IAVAImporter>();

		public static void addImporter(IAVAImporter importer)
		{
			importerList.Add(importer);
		}

#if UNITY_EDITOR

		public static void run(GameObject go)
		{
			Debug.Log("Running AVA Importers");
			FolderManager.clean(go.name);
			foreach(IAVAImporter importer in importerList) // parallelize this
			{
				importer.run(go);
			}
		}
		
		[MenuItem("AVA/run")]
		public static void runManual()
		{
			var path = "";
			var obj = Selection.activeObject;
			if (obj == null) path = "Assets";
			else path = AssetDatabase.GetAssetPath(obj.GetInstanceID());
			if (path.Length > 0)
			{
				if (Directory.Exists(path))
				{
					Debug.Log("Folder");
				}
				else
				{
					GameObject root = (GameObject)AssetDatabase.LoadMainAssetAtPath(path);
					RegisteredImporters.run(root);
				}
			}
			else
			{
				Debug.Log("Not in assets folder");
			}
		}
#endif
	}
}
