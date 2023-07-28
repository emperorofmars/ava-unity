
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

				var vrmLookat = component.gameObject.AddComponent<VRMLookAtBoneApplyer>();
				vrmLookat.LeftEye.Transform = humanoid.mappings.Find(p => p.humanoidName == "EyeLeft")?.bone?.transform;
				vrmLookat.RightEye.Transform = humanoid.mappings.Find(p => p.humanoidName == "EyeRight")?.bone?.transform;
				
				// vrmLookat.VerticalDown = // i cant be arsed to look at how this nonesense is defined
				// etc
				
				context.RelMat.AddConverted(component, vrmLookat);
			}));
		}
	}
}

#endif
