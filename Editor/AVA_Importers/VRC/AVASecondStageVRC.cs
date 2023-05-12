
#if VRCSDK3_FOUND

using System;
using System.Collections.Generic;
using stf;
using stf.Components;
using stf.serialisation;
using UnityEngine;
using ava.Components;
using ava.Converters;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava
{
	public class AVASecondStageVRC : ISTFSecondStage
	{
		private Dictionary<Type, ISTFSecondStageConverter> converters = new Dictionary<Type, ISTFSecondStageConverter>() {
			{typeof(STFTwistConstraintBack), new STFTwistConstraintBackConverter()},
			{typeof(AVAAvatar), new AVAAvatarVRCConverter()}
		};
		
		public bool CanHandle(ISTFAsset asset)
		{
			return asset.GetSTFAssetType() == "asset" && asset.GetAsset().GetType() == typeof(GameObject) && ((GameObject)asset.GetAsset()).GetComponent<AVAAvatar>() != null;
		}

		public SecondStageResult Convert(ISTFAsset asset)
		{
			var originalRoot = (GameObject)asset.GetAsset();

			GameObject convertedRoot = UnityEngine.Object.Instantiate(originalRoot);
			convertedRoot.name = originalRoot.name;

			convertTree(convertedRoot);

			var secondStageAsset = new STFSecondStageAsset(convertedRoot, asset.getId() + "_VRC", asset.GetSTFAssetName(), "VRChat Avatar");
			return new SecondStageResult {assets = new List<ISTFAsset>{secondStageAsset}, resources = new List<UnityEngine.Object>{}};
		}

		private void convertTree(GameObject root)
		{
			foreach(var converter in converters)
			{
				var components = root.GetComponentsInChildren(converter.Key);
				foreach(var component in components)
				{
					converter.Value.convert(component, root);
				}
			}
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVASecondStageVRC
	{
		static Register_AVASecondStageVRC()
		{
			STFImporterStageRegistry.RegisterStage(new AVASecondStageVRC());
			Debug.Log("Registered AVA VRC Loader");
		}
	}
#endif
}

#endif
