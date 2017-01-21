﻿using UnityEngine;
using System.Collections;
using LitJson;
using RadioWaves;
using Wundee.Stories;

namespace Wundee
{
	public class PersonDefinition : Definition<Person>
	{
		public Definition<Effect>[] _onStartRewardDefinitions;
		public Definition<Effect>[] _onTuneInRewardDefinitions;


		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
			this.definitionKey = definitionKey;

			var keys = jsonData.Keys;

			if (keys.Contains(D.REWARDS_ON_START))
				this._onStartRewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS_ON_START], definitionKey);
			else
				this._onStartRewardDefinitions = new Definition<Effect>[0];


			if (keys.Contains(D.REWARDS_ON_TUNE))
				this._onTuneInRewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS_ON_TUNE], definitionKey);
			else
				this._onTuneInRewardDefinitions = new Definition<Effect>[0];

		}

		// parent == habitat
		public override Person GetConcreteType(object parent = null)
		{
			var radioChannel = parent as RadioChannel;

			var newPerson = new Person(radioChannel);
			newPerson.definition = this;
			newPerson.ExecuteEffectFromDefinition(ref _onStartRewardDefinitions);
			
			return newPerson;
		}
	}
}
