﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HAM
{
	public class Game : MonoBehaviour
	{
		public AudioSource staticChannel;
		public RadioWaves.PlayerTuner player;

		public static MidiController midiController;
		public static SpeechController speechController;

		private static AudioSource sStaticChannel;
		private static RadioWaves.PlayerTuner sPlayer;
		
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
			//string[] combo = { "c#1", "d#1", "a#1"};
			//midiController.AddComboListener(combo, ComboTest);

			sStaticChannel = staticChannel;
			sPlayer = player;
		}

		void Update(){
			speechController.Update ();
			midiController.Update ();
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

		public static void UnlockAxis(string axis){
			sPlayer.EnableAxis (axis);
		}
	}

}

