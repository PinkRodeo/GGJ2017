using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zWave : MonoBehaviour {

	public int knobId = 79;

	private LineRenderer trail;
	private float speed = 2.0f;
	private float phase = 0.0f;
	// Use this for initialization
	void Start () {
		trail = gameObject.GetComponent<LineRenderer> ();

		trail.positionCount = 32;

		int ii = 0;
		for (ii = 0; ii < trail.positionCount; ii++) {
			trail.SetPosition (ii, new Vector3 (ii * (1.0f / (trail.positionCount-1)), 0, 0));
		}

		HAM.Game.midiController.AddKnobListener (79, OnKnob);
	}
	
	// Update is called once per frame
	void Update () {
		phase += Time.deltaTime * speed;

		int ii;
		for (ii = trail.positionCount -1; ii > 0; ii--) {
			float nextY = trail.GetPosition (ii - 1).y;
			Vector3 nextPos = trail.GetPosition (ii);
			nextPos.y = nextY;
			trail.SetPosition (ii, nextPos);
		}


		trail.SetPosition (0, new Vector3 (0, Mathf.Sin (phase) * 0.05f, 0));
	}

	void OnKnob(float val){
		speed = 5 + val * 20;
	}
}
