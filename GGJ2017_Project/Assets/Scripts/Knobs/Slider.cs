using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HAM;

public class Slider : MonoBehaviour {

	Vector3 startPos;

	public int knobId = 74;
	public float min = 0;
	public float max = -180;
	public Vector3 direction = new Vector3(1,0,0);

	// Use this for initialization
	void Awake () {
		HAM.Game.midiController.AddKnobListener (knobId, OnTurn);

		startPos = transform.localPosition;
	}

	// Update is called once per frame
	void OnTurn (float val) {
		float dist = min + (max - min) * val;
		Vector3 diff = direction * dist;
		Vector3 newPos = startPos + diff;
		transform.localPosition = newPos;
	}
}
