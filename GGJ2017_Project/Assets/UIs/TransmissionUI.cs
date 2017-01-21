using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace HAM
{
	public class TransmissionUI : MonoBehaviour
	{
		
		public string TextContent
		{
			set
			{
				if (value == m_TextContent)
					return;

				m_TextContent = value;

				m_TextPanel.text = m_TextContent;

			}
		}

		[ReadOnly, SerializeField]
		private string m_TextContent;

		[SerializeField]
		private Text m_TextPanel;

		[SerializeField]
		private RectTransform m_MainPanel;

		private bool m_bVisible = false;

		private Tweener m_CurrentVisibilityTween;

		public void SetVisible(bool p_IsVisible)
		{
			if (m_bVisible == p_IsVisible)
				return;

			if (m_CurrentVisibilityTween != null)
			{
				if (m_CurrentVisibilityTween.IsActive())
					m_CurrentVisibilityTween.Kill();
				m_CurrentVisibilityTween = null;
			}

			if (p_IsVisible)
			{
				m_CurrentVisibilityTween = m_MainPanel.DOScaleY(1f, 0.3f).SetEase(Ease.OutElastic);
			}
			else
			{
				m_CurrentVisibilityTween = m_MainPanel.DOScaleY(0f, 0.3f).SetEase(Ease.InBack);

			}
			m_bVisible = p_IsVisible;


		}

		protected void Awake()
		{
			m_MainPanel.localScale = new Vector3(1f,0,1f);

			m_TextContent = "bla";
			TextContent = "";
		}

		/*
		protected void Update()
		{
			if (Input.GetKeyUp(KeyCode.Y))
			{
				SetVisible(!m_bVisible);
			}
		}
		*/

	}


}
