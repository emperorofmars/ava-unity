
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

namespace stf.ava.importer.common
{
	public interface IComponentConverter
	{
		void convert(GameObject node, GameObject root, Component originalComponent, string assetName);
		bool cleanup();
	}

	public class CommonConverters
	{
		static Dictionary<Type, IComponentConverter> converters = new Dictionary<Type, IComponentConverter>();

		public static void addConverter(Type type, IComponentConverter converter)
		{
			converters.Add(type, converter);
		}

		public static Dictionary<Type, IComponentConverter> getConverters()
		{
			return converters;
		}

		public static IComponentConverter getConverter(Type type)
		{
			return converters[type];
		}

		public static Type[] getSupportedTypes()
		{
			Type[] arr = new Type[converters.Keys.Count];
			converters.Keys.CopyTo(arr, 0);
			return arr;
		}
	}
}

#endif
