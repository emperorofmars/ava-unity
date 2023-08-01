
#if UNIVRM_FOUND

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
using System.IO;
using VRM;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava
{
	public class AVASecondStageVRM : ASTFSecondStageDefault
	{
		protected override Dictionary<Type, ISTFSecondStageResourceProcessor> ResourceProcessors => new Dictionary<Type, ISTFSecondStageResourceProcessor> {
#if UNITY_EDITOR
			{typeof(AnimationClip), new STFAnimationSecondStageProcessor()}
#endif
		};

		protected override Dictionary<Type, ISTFSecondStageConverter> Converters => new Dictionary<Type, ISTFSecondStageConverter>() {
			{typeof(AVAAvatar), new AVAAvatarVRMConverter()},
			{typeof(AVAEyeBoneLimitsSimple), new AVAEyeBoneLimitsSimpleVRMConverter()},
			{typeof(AVAFacialTrackingSimple), new AVAFacialTrackingSimpleVRMConverter()},
			/*{typeof(AVAJankyFallbackPhysics), new AVAJankyFallbackPhysicsVRCConverter()},
			{typeof(AVAVRCPhysbones), new AVAPhysboneVRCConverter()},
#if UNITY_EDITOR
			{typeof(AVAExpressionsSimple), new AVAExpressionsSimpleVRCConverter()}
#endif*/
		};

		protected override List<Type> WhitelistedComponents => new List<Type> {
			typeof(Transform), typeof(Animator), typeof(RotationConstraint), typeof(SkinnedMeshRenderer), typeof(VRMMeta), typeof(VRMLookAtHead), typeof(VRMHumanoidDescription), typeof(VRMFirstPerson), typeof(VRMLookAtBoneApplyer), typeof(VRMBlendShapeProxy)
		};


		protected override string GameObjectSuffix => "VRM";
		protected override string StageName => "VRM";
		protected override string AssetTypeName => "VRM Avatar";
		protected override List<string> Targets => new List<string> {"unity", "gltf", "vrm"};

		public override bool CanHandle(ISTFAsset asset, UnityEngine.Object adaptedUnityAsset)
		{
			return asset.GetSTFAssetType() == "asset" && asset.GetAsset().GetType() == typeof(GameObject) && ((GameObject)asset.GetAsset()).GetComponent<AVAAvatar>() != null && ((GameObject)asset.GetAsset()).GetComponent<AVAHumanoidMapping>() != null;
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
	public class Register_AVASecondStageVRM
	{
		static Register_AVASecondStageVRM()
		{
			AVASecondStage.RegisterStage(new AVASecondStageVRM());
			Debug.Log("Registered AVA VRM Loader");
		}
	}
#endif

}
#endif
