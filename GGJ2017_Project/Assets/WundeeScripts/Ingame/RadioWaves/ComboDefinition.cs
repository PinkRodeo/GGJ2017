using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins;
using LitJson;
using UnityEngine;

using Wundee;
using Wundee.Stories;

namespace RadioWaves
{
	public class ComboDefinition : Definition<ComboListener>
	{
		public string[] combo;
		public string meaning;

		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
			var comboData = jsonData["combo"];

			if (comboData.IsArray)
			{
				combo = new string[comboData.Count];
				for (int i = 0; i < comboData.Count; i++)
				{
					combo[i] = comboData[i].ToString();
				}
			}
			else
			{
				Debug.Log("Invalid combo with key: " + definitionKey);
			}

			meaning = ContentHelper.ParseString(jsonData, "meaning", "<i>unknown</i>");
		}

		public override ComboListener GetConcreteType(object parent = null)
		{
			return new ComboListener()
			{
				combo = this.combo
			};
		}
	}

}