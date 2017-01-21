using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace HAM
{
	public class DictionaryUI : MonoBehaviour
	{
		
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
				m_CurrentVisibilityTween = m_MainPanel.DOScaleX(1f, 0.3f).SetEase(Ease.OutBack);
			}
			else
			{
				m_CurrentVisibilityTween = m_MainPanel.DOScaleX(0f, 0.1f).SetEase(Ease.InBack);

			}

			m_bVisible = p_IsVisible;
		}

		protected void Awake()
		{
			m_MainPanel.localScale = new Vector3(1f,1f,1f);
		}

		
		protected void Update()
		{
			if (Input.GetKeyDown(KeyCode.Y))
			{
				SetVisible(true);
			}

			if (Input.GetKeyUp(KeyCode.Y))
			{
				SetVisible(false);
			}
		}
		

	}


}
