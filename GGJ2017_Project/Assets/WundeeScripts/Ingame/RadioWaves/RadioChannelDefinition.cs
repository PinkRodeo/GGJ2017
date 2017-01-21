using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;


using Wundee;
using Wundee.Stories;

namespace RadioWaves
{
	public class RadioChannelDefinition : Definition<RadioChannel>
	{
		private Definition<Effect>[] _onStartRewardDefinitions;
		private Definition<Effect>[] _onTuneRewardDefinitions;

		public override void ParseDefinition (string definitionKey, JsonData jsonData)
		{
			this.definitionKey = definitionKey;

			var keys = jsonData.Keys;


			if (keys.Contains(D.REWARDS_ON_START))
				this._onStartRewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS_ON_START], definitionKey);
			else
				this._onStartRewardDefinitions = new Definition<Effect>[0];

			if (keys.Contains(D.REWARDS_ON_TUNE))
				this._onTuneRewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS_ON_TUNE], definitionKey);
			else
				this._onTuneRewardDefinitions = new Definition<Effect>[0];
		}

		public override RadioChannel GetConcreteType (object parent = null)
		{
			throw new System.NotImplementedException();
		}

		public void MakeConcreteType(RadioChannel p_RadioChannel)
		{
			p_RadioChannel.definition = this;

			p_RadioChannel.onStartRewards = _onStartRewardDefinitions.GetConcreteTypes(p_RadioChannel);
			p_RadioChannel.onTuneRewards = _onTuneRewardDefinitions.GetConcreteTypes(p_RadioChannel);
		}
	}
}

