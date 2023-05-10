
#if UNITY_EDITOR

using stf.ava.importer.common;
using stf.Components;
using UnityEditor;
using UnityEngine;

namespace oap.ava.importer.common
{
    public class OAP_STF_twist_constraint_converter : IComponentConverter
    {
		public bool cleanup()
		{
			return true;
		}
		
        public void convert(GameObject node, GameObject root, Component originalComponent, string assetName)
        {
			STFTwistConstraint s = (STFTwistConstraint)originalComponent;
            var ret = node.AddComponent<UnityEngine.Animations.RotationConstraint>();
			ret.weight = s.weight;
			ret.rotationAxis = UnityEngine.Animations.Axis.Y;

			var source = new UnityEngine.Animations.ConstraintSource();
			source.weight = 1;
			GameObject sourceTransformGO = s.source;// TreeUtils.findByUUID(root, s.source_uuid);
			if(sourceTransformGO != null) source.sourceTransform = sourceTransformGO.transform;

			ret.AddSource(source);
			ret.locked = true;
			ret.constraintActive = true;
        }
    }

	[InitializeOnLoad]
	class Register_OAP_STF_twist_constraint_converter
	{
		static Register_OAP_STF_twist_constraint_converter()
		{
			CommonConverters.addConverter(typeof(STFTwistConstraint), new OAP_STF_twist_constraint_converter());
		}
	}
}

#endif
