﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechController {

	public GUIText guiText;

	string currentSentence = "";
	int currentIndex = 0;
	float rollingIndex = 0.0f;
	float textSpeed = 1.0f;
	string lastChar = "";
	public AudioClip[] audioClips = {};
	AudioSource source;

	// Update is called once per frame
	public void Update () {
		bool rewrite = false;
		rollingIndex += Time.deltaTime * 20f * textSpeed;

		while (Mathf.FloorToInt (rollingIndex) > currentIndex && currentIndex < currentSentence.Length) {
			string c = currentSentence.Substring (currentIndex, 1);

			if (c == " " && lastChar != " ") {
				PlaySound();
			}
			lastChar = c;

			currentIndex++;
			rewrite = true;
		}

		if (rewrite){
			guiText.text = currentSentence.Substring (0, currentIndex);
		}
	}

	public void Say(string text){
		Clear ();
		currentSentence = text;	
		PlaySound ();
	}

	public void Clear(){
		currentSentence = "";
		currentIndex = 0;
		rollingIndex = 0.0f;
		guiText.text = "";
	}

	public void SetClips(AudioClip[] clips){
		audioClips = clips;
		Debug.Log (clips.Length);
	}

	public void SetSpeed(float speed){
		textSpeed = speed;
	}

	public void SetSource(AudioSource s){
		source = s;
	}

	public void PlaySound(){
		if (audioClips.Length < 1 || !source){
			return;
		}

		AudioClip clip = audioClips[Mathf.FloorToInt(Random.value * audioClips.Length)];
		source.clip = clip;
		source.Play ();
	}
}
