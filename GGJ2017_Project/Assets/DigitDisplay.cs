using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitDisplay : MonoBehaviour {

	public SpriteRenderer[] digits;

	public int knobId = 79;
	public int min = 0;
	public int max = 100;

	Sprite[] sprites;

	// Use this for initialization
	void Start () {
		Object[] numbers = Resources.LoadAll ("t_numbers");

		sprites = new Sprite[numbers.Length -1];

		Debug.Log (sprites.Length);
		int ii;
		for (ii = 1; ii < numbers.Length; ii++) {
			sprites [ii-1] = numbers [ii] as Sprite;	
			Debug.Log (numbers[ii].name);
		}

		HAM.Game.midiController.AddKnobListener (knobId, OnKnob);
	}
	
	// Update is called once per frame
	void OnKnob (float val) {
		int value = Mathf.FloorToInt(min + (max-min)*val);

		digits[2].sprite = sprites[value%10]; 
		digits[1].sprite = sprites[Mathf.FloorToInt(value%100/10)];
		digits[0].sprite = sprites[Mathf.FloorToInt(value/100)];
	}
}
