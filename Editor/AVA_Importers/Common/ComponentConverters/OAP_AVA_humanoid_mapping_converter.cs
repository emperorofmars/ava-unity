
#if UNITY_EDITOR

using stf.ava.importer.common;
using UnityEditor;
using UnityEngine;

namespace oap.ava.importer.common
{
	public class OAP_AVA_humanoid_mapping_converter : IComponentConverter
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

		public bool cleanup()
		{
			return true;
		}
		
        public void convert(GameObject node, GameObject root, Component originalComponent, string assetName)
        {
			/*OAP_AVA_humanoid_mapping component = (OAP_AVA_humanoid_mapping)originalComponent;
			
			Animator animator = root.GetComponent<Animator>();
			if(!animator)
			{
				animator = root.AddComponent<Animator>();
				animator.applyRootMotion = true;
				animator.updateMode = AnimatorUpdateMode.Normal;
				animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			}

			var name_mappings = component.mappings.FindAll(mapping => mapping.bone != null && mapping.bone.Length > 0 && mapping.uuid != null && mapping.uuid.Length > 0);
			var mappings = new List<HumanoidTransformMapping>();
			foreach(var mapping in name_mappings)
			{
				var humanName = component.translateHumanoidAVAtoUnity(mapping.bone, component.locomotion_type);
				var go = TreeUtils.findByUUID(root, mapping.uuid);
				if(humanName != null && humanName.Length > 0 && go != null) mappings.Add(new HumanoidTransformMapping(humanName, go));
			}
			/*var hips = mappings.Find(m => m.humanoidName == "Hips");
			var armature = hips.go.transform.parent;
			var transforms = armature.GetComponentsInChildren<Transform>();*/

			/*var humanDescription = new HumanDescription
			{
				/*skeleton = transforms.Select(t =>
				{
					var sb = new SkeletonBone();
					sb.name = t.name;
					sb.position = t.localPosition;
					sb.rotation = t.localRotation;
					sb.scale = t.localScale;
					Debug.Log(sb.name);
					return sb;
				}).Append(new SkeletonBone
				{
					name = armature.name,
					position = Vector3.zero,
					rotation = Quaternion.identity,
					scale = Vector3.one
				}).ToArray(),*/
				/*human = mappings.Select(mapping => 
				{
					var bone = new HumanBone {humanName = mapping.humanoidName, boneName = mapping.go.name};
					//Debug.Log(bone.humanName + " : " + bone.boneName);
					bone.limit.useDefaultValues = true;
					return bone;
				}).ToArray()
				//not handling all the rest here as its unity specific for now
			};

			var avatar = AvatarBuilder.BuildHumanAvatar(root, humanDescription);
			avatar.name = root.name;
			if (!avatar.isValid)
			{
				Debug.LogError("Invalid humanoid avatar");
			}
			animator.avatar = avatar;
			
			AssetDatabase.CreateAsset(avatar, FolderManager.getCommonFolder(assetName) + "/" + assetName + ".ht");*/
        }
    }

	[InitializeOnLoad]
	class Register_OAP_AVA_humanoid_mapping_converter
	{
		static Register_OAP_AVA_humanoid_mapping_converter()
		{
			//CommonConverters.addConverter(typeof(OAP_AVA_humanoid_mapping), new OAP_AVA_humanoid_mapping_converter());
		}
	}
}

#endif
