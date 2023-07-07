
#if VRCSDK3_FOUND

using System.Collections.Generic;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace ava.Converters
{
	public class AVAExpressionsSimpleVRCConverter : ISTFSecondStageConverter
	{
		public void Convert(Component component, GameObject root, List<UnityEngine.Object> resources, ISTFSecondStageContext context)
		{
			var c = (AVAExpressionsSimple)component;
			context.AddTask(new Task(() => {
				AVAAvatar avaAvatar = context.RelMat.GetExtended<AVAAvatar>(component);
				AVAHumanoidMapping humanoid = context.RelMat.GetExtended<AVAHumanoidMapping>(component);
				
				var avatar = (VRCAvatarDescriptor)context.RelMat.GetConverted(avaAvatar);

				// generate animator controller here
				var controller = avatar.GetComponent<Animator>().runtimeAnimatorController;
				Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAA: " + controller);
				
				context.RelMat.AddConverted(component, avatar);
			}));
		}
	}
}

#endif
