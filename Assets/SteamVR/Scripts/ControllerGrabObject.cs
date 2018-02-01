using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject; // object that trigger is colliding with
    private AudioSource soundAudio;
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

    private void PlayAudio(Collider col) // play audio unless there's no rigidbody, audio, or the user is already hitting something with that hand
    {

        if (collidingObject)
        {
            return;
        }

        collidingObject = col.gameObject;
        soundAudio = collidingObject.GetComponent<AudioSource>();
        Debug.Log("Sound audio: " + soundAudio);

        if (soundAudio)
        {
            if (collidingObject.tag == "DrumSound")
            {
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
        Debug.Log("Controller velocity: " + Controller.velocity);
        // Debug.Log("Controller angularvelocity: " + Controller.angularVelocity);
        Controller.TriggerHapticPulse(7000);
        
    }

    public void OnTriggerStay(Collider other)
    {
        //Debug.Log("Hit object: " + collidingObject);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    public void GrabObject()
    {
        objectInHand = collidingObject; // put game object in player's hand & remove it from colliding object
        collidingObject = null;

        var joint = AddFixedJoint(); // connects controller to the object using the AddFixedJoint()
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint() // make new fixed joint, add to controller, & then set it up so it doesn't 
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>()) // make sure there's a fixed joint attached to controller
        {
            GetComponent<FixedJoint>().connectedBody = null; // if so, remove joint & destroy the joint
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity; // add speed and rotation of controller when the player releases the object, so the result is a realistic arc
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }

        objectInHand = null; // remove reference to formerly attached object
    }

    // Update is called once per frame
    void Update () {
		if (Controller.GetHairTriggerDown()) // grab object when player squeezes trigger and there's a potential target
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        if (Controller.GetHairTriggerUp()) // release object when player releases trigger and there's an object in hand
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
	}
}
