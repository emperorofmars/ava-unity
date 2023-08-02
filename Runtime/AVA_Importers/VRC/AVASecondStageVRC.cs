
#if VRCSDK3_FOUND

using System;
using System.Collections.Generic;
using stf;
using stf.Components;
using stf.serialisation;
using UnityEngine;
using ava.Components;
using ava.Converters;
using VRC.SDK3.Avatars.Components;
using UnityEngine.Animations;
using System.Threading.Tasks;
using VRC.SDK3.Dynamics.PhysBone.Components;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava
{
	public class AVASecondStageVRC : ASTFSecondStageDefault
	{
		protected override Dictionary<Type, ISTFSecondStageResourceProcessor> ResourceProcessors => new Dictionary<Type, ISTFSecondStageResourceProcessor> {
#if UNITY_EDITOR
			{typeof(AnimationClip), new STFAnimationSecondStageProcessor()}
#endif
		};

		protected override Dictionary<Type, ISTFSecondStageConverter> Converters => new Dictionary<Type, ISTFSecondStageConverter>() {
			{typeof(AVAAvatar), new AVAAvatarVRCConverter()},
			{typeof(AVAEyeBoneLimitsSimple), new AVAEyeBoneLimitsSimpleVRCConverter()},
			{typeof(AVAFacialTrackingSimple), new AVAFacialTrackingSimpleVRCConverter()},
			{typeof(AVAJankyFallbackPhysics), new AVAJankyFallbackPhysicsVRCConverter()},
			{typeof(AVAVRCPhysbones), new AVAPhysboneVRCConverter()},
#if UNITY_EDITOR
			{typeof(AVAExpressionsSimple), new AVAExpressionsSimpleVRCConverter()}
#endif
		};

		protected override List<Type> WhitelistedComponents => new List<Type> {
			typeof(Transform), typeof(Animator), typeof(RotationConstraint), typeof(SkinnedMeshRenderer), typeof(VRCAvatarDescriptor), typeof(VRCPipelineManagerEditor), typeof(VRCPhysBone)
		};


		protected override string GameObjectSuffix => "VRC";
		protected override string StageName => "VRChat";
		protected override string AssetTypeName => "VRChat Avatar";
		protected override List<string> Targets => new List<string> {"unity", "vrchat"};

		public override bool CanHandle(ISTFAsset asset, UnityEngine.Object adaptedUnityAsset)
		{
			return asset.GetSTFAssetType() == STFAssetExporter._TYPE && asset.GetAsset().GetType() == typeof(GameObject) && ((GameObject)asset.GetAsset()).GetComponent<AVAAvatar>() != null;
		}

#if UNITY_EDITOR
		override public string GetPathForResourcesThatMustExistInFS(ISTFAsset asset, UnityEngine.Object adaptedUnityAsset)
		{
			AssetDatabase.CreateFolder(AVASecondStage.PathForResourcesThatMustExistInFS + "/" + asset.getId(), StageName);
			AssetDatabase.Refresh();
			return AVASecondStage.PathForResourcesThatMustExistInFS + "/" + asset.getId() + "/" + StageName;
		}
#endif
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVASecondStageVRC
	{
		static Register_AVASecondStageVRC()
		{
			AVASecondStage.RegisterStage(new AVASecondStageVRC());
			Debug.Log("Registered AVA VRChat Loader");
		}
	}
#endif

}
#endif
