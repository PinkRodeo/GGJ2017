using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechController : MonoBehaviour {

	GUIText guiText;

	string currentSentence = "";
	int currentIndex = 0;
	float rollingIndex = 0.0f;
	float textSpeed = 1.0f;
	string lastChar = "";
	public AudioClip[] audioClips;
	AudioSource source;

	// Use this for initialization
	void Start () {
		guiText = gameObject.GetComponent<GUIText> ();

		Say ("Lorum Ipsum Dolor Sit Amet");
	}
	
	// Update is called once per frame
	void Update () {
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
		int spaceCount = text.Split (' ').Length - 1;
		currentSentence = text;
		currentIndex = 0;
		rollingIndex = 0.0f;
		guiText.text = "";

		PlaySound ();
	}

	public void SetClips(AudioClip[] clips){
		audioClips = clips;
	}

	public void SetSpeed(float speed){
		textSpeed = speed;
	}

	public void SetSource(AudioSource s){
		source = s;
	}

	public void PlaySound(){
		//TODO
		AudioClip clip = audioClips[Mathf.FloorToInt(Random.value * audioClips.Length)];
		if (!source) {
			return;
		}
		source.clip = clip;
		source.Play ();
	}
}
