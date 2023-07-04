
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
	public class AVAAvatarVoiceCVRConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root, List<UnityEngine.Object> resources, ISTFSecondStageContext context)
		{
			var c = (AVAAvatarVoice)component;
			context.AddTask(new Task(() => {
				AVAAvatar avaAvatar = context.RelMat.GetExtended<AVAAvatar>(component);
				var avatar = (CVRAvatar)context.RelMat.GetConverted(avaAvatar);

				avatar.voiceParent = CVRAvatar.CVRAvatarVoiceParent.Head;
				avatar.voicePosition = c.voice_parent.transform.position - root.transform.position + c.voice_position;

				context.RelMat.AddConverted(component, avatar);
			}));
		}
	}
}

#endif
