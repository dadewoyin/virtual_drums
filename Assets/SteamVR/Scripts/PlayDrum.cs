using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDrum : MonoBehaviour {

	[SerializeField]
	private GameObject videoPlayer;
    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject; // object that trigger is colliding with
    private AudioSource soundAudio;
	private bool playingSound;
    private GameObject objectInHand; // Game object user is currently grabbing
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); } // get access to controller input tracking
    }

    public Light light1, light2, light3, light4;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

	//length is how long the vibration should go for
	//strength is vibration strength from 0-1
	private void LongVibration(float length, float strength) {
		for(float i = 0; i < length; i += Time.deltaTime) {
			Controller.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
		}
	}

    private void PlayAudio(Collider col) // play audio unless there's no rigidbody, audio, or the user is already hitting something with that hand
    {

        if (collidingObject)
        {
            return;
        }

        collidingObject = col.gameObject;
		Debug.Log ("bitch" + collidingObject);
        soundAudio = collidingObject.GetComponent<AudioSource>();
        Debug.Log("Sound audio: " + soundAudio);

        if (soundAudio)
        {
            if (collidingObject.tag == "DrumSound")
			{
				// playingSound = true;
				Debug.Log("Yah boi");
				Debug.Log(Controller.velocity);
				//soundAudio.volume = Controller.velocity.y * -1.2f;
				//Debug.Log (soundAudio.volume);
				LongVibration(2000, 1);
                soundAudio.Play();
            }

            if (collidingObject.tag == "LoopSound")
            {
                if (soundAudio.isPlaying)
                {
                    soundAudio.Stop();
                    Debug.Log("Stopping Looped Sound");
                }
                else
                {
                    soundAudio.Play();
                    Debug.Log("Playing Looped Sound");
                }
            }

			playingSound = false;
            ChangeLightColor(light1, light2, light3, light4);

        }
    }

    public void ChangeLightColor(Light light1, Light light2, Light light3, Light light4)
    {
        UnityEngine.Color newColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        light1.color = newColor;
        light2.color = newColor;
        light3.color = newColor;
        light4.color = newColor;
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayAudio(other);
		Debug.Log ("Collider name: " + other.gameObject);
        Debug.Log ("Controller velocity: " + Controller.velocity);
    }

    public void OnTriggerStay(Collider other)
    {
        //Debug.Log("Hit object: " + collidingObject);
		// LongVibration(100, 0.8f);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }
		

    // Update is called once per frame
    void Update () {
		// LongVibration (800, 800);

		if (Controller.GetHairTriggerDown()) // grab object when player squeezes trigger and there's a potential target
        {

			HighQualityPlayback tutorialControls = videoPlayer.GetComponent<HighQualityPlayback> ();
			VideoController youtubeVideo = videoPlayer.GetComponent<VideoController> ();

			if (!youtubeVideo.sourceVideo.isPlaying) {
				tutorialControls.PlayYoutubeVideo (tutorialControls.videoId);	
			} else if (youtubeVideo.sourceVideo.isPlaying) {
				youtubeVideo.Pause();
			}


        }

        if (Controller.GetHairTriggerUp()) // when player releases trigger
        {

        }
	}
}
