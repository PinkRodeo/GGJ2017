using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAM
{
	public class Game : MonoBehaviour
	{

		public static MidiController midiController;
		public static SpeechController speechController;

		public GUIText guiText;

		void Start ()
		{
			//midiController = gameObject.AddComponent<MidiController> ();
			midiController = new MidiController();

			speechController = new SpeechController ();
			speechController.guiText = guiText;


			//TESTS
			string[] combo = { "e2", "e2", "e2", "c2", "e2", "g2" };
			midiController.AddComboListener(combo, ComboTest);

			speechController.Say("hello world");
		}

		void Update(){
			speechController.Update ();
		}


		void ComboTest ()
		{
			Debug.Log("test combo entered");
		}
	}

}

