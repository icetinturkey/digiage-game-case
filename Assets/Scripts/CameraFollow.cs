using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerTransform;
	private Vector3 _cameraOffset;
	[Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;
	private Vector3 _currentRotation;
	
    void Start()
    {
		_currentRotation = transform.localRotation.eulerAngles;
        _cameraOffset = transform.position - PlayerTransform.position;	
    }

    void LateUpdate()
    {
        Vector3 newPos = PlayerTransform.position + _cameraOffset;
		newPos.x *= 1.3f;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
		transform.rotation = Quaternion.Euler(_currentRotation.x,newPos.x*-10f,_currentRotation.z);
    }
}
