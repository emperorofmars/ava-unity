
#if VRCSDK3_FOUND

using System.Collections.Generic;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace ava.Converters
{
	public class AVAEyeBoneLimitsSimpleVRCConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, STFSecondStageContext context)
		{
			var c = (AVAEyeBoneLimitsSimple)component;
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
				}
				var avatar = (VRCAvatarDescriptor)context.RelMat.STFToConverted[avaAvatar];

				avatar.enableEyeLook = true;
				avatar.customEyeLookSettings.leftEye = humanoid.mappings.Find(m => m.humanoidName == "EyeLeft")?.bone.transform;
				avatar.customEyeLookSettings.rightEye = humanoid.mappings.Find(m => m.humanoidName == "EyeRight")?.bone.transform;

				avatar.customEyeLookSettings.eyesLookingUp = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(-c.up, 0f, 0f), right = Quaternion.Euler(-c.up, 0f, 0f), linked = true};
				avatar.customEyeLookSettings.eyesLookingDown = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(c.down, 0f, 0f), right = Quaternion.Euler(c.down, 0f, 0f), linked = true};
				avatar.customEyeLookSettings.eyesLookingLeft = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(0f, -c.outer, 0f), right = Quaternion.Euler(0f, -c.inner, 0f), linked = false};
				avatar.customEyeLookSettings.eyesLookingRight = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(0f, c.inner, 0f), right = Quaternion.Euler(0f, c.outer, 0f), linked = false};
				
				context.RelMat.STFToConverted.Add(component, avatar);
			}));
		}
	}
}

#endif
