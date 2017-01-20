using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RadioWaves
{
	public class Ether : MonoBehaviour
	{
		public const string ETHERCHANNEL_TAG = "EtherChannelTag";

		private const int ETHER_RANGE = 127;

		[ReadOnly]
		public List<RadioChannel> m_RadioChannels;

		private void Reset()
		{
			transform.tag = ETHERCHANNEL_TAG;
			gameObject.layer = 8;

			gameObject.name = "Ether";
		}

		private void Awake ()
		{

		}

		// Use this for initialization
		void Start ()
		{
			var radioChannelGameObjects = GameObject.FindGameObjectsWithTag(RadioChannel.RADIOCHANNEL_TAG);

			m_RadioChannels = new List<RadioChannel>(radioChannelGameObjects.Length);

			foreach (var radioChannelObj in radioChannelGameObjects)
			{
				var radioChannel = radioChannelObj.GetComponent<RadioChannel>();
				m_RadioChannels.Add(radioChannel);
				radioChannel.m_Ether = this;

			}
		}

		// Update is called once per frame
		void Update ()
		{

		}


		public void RemoveChannel (RadioChannel p_RadioChannel)
		{
			m_RadioChannels.Remove(p_RadioChannel);

			p_RadioChannel.m_Ether = null;
		}

		private void OnDrawGizmos()
		{
			var pos = transform.position;
			var dimensions = new Vector3(ETHER_RANGE, ETHER_RANGE, ETHER_RANGE);
			var center = pos + dimensions/2f;

			var bounds = new Bounds(center, dimensions);
			DebugExtension.DrawBounds(bounds, Color.yellow);
		}
	}

}

