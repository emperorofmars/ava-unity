
#if CVRCCK3_FOUND

using System;
using System.Collections.Generic;
using stf;
using stf.Components;
using stf.serialisation;
using UnityEngine;
using ava.Components;
using ava.Converters;
using UnityEngine.Animations;
using System.Threading.Tasks;
using ABI.CCK.Components;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava
{
	public class AVASecondStageCVR : ASTFSecondStageDefault
	{
		protected override Dictionary<Type, ISTFSecondStageResourceProcessor> ResourceProcessors => new Dictionary<Type, ISTFSecondStageResourceProcessor> {
#if UNITY_EDITOR
			{typeof(AnimationClip), new STFAnimationSecondStageProcessor()}
#endif
		};

		protected override Dictionary<Type, ISTFSecondStageConverter> Converters => new Dictionary<Type, ISTFSecondStageConverter>() {
			{typeof(AVAAvatar), new AVAAvatarCVRConverter()},
			{typeof(AVAFacialTrackingSimple), new AVAFacialTrackingSimpleCVRConverter()},
			{typeof(AVAAvatarVoice), new AVAAvatarVoiceCVRConverter()},
			{typeof(AVAEyeBoneLimitsSimple), new AVAEyeBoneLimitsSimpleCVRConverter()},
#if DynamicBones_FOUND
			{typeof(AVAJankyFallbackPhysics), new AVAJankyFallbackPhysicsCVRConverter()},
#endif
		};

		protected override List<Type> WhitelistedComponents => new List<Type> {
			typeof(Transform), typeof(Animator), typeof(RotationConstraint), typeof(SkinnedMeshRenderer), typeof(CVRAvatar), typeof(CVRAssetInfo),
#if DynamicBones_FOUND
			typeof(DynamicBone),
#endif
		};

		protected override string GameObjectSuffix => "CVR";
		protected override string StageName => "ChilloutVR";
		protected override string AssetTypeName => "ChilloutVR Avatar";
		protected override List<string> Targets => new List<string> {
			"unity",
			"chilloutvr",
#if DynamicBones_FOUND
			"dynamic_bones"
#endif
		};

		public override bool CanHandle(ISTFAsset asset, UnityEngine.Object adaptedUnityAsset)
		{
			return asset.GetSTFAssetType() == STFAssetExporter._TYPE && asset.GetAsset().GetType() == typeof(GameObject) && ((GameObject)asset.GetAsset()).GetComponent<AVAAvatar>() != null;
		}

#if UNITY_EDITOR
		override public string GetPathForResourcesThatMustExistInFS(ISTFAsset asset, UnityEngine.Object adaptedUnityAsset)
		{
			AssetDatabase.CreateFolder(AVASecondStage.PathForResourcesThatMustExistInFS + "/" + asset.getId(), GameObjectSuffix);
			return AVASecondStage.PathForResourcesThatMustExistInFS + "/" + asset.getId() + "/" + GameObjectSuffix;
		}
#endif
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVASecondStageCVR
	{
		static Register_AVASecondStageCVR()
		{
			AVASecondStage.RegisterStage(new AVASecondStageCVR());
			Debug.Log("Registered AVA ChilloutVR Loader");
		}
	}
#endif

}
#endif
