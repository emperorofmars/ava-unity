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
		public BoneMappingPair(string humanoidName, GameObject bone)
		{
			this.humanoidName = humanoidName;
			this.bone = bone;
		}

		public string humanoidName;
		public GameObject bone;
	}

	public class AVAHumanoidMapping : MonoBehaviour, ISTFComponent
	{
		[HideInInspector] public string _id;
		public string id {get => _id; set => _id = value;}
		[HideInInspector] public List<string> _extends;
		public List<string> extends {get => _extends; set => _extends = value;}
		[HideInInspector] public List<string> _overrides;
		public List<string> overrides {get => _overrides; set => _overrides = value;}
		[HideInInspector] public List<string> _targets;
		public List<string> targets {get => _targets; set => _targets = value;}
		
		public static readonly string _TYPE = "AVA.humanoid_mappings";
		
		public static readonly Dictionary<string, string> _Translations = new Dictionary<string, string> {
			{"Hip", HumanBodyBones.Hips.ToString()},
			{"Spine", HumanBodyBones.Spine.ToString()},
			{"Chest", HumanBodyBones.Chest.ToString()},
			{"UpperChest", HumanBodyBones.UpperChest.ToString()},
			{"Neck", HumanBodyBones.Neck.ToString()},
			{"Head",  HumanBodyBones.Head.ToString()},
			{"Jaw", HumanBodyBones.Jaw.ToString()},
			{"EyeLeft", HumanBodyBones.LeftEye.ToString()},
			{"EyeRight", HumanBodyBones.RightEye.ToString()},
			{"ShoulderLeft", HumanBodyBones.LeftShoulder.ToString()},
			{"UpperArmLeft", HumanBodyBones.LeftUpperArm.ToString()},
			{"LowerArmLeft", HumanBodyBones.LeftLowerArm.ToString()},
			{"HandLeft", HumanBodyBones.LeftHand.ToString()},
			{"FingerThumb1Left", HumanBodyBones.LeftThumbProximal.ToString()},
			{"FingerThumb2Left", HumanBodyBones.LeftThumbIntermediate.ToString()},
			{"FingerThumb3Left", HumanBodyBones.LeftThumbDistal.ToString()},
			{"FingerIndex1Left", HumanBodyBones.LeftIndexProximal.ToString()},
			{"FingerIndex2Left", HumanBodyBones.LeftIndexIntermediate.ToString()},
			{"FingerIndex3Left", HumanBodyBones.LeftIndexDistal.ToString()},
			{"FingerMiddle1Left", HumanBodyBones.LeftMiddleProximal.ToString()},
			{"FingerMiddle2Left", HumanBodyBones.LeftMiddleIntermediate.ToString()},
			{"FingerMiddle3Left", HumanBodyBones.LeftMiddleDistal.ToString()},
			{"FingerRing1Left", HumanBodyBones.LeftRingProximal.ToString()},
			{"FingerRing2Left", HumanBodyBones.LeftRingIntermediate.ToString()},
			{"FingerRing3Left", HumanBodyBones.LeftRingDistal.ToString()},
			{"FingerLittle1Left", HumanBodyBones.LeftLittleProximal.ToString()},
			{"FingerLittle2Left", HumanBodyBones.LeftLittleIntermediate.ToString()},
			{"FingerLittle3Left", HumanBodyBones.LeftLittleDistal.ToString()},
			{"ShoulderRight", HumanBodyBones.RightShoulder.ToString()},
			{"UpperArmRight", HumanBodyBones.RightUpperArm.ToString()},
			{"LowerArmRight", HumanBodyBones.RightLowerArm.ToString()},
			{"HandRight", HumanBodyBones.RightHand.ToString()},
			{"FingerThumb1Right", HumanBodyBones.RightThumbProximal.ToString()},
			{"FingerThumb2Right", HumanBodyBones.RightThumbIntermediate.ToString()},
			{"FingerThumb3Right", HumanBodyBones.RightThumbDistal.ToString()},
			{"FingerIndex1Right", HumanBodyBones.RightIndexProximal.ToString()},
			{"FingerIndex2Right", HumanBodyBones.RightIndexIntermediate.ToString()},
			{"FingerIndex3Right", HumanBodyBones.RightIndexDistal.ToString()},
			{"FingerMiddle1Right", HumanBodyBones.RightMiddleProximal.ToString()},
			{"FingerMiddle2Right", HumanBodyBones.RightMiddleIntermediate.ToString()},
			{"FingerMiddle3Right", HumanBodyBones.RightMiddleDistal.ToString()},
			{"FingerRing1Right", HumanBodyBones.RightRingProximal.ToString()},
			{"FingerRing2Right", HumanBodyBones.RightRingIntermediate.ToString()},
			{"FingerRing3Right", HumanBodyBones.RightRingDistal.ToString()},
			{"FingerLittle1Right", HumanBodyBones.RightLittleProximal.ToString()},
			{"FingerLittle2Right", HumanBodyBones.RightLittleIntermediate.ToString()},
			{"FingerLittle3Right", HumanBodyBones.RightLittleDistal.ToString()},
			{"UpperLegLeft", HumanBodyBones.LeftUpperLeg.ToString()},
			{"LowerLegLeft", HumanBodyBones.LeftLowerLeg.ToString()},
			{"FootLeft", HumanBodyBones.LeftFoot.ToString()},
			{"ToesLeft", HumanBodyBones.LeftToes.ToString()},
			{"UpperLegRight", HumanBodyBones.RightUpperLeg.ToString()},
			{"LowerLegRight", HumanBodyBones.RightLowerLeg.ToString()},
			{"FootRight", HumanBodyBones.RightFoot.ToString()},
			{"ToesRight", HumanBodyBones.RightToes.ToString()}
		};

		public static readonly List<string> _MappingsLeftList = new List<string>{"left", "_l", ".l", "-l"};
		public static readonly List<string> _MappingsRightList = new List<string>{"right", "_r", ".r", "-r"};
		public static readonly Dictionary<string, List<List<string>>> NameMappings = new Dictionary<string, List<List<string>>> {
			{"Hip", new List<List<string>>{new List<string>{"hip", "hips"}}},
			{"Spine", new List<List<string>>{new List<string>{"spine"}}},
			{"Chest", new List<List<string>>{new List<string>{"chest"}}},
			{"UpperChest", new List<List<string>>{new List<string>{"upper"}, new List<string>{"chest"}}},
			{"Neck", new List<List<string>>{new List<string>{"neck"}}},
			{"Head", new List<List<string>>{new List<string>{"head"}}},
			{"Jaw", new List<List<string>>{new List<string>{"jaw"}}},
			{"EyeLeft", new List<List<string>>{new List<string>{"eye"}, _MappingsLeftList}},
			{"EyeRight", new List<List<string>>{new List<string>{"eye"}, _MappingsRightList}},
			{"ShoulderLeft", new List<List<string>>{new List<string>{"shoulder"}, _MappingsLeftList}},
			{"UpperArmLeft", new List<List<string>>{new List<string>{"upper"}, new List<string>{"arm"}, _MappingsLeftList}},
			{"LowerArmLeft", new List<List<string>>{new List<string>{"lower"}, new List<string>{"arm"}, _MappingsLeftList}},
			{"HandLeft", new List<List<string>>{new List<string>{"hand", "wrist"}, _MappingsLeftList}},
			{"FingerThumb1Left", new List<List<string>>{new List<string>{"thumb"}, new List<string>{"1", "proximal"}, _MappingsLeftList}},
			{"FingerThumb2Left", new List<List<string>>{new List<string>{"thumb"}, new List<string>{"2", "intermediate"}, _MappingsLeftList}},
			{"FingerThumb3Left", new List<List<string>>{new List<string>{"thumb"}, new List<string>{"3", "distal"}, _MappingsLeftList}},
			{"FingerIndex1Left", new List<List<string>>{new List<string>{"index"}, new List<string>{"1", "proximal"}, _MappingsLeftList}},
			{"FingerIndex2Left", new List<List<string>>{new List<string>{"index"}, new List<string>{"2", "intermediate"}, _MappingsLeftList}},
			{"FingerIndex3Left", new List<List<string>>{new List<string>{"index"}, new List<string>{"3", "distal"}, _MappingsLeftList}},
			{"FingerMiddle1Left", new List<List<string>>{new List<string>{"middle"}, new List<string>{"1", "proximal"}, _MappingsLeftList}},
			{"FingerMiddle2Left", new List<List<string>>{new List<string>{"middle"}, new List<string>{"2", "intermediate"}, _MappingsLeftList}},
			{"FingerMiddle3Left", new List<List<string>>{new List<string>{"middle"}, new List<string>{"3", "distal"}, _MappingsLeftList}},
			{"FingerRing1Left", new List<List<string>>{new List<string>{"ring"}, new List<string>{"1", "proximal"}, _MappingsLeftList}},
			{"FingerRing2Left", new List<List<string>>{new List<string>{"ring"}, new List<string>{"2", "intermediate"}, _MappingsLeftList}},
			{"FingerRing3Left", new List<List<string>>{new List<string>{"ring"}, new List<string>{"3", "distal"}, _MappingsLeftList}},
			{"FingerLittle1Left", new List<List<string>>{new List<string>{"little", "pinkie"}, new List<string>{"1", "proximal"}, _MappingsLeftList}},
			{"FingerLittle2Left", new List<List<string>>{new List<string>{"little", "pinkie"}, new List<string>{"2", "intermediate"}, _MappingsLeftList}},
			{"FingerLittle3Left", new List<List<string>>{new List<string>{"little", "pinkie"}, new List<string>{"3", "distal"}, _MappingsLeftList}},
			{"ShoulderRight", new List<List<string>>{new List<string>{"shoulder"}, _MappingsRightList}},
			{"UpperArmRight", new List<List<string>>{new List<string>{"upper"}, new List<string>{"arm"}, _MappingsRightList}},
			{"LowerArmRight", new List<List<string>>{new List<string>{"lower"}, new List<string>{"arm"}, _MappingsRightList}},
			{"HandRight", new List<List<string>>{new List<string>{"hand", "wrist"}, _MappingsRightList}},
			{"FingerThumb1Right", new List<List<string>>{new List<string>{"thumb"}, new List<string>{"1", "proximal"}, _MappingsRightList}},
			{"FingerThumb2Right", new List<List<string>>{new List<string>{"thumb"}, new List<string>{"2", "intermediate"}, _MappingsRightList}},
			{"FingerThumb3Right", new List<List<string>>{new List<string>{"thumb"}, new List<string>{"3", "distal"}, _MappingsRightList}},
			{"FingerIndex1Right", new List<List<string>>{new List<string>{"index"}, new List<string>{"1", "proximal"}, _MappingsRightList}},
			{"FingerIndex2Right", new List<List<string>>{new List<string>{"index"}, new List<string>{"2", "intermediate"}, _MappingsRightList}},
			{"FingerIndex3Right", new List<List<string>>{new List<string>{"index"}, new List<string>{"3", "distal"}, _MappingsRightList}},
			{"FingerMiddle1Right", new List<List<string>>{new List<string>{"middle"}, new List<string>{"1", "proximal"}, _MappingsRightList}},
			{"FingerMiddle2Right", new List<List<string>>{new List<string>{"middle"}, new List<string>{"2", "intermediate"}, _MappingsRightList}},
			{"FingerMiddle3Right", new List<List<string>>{new List<string>{"middle"}, new List<string>{"3", "distal"}, _MappingsRightList}},
			{"FingerRing1Right", new List<List<string>>{new List<string>{"ring"}, new List<string>{"1", "proximal"}, _MappingsRightList}},
			{"FingerRing2Right", new List<List<string>>{new List<string>{"ring"}, new List<string>{"2", "intermediate"}, _MappingsRightList}},
			{"FingerRing3Right", new List<List<string>>{new List<string>{"ring"}, new List<string>{"3", "distal"}, _MappingsRightList}},
			{"FingerLittle1Right", new List<List<string>>{new List<string>{"little", "pinkie"}, new List<string>{"1", "proximal"}, _MappingsRightList}},
			{"FingerLittle2Right", new List<List<string>>{new List<string>{"little", "pinkie"}, new List<string>{"2", "intermediate"}, _MappingsRightList}},
			{"FingerLittle3Right", new List<List<string>>{new List<string>{"little", "pinkie"}, new List<string>{"3", "distal"}, _MappingsRightList}},
			{"UpperLegLeft", new List<List<string>>{new List<string>{"upper"}, new List<string>{"leg"}, _MappingsLeftList}},
			{"LowerLegLeft", new List<List<string>>{new List<string>{"lower"}, new List<string>{"leg"}, _MappingsLeftList}},
			{"FootLeft", new List<List<string>>{new List<string>{"foot"}, _MappingsLeftList}},
			{"ToesLeft", new List<List<string>>{new List<string>{"toes"}, _MappingsLeftList}},
			{"UpperLegRight", new List<List<string>>{new List<string>{"upper"}, new List<string>{"leg"}, _MappingsRightList}},
			{"LowerLegRight", new List<List<string>>{new List<string>{"lower"}, new List<string>{"leg"}, _MappingsRightList}},
			{"FootRight", new List<List<string>>{new List<string>{"foot"}, _MappingsRightList}},
			{"ToesRight", new List<List<string>>{new List<string>{"toes"}, _MappingsRightList}}
		};

		[HideInInspector] public STFArmatureInstance armatureInstance;
		[HideInInspector] public string locomotion_type;
		public List<BoneMappingPair> mappings = new List<BoneMappingPair>();

		public string translateHumanoidAVAtoUnity(string avaName, string locomotion_type)
		{
			if(locomotion_type == "digitigrade")
			{
				switch(avaName)
				{
					case "ToesLeft":
						return _Translations["FootLeft"];
					case "ToesRight":
						return _Translations["FootRight"];
					case "FootLeft":
						return null;
					case "FootRight":
						return null;
				}
			}
			if(avaName == "Jaw") return null;
			return _Translations[avaName];
		}

		public void Map()
		{
			var tmpMappings = new Dictionary<string, GameObject>();
			if(armatureInstance == null) throw new Exception("Armature must be set");
			foreach(var bone in armatureInstance.bones)
			{
				foreach(var mapping in NameMappings)
				{
					var and_list = mapping.Value;
					var and_condition = true;
					foreach(var or_list in and_list)
					{
						var or_condition = false;
						foreach(var or_arg in or_list)
						{
							if(bone.name.ToLower().Contains(or_arg))
							{
								or_condition = true;
								break;
							}
						}
						if(!or_condition)
						{
							and_condition = false;
						}
					}
					if(and_condition)
					{
						if(tmpMappings.ContainsKey(mapping.Key))
						{
							if(tmpMappings[mapping.Key].name.Length > bone.name.Length)
							{
								tmpMappings[mapping.Key] = bone;
							}
						}
						else
						{
							tmpMappings.Add(mapping.Key, bone);
						}
					}
				}
			}
			this.mappings = tmpMappings.Select(m => new BoneMappingPair(m.Key, m.Value)).ToList();
		}
	}

	public class AVAHumanoidMappingImporter : ASTFComponentImporter
	{
		public override void parseFromJson(ISTFImporter state, JToken json, string id, GameObject go)
		{
			var c = go.AddComponent<AVAHumanoidMapping>();
			c.id = id;
			c.locomotion_type = (string)json["locomotion_type"];
			state.AddTask(new Task(() => {
				c.armatureInstance = state.GetNode((string)json["target_armature_instance"]).GetComponent<STFArmatureInstance>();
				c.mappings = new List<BoneMappingPair>();
				foreach(var t in AVAHumanoidMapping._Translations)
				{
					if((string)json[t.Key] != null) c.mappings.Add(new BoneMappingPair(t.Key, c.armatureInstance.bones.First(b => b.GetComponent<STFUUID>().boneId == (string)json[t.Key])));
				}
			}));
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
				// handle armatures not yet being set up, maybe just use normal id
				ret.Add(mapping.humanoidName, mapping.bone?.GetComponent<STFUUID>().boneId);
			}
			return ret;
		}
	}
	public class AVAHumanoidMappingConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources)
		{
			var stfComponent = (AVAHumanoidMapping)component;
			var animator = root.AddComponent<Animator>();
			animator.applyRootMotion = true;
			animator.updateMode = AnimatorUpdateMode.Normal;
			animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			
			var mappings = stfComponent.mappings
					.FindAll(mapping => !String.IsNullOrWhiteSpace(mapping.humanoidName) && mapping.bone != null)
					.Select(mapping => new KeyValuePair<string, GameObject>(stfComponent.translateHumanoidAVAtoUnity(mapping.humanoidName, stfComponent.locomotion_type), mapping.bone))
					.Where(mapping => !String.IsNullOrWhiteSpace(mapping.Key)).ToList();
			
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
						humanName = mapping.Key,
						boneName = mapping.Value.name,
						limit = new HumanLimit {useDefaultValues = true}
					};
					//Debug.Log(bone.humanName + " : " + bone.boneName);
					return bone;
				}).ToArray()
			};

			var avatar = AvatarBuilder.BuildHumanAvatar(root, humanDescription);
			avatar.name = root.name + "Avatar";
			if (!avatar.isValid)
			{
				throw new Exception("Invalid humanoid avatar");
			}

			//foreach(var h in avatar.humanDescription.human) Debug.Log(h.humanName + " : " + h.boneName);

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

	[CustomEditor(typeof(AVAHumanoidMapping))]
	public class AVAHumanoidMappingInspector : Editor
	{
		private int locomotionSelection = 0;
		private string[] locomotionOptions = new string[] { "plantigrade", "digitigrade" };
		private string[] locomotionDisplayOptions = new string[] { "Plantigrade", "Digitigrade" };
		private bool _foldoutMappings = true;

		void OnEnable()
		{
			var c = (AVAHumanoidMapping)target;
			if(c.locomotion_type != null)
			{
				for(int i = 0; i < locomotionOptions.Length; i++)
				{
					if(c.locomotion_type == locomotionOptions[i])
					{
						locomotionSelection = i;
						break;
					}
				}
			}
		}

		public override void OnInspectorGUI()
		{
			//base.DrawDefaultInspector();
			var c = (AVAHumanoidMapping)target;

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Id");
			c.id = EditorGUILayout.TextField(c.id);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Armature Instance");
			c.armatureInstance = (STFArmatureInstance)EditorGUILayout.ObjectField(c.armatureInstance, typeof(STFArmatureInstance), true);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Locomotion Type");

			var locomotionSelectionNew = EditorGUILayout.Popup(locomotionSelection, locomotionDisplayOptions);
			if(locomotionSelectionNew != locomotionSelection || c.locomotion_type == null || c.locomotion_type.Length == 0)
			{
				c.locomotion_type = locomotionOptions[locomotionSelectionNew];
				locomotionSelection = locomotionSelectionNew;
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10f);
			
			EditorGUILayout.PrefixLabel("Mapped " + (c.mappings != null ? c.mappings.Count : 0) + " bones.");
			if(c.armatureInstance != null)
			{
				if(GUILayout.Button("Map Humanoid Bones", GUILayout.ExpandWidth(false))) {
					c.Map();
				}
			}

			GUILayout.Space(10f);

			_foldoutMappings = EditorGUILayout.Foldout(_foldoutMappings, "Mappings", true, EditorStyles.foldoutHeader);
			if(_foldoutMappings)
			{
				GUILayout.Space(5f);
				//base.DrawDefaultInspector();

				foreach(var mapping in AVAHumanoidMapping.NameMappings)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel(mapping.Key);
					var bone = c.mappings.Find(m => m.humanoidName == mapping.Key);
					if(bone != null)
					{
						bone.bone = (GameObject)EditorGUILayout.ObjectField(bone.bone, typeof(GameObject), true);
					}
					else
					{
						if(GUILayout.Button("Add Bone Mapping", GUILayout.ExpandWidth(false))) {
							c.mappings.Add(new BoneMappingPair(mapping.Key, null));
						}
					}
					EditorGUILayout.EndHorizontal();
				}
			}

			if(EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(c);
			}
			
		}
	}
#endif
}
