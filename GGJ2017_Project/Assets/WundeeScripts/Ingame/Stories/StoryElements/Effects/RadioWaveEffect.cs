using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RadioWaves;
using HAM;
using LitJson;

namespace Wundee.Stories
{

	public class TransmitTextEffect : Effect
	{
		private string _TextToTransmit;

		public override void ParseParams(JsonData parameters)
		{
			

			_TextToTransmit = ContentHelper.ParseString(parameters, D.TEXT, "MISSING_TEXT");
			if (_TextToTransmit == "MISSING_TEXT")
			{
				Logger.Error(definition.definitionKey + " Invalid text found");
			}


		}

		public override void ExecuteEffect()
		{
			var testParent = parentStoryNode;

			HAM.Game.speechController.Say(_TextToTransmit);

			//parentStoryNode.parentStory.parentPerson.radioChannel;

		}
	}


}
