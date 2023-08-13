
#if UNIVRM_FOUND

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ava.Components;
using stf;
using stf.serialisation;
using UnityEngine;
using VRM;

namespace ava.Converters
{
	public class AVAFacialTrackingSimpleVRMConverter : ISTFSecondStageConverter
	{
		public Dictionary<string, UnityEngine.Object> CollectOriginalResources(Component component, GameObject root, ISTFSecondStageContext context) { return null; }
		
		public void Convert(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var c = (AVAFacialTrackingSimple)component;
			VRMBlendShapeProxy vrmBlendshapeProxy = context.RelMat.GetExtended<VRMBlendShapeProxy>(component);
			if(vrmBlendshapeProxy == null) vrmBlendshapeProxy = component.gameObject.AddComponent<VRMBlendShapeProxy>();

			if(vrmBlendshapeProxy.BlendShapeAvatar == null)
			{
				BlendShapeAvatar vrmBlendShapeAvatar = new BlendShapeAvatar();
				vrmBlendShapeAvatar.name = "VRM_BlendshapeAvatar";
				vrmBlendshapeProxy.BlendShapeAvatar = vrmBlendShapeAvatar;
				context.AddResource(vrmBlendShapeAvatar);
			}

			createBlendshapeClip("aa", BlendShapePreset.A, c, vrmBlendshapeProxy, context);
			createBlendshapeClip("e", BlendShapePreset.E, c, vrmBlendshapeProxy, context);
			createBlendshapeClip("ih", BlendShapePreset.I, c, vrmBlendshapeProxy, context);
			createBlendshapeClip("oh", BlendShapePreset.O, c, vrmBlendshapeProxy, context);
			createBlendshapeClip("ou", BlendShapePreset.U, c, vrmBlendshapeProxy, context);

			createBlendshapeClip("blink", BlendShapePreset.Blink, c, vrmBlendshapeProxy, context);

			context.RelMat.AddConverted(component, vrmBlendshapeProxy);
		}

		private void createBlendshapeClip(string visemeName, BlendShapePreset blendshapePreset, AVAFacialTrackingSimple c, VRMBlendShapeProxy vrmBlendshapeProxy, ISTFSecondStageContext context)
		{
			BlendShapeClip clip = new BlendShapeClip();
			clip.name = "VRM_Clip_" + visemeName;
			clip.BlendShapeName = c.Mappings.Find(m => m.VisemeName == visemeName)?.BlendshapeName;
			clip.Preset = blendshapePreset;
			var bindingsList = new BlendShapeBinding[1];
			BlendShapeBinding binding = new BlendShapeBinding();
			binding.Index = c.TargetMeshInstance.sharedMesh.GetBlendShapeIndex(clip.BlendShapeName);
			binding.RelativePath = Utils.getPath(c.transform, c.TargetMeshInstance.transform)?.Substring(1);
			binding.Weight = 100;
			bindingsList[0] = binding;
			clip.Values = bindingsList;
			vrmBlendshapeProxy.BlendShapeAvatar.Clips.Add(clip);
			context.AddResource(clip);
		}

		public int GetBlendshapeIndex(Mesh mesh, string name)
		{
		
			for(int i = 0; i < mesh.blendShapeCount; i++)
			{
				var bName = mesh.GetBlendShapeName(i);
				if(bName == name) return i;
			}
			return -1;
		}
	}
}

#endif
