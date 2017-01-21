using UnityEngine;
using System.Collections;
using LitJson;
using RadioWaves;
using Wundee.Stories;

namespace Wundee
{
	public class SettlementDefinition : Definition<Settlement>
	{
		private Definition<Effect>[] _onStartRewardDefinitions; 

		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
			this.definitionKey = definitionKey;

			var keys = jsonData.Keys;

			if (keys.Contains(D.REWARDS_ON_START))
				this._onStartRewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS_ON_START], definitionKey);
			else
				this._onStartRewardDefinitions = new Definition<Effect>[0];

		}

		// parent == habitat
		public override Settlement GetConcreteType(object parent = null)
		{
			var radioChannel = parent as RadioChannel;

			var newSettlement = new Settlement(radioChannel);
			newSettlement.ExecuteEffectFromDefinition(ref _onStartRewardDefinitions);
			
			return newSettlement;
		}
	}
}

