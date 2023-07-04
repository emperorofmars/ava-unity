
#if CVRCCK3_FOUND

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABI.CCK.Components;
using ava.Components;
using stf.serialisation;
using UnityEngine;

namespace ava.Converters
{
	public class AVAFacialTrackingSimpleCVRConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, ISTFSecondStageContext context)
		{
			var c = (AVAFacialTrackingSimple)component;
			context.AddTask(new Task(() => {
				AVAAvatar avaAvatar = context.RelMat.GetExtended<AVAAvatar>(component);

				var avatar = (CVRAvatar)context.RelMat.GetConverted(avaAvatar);

				avatar.useVisemeLipsync = true;
				avatar.bodyMesh = c.TargetMeshInstance;
				avatar.visemeMode = CVRAvatar.CVRAvatarVisemeMode.Visemes;
				avatar.visemeBlendshapes = new string[15];
				avatar.visemeBlendshapes[0] = c.Mappings.Find(m => m.VisemeName == "sil")?.BlendshapeName;
				avatar.visemeBlendshapes[1] = c.Mappings.Find(m => m.VisemeName == "pp")?.BlendshapeName;
				avatar.visemeBlendshapes[2] = c.Mappings.Find(m => m.VisemeName == "ff")?.BlendshapeName;
				avatar.visemeBlendshapes[3] = c.Mappings.Find(m => m.VisemeName == "th")?.BlendshapeName;
				avatar.visemeBlendshapes[4] = c.Mappings.Find(m => m.VisemeName == "dd")?.BlendshapeName;
				avatar.visemeBlendshapes[5] = c.Mappings.Find(m => m.VisemeName == "kk")?.BlendshapeName;
				avatar.visemeBlendshapes[6] = c.Mappings.Find(m => m.VisemeName == "ch")?.BlendshapeName;
				avatar.visemeBlendshapes[7] = c.Mappings.Find(m => m.VisemeName == "ss")?.BlendshapeName;
				avatar.visemeBlendshapes[8] = c.Mappings.Find(m => m.VisemeName == "nn")?.BlendshapeName;
				avatar.visemeBlendshapes[9] = c.Mappings.Find(m => m.VisemeName == "rr")?.BlendshapeName;
				avatar.visemeBlendshapes[10] = c.Mappings.Find(m => m.VisemeName == "aa")?.BlendshapeName;
				avatar.visemeBlendshapes[11] = c.Mappings.Find(m => m.VisemeName == "e")?.BlendshapeName;
				avatar.visemeBlendshapes[12] = c.Mappings.Find(m => m.VisemeName == "ih")?.BlendshapeName;
				avatar.visemeBlendshapes[13] = c.Mappings.Find(m => m.VisemeName == "oh")?.BlendshapeName;
				avatar.visemeBlendshapes[14] = c.Mappings.Find(m => m.VisemeName == "ou")?.BlendshapeName;

				if(c.Mappings.Find(m => m.VisemeName == "blink") != null)
				{
					avatar.useBlinkBlendshapes = true;
					avatar.blinkBlendshape = new string[4];
					avatar.blinkBlendshape[0] = c.Mappings.Find(m => m.VisemeName == "blink")?.BlendshapeName;
				}

				context.RelMat.AddConverted(component, avatar);
			}));
		}
	}
}

#endif
