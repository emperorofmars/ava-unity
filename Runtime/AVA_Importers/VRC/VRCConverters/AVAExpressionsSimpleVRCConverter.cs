
#if VRCSDK3_FOUND
#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
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

				var layers = new AnimatorControllerLayer[3];
				layers[0] = controller.layers[0];

				{
					controller.AddLayer("_vrc_test");
					var layer = controller.layers.First(l => l.name == "_vrc_test");
					layer.defaultWeight = 1;
					layer.name = "REEEEE";

					var state = layer.stateMachine.AddState("eeeee");
					layer.stateMachine.AddAnyStateTransition(state);

					layers[controller.layers.Length - 1] = layer;
				}
				{
					//var layer = new AnimatorControllerLayer();
					//layer.name = "_vrc_hands";
					//layer.defaultWeight = 1;
					//layer.blendingMode = AnimatorLayerBlendingMode.Override;
					
					var stateMachine = new AnimatorStateMachine();
					stateMachine.anyStatePosition = Vector3.up;
					stateMachine.entryPosition = Vector3.left;
					stateMachine.exitPosition = Vector3.right;
					var state = stateMachine.AddState("aaaaaaa");
					stateMachine.AddAnyStateTransition(state);
					
					var layer = new AnimatorControllerLayer {
						name = "_vrc_hands",
						defaultWeight = 1,
						blendingMode = AnimatorLayerBlendingMode.Override,
						stateMachine = stateMachine,
					};
					
					//layer.stateMachine = stateMachine;
					controller.AddLayer(layer);
					layers[controller.layers.Length - 1] = layer;
				}

				controller.layers = layers;

				var animator = avatar.GetComponent<Animator>();
				animator.runtimeAnimatorController = controller;
				
				AssetDatabase.Refresh();
				EditorUtility.SetDirty(controller);
				AssetDatabase.SaveAssets();

				context.AddResource(controller);
				context.RelMat.AddConverted(component, avatar);
			}));
		}
	}
}

#endif
#endif
