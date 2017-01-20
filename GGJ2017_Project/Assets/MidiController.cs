using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiController : MonoBehaviour {

	public Transform go;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		MidiJack.MidiMaster.knobDelegate = OnKnob;
		MidiJack.MidiMaster.noteOnDelegate = OnNoteOn;
	}

	void OnNoteOn(MidiJack.MidiChannel channel, int noteNumber, float velocity){
		Debug.Log (noteNumber.ToString() +' '+ velocity.ToString());
	}

	void OnKnob(MidiJack.MidiChannel channel, int knobNumber, float knobValue){
		knobValue = Mathf.Round (knobValue * 127);
		Debug.Log (knobNumber.ToString() +' '+ knobValue.ToString());
		var pos = go.localPosition;

		switch (knobNumber) {
		case 74:
			pos.x = knobValue;
			break;
		case 71:
			pos.y = knobValue;
			break;
		case 79:
			pos.z = knobValue;
			break;
		}
		go.localPosition = pos;
	}
}
