
#if UNIVRM_FOUND

using System.Collections.Generic;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEngine;
using VRM;

namespace ava.Converters
{
	public class AVAJankyFallbackPhysicsVRMConverter : ISTFSecondStageConverter
	{
		public Dictionary<string, UnityEngine.Object> CollectOriginalResources(Component component, GameObject root, ISTFSecondStageContext context) { return null; }
		
		public void Convert(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var c = (AVAJankyFallbackPhysics)component;

			context.AddTask(new Task(() => {
				var secondary = root.transform.Find("VRM_secondary");
				var springbone = secondary.gameObject.AddComponent<VRMSpringBone>();

				springbone.RootBones.Add(c.target.transform);
				springbone.m_dragForce = c.pull;
				springbone.m_stiffnessForce = c.stiffness;
				
				context.RelMat.AddConverted(component, springbone);
			}));
		}
	}
}

#endif
