
#if VRCSDK3_FOUND

using System.Collections.Generic;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace ava.Converters
{
	public class AVAJankyFallbackPhysicsVRCConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, ISTFSecondStageContext context)
		{
			var c = (AVAJankyFallbackPhysics)component;
			var physbone = component.gameObject.AddComponent<VRCPhysBone>();
			
			physbone.rootTransform = c.target ? c.target.transform : c.transform;
			physbone.pull = c.pull;
			physbone.spring = c.spring;
			physbone.limitType = VRC.Dynamics.VRCPhysBoneBase.LimitType.Angle;
			physbone.maxAngleX = c.stiffness * 180;
			physbone.maxAngleZ = c.stiffness * 180;
			
			context.RelMat.AddConverted(component, physbone);
		}
	}
}

#endif
