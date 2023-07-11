
#if VRCSDK3_FOUND

using System.Collections.Generic;
using ava.Components;
using stf.serialisation;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace ava.Converters
{
	public class AVAAvatarVRCConverter : ISTFSecondStageConverter
	{
		public Dictionary<string, UnityEngine.Object> CollectOriginalResources(Component component, GameObject root, ISTFSecondStageContext context)
		{
			// actually return the icon resource for correctness sake, even if its not needed (unless for some reason some future target application allows you to animate that)
			return null;
		}

		public void Convert(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var avaAvatar = (AVAAvatar)component;

			var vrcAvatar = component.gameObject.AddComponent<VRCAvatarDescriptor>();
			if(avaAvatar.viewport_parent != null && avaAvatar.viewport_position != null) vrcAvatar.ViewPosition = avaAvatar.viewport_parent.transform.position - root.transform.position + avaAvatar.viewport_position;

			context.RelMat.AddConverted(component, vrcAvatar);
		}
	}
}

#endif
