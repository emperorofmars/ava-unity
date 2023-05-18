
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using stf.serialisation;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ava
{

	public class STFShaderTranslatorPoiyomi8 : ISTFShaderTranslator
	{
		public static readonly string _SHADER_NAME = ".poiyomi/Poiyomi 8.1/Poiyomi Toon";

		public Material TranslateSTFToUnity(ISTFImporter state, STFMaterial stfMaterial)
		{
			var ret = new Material(Shader.Find(_SHADER_NAME));
			STFShaderTranslatorHelpers.ImportTexture(state, ret, stfMaterial, "_MainTex", "albedo");
			STFShaderTranslatorHelpers.ImportTexture(state, ret, stfMaterial, "_BumpMap", "normal");
			STFShaderTranslatorHelpers.ImportTexture(state, ret, stfMaterial, "_ClippingMask", "alpha");
			foreach(var property in stfMaterial.Properties)
			{
				if(property.Name == "lighting_hint" && property.Type == "string")
				{
					ret.SetFloat("_ShadingEnabled", 1);
					ret.SetFloat("_LightingAdditiveType", property.Value == "realistic" ? 0 : 1);
					ret.SetFloat("_LightingMode", property.Value == "realistic" ? 6 : 0);
				}
				else if(property.Name == "MochieMetallicMaps")
				{
					STFShaderTranslatorHelpers.ImportTexture(state, ret, stfMaterial, "_MochieMetallicMaps", "MochieMetallicMaps");
					ret.SetFloat("_MochieBRDF", 1);
					ret.SetFloat("_MochieMetallicMultiplier", 1);
				}
				else if(property.Name == "transparency_mode")
				{
					int mode = 0;
					switch(property.Value) {
						case "opaque": mode = 0; break;
						case "cutout": mode = 1; break;
						case "transparent": mode = 3; break;
					}
					ret.SetFloat("_Mode", mode);
				}
			}

			return ret;
		}

		public STFMaterial TranslateUnityToSTF(ISTFExporter state, Material material)
		{
			var ret = ScriptableObject.CreateInstance<STFMaterial>();
			ret.name = material.name;
			ret.ShaderTargets.Add(new STFMaterial.ShaderTarget {target = "Unity", shaders = new List<string> {_SHADER_NAME}});

			STFShaderTranslatorHelpers.ExportTexture(state, material, ret, "_MainTex", "albedo");
			STFShaderTranslatorHelpers.ExportTexture(state, material, ret, "_BumpMap", "normal");
			STFShaderTranslatorHelpers.ExportTexture(state, material, ret, "_ClippingMask", "alpha");
			STFShaderTranslatorHelpers.ExportTexture(state, material, ret, "_MochieMetallicMaps", "MochieMetallicMaps");

			ret.Properties.Add(new STFMaterial.ShaderProperty {
				Name = "lighting_hint",
				Type = "string",
				Value = material.GetFloat("_LightingAdditiveType") == 0 ? "realistic" : "toony"
			});
			string mode = "";
			switch(material.GetFloat("_Mode")) {
				case 0: mode = "opaque"; break;
				case 1: mode = "cutout"; break;
				case 3: mode = "transparent"; break;
			}
			ret.Properties.Add(new STFMaterial.ShaderProperty {
				Name = "transparency_mode",
				Type = "string",
				Value = mode
			});
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
