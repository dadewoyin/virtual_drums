using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickDrumAudio : MonoBehaviour {

    public GameObject kickDrum;
	private IEnumerator coroutine;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

	void Start() 
	{
		// Start function HitKickDrum as a coroutine.
		// coroutine = HitKickDrum(kickDrum);
		// StartCoroutine (coroutine);
	}

	private void HitKickDrum(GameObject kick)
	{
		var soundAudio = kick.GetComponent<AudioSource>();
		if (soundAudio.isPlaying)
		{
			Debug.Log("Already playing");
		} else
		{
			soundAudio.Play();
			Debug.Log("Sound played");
		}
	}

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Tracker Position: " + Controller.transform.pos.y);
        // HitKickDrum(KickDrum);

		if (Controller.velocity.y < -0.6) {
			Debug.Log("Controller velocity: " + Controller.velocity);
			HitKickDrum (kickDrum);
			Debug.Log (kickDrum);
			// coroutine = HitKickDrum(kickDrum);
			// StartCoroutine (coroutine);
		}
	}
}