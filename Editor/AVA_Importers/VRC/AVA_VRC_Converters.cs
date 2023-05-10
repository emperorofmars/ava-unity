
#if UNITY_EDITOR
#if VRCSDK3_FOUND

using stf.ava.Components;
using stf.ava.importer.common;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace stf.ava.importer.vrc
{
    public class OAP_AVA_avatar_converter : IComponentConverter
    {
		public bool cleanup()
		{
			return true;
		}
		
        public void convert(GameObject node, GameObject root, Component originalComponent, string assetName)
        {
			var _component = (AVAAvatar) originalComponent;
			var avatarDescriptor = node.AddComponent<VRCAvatarDescriptor>();
			var head = _component.viewport_parent;
			if(head != null && _component.viewport_position != null) avatarDescriptor.ViewPosition = head.transform.position - root.transform.position + _component.viewport_position;
        }
    }

	[InitializeOnLoad]
	class Register_OAP_AVA_avatar_converter
	{
		static Register_OAP_AVA_avatar_converter()
		{
			AVA_Importer_VRC.addConverter(typeof(AVAAvatar), new OAP_AVA_avatar_converter());
		}
	}
}

#endif
#endif
