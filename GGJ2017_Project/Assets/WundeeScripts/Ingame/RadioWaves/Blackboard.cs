using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wundee
{
	[Serializable]
	public class Blackboard
	{
		[SerializeField]
		private Dictionary<string, string> memory;

		public void AddValue(string p_Key, string p_Value)
		{
			if (!memory.ContainsKey(p_Key))
			{
				memory.Add(p_Key, p_Value);
			}
		}

		public string GetValue(string p_Key)
		{
			string value;
			if (memory.TryGetValue(p_Key, out value))
			{
				return value;
			}
			return null;
		}

		public void RemoveValue(string p_Key)
		{
			memory.Remove(p_Key);
		}
	}


}
