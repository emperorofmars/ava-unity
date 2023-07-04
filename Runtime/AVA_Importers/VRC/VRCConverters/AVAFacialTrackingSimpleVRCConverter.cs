
#if VRCSDK3_FOUND

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace ava.Converters
{
	public class AVAFacialTrackingSimpleVRCConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, ISTFSecondStageContext context)
		{
			var c = (AVAFacialTrackingSimple)component;
			context.AddTask(new Task(() => {
				AVAAvatar avaAvatar = context.RelMat.GetExtended<AVAAvatar>(component);

				var avatar = (VRCAvatarDescriptor)context.RelMat.GetConverted(avaAvatar);

				avatar.VisemeSkinnedMesh = c.TargetMeshInstance;
				avatar.lipSync = VRC.SDKBase.VRC_AvatarDescriptor.LipSyncStyle.VisemeBlendShape;
				avatar.VisemeBlendShapes = new string[15];
				avatar.VisemeBlendShapes[0] = c.Mappings.Find(m => m.VisemeName == "sil")?.BlendshapeName;
				avatar.VisemeBlendShapes[1] = c.Mappings.Find(m => m.VisemeName == "pp")?.BlendshapeName;
				avatar.VisemeBlendShapes[2] = c.Mappings.Find(m => m.VisemeName == "ff")?.BlendshapeName;
				avatar.VisemeBlendShapes[3] = c.Mappings.Find(m => m.VisemeName == "th")?.BlendshapeName;
				avatar.VisemeBlendShapes[4] = c.Mappings.Find(m => m.VisemeName == "dd")?.BlendshapeName;
				avatar.VisemeBlendShapes[5] = c.Mappings.Find(m => m.VisemeName == "kk")?.BlendshapeName;
				avatar.VisemeBlendShapes[6] = c.Mappings.Find(m => m.VisemeName == "ch")?.BlendshapeName;
				avatar.VisemeBlendShapes[7] = c.Mappings.Find(m => m.VisemeName == "ss")?.BlendshapeName;
				avatar.VisemeBlendShapes[8] = c.Mappings.Find(m => m.VisemeName == "nn")?.BlendshapeName;
				avatar.VisemeBlendShapes[9] = c.Mappings.Find(m => m.VisemeName == "rr")?.BlendshapeName;
				avatar.VisemeBlendShapes[10] = c.Mappings.Find(m => m.VisemeName == "aa")?.BlendshapeName;
				avatar.VisemeBlendShapes[11] = c.Mappings.Find(m => m.VisemeName == "e")?.BlendshapeName;
				avatar.VisemeBlendShapes[12] = c.Mappings.Find(m => m.VisemeName == "ih")?.BlendshapeName;
				avatar.VisemeBlendShapes[13] = c.Mappings.Find(m => m.VisemeName == "oh")?.BlendshapeName;
				avatar.VisemeBlendShapes[14] = c.Mappings.Find(m => m.VisemeName == "ou")?.BlendshapeName;

				if(c.Mappings.Find(m => m.VisemeName == "blink") != null)
				{
					avatar.customEyeLookSettings.eyelidType = VRCAvatarDescriptor.EyelidType.Blendshapes;
					avatar.customEyeLookSettings.eyelidsSkinnedMesh = c.TargetMeshInstance;
					avatar.customEyeLookSettings.eyelidsBlendshapes = new int[3];
					avatar.customEyeLookSettings.eyelidsBlendshapes[0] = GetBlendshapeIndex(c.TargetMeshInstance.sharedMesh, c.Mappings.Find(m => m.VisemeName == "blink")?.BlendshapeName);
					avatar.customEyeLookSettings.eyelidsBlendshapes[1] = GetBlendshapeIndex(c.TargetMeshInstance.sharedMesh, c.Mappings.Find(m => m.VisemeName == "look_up")?.BlendshapeName);
					avatar.customEyeLookSettings.eyelidsBlendshapes[2] = GetBlendshapeIndex(c.TargetMeshInstance.sharedMesh, c.Mappings.Find(m => m.VisemeName == "look_down")?.BlendshapeName);
				}

				context.RelMat.AddConverted(component, avatar);
			}));
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
