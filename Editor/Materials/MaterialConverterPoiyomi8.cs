
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace stf.serialisation
{

	public class STFShaderTranslatorPoiyomi8 : ISTFShaderTranslator
	{
		public static readonly string _SHADER_NAME = ".poiyomi/Poiyomi 8.1/Poiyomi Toon";

		public Material TranslateSTFToUnity(ISTFImporter state, STFMaterial stfMaterial)
		{
			var ret = new Material(Shader.Find(_SHADER_NAME));
			foreach(var property in stfMaterial.Properties)
			{
				if(property.Name == "Albedo")
				{
					if(property.Type == "Texture")
					{
						state.AddTask(new Task(() => {
							ret.SetTexture("_MainTex", state.GetResource(property.Value));
						}));
					}
				}
			}
			return ret;
		}

		public STFMaterial TranslateUnityToSTF(ISTFExporter state, Material material)
		{
			var ret = ScriptableObject.CreateInstance<STFMaterial>();
			ret.name = material.name;
			ret.ShaderTargets.Add(new STFMaterial.ShaderTarget {target = "Unity", shaders = new List<string> {_SHADER_NAME}});
			
			/*for(int i = 0; i < material.shader.GetPropertyCount(); i++)
			{
				foreach(var attribute in material.shader.GetPropertyAttributes(i))
				{
					Debug.Log($"Property Index: {i}, Attribute: {attribute}");
				}
			}
			foreach(var name in material.GetTexturePropertyNames())
			{
				Debug.Log($"GetTexturePropertyNames: {name}");
			}*/



			if(material.GetTexture("_MainTex") != null)
			{
				Texture mainTex = material.GetTexture("_MainTex");
				Debug.Log($"Main Tex Type: {mainTex.GetType()}");
				state.RegisterResource(mainTex);
				ret.Properties.Add(new STFMaterial.ShaderProperty {
					Name = "Albedo",
					Type = "Texture",
					Value = state.GetResourceId(mainTex)
				});
			}
			return ret;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Register_AVAAvatar
	{
		static Register_AVAAvatar()
		{
			STFShaderRegistry.Converters.Add(STFShaderTranslatorPoiyomi8._SHADER_NAME, new STFShaderTranslatorPoiyomi8());
		}
	}
#endif

}