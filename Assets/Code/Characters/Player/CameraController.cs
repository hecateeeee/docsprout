﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera camera;
    public Transform target;
    float camRotY;
    float camRotX;

    public LayerMask obscuresCamera;

    public float xSensitivity = 0.0005f;
    public float ySensitivity = 0.0005f;
    public float followDistance = 16f;
    public float minYAngle = -20f;
    public float maxYAngle = 50f;
    public bool invertY = false;
    public float stiffness = 0.5f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void moveCamera (float x, float y) {

        camRotX += x*xSensitivity;
        camRotX = camRotX % 360;

        float dYCam = y;
        if(!invertY) dYCam *= -1;

        camRotY += dYCam*ySensitivity;

        camRotY = Mathf.Clamp(camRotY, minYAngle, maxYAngle);
		
	}

    void FixedUpdate()
    {
        Quaternion goalCamRot = Quaternion.AngleAxis(camRotX, Vector3.up);
        goalCamRot *= Quaternion.AngleAxis(camRotY, Vector3.right);

        RaycastHit hit;
        float camDist = followDistance;
        if(Physics.SphereCast(target.position, .5f, -(goalCamRot*Vector3.forward), out hit, followDistance - .5f, obscuresCamera)) {
            camDist = hit.distance;
        }

        Vector3 goalCamPos = target.position - goalCamRot*Vector3.forward*camDist;


        camera.transform.position = Vector3.Lerp(camera.transform.position, goalCamPos, stiffness);
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, goalCamRot, stiffness);

    }
}
