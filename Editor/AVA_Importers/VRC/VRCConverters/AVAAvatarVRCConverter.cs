
#if VRCSDK3_FOUND

using ava.Components;
using stf.serialisation;
using UnityEngine;

namespace ava.Converters
{
	public class AVAAvatarVRCConverter : ISTFSecondStageConverter
	{
		public void convert(Component component, GameObject root)
		{
			var avaAvatar = (AVAAvatar)component;
			
			#if UNITY_EDITOR
            UnityEngine.Object.DestroyImmediate(component);
			#else
            UnityEngine.Object.Destroy(component);
			#endif
		}
	}
}

#endif
