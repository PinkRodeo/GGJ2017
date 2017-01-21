using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Wundee;
using Wundee.Stories;
using HAM;

namespace RadioWaves
{
	[RequireComponent(typeof(SphereCollider))]
	[RequireComponent(typeof(AudioSource))]
	public class RadioChannel : MonoBehaviour
	{
		public const string RADIOCHANNEL_TAG = "RadioChannelTag";

		[Range(1f, 20f)] public float m_Range = 1f;

		[HideInInspector] public Ether m_Ether;

		private Transform m_Transform;
		private SphereCollider m_SphereCollider;
		private AudioSource m_AudioSource;

		public string DefinitionKey = "RC_DEFAULT";

		[ReadOnly]
		public RadioChannelDefinition definition;


		public Person p_Person;

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

			m_AudioSource = GetComponent<AudioSource> ();
			m_AudioSource.playOnAwake = false;
			m_AudioSource.minDistance = m_Range*0.75f;
			m_AudioSource.maxDistance = m_Range;
			m_AudioSource.spatialBlend = 1.0f;
			m_AudioSource.spatialize = false;
		}

		private void Start()
		{
			Wundee.Game.instance.definitions.radioChannelDefinitions[DefinitionKey].MakeConcreteType(this);
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
			HAM.Game.speechController.SetSource (m_AudioSource);
			p_Person.TuneIn();
		}
	}
}