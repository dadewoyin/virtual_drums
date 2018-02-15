using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

    // Controller / laser variables
    private SteamVR_TrackedObject trackedObj;
    public GameObject laserPrefab; // reference to laser's prefab
    private GameObject laser; // stores a reference to an instance of the laser
    private Transform laserTransform; // the transform component (stored for easy use)
    private Vector3 hitPoint; // where laser hits

    // Transport variables
    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab; // teleport reticle prefab reference
    private GameObject reticle; // reference to an instance of the reticle
    private Transform teleportReticleTransform; // reference to teleport reticle transform
    public Transform headTransform; // player's head (camera)
    public Vector3 teleportReticleOffset; // reticle offset from floor (so there's no "Z-fighting" with other surfaces)
    public LayerMask teleportMask; // a layer mask to filter the areas on which teleports are allowed
    private bool shouldTeleport; // true when a valid teleport location is found

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true); // show laser
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f); // position laser between controller and point where raycast hits. Using lerp because you can give it two positions and the percent it should travel. If you pass it 0.5f (50%), it returns the precise middle point.
        laserTransform.LookAt(hitPoint); // point laser where raycast hits
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,hit.distance); // scale laser so it fits between two positions
        // Debug.Log("headTransform.position: " + headTransform.position);
        // Debug.Log("cameraRigTransform's position: " + cameraRigTransform.position);
    }

    private void Teleport()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0; // reset y to 0, difference doesn't consider verticle position
        cameraRigTransform.position = hitPoint + difference;
        Debug.Log("Difference: " + difference);
    }

    // Use this for initialization
    void Start () {
        laser = Instantiate(laserPrefab); // spawn a new laser and save reference to laser variable
        laserTransform = laser.transform; // store laser's transform component

        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) // if touchpad is held down...
        {
            RaycastHit hit;

            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, teleportMask)) // shoot a ray from the controller. If it hits something, make it store the point where it hit and show the laser
            {
				hitPoint = hit.point;
				ShowLaser(hit);
				reticle.SetActive(true); // show reticle
				teleportReticleTransform.position = hitPoint + teleportReticleOffset; // move reticle to where the raycast hit w/ offset to avoid Z-fighting
				shouldTeleport = true;
            }
        }
        else
        {
            laser.SetActive(false); // hide laser when player released the touchpad
            reticle.SetActive(false);
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport)
        {
            Teleport();
        }
	}
}
