
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
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, STFSecondStageContext context)
		{
			var c = (AVAFacialTrackingSimple)component;
			context.Tasks.Add(new Task(() => {
				AVAAvatar avaAvatar = null;
				foreach(var extend in context.RelMat.Extends[component])
				{
					if(extend is AVAAvatar)
					{
						avaAvatar = (AVAAvatar)extend; break;
					}
				}
				var avatar = (VRCAvatarDescriptor)context.RelMat.STFToConverted[avaAvatar];

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

				/*avatar.enableEyeLook = true;
				avatar.customEyeLookSettings.leftEye = humanoid.mappings.Find(m => m.humanoidName == "EyeLeft")?.bone.transform;
				avatar.customEyeLookSettings.rightEye = humanoid.mappings.Find(m => m.humanoidName == "EyeRight")?.bone.transform;

				avatar.customEyeLookSettings.eyesLookingUp = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(-c.up, 0f, 0f), right = Quaternion.Euler(-c.up, 0f, 0f), linked = true};
				avatar.customEyeLookSettings.eyesLookingDown = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(c.down, 0f, 0f), right = Quaternion.Euler(c.down, 0f, 0f), linked = true};
				avatar.customEyeLookSettings.eyesLookingLeft = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(0f, -c.outer, 0f), right = Quaternion.Euler(0f, -c.inner, 0f), linked = false};
				avatar.customEyeLookSettings.eyesLookingRight = new VRCAvatarDescriptor.CustomEyeLookSettings.EyeRotations
						{left = Quaternion.Euler(0f, c.inner, 0f), right = Quaternion.Euler(0f, c.outer, 0f), linked = false};*/
				
				context.RelMat.STFToConverted.Add(component, avatar);
			}));
		}
	}
}

#endif
