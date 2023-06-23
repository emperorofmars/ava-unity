
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
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, STFSecondStageContext context)
		{
			var avaAvatar = (AVAAvatar)component;

			var cvrAvatar = component.gameObject.AddComponent<CVRAvatar>();
			//if(avaAvatar.viewport_parent != null && avaAvatar.viewport_position != null) vrcAvatar.ViewPosition = avaAvatar.viewport_parent.transform.position - root.transform.position + avaAvatar.viewport_position;
			

			context.RelMat.STFToConverted.Add(component, cvrAvatar);
		}
	}
}

#endif
