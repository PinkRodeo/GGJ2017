using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public static MidiController midiController;
	public Transform tuner;

	void Start(){
		//midiController = gameObject.AddComponent<MidiController> ();
		midiController = new MidiController();
		midiController.tuner = tuner;
	
		string[] combo = {"e2","e2","e2","c2","e2","g2"};
		midiController.AddComboListener(combo, ComboTest);
	}


	void ComboTest(){
		Debug.Log("test combo entered");
	}
}
