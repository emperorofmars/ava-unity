using System;
using System.Collections.Generic;
using stf.serialisation;
using stf;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava.Components
{
	[Serializable]
	public class BoneMappingPair
	{
		public BoneMappingPair(string bone, string uuid)
		{
			this.bone = bone;
			this.uuid = uuid;
		}

		public string bone;
		public string uuid;
	}

	public class AVAHumanoidMapping : MonoBehaviour, ISTFComponent
	{
		public string _id;
		public string id {get => _id; set => _id = value;}
		public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		public List<string> _targets;
		public List<string> targets {get => _targets; set => _targets = value;}
		
		public static string _TYPE = "AVA.humanoid_mappings";
		
		public static readonly Dictionary<string, string> translations = new Dictionary<string, string> {
			{"Hip", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.Hips)},
			{"Spine", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.Spine)},
			{"Chest", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.Chest)},
			{"UpperChest", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.UpperChest)},
			{"Neck", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.Neck)},
			{"Head", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.Head)},
			{"Jaw", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.Jaw)},
			{"EyeLeft", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftEye)},
			{"EyeRight", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightEye)},
			{"ShoulderLeft", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftShoulder)},
			{"UpperArmLeft", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftUpperArm)},
			{"LowerArmLeft", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftLowerArm)},
			{"HandLeft", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftHand)},
			{"ShoulderRight", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightShoulder)},
			{"UpperArmRight", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightUpperArm)},
			{"LowerArmRight", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightLowerArm)},
			{"HandRight", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightHand)},
			{"UpperLegLeft", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftUpperLeg)},
			{"LowerLegLeft", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftLowerLeg)},
			{"FootLeft", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftFoot)},
			{"ToesLeft", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftToes)},
			{"UpperLegRight", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightUpperLeg)},
			{"LowerLegRight", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightLowerLeg)},
			{"FootRight", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightFoot)},
			{"ToesRight", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightToes)},
			{"FingerThumb1Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftThumbProximal)},
			{"FingerThumb2Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftThumbIntermediate)},
			{"FingerThumb3Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftThumbDistal)},
			{"FingerIndex1Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftIndexProximal)},
			{"FingerIndex2Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftIndexIntermediate)},
			{"FingerIndex3Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftIndexDistal)},
			{"FingerMiddle1Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftMiddleProximal)},
			{"FingerMiddle2Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftMiddleIntermediate)},
			{"FingerMiddle3Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftMiddleDistal)},
			{"FingerRing1Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftRingProximal)},
			{"FingerRing2Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftRingIntermediate)},
			{"FingerRing3Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftRingDistal)},
			{"FingerLittle1Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftLittleProximal)},
			{"FingerLittle2Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftLittleIntermediate)},
			{"FingerLittle3Left", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.LeftLittleDistal)},
			{"FingerThumb1Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightThumbProximal)},
			{"FingerThumb2Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightThumbIntermediate)},
			{"FingerThumb3Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightThumbDistal)},
			{"FingerIndex1Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightIndexProximal)},
			{"FingerIndex2Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightIndexIntermediate)},
			{"FingerIndex3Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightIndexDistal)},
			{"FingerMiddle1Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightMiddleProximal)},
			{"FingerMiddle2Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightMiddleIntermediate)},
			{"FingerMiddle3Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightMiddleDistal)},
			{"FingerRing1Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightRingProximal)},
			{"FingerRing2Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightRingIntermediate)},
			{"FingerRing3Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightRingDistal)},
			{"FingerLittle1Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightLittleProximal)},
			{"FingerLittle2Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightLittleIntermediate)},
			{"FingerLittle3Right", Enum.GetName(typeof(HumanBodyBones), HumanBodyBones.RightLittleDistal)},
		};

		public STFArmatureInstance armatureInstance;
		public string locomotion_type;
		public List<BoneMappingPair> mappings = new List<BoneMappingPair>();

		public AVAHumanoidMapping()
		{
			foreach(string avaBone in translations.Keys)
			{
				mappings.Add(new BoneMappingPair(avaBone, null));
			}
		}

		public string translateHumanoidAVAtoUnity(string avaName, string locomotion_type)
		{
			if(locomotion_type == "1")
			{
				switch(avaName)
				{
					case "ToesLeft":
						return translations["FootLeft"];
					case "ToesRight":
						return translations["FootRight"];
					case "FootLeft":
						return null;
					case "FootRight":
						return null;
				}
			}
			if(avaName == "Jaw") return null;
			return translations[avaName];
		}
	}

	public class AVAHumanoidMappingImporter : ASTFComponentImporter
	{
		public override void parseFromJson(ISTFImporter state, JToken json, string id, GameObject go)
		{
			var c = go.AddComponent<AVAHumanoidMapping>();
			c.locomotion_type = (string)json["locomotion_type"];
			state.AddTask(new Task(() => {
				c.armatureInstance = state.GetNode((string)json["target_armature_instance"]).GetComponent<STFArmatureInstance>();
			}));
			c.mappings = new List<BoneMappingPair>();
			foreach(var t in AVAHumanoidMapping.translations)
			{
				c.mappings.Add(new BoneMappingPair(t.Key, (string)json[t.Key]));
			}
		}
	}

	public class AVAHumanoidMappingExporter : ASTFComponentExporter
	{
		override public List<GameObject> gatherNodes(Component component)
		{
			var c = (AVAHumanoidMapping)component;
			var ret = new List<GameObject>();
			if(c.armatureInstance) ret.Add(c.armatureInstance.gameObject);
			return ret;
		}

		public override JToken serializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAHumanoidMapping)component;
			var ret = new JObject();
			ret.Add("type", AVAHumanoidMapping._TYPE);
			ret.Add("locomotion_type", c.locomotion_type);
			ret.Add("target_armature_instance", state.GetNodeId(c.armatureInstance.gameObject));
			foreach(var mapping in c.mappings)
			{
				ret.Add(mapping.bone, mapping.uuid);
			}
			return ret;
		}
	}
	public class AVAHumanoidMappingConverter : ISTFSecondStageConverter
	{
		private class HumanoidTransformMapping
		{
			public HumanoidTransformMapping(string humanoidName, GameObject go)
			{
				this.humanoidName = humanoidName;
				this.go = go;
			}

			public string humanoidName;
			public GameObject go;
		}

		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources)
		{
			var stfComponent = (AVAHumanoidMapping)component;
			var animator = root.AddComponent<Animator>();
			animator.applyRootMotion = true;
			animator.updateMode = AnimatorUpdateMode.Normal;
			animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			
			var name_mappings = stfComponent.mappings.FindAll(mapping => mapping.bone != null && mapping.bone.Length > 0 && mapping.uuid != null && mapping.uuid.Length > 0);
			var mappings = new List<HumanoidTransformMapping>();
			foreach(var mapping in name_mappings)
			{
				var humanName = stfComponent.translateHumanoidAVAtoUnity(mapping.bone, stfComponent.locomotion_type);
				var go = TreeUtils.findByBoneId(root, mapping.uuid);
				if(humanName != null && humanName.Length > 0 && go != null) mappings.Add(new HumanoidTransformMapping(humanName, go));
			}

			var humanDescription = new HumanDescription
			{
				armStretch = 0.05f,
				feetSpacing = 0f,
				hasTranslationDoF = false,
				legStretch = 0.05f,
				lowerArmTwist = 0.5f,
				lowerLegTwist = 0.5f,
				upperArmTwist = 0.5f,
				upperLegTwist = 0.5f,
				skeleton = (new List<Transform>(root.GetComponentsInChildren<Transform>())).Select(t => {
					return new SkeletonBone()
					{
						name = t.name,
						position = t.localPosition,
						rotation = t.localRotation,
						scale = t.localScale
					};
				}).ToArray(),
				human = mappings.Select(mapping => 
				{
					var bone = new HumanBone {
						humanName = mapping.humanoidName,
						boneName = mapping.go.name,
						limit = new HumanLimit()
					};
					//Debug.Log(bone.humanName + " : " + bone.boneName);
					bone.limit.useDefaultValues = true;
					return bone;
				}).ToArray()
			};

			var avatar = AvatarBuilder.BuildHumanAvatar(root, humanDescription);
			avatar.name = root.name;
			if (!avatar.isValid)
			{
				Debug.LogError("Invalid humanoid avatar");
			}
			animator.avatar = avatar;
			resources.Add(avatar);

			#if UNITY_EDITOR
            UnityEngine.Object.DestroyImmediate(component);
			#else
            UnityEngine.Object.Destroy(component);
			#endif
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAHumanoidMapping
	{
		static Register_AVAHumanoidMapping()
		{
			STFRegistry.RegisterComponentImporter(AVAHumanoidMapping._TYPE, new AVAHumanoidMappingImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAHumanoidMapping), new AVAHumanoidMappingExporter());
			AVASecondStage.RegisterPreStageConverter(typeof(AVAHumanoidMapping), new AVAHumanoidMappingConverter());
		}
	}
#endif
}
