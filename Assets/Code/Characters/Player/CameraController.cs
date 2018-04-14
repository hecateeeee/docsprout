﻿using UnityEngine;

namespace Code.Characters.Player {
	public class CameraController : MonoBehaviour {

		public new Camera camera;
		public Transform target;
		float camRotY = 30f;
		float camRotX;

		public LayerMask obscuresCamera;

		public float xSensitivity = 100f;
		public float ySensitivity = 50f;
		public float followDistance = 16f;
		public float minYAngle = -20f;
		public float maxYAngle = 50f;
		public bool invertY = false;
		public float stiffness = 25f;
		public float AlphaSlope = 5f;

		public float CollisionRadius;

		RaycastHit [] alphaHits;
		Collider [] alphaOverlaps;

		// Use this for initialization
		void Start ()
		{
			if (target == null) target = transform;
			Quaternion goalCamRot = target.rotation * Quaternion.AngleAxis (camRotX, Vector3.up);
			goalCamRot *= Quaternion.AngleAxis (camRotY, Vector3.right);

			RaycastHit hit;
			float camDist = followDistance;
			if (Physics.SphereCast (target.position, .5f, -(goalCamRot * Vector3.forward), out hit, followDistance - .5f, obscuresCamera)) {
				camDist = hit.distance;
			}

			Vector3 goalCamPos = target.position - goalCamRot * Vector3.forward * camDist;


			camera.transform.position = goalCamPos;
			camera.transform.rotation = goalCamRot;

			alphaHits = new RaycastHit [8];
			alphaOverlaps = new Collider [4];

		}

		// Update is called once per frame
		public void moveCamera (float x, float y)
		{

			camRotX += x * xSensitivity;
			camRotX = camRotX % 360;

			float dYCam = y;
			if (!invertY) dYCam *= -1;

			camRotY += dYCam * ySensitivity;

			camRotY = Mathf.Clamp (camRotY, minYAngle, maxYAngle);

		}

		Environment.ScreenDoorTransparency lastSet;
		void Update ()
		{
			//float camDist = followDistance;
			Environment.ScreenDoorTransparency sdt = null;
			float dist = 0f;
			int n = Physics.OverlapSphereNonAlloc (camera.transform.position, CollisionRadius, alphaOverlaps);
			for (int i = 0; i < n; ++i) {
				if (sdt = alphaHits [i].collider.gameObject.GetComponent<Environment.ScreenDoorTransparency> ()) {
					dist = 0f;
					break;
				}
			}
			if (sdt == null) {
#if UNITY_EDITOR
				Debug.DrawLine (camera.transform.position, camera.transform.position +
						   camera.transform.forward *
						   Vector3.Distance (camera.transform.position, target.position));
#endif
				n = Physics.RaycastNonAlloc (camera.transform.position, camera.transform.forward, alphaHits,
												  Vector3.Distance (camera.transform.position, target.position), ~obscuresCamera.value,
												  QueryTriggerInteraction.Ignore);

				for (int i = 0; i < n; ++i) {
					if (sdt = alphaHits [i].collider.gameObject.GetComponent<Environment.ScreenDoorTransparency> ()) {
						dist = alphaHits [i].distance;
						break;
					}
				}
			}
			if (lastSet) {
				lastSet.Alpha = 1f;
			}
			if (sdt) {
				sdt.Alpha = dist / (AlphaSlope + 0.0001f);
			}
			lastSet = sdt;

		}

		void FixedUpdate ()
		{
			Quaternion goalCamRot = target.rotation * Quaternion.AngleAxis (camRotX, Vector3.up);
			goalCamRot *= Quaternion.AngleAxis (camRotY, Vector3.right);

			RaycastHit hit;
			float camDist = followDistance;
			if (Physics.SphereCast (target.position, CollisionRadius, -(goalCamRot * Vector3.forward), out hit, followDistance - CollisionRadius, obscuresCamera, QueryTriggerInteraction.Ignore)) {
				camDist = hit.distance;
			}

			Vector3 goalCamPos = target.position - goalCamRot * Vector3.forward * camDist;


			camera.transform.position = Vector3.Lerp (camera.transform.position, goalCamPos, stiffness * Time.fixedDeltaTime);
			camera.transform.rotation = Quaternion.Slerp (camera.transform.rotation, goalCamRot, stiffness * Time.fixedDeltaTime);



		}
	}

}
