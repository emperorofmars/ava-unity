
using stf;
using UnityEngine;

namespace ava
{
	public class TreeUtils
	{
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
