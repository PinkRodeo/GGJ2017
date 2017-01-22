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
		public Dictionary<Vector3, GameObject> m_PositionPanels = new Dictionary<Vector3, GameObject>();
		public Dictionary<string[], GameObject> m_DictionaryPanelsByCombo = new Dictionary<string[], GameObject>();

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

			//AddToDictionary (new Vector3 (121, 76, 24));
		}

		protected void Start()
		{
			//AddToDictionary(Wundee.Game.instance.definitions.comboDefinitions["COMBO_SAY_HELLO"]);
			HAM.Game.midiController.AddNoteListener ("c1", (bool pressed) => {
				SetVisible(pressed);	
			});
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
					string keyName = p_ComboDefinition.combo [i];
					keyName = keyName.Substring (0, keyName.Length - 1);
					if (i != 0)
						panelContents += ", " + keyName;
					else
					{
						panelContents += keyName;
					}
				}

				panelContents += " = " + p_ComboDefinition.meaning;

				textComponent.text = panelContents;
				m_DictionaryPanels.Add(p_ComboDefinition, newDictionaryPanel);
				m_DictionaryPanelsByCombo.Add(p_ComboDefinition.combo, newDictionaryPanel);

				HAM.Game.PlaySuccessSound();
				SetVisible(true);
			}
		}

		public void AddToDictionary(Vector3 p_Position)
		{
			if (m_PositionPanels.ContainsKey(p_Position) == false)
			{
				var newDictionaryPanel = GameObject.Instantiate(m_DictionaryPanelPrefab, m_DictionaryContentTransform) as GameObject;
				newDictionaryPanel.transform.localScale = Vector3.one;

				var child = newDictionaryPanel.transform.GetChild(0);
				var textComponent = child.GetComponent<Text>();

				var channelPos = HAM.Game.PosToChannel (p_Position);

				var panelContents = "";

				if (p_Position.x >= 0) {
					panelContents += "<color='red'>AM: " + channelPos.x + ". </color>";
				}
				if (p_Position.y >= 0) {
					panelContents += "<color='blue'>MIX: " + channelPos.y + ". </color>";
				}
				if (p_Position.z >= 0) {
					panelContents += "<color='orange'>WAVE: " + channelPos.z + ". </color>";
				}

				textComponent.text = panelContents;
				m_PositionPanels.Add(p_Position, newDictionaryPanel);

				HAM.Game.PlaySuccessSound();
				SetVisible(true);
			}
		}


		public void RemoveFromDictionary(ComboDefinition p_ComboDefinition)
		{
			if (m_DictionaryPanels.ContainsKey(p_ComboDefinition))
			{
				m_DictionaryPanels.Remove(p_ComboDefinition);
			}
		}

		public void OnComboPlayed(string[] p_PlayedCombo)
		{
			if (m_DictionaryPanelsByCombo.ContainsKey(p_PlayedCombo))
			{
				var uiPanel = m_DictionaryPanelsByCombo[p_PlayedCombo].GetComponent<Image>();

				if (uiPanel == null)
					return;
				var oldColor = uiPanel.color;
				;

				uiPanel.DOColor(Color.yellow, 0.3f).OnComplete(() =>
				{
					uiPanel.DOColor(oldColor, 0.5f);
				});
			}
		}
		
		protected void Update()
		{
		/*
			if (Input.GetKeyDown(KeyCode.Z))
			{
				SetVisible(true);
			}

			if (Input.GetKeyUp(KeyCode.Z))
			{
				SetVisible(false);
			}
			*/
		}
		

	}


}
