﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ComboDelegate();
public delegate void NoteDelegate(bool pressed);
public delegate void KnobDelegate(float value);

public class ComboListener {
	public string[] combo;
	public ComboDelegate callback;
}

public class NoteListener {
	public int note;
	public NoteDelegate callback;
}

public class KnobListener {
	public int knobNumber;
	public KnobDelegate callback;
}

public class MidiController {

	List<string> lastNotes;
	List<ComboListener> comboListeners;
	List<NoteListener> noteListeners;
	List<KnobListener> knobListeners;


	List<int> disabledKnobs;


	float knobX = 0.0f;
	float knobY = 0.0f;
	float knobZ = 0.0f;

	// Use this for initialization
	public MidiController () {
		MidiJack.MidiMaster.knobDelegate = OnKnob;
		MidiJack.MidiMaster.noteOnDelegate = OnNoteOn;
		MidiJack.MidiMaster.noteOffDelegate = OnNoteOff;

		lastNotes = new List<string> ();
		comboListeners = new List<ComboListener> ();
		noteListeners = new List<NoteListener> ();
		knobListeners = new List<KnobListener> ();

		disabledKnobs = new List<int> ();
	}

	public void Update(){
		//for debug porpoises
		CheckKeyboardKey(KeyCode.Z, 48);
		CheckKeyboardKey(KeyCode.X, 49);
		CheckKeyboardKey(KeyCode.C, 51);
		CheckKeyboardKey(KeyCode.V, 54);
		CheckKeyboardKey(KeyCode.B, 56);
		CheckKeyboardKey(KeyCode.N, 58);
		
		if (Input.GetKey(KeyCode.Q)){
			knobX += 1f / 96f;
			knobX = Mathf.Min (1, knobX);
			OnKnob (MidiJack.MidiChannel.All, 74, knobX);
		}
		if (Input.GetKey(KeyCode.A)){
			knobX -= 1f / 96f;
			knobX = Mathf.Max (0, knobX);
			OnKnob (MidiJack.MidiChannel.All, 74, knobX);
		}

		if (Input.GetKey(KeyCode.W)){
			knobY += 1f / 96f;
			knobY = Mathf.Min (1, knobY);
			OnKnob (MidiJack.MidiChannel.All, 71, knobY);
		}
		if (Input.GetKey(KeyCode.S)){
			knobY -= 1f / 96f;
			knobY = Mathf.Max (0, knobY);
			OnKnob (MidiJack.MidiChannel.All, 71, knobY);
		}

		if (Input.GetKey(KeyCode.E)){
			knobZ += 1f / 96f;
			knobZ = Mathf.Min (1, knobZ);
			OnKnob (MidiJack.MidiChannel.All, 79, knobZ);
		}
		if (Input.GetKey(KeyCode.D)){
			knobZ -= 1f / 96f;
			knobZ = Mathf.Max (0, knobZ);
			OnKnob (MidiJack.MidiChannel.All, 79, knobZ);
		}
	}

	private void CheckKeyboardKey(KeyCode p_KeyCode, int noteIndex)
	{
		if (Input.GetKeyDown(p_KeyCode))
		{
			OnNoteOn(MidiJack.MidiChannel.All, noteIndex, 1f);
		}
		if (Input.GetKeyUp(p_KeyCode))
		{
			OnNoteOff(MidiJack.MidiChannel.All, noteIndex);
		}
	}

	void FlushCombo(){
		lastNotes.Clear ();
	}

	public void AddComboListener (string[] combo, ComboDelegate callback) {
		ComboListener listener = new ComboListener ();
		listener.combo = combo;
		listener.callback = callback;

		comboListeners.Add (listener);
	}

	public void AddNoteListener (string note, NoteDelegate callback){
		NoteListener listener = new NoteListener ();
		listener.note = StringToNote (note);
		listener.callback = callback;

		noteListeners.Add (listener);
	}

	public void ClearComboListeners (){
		comboListeners.Clear ();
	}

	void OnNoteOn(MidiJack.MidiChannel channel, int noteNumber, float velocity){
		string name = NoteToString (noteNumber);
		lastNotes.Add (name);

		int ii;
		int jj;
		for (ii = comboListeners.Count - 1; ii >= 0; ii--) {
			bool isComplete = true;
			ComboListener listener = comboListeners [ii];
			for (jj = 0; jj < listener.combo.Length; jj++) {
				int k = listener.combo.Length -1 - jj;
				int p = lastNotes.Count - 1 - jj;

				if (p < 0) {
					isComplete = false;
					break;
				}

				if (lastNotes [p] != listener.combo [k]) {
					isComplete = false;
					break;
				}
			}

			if (isComplete) 
			{
				HAM.Game.speechController.dictionaryUI.OnComboPlayed(listener.combo);

				listener.callback ();
				comboListeners.Remove(listener);
			}
		}


		for (ii = 0; ii < noteListeners.Count; ii++) {
			if (noteListeners [ii].note == noteNumber) {
				noteListeners [ii].callback (true);
			}
		}
	}

	void OnNoteOff(MidiJack.MidiChannel channel, int noteNumber){
		int ii;
		for (ii = 0; ii < noteListeners.Count; ii++) {
			if (noteListeners [ii].note == noteNumber) {
				noteListeners [ii].callback (false);
			}
		}
	}

	public void AddKnobListener (int knobNumber, KnobDelegate callback){
		KnobListener listener = new KnobListener ();

		listener.knobNumber = knobNumber;
		listener.callback = callback;

		knobListeners.Add (listener);

		float currentVal = MidiJack.MidiMaster.GetKnob (knobNumber, 0.0f);
		callback (currentVal);
	}

	void OnKnob(MidiJack.MidiChannel channel, int knobNumber, float knobValue){
		int ii;

		if (knobNumber == 1) {
			knobNumber = 74;
		}

		if (knobNumber == 81) {
			knobNumber = 79;
		}

		if (disabledKnobs.Contains (knobNumber)) {
			return;
		}

		for (ii = 0; ii < knobListeners.Count; ii++) {
			if (knobListeners [ii].knobNumber == knobNumber) {
				knobListeners [ii].callback (knobValue);
			}
		}

		return;
	}

	private static readonly string[] NoteLetters = {"c","c#","d","d#","e","f","f#","g","g#","a","a#","b"};

	public static string NoteToString(int note){
		note -= 48;

		string letter = NoteLetters[note%12];
		string number = Mathf.Ceil((note + 1.0f) / 12.0f).ToString();

		return letter + number;
	}

	public static int StringToNote(string note){
		int ii;
		string letter = note.Substring (0, note.Length - 1).ToLower();
		int number = int.Parse(note.Substring (note.Length - 1));
		for (ii = 0; ii < 12; ii++) {
			if (NoteLetters [ii] == letter) {
				break;
			}
		}
		return ii + (number-1)*12 + 48;
	}

	public void DisableKnob(int number){
		if (!disabledKnobs.Contains (number)) {
			disabledKnobs.Add (number);
		}
	}

	public void EnableKnob(int number){
		if (disabledKnobs.Contains (number)) {
			disabledKnobs.Remove (number);
		}
	}
}
