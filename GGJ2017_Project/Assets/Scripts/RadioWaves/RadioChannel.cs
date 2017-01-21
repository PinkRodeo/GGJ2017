using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		// Update is called once per frame
		void Update()
		{

		}

		void OnDrawGizmos()
		{
			DebugExtension.DrawCapsule(transform.localPosition, transform.localPosition + Vector3.up * 0.01f, Color.blue, m_Range);

		}
	}
}