using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using RadioWaves;

namespace HAM
{
	public class DictionaryUI : MonoBehaviour
	{
		
		[SerializeField]
		private RectTransform m_MainPanel;

		private bool m_bVisible = false;

		private Tweener m_CurrentVisibilityTween;

		[SerializeField]
		private GameObject m_DictionaryPanelPrefab;

		[SerializeField]
		private RectTransform m_DictionaryContentTransform;

		public Dictionary<ComboDefinition, GameObject> m_DictionaryPanels = new Dictionary<ComboDefinition, GameObject>();

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
			m_MainPanel.localScale = new Vector3(0f,1f,1f);

		}

		protected void Start()
		{
			//AddToDictionary(Wundee.Game.instance.definitions.comboDefinitions["COMBO_SAY_HELLO"]);

		}

		public void AddToDictionary(ComboDefinition p_ComboDefinition)
		{
			if (m_DictionaryPanels.ContainsKey(p_ComboDefinition) == false)
			{
				var newDictionaryPanel = GameObject.Instantiate(m_DictionaryPanelPrefab, m_DictionaryContentTransform) as GameObject;
				newDictionaryPanel.transform.localScale = Vector3.one;

				var child = newDictionaryPanel.transform.GetChild(0);
				var textComponent = child.GetComponent<Text>();

				var panelContents = "";

				for (int i = 0; i < p_ComboDefinition.combo.Length; i++)
				{
					if (i != 0)
						panelContents += ", " + p_ComboDefinition.combo[i];
					else
					{
						panelContents += p_ComboDefinition.combo[i];
					}
				}

				panelContents += " = " + p_ComboDefinition.meaning;

				textComponent.text = panelContents;
				Debug.Log(panelContents);

				m_DictionaryPanels.Add(p_ComboDefinition, newDictionaryPanel);
			}
		}

		public void RemoveFromDictionary(ComboDefinition p_ComboDefinition)
		{
			if (m_DictionaryPanels.ContainsKey(p_ComboDefinition))
			{
				m_DictionaryPanels.Remove(p_ComboDefinition);
			}
		}
		
		protected void Update()
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				SetVisible(true);
			}

			if (Input.GetKeyUp(KeyCode.Z))
			{
				SetVisible(false);
			}
		}
		

	}


}
