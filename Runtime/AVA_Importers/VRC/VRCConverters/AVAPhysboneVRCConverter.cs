
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
	public class AVAPhysboneVRCConverter : ISTFSecondStageConverter
	{
		public Dictionary<string, UnityEngine.Object> CollectOriginalResources(Component component, GameObject root, ISTFSecondStageContext context) { return null; }
		
		public void Convert(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var c = (AVAVRCPhysbones)component;
			var physbone = component.gameObject.AddComponent<VRCPhysBone>();
			
			physbone.rootTransform = c.target ? c.target.transform : c.transform;
			physbone.version = c.version == "1.1" ? VRC.Dynamics.VRCPhysBoneBase.Version.Version_1_1 : VRC.Dynamics.VRCPhysBoneBase.Version.Version_1_0;
			physbone.integrationType = c.integration_type == "simplified" ? VRC.Dynamics.VRCPhysBoneBase.IntegrationType.Simplified : VRC.Dynamics.VRCPhysBoneBase.IntegrationType.Advanced;
			physbone.pull = c.pull;
			physbone.spring = c.spring;
			physbone.stiffness = c.stiffness;
			physbone.gravity = c.gravity;
			physbone.gravityFalloff = c.gravity_falloff;
			
			context.RelMat.AddConverted(component, physbone);
		}
	}
}

#endif
