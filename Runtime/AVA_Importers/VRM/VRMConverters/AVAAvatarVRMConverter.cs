
#if UNIVRM_FOUND

using System.Collections.Generic;
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
			vrmMetaComponent.Meta = vrmMeta;
			vrmMeta.Title = avaAvatar.avatar_name;
			vrmMeta.Version = avaAvatar.avatar_version;
			vrmMeta.Author = avaAvatar.author;
			vrmMeta.ExporterVersion = avaAvatar.avatar_version;
			vrmMeta.OtherLicenseUrl = avaAvatar.license_link;
			vrmMeta.Thumbnail = avaAvatar.icon;

			context.AddResource(vrmMeta);
			context.RelMat.AddConverted(component, vrmMetaComponent);
		}
	}
}

#endif
