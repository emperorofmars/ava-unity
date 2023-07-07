
#if VRCSDK3_FOUND
#if UNITY_EDITOR

using System.Collections.Generic;
using System.Threading.Tasks;
using ava.Components;
using stf.serialisation;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace ava.Converters
{
	public class AVAExpressionsSimpleVRCConverter : ISTFSecondStageConverter
	{
		public void Convert(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var c = (AVAExpressionsSimple)component;
			context.AddTask(new Task(() => {
				AVAAvatar avaAvatar = context.RelMat.GetExtended<AVAAvatar>(component);
				AVAHumanoidMapping humanoid = context.RelMat.GetExtended<AVAHumanoidMapping>(component);
				
				var avatar = (VRCAvatarDescriptor)context.RelMat.GetConverted(avaAvatar);

				//var templateController = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/empty.controller");

				var controller = AnimatorController.CreateAnimatorControllerAtPath(context.GetPathForResourcesThatMustExistInFS() + "/" + component.name + "-animator-controller.controller");
				controller.AddParameter(new AnimatorControllerParameter {name = "_BlendWeight", type = AnimatorControllerParameterType.Float, defaultFloat = 1.0f});

				// generate animator controller here

				var animator = avatar.GetComponent<Animator>();
				animator.runtimeAnimatorController = controller;
				context.AddResource(controller);
				
				context.RelMat.AddConverted(component, avatar);
			}));
		}
	}
}

#endif
#endif
