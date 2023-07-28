
#if UNIVRM_FOUND

using System.Collections.Generic;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEngine;
using VRM;

namespace ava.Converters
{
	public class AVAEyeBoneLimitsSimpleVRMConverter : ISTFSecondStageConverter
	{
		public Dictionary<string, UnityEngine.Object> CollectOriginalResources(Component component, GameObject root, ISTFSecondStageContext context) { return null; }
		
		public void Convert(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var c = (AVAEyeBoneLimitsSimple)component;
			context.AddTask(new Task(() => {
				AVAAvatar avaAvatar = context.RelMat.GetExtended<AVAAvatar>(component);
				AVAHumanoidMapping humanoid = context.RelMat.GetExtended<AVAHumanoidMapping>(component);

				var vrmFirstPerson = component.gameObject.AddComponent<VRMFirstPerson>();
				vrmFirstPerson.FirstPersonBone = avaAvatar.viewport_parent?.transform;
				vrmFirstPerson.FirstPersonOffset = avaAvatar.viewport_position;
				
				context.RelMat.AddConverted(component, vrmFirstPerson);
			}));
		}
	}
}

#endif
