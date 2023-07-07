
#if CVRCCK3_FOUND && DynamicBones_FOUND

using System.Collections.Generic;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEngine;

namespace ava.Converters
{
	public class AVAJankyFallbackPhysicsCVRConverter : ISTFSecondStageConverter
	{
		public void Convert(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var c = (AVAJankyFallbackPhysics)component;
			var dybone = component.gameObject.AddComponent<DynamicBone>();

			dybone.m_Root = c.target ? c.target.transform : c.transform;
			dybone.m_Stiffness = c.stiffness;
			dybone.m_Damping = c.pull;
			dybone.m_Elasticity = c.spring;
			
			context.RelMat.AddConverted(component, dybone);
		}
	}
}

#endif
