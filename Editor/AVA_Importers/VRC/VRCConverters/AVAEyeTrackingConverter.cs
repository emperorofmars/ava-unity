
#if VRCSDK3_FOUND

using System.Collections.Generic;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace ava.Converters
{
	public class AVAEyeTrackingVRCConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, STFSecondStageContext context)
		{
			var c = (AVAEyeTracking)component;
			context.Tasks.Add(new Task(() => {
				AVAAvatar avaAvatar = null;
				AVAHumanoidMapping humanoid = null;
				foreach(var extend in context.RelMat.Extends[component])
				{
					switch(extend)
					{
						case AVAAvatar a: avaAvatar = a; break;
						case AVAHumanoidMapping a: humanoid = a; break;
					}

					//if(extend.GetType() == typeof(AVAAvatar)) avaAvatar = (AVAAvatar)extend;
					//else if(extend.GetType() == typeof(AVAHumanoidMapping)) humanoid = (AVAHumanoidMapping)extend;
				}
				var avatar = (VRCAvatarDescriptor)context.RelMat.STFToConverted[avaAvatar];

				avatar.enableEyeLook = true;
				avatar.customEyeLookSettings.eyelidType = VRCAvatarDescriptor.EyelidType.Bones;
				avatar.customEyeLookSettings.leftEye = humanoid.mappings.Find(m => m.humanoidName == "EyeLeft")?.bone.transform;
				avatar.customEyeLookSettings.rightEye = humanoid.mappings.Find(m => m.humanoidName == "EyeRight")?.bone.transform;
				avatar.customEyeLookSettings.eyesLookingUp = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations {left = Quaternion.Euler(-c.limitLeft[0], 0f, 0f), right = Quaternion.Euler(-c.limitRight[0], 0f, 0f)};
				avatar.customEyeLookSettings.eyesLookingDown = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations {left = Quaternion.Euler(c.limitLeft[1], 0f, 0f), right = Quaternion.Euler(c.limitRight[1], 0f, 0f)};
				avatar.customEyeLookSettings.eyesLookingLeft = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations {left = Quaternion.Euler(0f, -c.limitLeft[2], 0f), right = Quaternion.Euler(0f, -c.limitRight[2], 0f)};
				avatar.customEyeLookSettings.eyesLookingRight = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations {left = Quaternion.Euler(0f, c.limitLeft[3], 0f), right = Quaternion.Euler(0f, c.limitRight[3], 0f)};
			}));
		}
	}
}

#endif
