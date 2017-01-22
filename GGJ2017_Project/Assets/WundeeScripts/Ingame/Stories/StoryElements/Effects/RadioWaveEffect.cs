using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RadioWaves;
using HAM;
using LitJson;
using UnityEditor;

namespace Wundee.Stories
{

	public class TransmitTextEffect : Effect
	{
		private string _TextToTransmit;


		protected Definition<Effect>[] _onCompleteEffects;
		protected Definition<Effect>[] _onBreakoffEffects;




		public override void ParseParams(JsonData parameters)
		{
			var keys = parameters.Keys;


			_TextToTransmit = ContentHelper.ParseString(parameters, D.TEXT, "MISSING_TEXT");
			if (_TextToTransmit == "MISSING_TEXT")
			{
				Logger.Error(definition.definitionKey + " Invalid text found");
			}



			if (keys.Contains(D.EFFECTS_ON_COMPLETE))
				_onCompleteEffects = EffectDefinition.ParseDefinitions(parameters[D.EFFECTS_ON_COMPLETE], definition.definitionKey);
			else
				_onCompleteEffects = new Definition<Effect>[0];

			if (keys.Contains(D.EFFECTS_ON_BREAKOFF))
				_onBreakoffEffects = EffectDefinition.ParseDefinitions(parameters[D.EFFECTS_ON_BREAKOFF], definition.definitionKey);
			else
				_onBreakoffEffects = new Definition<Effect>[0];

		}

		public override void ExecuteEffect()
		{
			var testParent = parentStoryNode;
		

			HAM.Game.speechController.Say(_TextToTransmit, 
				() =>
				{
					var confirmComboDef = Wundee.Game.instance.definitions.comboDefinitions["CONFIRM_COMBO"];
					HAM.Game.midiController.AddComboListener(confirmComboDef.combo, () =>
					{
						parentStoryNode.parentStory.parentPerson.ExecuteEffectFromDefinition(ref _onCompleteEffects);

					});
				},
				() =>
				{
					parentStoryNode.parentStory.parentPerson.ExecuteEffectFromDefinition(ref _onBreakoffEffects);
				}
			);


			//.radioChannel;

		}
	}

	public class WaitForComboEffect : Effect
	{

		protected Definition<ComboListener> _comboDefinition;
		protected Definition<Effect>[] _effectDefinitions;


		public override void ParseParams(JsonData parameters)
		{
			var keys = parameters.Keys;

			if (keys.Contains(D.EFFECTS))
				_effectDefinitions = EffectDefinition.ParseDefinitions(parameters[D.EFFECTS], definition.definitionKey);
			else
				_effectDefinitions = new Definition<Effect>[0];

			_comboDefinition = ContentHelper.GetDefinition<ComboDefinition, ComboListener>(parameters["comboDefinition"], definition.definitionKey, "_COMBO_");
		}

		public override void ExecuteEffect()
		{
			var comboArray = _comboDefinition.GetConcreteType();
			HAM.Game.midiController.AddComboListener(comboArray.combo, () =>
			{
				parentStoryNode.parentStory.parentPerson.ExecuteEffectFromDefinition(ref _effectDefinitions);
			});
		}
	}


	public class AddComboToDictionaryEffect : Effect
	{
		public string comboDefinitionKey;

		public override void ParseParams(JsonData parameters)
		{
			comboDefinitionKey = ContentHelper.ParseString(parameters, "comboDefinitionKey", "BORK");
		}

		public override void ExecuteEffect()
		{
			var comboDefition = Wundee.Game.instance.definitions.comboDefinitions[comboDefinitionKey];

			if (comboDefition == null)
			{
				Debug.LogError("[AddComboToDictionaryEffect] Could not find combo with key: " + comboDefinitionKey);
				return;
			}

			HAM.Game.speechController.dictionaryUI.AddToDictionary(comboDefition);
		}
	}

	public class RemoveComboFromDictionaryEffect : Effect
	{
		public string comboDefinitionKey;

		public override void ParseParams (JsonData parameters)
		{
			comboDefinitionKey = ContentHelper.ParseString(parameters, "comboDefinitionKey", "BORK");
		}

		public override void ExecuteEffect ()
		{
			var comboDefition = Wundee.Game.instance.definitions.comboDefinitions[comboDefinitionKey];

			if (comboDefition == null)
			{
				Debug.LogError("[RemoveComboFromDictionaryEffect] Could not find combo with key: " + comboDefinitionKey);
				return;
			}

			HAM.Game.speechController.dictionaryUI.RemoveFromDictionary(comboDefition);

		}
	}

}
