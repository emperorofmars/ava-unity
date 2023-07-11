
#if VRCSDK3_FOUND
#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ava.Components;
using stf;
using stf.serialisation;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace ava.Converters
{
	public class AVAExpressionsSimpleVRCConverter : ISTFSecondStageConverter
	{
		public Dictionary<string, UnityEngine.Object> CollectOriginalResources(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var c = (AVAExpressionsSimple)component;
			var ret = new Dictionary<string, UnityEngine.Object>();
			var assetInfo = root.GetComponent<STFAssetInfo>();
			foreach(var e in c.expressions)
			{
				var meta = assetInfo.originalMetaInformation?.resourceInfo?.Find(r => r.originalResource == e.animation);
				if(meta == null) meta = assetInfo.addonMetaInformation?.Find(a => a.resourceInfo?.Find(r => r.originalResource == e.animation) != null)?.resourceInfo?.Find(r => r.originalResource == e.animation); // this is stupid
				if(meta != null) ret.Add(meta.id, e.animation);
			}
			return ret;
		}

		public void Convert(Component component, GameObject root, ISTFSecondStageContext context)
		{
			var c = (AVAExpressionsSimple)component;
			context.AddTask(new Task(() => {
				AVAAvatar avaAvatar = context.RelMat.GetExtended<AVAAvatar>(component);
				AVAHumanoidMapping humanoid = context.RelMat.GetExtended<AVAHumanoidMapping>(component);
				
				var avatar = (VRCAvatarDescriptor)context.RelMat.GetConverted(avaAvatar);

				var controller = AnimatorController.CreateAnimatorControllerAtPath(context.GetPathForResourcesThatMustExistInFS() + "/" + component.name + "-animator-controller.controller");
				controller.AddParameter(new AnimatorControllerParameter {name = "_BlendWeight", type = AnimatorControllerParameterType.Float, defaultFloat = 1.0f});
				controller.AddParameter(new AnimatorControllerParameter {name = "GestureLeft", type = AnimatorControllerParameterType.Int, defaultInt = 0});
				controller.AddParameter(new AnimatorControllerParameter {name = "GestureLeftWeight", type = AnimatorControllerParameterType.Float, defaultFloat = 1.0f});
				controller.AddParameter(new AnimatorControllerParameter {name = "GestureRight", type = AnimatorControllerParameterType.Int, defaultInt = 0});
				controller.AddParameter(new AnimatorControllerParameter {name = "GestureRightWeight", type = AnimatorControllerParameterType.Float, defaultFloat = 1.0f});

				var layers = new AnimatorControllerLayer[3];
				layers[0] = controller.layers[0];
				{
					controller.AddLayer("_vrc_hand_left");
					var layer = controller.layers.First(l => l.name == "_vrc_hand_left");
					layer.defaultWeight = 1;

					var idle = addHandGesture(c, root, context, layer, 'l', "idle", 0);
					layer.stateMachine.AddEntryTransition(idle);

					addHandGesture(c, root, context, layer, 'l', "open", 2);
					addHandGesture(c, root, context, layer, 'l', "fist", 1);
					addHandGesture(c, root, context, layer, 'l', "point", 3);
					addHandGesture(c, root, context, layer, 'l', "peace", 4);
					addHandGesture(c, root, context, layer, 'l', "rocknroll", 5);
					addHandGesture(c, root, context, layer, 'l', "pistol", 6);
					addHandGesture(c, root, context, layer, 'l', "thumbsup", 7);

					layers[controller.layers.Length - 1] = layer;
				}
				{
					controller.AddLayer("_vrc_hand_right");
					var layer = controller.layers.First(l => l.name == "_vrc_hand_right");
					layer.defaultWeight = 1;

					var idle = addHandGesture(c, root, context, layer, 'r', "idle", 0);
					layer.stateMachine.AddEntryTransition(idle);

					addHandGesture(c, root, context, layer, 'r', "open", 2);
					addHandGesture(c, root, context, layer, 'r', "fist", 1);
					addHandGesture(c, root, context, layer, 'r', "point", 3);
					addHandGesture(c, root, context, layer, 'r', "peace", 4);
					addHandGesture(c, root, context, layer, 'r', "rocknroll", 5);
					addHandGesture(c, root, context, layer, 'r', "pistol", 6);
					addHandGesture(c, root, context, layer, 'r', "thumbsup", 7);

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

		private AnimatorState addHandGesture(AVAExpressionsSimple c, GameObject root, ISTFSecondStageContext context, AnimatorControllerLayer layer, char hand, string gestureName, int conditionValue)
		{
			var state = layer.stateMachine.AddState(gestureName);
			state.writeDefaultValues = true;
			var animation = c.expressions.Find(e => e.mapping == (hand + ":" + gestureName))?.animation;
			if(animation != null)
			{
				Debug.Log($"EXPRESSION: {hand}:{gestureName} - {animation}");
				
				animation = (AnimationClip)context.GetConvertedResource(root, animation);

				Debug.Log($"ANIMATION CONVERTED: {animation}");

				state.motion = animation;
			}
			var transition = layer.stateMachine.AddAnyStateTransition(state);
			transition.AddCondition(AnimatorConditionMode.Equals, conditionValue, hand == 'l' ? "GestureLeft" : "GestureRight");
			transition.hasExitTime = false;
			transition.exitTime = 0;
			transition.offset = 0;
			transition.hasFixedDuration = true;
			transition.duration = 0.1f;
			return state;
		}
	}
}

#endif
#endif
