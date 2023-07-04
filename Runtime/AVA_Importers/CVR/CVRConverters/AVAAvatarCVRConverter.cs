
#if CVRCCK3_FOUND

using System.Collections.Generic;
using ava.Components;
using stf.serialisation;
using UnityEngine;
using ABI.CCK.Components;

namespace ava.Converters
{
	public class AVAAvatarCVRConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, ISTFSecondStageContext context)
		{
			var avaAvatar = (AVAAvatar)component;

			var avatar = component.gameObject.AddComponent<CVRAvatar>();
			if(avaAvatar.viewport_parent != null && avaAvatar.viewport_position != null) avatar.viewPosition = avaAvatar.viewport_parent.transform.position - root.transform.position + avaAvatar.viewport_position;
			
			context.RelMat.AddConverted(component, avatar);
		}
	}
}

#endif
