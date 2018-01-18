using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject; // object that trigger is colliding with
    private GameObject objectInHand; // Game object user is currently grabbing
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); } // get access to controller input tracking
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void SetCollidingObject(Collider col) // set collidingObject unless there's no rigidbody or the user is already holding something
    {

        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
        Debug.Log(collidingObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
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
