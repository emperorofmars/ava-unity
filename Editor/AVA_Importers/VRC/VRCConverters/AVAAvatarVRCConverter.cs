
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
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, STFSecondStageContext context)
		{
			var avaAvatar = (AVAAvatar)component;

			var vrcAvatar = component.gameObject.AddComponent<VRCAvatarDescriptor>();
			if(avaAvatar.viewport_parent != null && avaAvatar.viewport_position != null) vrcAvatar.ViewPosition = avaAvatar.viewport_parent.transform.position - root.transform.position + avaAvatar.viewport_position;

			context.RelMat.STFToConverted.Add(component, vrcAvatar);

			/*if(relMat.ExtendedBys.ContainsKey(component))
			{
				foreach(var extend in relMat.Extends[component])
				{
					if(!relMat.IsOverridden.Contains(extend))
					{

					}
				}
			}*/

			// get all extending components and handle and remove these
			
			/*#if UNITY_EDITOR
            UnityEngine.Object.DestroyImmediate(component);
			#else
            UnityEngine.Object.Destroy(component);
			#endif*/
		}
	}
}

#endif
