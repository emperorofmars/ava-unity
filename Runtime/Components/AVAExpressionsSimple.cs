using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using stf.Components;
using stf.serialisation;
using UnityEngine;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava.Components
{
	public class AVAExpressionsSimple : ASTFComponent
	{
		public static string _TYPE = "AVA.expressions_simple";
		
		[Serializable]
		public class SimpleExpression // for handgestures and such
		{
			public string mapping;
			public string semantics;
			public AnimationClip animation;
		}
		
		[Serializable]
		public class Toggle
		{
			public string name;
			public AnimationClip animation;
			// target object for gameobject toggle, or blendshape, or material switch, ...
		}
		
		[Serializable]
		public class Puppet
		{
			public string name;
			public AnimationClip animation; // use blendtree resource
		}

		public List<SimpleExpression> expressions = new List<SimpleExpression>();
		public List<Toggle> toggles = new List<Toggle>();
		public List<Puppet> puppets = new List<Puppet>();
	}

	public class AVAExpressionsSimpleImporter : ASTFComponentImporter
	{
		override public void ParseFromJson(ISTFImporter state, ISTFAsset asset, JToken json, string id, GameObject go)
		{
			var c = go.AddComponent<AVAExpressionsSimple>();
			state.AddComponent(id, c);
			c.id = id;
			this.ParseRelationships(json, c);

			foreach(JObject e in json["expressions"])
			{
				c.expressions.Add(new AVAExpressionsSimple.SimpleExpression{mapping = (string)e["mapping"], semantics = (string)e["semantics"], animation = (AnimationClip)state.GetResource((string)e["animation"])});
			}
		}
	}

	public class AVAExpressionsSimpleExporter : ASTFComponentExporter
	{
		public override List<KeyValuePair<UnityEngine.Object, Dictionary<string, object>>> GatherResources(Component component)
		{
			var c = (AVAExpressionsSimple)component;
			var ret = new List<KeyValuePair<UnityEngine.Object, Dictionary<string, System.Object>>>();
			foreach(var expression in c.expressions)
			{
				ret.Add(new KeyValuePair<UnityEngine.Object, Dictionary<string, object>>(expression.animation, new Dictionary<string, System.Object> {{"root", component.gameObject}}));
			}
			return ret;
		}

		override public JToken SerializeToJson(ISTFExporter state, Component component)
		{
			var c = (AVAExpressionsSimple)component;
			var ret = new JObject();
			ret.Add("type", AVAExpressionsSimple._TYPE);
			this.SerializeRelationships(c, ret);

			var jsonExpressions = new JArray();
			ret.Add("expressions", jsonExpressions);
			foreach(var expression in c.expressions)
			{
				jsonExpressions.Add(new JObject{{"mapping", expression.mapping}, {"semantics", expression.semantics}, {"animation", state.GetResourceId(expression.animation)}});
			}

			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAExpressionsSimple
	{
		static Register_AVAExpressionsSimple()
		{
			STFRegistry.RegisterComponentImporter(AVAExpressionsSimple._TYPE, new AVAExpressionsSimpleImporter());
			STFRegistry.RegisterComponentExporter(typeof(AVAExpressionsSimple), new AVAExpressionsSimpleExporter());
		}
	}
#endif
}