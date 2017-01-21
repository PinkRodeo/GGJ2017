using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HAM;

namespace RadioWaves
{
	[RequireComponent(typeof(SphereCollider))]
	public class PlayerTuner : MonoBehaviour
	{

		[SerializeField]
		private SphereCollider m_SphereCollider;

		[ReadOnly, SerializeField]
		private List<RadioChannel> m_ConnectedChannels = new List<RadioChannel>();

		public AudioClip soundOn;
		public AudioClip soundOff;

		private AudioSource m_AudioSource;

		private void Reset()
		{
			gameObject.name = "PlayerTuner";
			gameObject.layer = 8;

			m_SphereCollider = GetComponent<SphereCollider>();
		}

		private void Awake()
		{
			m_ConnectedChannels = new List<RadioChannel>();

			m_AudioSource = GetComponent<AudioSource>();

			HAM.Game.midiController.AddKnobListener (74, OnKnobX);
			HAM.Game.midiController.AddKnobListener (71, OnKnobY);
			HAM.Game.midiController.AddKnobListener (79, OnKnobZ);
		}

		private void OnTriggerEnter(Collider p_Collider)
		{
			var radioChannel = p_Collider.GetComponent<RadioChannel>();

			if (radioChannel)
			{
				Debug.Log("Player tuned into: " + p_Collider.gameObject.name);
				m_ConnectedChannels.Add(radioChannel);
				HAM.Game.SetStaticVolume (0.1f);
				radioChannel.TuneIn();

				m_AudioSource.clip = soundOn;
				m_AudioSource.Play ();
			}
		}

		private void OnTriggerExit(Collider p_Collider)
		{
			var radioChannel = p_Collider.GetComponent<RadioChannel>();

			if (radioChannel)
			{
				Debug.Log("Player tuned out of: " + p_Collider.gameObject.name);
				m_ConnectedChannels.Remove(radioChannel);
				HAM.Game.speechController.Clear ();
				radioChannel.TuneOut();

				m_AudioSource.clip = soundOff;
				m_AudioSource.Play ();
			}

			if (m_ConnectedChannels.Count == 0) {
				HAM.Game.SetStaticVolume (1.0f);
			}
		}


		private void OnDrawGizmos ()
		{
			DebugExtension.DrawCapsule(transform.localPosition, transform.localPosition + Vector3.up * 0.01f, Color.green, m_SphereCollider.radius);

		}

		private void Update()
		{
			if (m_ConnectedChannels.Count > 0)
			{
				transform.LookAt (m_ConnectedChannels [0].transform.position);
			}


			var localPos = transform.localPosition;
			var speed = 20f;

			if (Input.GetKey(KeyCode.A))
			{
				localPos.x += speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.D))
			{
				localPos.x -= speed * Time.deltaTime;
			}


			if (Input.GetKey(KeyCode.W))
			{
				localPos.z -= speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.S))
			{
				localPos.z += speed * Time.deltaTime;
			}

			transform.localPosition = localPos;
		}

		private void OnKnobX (float value){
			var localPos = transform.localPosition;
		
			localPos.x = Mathf.Round (value * 127f);

			transform.localPosition = localPos;
		}

		private void OnKnobY (float value){
			var localPos = transform.localPosition;

			localPos.y = Mathf.Round (value * 127f);

			transform.localPosition = localPos;

		}

		private void OnKnobZ (float value){
			var localPos = transform.localPosition;

			localPos.z = Mathf.Round (value * 127f);

			transform.localPosition = localPos;

		}

	}
}