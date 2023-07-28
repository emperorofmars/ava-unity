
#if UNIVRM_FOUND

using System.Collections.Generic;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEngine;
using VRM;

namespace ava.Converters
{
	public class AVAAvatarVRMConverter : ISTFSecondStageConverter
	{
		public Dictionary<string, UnityEngine.Object> CollectOriginalResources(Component component, GameObject root, ISTFSecondStageContext context)
		{
			// actually return the icon resource for correctness sake, even if its not needed (unless for some reason some future target application allows you to animate that)
			return null;
		}

		public void Convert(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var avaAvatar = (AVAAvatar)component;

			var vrmMetaComponent = component.gameObject.AddComponent<VRMMeta>();
			var vrmMeta = new VRMMetaObject();
			vrmMeta.name = "VRM_Meta";
			vrmMetaComponent.Meta = vrmMeta;
			vrmMeta.Title = avaAvatar.avatar_name;
			vrmMeta.Version = avaAvatar.avatar_version;
			vrmMeta.Author = avaAvatar.author;
			vrmMeta.ExporterVersion = avaAvatar.avatar_version;
			vrmMeta.OtherLicenseUrl = avaAvatar.license_link;
			vrmMeta.Thumbnail = avaAvatar.icon;
			
			context.AddTask(new Task(() => {
				AVAHumanoidMapping humanoid = context.RelMat.GetExtended<AVAHumanoidMapping>(component);

				var vrmFirstPerson = component.gameObject.AddComponent<VRMFirstPerson>();
				vrmFirstPerson.FirstPersonBone = avaAvatar.viewport_parent?.transform;
				vrmFirstPerson.FirstPersonOffset = avaAvatar.viewport_position;
				
				//context.RelMat.AddConverted(component, vrmFirstPerson);
			}));

			/*var vrmHumanoid = component.gameObject.AddComponent<VRMHumanoidDescription>();
			vrmHumanoid.Avatar = avaAvatar.GetComponent<Animator>().avatar;
			var humanDescription = ScriptableObject.CreateInstance<UniHumanoid.AvatarDescription>();
			//humanDescription.SetHumanBones() // cant be arsed to deal with this
			vrmHumanoid.Description = humanDescription;*/

			context.AddResource(vrmMeta);
			//context.AddResource(humanDescription);
			context.RelMat.AddConverted(component, vrmMetaComponent);
		}
	}
}

#endif
