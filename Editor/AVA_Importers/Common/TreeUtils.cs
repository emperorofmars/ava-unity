
#if UNITY_EDITOR

using stf.Components;
using UnityEngine;

namespace stf.ava.importer.common
{
	public class TreeUtils
	{
		public static GameObject findByUUID(GameObject go, string uuid)
		{
			STFUUID c = go.GetComponent<STFUUID>();
			if(c != null && c.id == uuid)
			{
				return go;
			}
			else
			{
				for(int i = 0; i < go.transform.childCount; i++)
				{
					GameObject ret = findByUUID(go.transform.GetChild(i).gameObject, uuid);
					if(ret != null) return ret;
				}
			}
			return null;
		}
	}
}

#endif
