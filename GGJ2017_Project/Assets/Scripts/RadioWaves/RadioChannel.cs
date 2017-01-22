using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Wundee;
using Wundee.Stories;
using HAM;
using UnityEditor;

namespace RadioWaves
{
	[RequireComponent(typeof(SphereCollider))]
	[RequireComponent(typeof(AudioSource))]
	public class RadioChannel : MonoBehaviour
	{
		public const string RADIOCHANNEL_TAG = "RadioChannelTag";

		[Range(1f, 30f)] public float m_Range = 15f;

		[HideInInspector] public Ether m_Ether;

		private Transform m_Transform;
		private SphereCollider m_SphereCollider;
		private AudioSource m_AudioSource;
		private AudioClip[] m_AudioClips;

		public string DefinitionKey = "RC_DEFAULT";

		[ReadOnly]
		public RadioChannelDefinition definition;


		public Person p_Person;

		private float inTuneTimer = 0.0f;
		private bool tunedIn = false;
		private bool connected = false;

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
			m_AudioSource.dopplerLevel = 0.1f;
		}

		private void Start()
		{
			Wundee.Game.instance.definitions.radioChannelDefinitions[DefinitionKey].MakeConcreteType(this);
			m_AudioClips = p_Person.GetAudioClips ();
		}

		// Update is called once per frame
		void Update()
		{
			if (tunedIn && !connected) {
				inTuneTimer += UnityEngine.Time.deltaTime;
				HAM.Game.speechController.transmissionUI.SetVisible(true);
				HAM.Game.speechController.transmissionUI.TextContent = "";
				HAM.Game.speechController.transmissionUI.PortraitTexture.overrideSprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Portraits/" + p_Person.definition.portraitKey + ".png", typeof(Sprite));

				if (inTuneTimer > 1.5f) {
					connected = true;
					p_Person.TuneIn();
				}
			}
		}

		void OnDrawGizmos()
		{
			DebugExtension.DrawCapsule(transform.localPosition, transform.localPosition + Vector3.up * 0.01f, Color.blue, m_Range);

		}

		public void TuneIn()
		{
			HAM.Game.speechController.SetSource (m_AudioSource);
			HAM.Game.speechController.SetClips (m_AudioClips);

			connected = false;
			tunedIn = true;
			inTuneTimer = 0.0f;


		}

		public void TuneOut()
		{
			tunedIn = false;
			HAM.Game.midiController.ClearComboListeners();
		}
	}
}