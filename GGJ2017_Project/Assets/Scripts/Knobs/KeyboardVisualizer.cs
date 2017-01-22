using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HAM
{

	public class KeyboardVisualizer : MonoBehaviour
	{

		public string m_Note;

		private Transform m_Transform;

		public bool m_IsAnimatingDown;

		public void Start()
		{
			m_Transform = transform;
			Game.midiController.AddNoteListener(m_Note, OnNote);
		}

		public void OnNote(bool p_PressedDown)
		{
			if (p_PressedDown == m_IsAnimatingDown)
				return;

			if (p_PressedDown)
			{
				m_Transform.DOLocalRotate(new Vector3(6.55f, 0f, 0f), 0.1f, RotateMode.Fast);

			}
			else
			{
				m_Transform.DOLocalRotate(new Vector3(-2.68f, 0f, 0f), 0.1f, RotateMode.Fast);
			}

			m_IsAnimatingDown = p_PressedDown;

		}
	

	}

}
