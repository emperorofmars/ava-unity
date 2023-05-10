
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

namespace stf.ava.importer.common
{
	public class ConvertTree: ScriptableObject
	{
		public static GameObject convertTree(GameObject rootAVA, Dictionary<Type, IComponentConverter> appConverters, string assetName)
		{
			GameObject targetRoot = Instantiate(rootAVA);

			convertComponents(targetRoot, targetRoot, CommonConverters.getConverters(), assetName);
			convertComponents(targetRoot, targetRoot, appConverters, assetName);

			cleanupComponents(targetRoot, targetRoot, CommonConverters.getConverters());
			cleanupComponents(targetRoot, targetRoot, appConverters);
			return targetRoot;
		}

		private static void convertComponents(GameObject node, GameObject root, Dictionary<Type, IComponentConverter> converters, string assetName)
		{
			foreach(var item in converters)
			{
				Component[] components = node.GetComponents(item.Key);
				foreach(Component component in components)
				{
					item.Value.convert(node, root, component, assetName);
				}
			}
			for(int i = 0; i < node.transform.childCount; i++)
			{
				convertComponents(node.transform.GetChild(i).gameObject, root, converters, assetName);
			}
		}

		private static void cleanupComponents(GameObject node, GameObject root, Dictionary<Type, IComponentConverter> converters)
		{
			foreach(var item in converters)
			{
				if(node.GetComponent(item.Key) != null && item.Value.cleanup())
				{
					DestroyImmediate(node.GetComponent(item.Key));
				}
			}
			for(int i = 0; i < node.transform.childCount; i++)
			{
				cleanupComponents(node.transform.GetChild(i).gameObject, root, converters);
			}
		}
	}
}

#endif
