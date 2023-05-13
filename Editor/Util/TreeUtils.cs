
using stf;
using UnityEngine;

namespace ava
{
	public class TreeUtils
	{
		/*public static GameObject findByUUID(GameObject go, string uuid)
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
		}*/

		public static GameObject findByID(GameObject go, string id)
		{
			foreach(var uuid in go.GetComponentsInChildren<STFUUID>())
			{
				if(uuid.id == id) return uuid.gameObject;
			}
			return null;
		}

		public static GameObject findByBoneId(GameObject go, string boneId)
		{
			foreach(var uuid in go.GetComponentsInChildren<STFUUID>())
			{
				if(uuid.boneId == boneId) return uuid.gameObject;
			}
			return null;
		}
	}
}
