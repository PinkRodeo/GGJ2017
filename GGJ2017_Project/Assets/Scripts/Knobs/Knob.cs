using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HAM;

public class Knob : MonoBehaviour {

	Quaternion startRot;

	public int knobId = 74;
	public float minAngle = 0;
	public float maxAngle = -180;

	// Use this for initialization
	void Awake () {
		startRot = transform.localRotation;

		HAM.Game.midiController.AddKnobListener (knobId, OnTurn);
	}
	
	// Update is called once per frame
	void OnTurn (float val) {
		float angle = minAngle + (maxAngle - minAngle) * val;
		var newRot = startRot * Quaternion.Euler (new Vector3(0,angle,0));
		transform.localRotation = newRot;
	}
}
