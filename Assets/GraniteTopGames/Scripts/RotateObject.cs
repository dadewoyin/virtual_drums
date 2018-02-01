using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

	private Vector3 rotation = new Vector3(0, 12, 0);

	void Update () {

		transform.Rotate (rotation * Time.deltaTime, Space.World);

	}
}
