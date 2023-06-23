
//#if VSFSDK_FOUND

using System;
using System.Collections.Generic;
using stf;
using stf.Components;
using stf.serialisation;
using UnityEngine;
using ava.Components;
using UnityEngine.Animations;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava
{
	public class AVASecondStageVSF : ISTFSecondStage
	{
		private Dictionary<Type, ISTFSecondStageConverter> converters = new Dictionary<Type, ISTFSecondStageConverter>() {
			//{typeof(AVAAvatar), new AVAAvatarVRCConverter()},
			//{typeof(AVAEyeBoneLimitsSimple), new AVAEyeBoneLimitsSimpleVRCConverter()}
		};

		private static List<Type> WhitelistedComponents = new List<Type> {
			typeof(Transform), typeof(Animator), typeof(RotationConstraint), typeof(SkinnedMeshRenderer)//, typeof(VRCAvatarDescriptor), typeof(VRCPipelineManagerEditor)
		};
		
		public bool CanHandle(ISTFAsset asset, UnityEngine.Object adaptedUnityAsset)
		{
			return asset.GetSTFAssetType() == "asset" && asset.GetAsset().GetType() == typeof(GameObject) && ((GameObject)asset.GetAsset()).GetComponent<AVAAvatar>() != null;
		}

		public SecondStageResult Convert(ISTFAsset asset, UnityEngine.Object adaptedUnityAsset)
		{
			var originalRoot = (GameObject)adaptedUnityAsset;
			var convertedResources = new List<UnityEngine.Object>();

			GameObject convertedRoot = UnityEngine.Object.Instantiate(originalRoot);
			convertedRoot.name = originalRoot.name + "_VSF";
			try
			{
				var context = new STFSecondStageContext {RelMat = new STFRelationshipMatrix(convertedRoot, new List<string> {"unity", "vrm", "vseeface"})};
				convertTree(convertedRoot, convertedResources, context);
				do
				{
					var currentTasks = context.Tasks;
					context.Tasks = new List<Task>();
					foreach(var task in currentTasks)
					{
						task.RunSynchronously();
						if(task.Exception != null) throw task.Exception;
					}
				}
				while(context.Tasks.Count > 0);
				cleanup(convertedRoot);
			}
			catch(Exception e)
			{
				#if UNITY_EDITOR
					UnityEngine.Object.DestroyImmediate(convertedRoot);
				#else
					UnityEngine.Object.Destroy(convertedRoot);
				#endif
				throw new Exception("Error during AVA VSeeFace Loader import: ", e);
			}

			var secondStageAsset = new STFSecondStageAsset(convertedRoot, asset.getId() + "_VSF", asset.GetSTFAssetName(), "VSeeFace Avatar");
			return new SecondStageResult {assets = new List<ISTFAsset>{secondStageAsset}, resources = convertedResources};
		}

		private void convertTree(GameObject root, List<UnityEngine.Object> resources, STFSecondStageContext context)
		{
			foreach(var converter in converters)
			{
				var components = root.GetComponentsInChildren(converter.Key);
				foreach(var component in components)
				{
					if(!context.RelMat.IsOverridden.Contains(component))
						converter.Value.convert(component, root, resources, context);
				}
			}
		}

		private void cleanup(GameObject root)
		{
			foreach(var component in root.GetComponentsInChildren<Component>())
			{
				if(!WhitelistedComponents.Contains(component.GetType()))
				{
					#if UNITY_EDITOR
						UnityEngine.Object.DestroyImmediate(component);
					#else
						UnityEngine.Object.Destroy(component);
					#endif
				}
			}
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVASecondStageVSF
	{
		static Register_AVASecondStageVSF()
		{
			AVASecondStage.RegisterStage(new AVASecondStageVSF());
			Debug.Log("Registered AVA VSeeFace Loader");
		}
	}
#endif

}
//#endif
