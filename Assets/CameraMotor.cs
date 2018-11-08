using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

	public Transform lookAt; // our char
	public Vector3 offset = new Vector3(0, 0f, 0f);

	// Use this for initialization
	void Start () {
		transform.position = lookAt.position + offset;
	}
	private void LateUpdate()
	{
		Vector3 desiredPosition = lookAt.position + offset;
		desiredPosition.x = 0;
		transform.position = Vector3.Lerp(transform.position , desiredPosition, Time.deltaTime * 1.5f);
	}

	
}
