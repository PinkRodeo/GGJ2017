using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HAM
{
	public class Game : MonoBehaviour
	{
		public AudioSource staticChannel;

		public static MidiController midiController;
		public static SpeechController speechController;
		private static AudioSource sStaticChannel;
		
		public TransmissionUI transmissionUI;
		public DictionaryUI dictionaryUI;


		void Awake ()
		{
			//midiController = gameObject.AddComponent<MidiController> ();
			midiController = new MidiController();

			speechController = new SpeechController ();
			speechController.transmissionUI = transmissionUI;
			speechController.dictionaryUI = dictionaryUI;


			//TESTS
			//string[] combo = { "e2", "e2", "e2", "c2", "e2", "g2" };
			//midiController.AddComboListener(combo, ComboTest);

			//speechController.Say("hello world", SpeechTest);

			sStaticChannel = staticChannel;
		}

		void Update(){
			speechController.Update ();
		}


		void ComboTest ()
		{
			Debug.Log("test combo entered");
		}

		void SpeechTest ()
		{
			Debug.Log("test speech completed");
		}

		public static void SetStaticVolume (float volume){
			sStaticChannel.volume = volume;
		}
	}

}

