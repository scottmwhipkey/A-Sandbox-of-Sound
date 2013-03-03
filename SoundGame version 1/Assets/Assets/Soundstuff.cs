using UnityEngine;
using System.Collections;

public class Soundstuff : MonoBehaviour {
	
	//AudioClip sample;
	bool soundOn = false;
	AudioSource music_source;
	
	// Use this for initialization
	void Start () 
	{
		GameObject ball = GameObject.Find("Sphere");
		
		//Add audio component to ball object and set music_source equal to it
		music_source = ball.AddComponent<AudioSource>();
		
		
		//Load in Audio file into audio clip of audio component
		music_source.clip = Resources.Load("Audio/01. One Thing Leads To Another") as AudioClip;
		
		//ball.audio.Play();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameObject ball = GameObject.Find("Sphere");
		
		//Press U to switch songs
		if (Input.GetKeyDown(KeyCode.U))
		{
			if (soundOn == false)
			{
				//Load in Audio file into audio clip of audio component
				music_source.clip = Resources.Load("Audio/Earth, Wind & Fire - Boogie Wonderland") as AudioClip;	
				soundOn = true;
			}
			else
			{
				//Load in Audio file into audio clip of audio component
				music_source.clip = Resources.Load("Audio/01. One Thing Leads To Another") as AudioClip;
				soundOn = false;
			}
		}
		
		// when A is pressed move in positvie z direction
		if(Input.GetKeyDown(KeyCode.A))
		{
			ball.transform.Translate(0, 0, 1);
			
		}
		
		//when S is pressed move in negative z direction
		if (Input.GetKeyDown(KeyCode.S))
		{
			ball.transform.Translate (0, 0, -1);	
		}
		
		//Press W to pause music
		if (Input.GetKeyDown (KeyCode.W))
		{
			music_source.Pause();
		}
		
		//Press Q to start music
		if (Input.GetKeyDown(KeyCode.Q))
		{
			//soundOn = true;
			music_source.Play();
		}
		
		//B increases volume
		if (Input.GetKeyDown(KeyCode.B))
		{
			if (music_source.volume < 1.0f)
			{
				music_source.volume += 0.02f;
			}
		}
		
		//V decreases volume
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (music_source.volume > 0.0f)
			{
				music_source.volume -= 0.02f;
			}
		}
		
		//X increases pitch
		if (Input.GetKeyDown(KeyCode.X))
		{
			if (music_source.pitch < 3.0f)
			{
				music_source.pitch += 0.02f;
			}
		}
		
		//C decreases pitch
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (music_source.pitch > -3.0f)
			{
				music_source.pitch -= 0.02f;
			}
		}
		
		//N increases spread
		if (Input.GetKeyDown(KeyCode.N))
		{
			if (music_source.spread < 360.0f)
			{
				music_source.spread += 1.0f;
			}
		}
		
		//M decreases spread
		if (Input.GetKeyDown(KeyCode.M))
		{
			if (music_source.spread > 0.0f)
			{
				music_source.spread -= 1.0f;
			}
		}
		
		//R toggles rolloff mode
		if (Input.GetKeyDown(KeyCode.R))
		{
			music_source.rolloffMode = (AudioRolloffMode) Random.Range(0, 2);		
		}
		
	
	
	}
	
	
	//Unity function that displays GUI widgets
	void OnGUI ()
	{
		//Music volume
		float volume = music_source.volume;
		//Music pitch
		float pitch = music_source.pitch;
		//Current point in time in song
		float time = music_source.time;
		//Length of audio clip associated with this audio component
		float length = music_source.clip.length;
		//Song name
		string songName = music_source.clip.ToString();
		//Mono = 1, Stereo = 2 (Read Only)
		int channel = music_source.clip.channels;
		
		string s_channel = "";
		
		if (channel == 1)
		{
			s_channel = "Mono";
		}
		else
		{
			s_channel = "Stereo";	
		}
		
		//Frequency in Hertz (Read Only)
		int frequency = music_source.clip.frequency;
		//Is it currently playing?
		bool isPlaying = music_source.isPlaying;
		//Pan level (controls whether audio is played as 2D or 3D)
		float panLevel = music_source.panLevel;
		//Spread angle of sound
		float spread = music_source.spread;
		//When camera is within this distance the volume will cease to increase
		float minDistance = music_source.minDistance;
		//Distance sound grows silent
		float maxDistance = music_source.maxDistance;
		//Audio roll off mode of sound
		int rm = (int) music_source.rolloffMode;
		
		string s_rm = "";
		
		switch (rm)
		{
			case 0:
				s_rm = "Logarithmic";
				break;
			case 1:
				s_rm = "Linear";
				break;
			case 2:
				s_rm = "Custom";
				break;
		}
		
		
		GUILayout.Label("Volume: " + volume);
		GUILayout.Label("Pitch: " + pitch);
		GUILayout.Label("Current Time: " + time);
		GUILayout.Label("Length: " + length);
		GUILayout.Label("Name: " + songName);
		GUILayout.Label("Channel: " + s_channel);
		GUILayout.Label("Frequency (Hz): " + frequency);
		GUILayout.Label("Is Playing?: " + isPlaying);
		GUILayout.Label("Pan Level: " + panLevel);
		GUILayout.Label("Spread Angle (Degrees): " + spread);
		GUILayout.Label("Min Distance: " + minDistance);
		GUILayout.Label("Max Distance: " + maxDistance);
		GUILayout.Label("Roll Off: " + s_rm);
		
		
	}
}
