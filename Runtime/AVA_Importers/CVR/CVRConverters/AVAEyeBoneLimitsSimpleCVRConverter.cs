
#if CVRCCK3_FOUND

using System.Collections.Generic;
using System.Threading.Tasks;
using ABI.CCK.Components;
using ava.Components;
using stf.serialisation;
using UnityEngine;

namespace ava.Converters
{
	public class AVAEyeBoneLimitsSimpleCVRConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, ISTFSecondStageContext context)
		{
			var c = (AVAEyeBoneLimitsSimple)component;
			context.AddTask(new Task(() => {
				AVAAvatar avaAvatar = context.RelMat.GetExtended<AVAAvatar>(component);
				AVAHumanoidMapping humanoid = context.RelMat.GetExtended<AVAHumanoidMapping>(component);
				
				var avatar = (CVRAvatar)context.RelMat.GetConverted(avaAvatar);

				avatar.useEyeMovement = true;
				// handle rotation limits and whatever when the cvr sdk supports that
				
				context.RelMat.AddConverted(component, avatar);
			}));
		}
	}
}

#endif
