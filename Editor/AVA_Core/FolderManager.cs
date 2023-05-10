
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace stf.ava
{
	public class FolderManager
	{
		public const string AVA_IMPORT_FOLDER = "Assets/AVA_import";

		public static void clean(string assetName)
		{
			if(AssetDatabase.IsValidFolder(AVA_IMPORT_FOLDER + "/" + assetName))
			{
				if(!FileUtil.DeleteFileOrDirectory(AVA_IMPORT_FOLDER + "/" + assetName))
				{
					Debug.LogError("Could not delete: " + AVA_IMPORT_FOLDER + "/" + assetName);
				}
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			if(!AssetDatabase.IsValidFolder(AVA_IMPORT_FOLDER)) AssetDatabase.CreateFolder("Assets", "AVA_import");
			if(!AssetDatabase.IsValidFolder(AVA_IMPORT_FOLDER + "/" + assetName)) AssetDatabase.CreateFolder(AVA_IMPORT_FOLDER, assetName);
			if(!AssetDatabase.IsValidFolder(AVA_IMPORT_FOLDER + "/" + assetName + "/Common")) AssetDatabase.CreateFolder(AVA_IMPORT_FOLDER + "/" + assetName, "Common");
			AssetDatabase.SaveAssets();
		}

		public static void ensureApplicationFolderExists(string assetName, string application)
		{
			if(!AssetDatabase.IsValidFolder(AVA_IMPORT_FOLDER + "/" + assetName + "/" + application)) AssetDatabase.CreateFolder(AVA_IMPORT_FOLDER + "/" + assetName, application);
			AssetDatabase.SaveAssets();
		}

		public static string getCommonFolder(string assetName)
		{
			return AVA_IMPORT_FOLDER + "/" + assetName + "/Common";
		}

		public static string getApplicationFolder(string assetName, string application)
		{
			return AVA_IMPORT_FOLDER + "/" + assetName + "/" + application;
		}
	}
}

#endif
