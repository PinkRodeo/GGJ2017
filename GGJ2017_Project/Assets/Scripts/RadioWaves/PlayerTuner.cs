using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RadioWaves
{
	[RequireComponent(typeof(SphereCollider))]
	public class PlayerTuner : MonoBehaviour
	{

		[SerializeField]
		private SphereCollider m_SphereCollider;

		[ReadOnly, SerializeField]
		private List<RadioChannel> m_ConnectedChannels = new List<RadioChannel>();

		private void Reset()
		{
			gameObject.name = "PlayerTuner";
			gameObject.layer = 8;

			m_SphereCollider = GetComponent<SphereCollider>();
		}

		private void Awake()
		{
			m_ConnectedChannels = new List<RadioChannel>();
		}

		private void OnTriggerEnter(Collider p_Collider)
		{
			var radioChannel = p_Collider.GetComponent<RadioChannel>();

			if (radioChannel)
			{
				Debug.Log("Player tuned into: " + p_Collider.gameObject.name);
				m_ConnectedChannels.Add(radioChannel);

				radioChannel.TuneIn();
			}
		}

		private void OnTriggerExit(Collider p_Collider)
		{
			var radioChannel = p_Collider.GetComponent<RadioChannel>();

			if (radioChannel)
			{
				Debug.Log("Player tuned out of: " + p_Collider.gameObject.name);
				m_ConnectedChannels.Remove(radioChannel);
			}
		}


		private void OnDrawGizmos ()
		{
			DebugExtension.DrawCapsule(transform.localPosition, transform.localPosition + Vector3.up * 0.01f, Color.green, m_SphereCollider.radius);

		}

		private void Update()
		{
			var localPos = transform.localPosition;
			var speed = 2f;

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

	}
}