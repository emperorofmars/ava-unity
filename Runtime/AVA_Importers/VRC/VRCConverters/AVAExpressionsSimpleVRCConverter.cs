
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

					var layerIdx = controller.layers.Length - 1;
					var idle = addHandGesture(controller, c, root, context, layer, layerIdx, 'l', "idle", 0);
					layer.stateMachine.AddEntryTransition(idle);

					addHandGesture(controller, c, root, context, layer, layerIdx, 'l', "open", 2);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'l', "fist", 1);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'l', "point", 3);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'l', "peace", 4);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'l', "rocknroll", 5);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'l', "pistol", 6);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'l', "thumbsup", 7);

					layers[controller.layers.Length - 1] = layer;
				}
				{
					controller.AddLayer("_vrc_hand_right");
					var layer = controller.layers.First(l => l.name == "_vrc_hand_right");
					layer.defaultWeight = 1;

					var layerIdx = controller.layers.Length - 1;
					var idle = addHandGesture(controller, c, root, context, layer, layerIdx, 'r', "idle", 0);
					layer.stateMachine.AddEntryTransition(idle);

					addHandGesture(controller, c, root, context, layer, layerIdx, 'r', "open", 2);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'r', "fist", 1);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'r', "point", 3);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'r', "peace", 4);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'r', "rocknroll", 5);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'r', "pistol", 6);
					addHandGesture(controller, c, root, context, layer, layerIdx, 'r', "thumbsup", 7);

					layers[controller.layers.Length - 1] = layer;
				}

				controller.layers = layers;

				avatar.customizeAnimationLayers = true;
				var baseLayers = new VRCAvatarDescriptor.CustomAnimLayer[5];
				
				var layerBase = new VRCAvatarDescriptor.CustomAnimLayer();
				layerBase.type = VRCAvatarDescriptor.AnimLayerType.Base;
				layerBase.isEnabled = false;
				layerBase.isDefault = true;
				baseLayers[0] = layerBase;
				
				var layerAdditive = new VRCAvatarDescriptor.CustomAnimLayer();
				layerAdditive.type = VRCAvatarDescriptor.AnimLayerType.Additive;
				layerAdditive.isDefault = true;
				baseLayers[1] = layerAdditive;
				
				var layerGesture = new VRCAvatarDescriptor.CustomAnimLayer();
				layerGesture.type = VRCAvatarDescriptor.AnimLayerType.Gesture;
				layerGesture.isDefault = true;
				baseLayers[2] = layerGesture;
				
				var layerAction = new VRCAvatarDescriptor.CustomAnimLayer();
				layerAction.type = VRCAvatarDescriptor.AnimLayerType.Action;
				layerAction.isDefault = true;
				baseLayers[3] = layerAction;
				
				var layerFX = new VRCAvatarDescriptor.CustomAnimLayer();
				layerFX.animatorController = controller;
				layerFX.type = VRCAvatarDescriptor.AnimLayerType.FX;
				layerFX.isEnabled = true;
				layerFX.isDefault = false;
				baseLayers[4] = layerFX;

				avatar.baseAnimationLayers = baseLayers;
				
				AssetDatabase.Refresh();
				EditorUtility.SetDirty(controller);
				AssetDatabase.SaveAssets();

				context.AddResource(controller);
				context.RelMat.AddConverted(component, avatar);
			}));
		}

		private AnimatorState addHandGesture(AnimatorController controller, AVAExpressionsSimple c, GameObject root, ISTFSecondStageContext context, AnimatorControllerLayer layer, int layerIdx, char hand, string gestureName, int conditionValue)
		{
			AnimatorState state = null;
			var animation = c.expressions.Find(e => e.mapping == (hand + ":" + gestureName))?.animation;
			if(animation != null)
			{
				Debug.Log($"EXPRESSION: {hand}:{gestureName} - {animation}");

				animation = (AnimationClip)context.GetConvertedResource(root, animation);

				// Why Unity, WHYYYY
				AssetDatabase.CreateAsset(animation, context.GetPathForResourcesThatMustExistInFS() + "/" + c.name + "-" + animation.name + "-animator-motion.anim");

				Debug.Log($"ANIMATION CONVERTED: {animation}");

				//state = controller.AddMotion(animation, layerIdx);
				state = layer.stateMachine.AddState(gestureName); // AAAAAAAAAAAAA
				controller.SetStateEffectiveMotion(state, animation, layerIdx);
			}
			else
			{
				state = layer.stateMachine.AddState(gestureName);
			}
			state.writeDefaultValues = true;

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
