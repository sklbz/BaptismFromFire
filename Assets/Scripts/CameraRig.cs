using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour {

	Transform player;
	[SerializeField]
	Vector3 camOffset;
	[SerializeField]
	float interpolationRatio = 1f;

	void Start()
	{
		player = GameObject.FindObjectOfType<PlayerController>().transform;
		transform.position = player.transform.position + camOffset;
	}

	void LateUpdate () {
		transform.position = Vector3.Lerp(transform.position, player.transform.position + camOffset, Time.deltaTime * interpolationRatio);
	}
}
