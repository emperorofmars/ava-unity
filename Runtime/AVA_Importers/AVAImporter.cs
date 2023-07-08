
using System;
using System.Collections.Generic;
using stf;
using stf.Components;
using stf.serialisation;
using UnityEngine;
using ava.Components;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava
{
	public class AVASecondStage : ISTFSecondStage
	{
		private static string AssetFolderForResourcesThatMustExistInFS = "ava-unity-imported-resources";
		public static string PathForResourcesThatMustExistInFS = "Assets/ava-unity-imported-resources";
		private static List<ISTFSecondStage> RegisteredApplicationStages = new List<ISTFSecondStage>();
		public static void RegisterStage(ISTFSecondStage stage)
		{
			RegisteredApplicationStages.Add(stage);
		}
		private static Dictionary<Type, ISTFSecondStageConverter> PreStageConverters = new Dictionary<Type, ISTFSecondStageConverter>() {
			{typeof(STFTwistConstraintBack), new STFTwistConstraintBackConverter()},
			{typeof(STFTwistConstraintForward), new STFTwistConstraintForwardConverter()}
		};
		public static void RegisterPreStageConverter(Type type, ISTFSecondStageConverter converter)
		{
			PreStageConverters.Add(type, converter);
		}
		
		public bool CanHandle(ISTFAsset asset, UnityEngine.Object adaptedUnityAsset)
		{
			return asset.GetSTFAssetType() == "asset" && asset.GetAsset().GetType() == typeof(GameObject) && ((GameObject)asset.GetAsset()).GetComponent<AVAAvatar>() != null;
		}

		public SecondStageResult Convert(ISTFAsset asset, UnityEngine.Object adaptedUnityAsset)
		{
			var originalRoot = (GameObject)adaptedUnityAsset;
			var convertedAssets = new List<ISTFAsset>();
			var convertedResources = new List<UnityEngine.Object>();

			GameObject tmpRoot = UnityEngine.Object.Instantiate(originalRoot);
			tmpRoot.name = originalRoot.name;

#if UNITY_EDITOR
			if(!Directory.Exists(PathForResourcesThatMustExistInFS))
			{
				AssetDatabase.CreateFolder("Assets", AssetFolderForResourcesThatMustExistInFS);
				AssetDatabase.Refresh();
			}
			if(Directory.Exists(PathForResourcesThatMustExistInFS + "/" + asset.getId()))
			{
				AssetDatabase.DeleteAsset(PathForResourcesThatMustExistInFS + "/" + asset.getId());
				AssetDatabase.Refresh();
			}
			AssetDatabase.CreateFolder(PathForResourcesThatMustExistInFS, asset.getId());
			AssetDatabase.Refresh();
#endif
			try
			{
				// handle path
				var context = new STFSecondStageContext(tmpRoot, new List<string> {"unity"}, new List<Type>(), new Dictionary<Type, ISTFSecondStageResourceProcessor>());
				convertTree(tmpRoot, convertedResources, context);
				var intermediaryAsset = new STFSecondStageAsset(tmpRoot, asset.getId(), asset.GetSTFAssetName());
				if(context.ResourceConversions.Count > 0) convertedResources.AddRange(context.ResourceConversions.Values);
				if(context.Resources.Count > 0) convertedResources.AddRange(context.Resources);
				foreach(var appStage in RegisteredApplicationStages)
				{
					var result = appStage.Convert(intermediaryAsset, intermediaryAsset.GetAsset());
					convertedAssets.AddRange(result.assets);
					convertedResources.AddRange(result.resources);
				}
			}
			catch(Exception e)
			{
				throw new Exception("Error during AVA Loader import: ", e);
			}
			finally
			{
				#if UNITY_EDITOR
					UnityEngine.Object.DestroyImmediate(tmpRoot);
				#else
					UnityEngine.Object.Destroy(tmpRoot);
				#endif
			}
			
#if UNITY_EDITOR
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
#endif
			
			return new SecondStageResult {assets = convertedAssets, resources = convertedResources};
		}

		private void convertTree(GameObject root, List<UnityEngine.Object> resources, STFSecondStageContext context)
		{
			foreach(var converter in PreStageConverters)
			{
				var components = root.GetComponentsInChildren(converter.Key);
				foreach(var component in components)
				{
					converter.Value.Convert(component, root, context);
				}
			}
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVASecondStage
	{
		static Register_AVASecondStage()
		{
			STFImporterStageRegistry.RegisterStage(new AVASecondStage());
			Debug.Log("Registered AVA Loader");
		}
	}
#endif
}
