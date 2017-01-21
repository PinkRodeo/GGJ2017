using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ComboDelegate();
public delegate void KnobDelegate(float value);

public class ComboListener {
	public string[] combo;
	public ComboDelegate callback;
}

public class KnobListener {
	public int knobNumber;
	public KnobDelegate callback;
}

public class MidiController {

	List<string> lastNotes;
	List<ComboListener> comboListeners;
	List<KnobListener> knobListeners;

	// Use this for initialization
	public MidiController () {
		MidiJack.MidiMaster.knobDelegate = OnKnob;
		MidiJack.MidiMaster.noteOnDelegate = OnNoteOn;

		lastNotes = new List<string> ();
		comboListeners = new List<ComboListener> ();
		knobListeners = new List<KnobListener> ();
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

	public void ClearComboListeners (){
		comboListeners.Clear ();
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

				Debug.Log (lastNotes [p] + " - " + listener.combo [k]);
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
}
