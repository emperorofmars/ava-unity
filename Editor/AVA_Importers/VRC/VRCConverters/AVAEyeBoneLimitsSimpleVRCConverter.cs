
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
				
				/*avatar.customEyeLookSettings.eyesLookingUp = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(-c.limitLeft[0], 0f, 0f), right = Quaternion.Euler(-c.limitRight[0], 0f, 0f), linked = (Mathf.Abs(c.limitLeft[0] - c.limitRight[0]) < 0.0001)};
				avatar.customEyeLookSettings.eyesLookingDown = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(c.limitLeft[1], 0f, 0f), right = Quaternion.Euler(c.limitRight[1], 0f, 0f), linked = (Mathf.Abs(c.limitLeft[1] - c.limitRight[1]) < 0.0001)};
				avatar.customEyeLookSettings.eyesLookingLeft = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(0f, -c.limitLeft[2], 0f), right = Quaternion.Euler(0f, -c.limitRight[2], 0f), linked = (Mathf.Abs(c.limitLeft[2] - c.limitRight[2]) < 0.0001)};
				avatar.customEyeLookSettings.eyesLookingRight = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(0f, c.limitLeft[3], 0f), right = Quaternion.Euler(0f, c.limitRight[3], 0f), linked = (Mathf.Abs(c.limitLeft[3] - c.limitRight[3]) < 0.0001)};*/
						
				avatar.customEyeLookSettings.eyesLookingUp = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(-c.limits[0], 0f, 0f), right = Quaternion.Euler(-c.limits[0], 0f, 0f), linked = true};
				avatar.customEyeLookSettings.eyesLookingDown = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(c.limits[1], 0f, 0f), right = Quaternion.Euler(c.limits[1], 0f, 0f), linked = true};
				avatar.customEyeLookSettings.eyesLookingLeft = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(0f, -c.limits[3], 0f), right = Quaternion.Euler(0f, -c.limits[2], 0f), linked = false};
				avatar.customEyeLookSettings.eyesLookingRight = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(0f, c.limits[2], 0f), right = Quaternion.Euler(0f, c.limits[3], 0f), linked = false};
				
				context.RelMat.STFToConverted.Add(component, avatar);
			}));
		}
	}
}

#endif
