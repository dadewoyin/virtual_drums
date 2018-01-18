using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerInputTest : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj; // computer 
    private SteamVR_Controller.Device Controller // get controller input tracking
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

    // Update is called once per frame
    void Update () {

        if (Controller.GetAxis() != Vector2.zero) // Get the position of the finger when it’s on the touchpad and write it to the Console.
        {
            Debug.Log(gameObject.name + Controller.GetAxis());
        }

        if (Controller.GetHairTriggerDown()) // When you squeeze the hair trigger, this line writes to the Console. The hair trigger has special methods to check whether it is pressed or not: GetHairTrigger(), GetHairTriggerDown() and GetHairTriggerUp()
        {
            Debug.Log(gameObject.name + " Trigger Press");
        }

        if (Controller.GetHairTriggerUp()) // If you release the hair trigger, this if statement writes to the Console.
        {
            Debug.Log(gameObject.name + " Trigger Release");
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) // If you press a grip button, this section writes to the Console. Using the GetPressDown() method is the standard method to check if a button was pressed.
        {
            Debug.Log(gameObject.name + " Grip Press");
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip)) // When you release one of the grip buttons, this writes that action to the Console. Using the GetPressUp() method is the standard way to check if a button was released.
        {
            Debug.Log(gameObject.name + " Grip Release");
        }

    }
}
