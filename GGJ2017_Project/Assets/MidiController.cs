﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ComboDelegate();

public class ComboListener {
	public string[] combo;
	public ComboDelegate callback;
}

public class MidiController {

	List<string> lastNotes;
	List<ComboListener> comboListeners;

	public Transform tuner;

	// Use this for initialization
	public MidiController () {
		MidiJack.MidiMaster.knobDelegate = OnKnob;
		MidiJack.MidiMaster.noteOnDelegate = OnNoteOn;

		lastNotes = new List<string> ();
		comboListeners = new List<ComboListener> ();
	}
		

	void FlushCombo(){
		lastNotes = new List<string> ();
	}

	public void AddComboListener (string[] combo, ComboDelegate callback) {
		ComboListener listener = new ComboListener ();
		listener.combo = combo;
		listener.callback = callback;

		comboListeners.Add (listener);
	}

	void OnNoteOn(MidiJack.MidiChannel channel, int noteNumber, float velocity){
		string name = NoteToString (noteNumber);
		lastNotes.Add (name);

		//TODO check for combos;
		int ii;
		int jj;
		for (ii = comboListeners.Count - 1; ii >= 0; ii--) {
			bool isComplete = true;
			ComboListener listener = comboListeners [ii];
			for (jj = 0; jj < listener.combo.Length; jj++) {
				int k = listener.combo.Length -1 - jj;
				int p = lastNotes.Count - 1 - jj;

				if (lastNotes [p] != listener.combo [k]) {
					isComplete = false;
					break;
				}
			}
			if (isComplete) {
				listener.callback ();
			}
		}
	}

	void OnKnob(MidiJack.MidiChannel channel, int knobNumber, float knobValue){
		knobValue = Mathf.Round (knobValue * 127);
		var pos = tuner.localPosition;

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
		tuner.localPosition = pos;
	}

	string[] letters = {"c","c#","d","d#","e","f","f#","g","g#","a","a#","b"};

	string NoteToString(int note){
		note -= 48;

		string letter = letters[note%12];
		string number = Mathf.Ceil((note + 1.0f) / 12.0f).ToString();

		return letter + number;
	}

	int StringToNote(string note){
		int ii;
		string letter = note.Substring (0, note.Length - 1);
		int number = int.Parse(note.Substring (note.Length - 1));
		for (ii = 0; ii < 12; ii++) {
			if (letters [ii] == letter) {
				break;
			}
		}
		return ii + (number-1)*12 + 48;
	}
}
