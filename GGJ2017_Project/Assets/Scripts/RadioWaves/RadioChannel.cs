using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wundee;
using Wundee.Stories;

namespace RadioWaves
{
	[RequireComponent(typeof(SphereCollider))]
	public class RadioChannel : MonoBehaviour
	{
		public const string RADIOCHANNEL_TAG = "RadioChannelTag";

		[Range(1f, 20f)] public float m_Range = 1f;

		[HideInInspector] public Ether m_Ether;

		private Transform m_Transform;
		private SphereCollider m_SphereCollider;

		public string DefinitionKey = "RC_DEFAULT";

		[ReadOnly]
		public RadioChannelDefinition definition;
		[ReadOnly]
		public Effect[] onStartRewards;
		[ReadOnly]
		public Effect[] onTuneRewards;

		[ReadOnly]
		public Blackboard blackboard;

		public Settlement settlement;

		private void Reset()
		{
			transform.tag = RADIOCHANNEL_TAG;
			gameObject.layer = 8;

			var currentObjectName = gameObject.name;
			if (!currentObjectName.Contains("RadioChannel"))
			{
				gameObject.name = "RadioChannel - " + gameObject.name;
			}
		}

		private void Awake()
		{
			m_SphereCollider = GetComponent<SphereCollider>();
			m_Transform = transform;
			m_SphereCollider.radius = m_Range;
		}

		private void Start()
		{
			Wundee.Game.instance.definitions.radioChannelDefinitions[DefinitionKey].MakeConcreteType(this);

			onStartRewards.ExecuteEffects();
		}

		// Update is called once per frame
		void Update()
		{

		}

		void OnDrawGizmos()
		{
			DebugExtension.DrawCapsule(transform.localPosition, transform.localPosition + Vector3.up * 0.01f, Color.blue, m_Range);

		}

		public void TuneIn()
		{
			onTuneRewards.ExecuteEffects();
		}
	}
}