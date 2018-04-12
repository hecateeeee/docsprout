﻿using System.Collections;
using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (FlockBehaviour))]
	public class Dood : MonoBehaviour {

		public Root Behavior { get; private set; }
		public Characters.Walk Walk;
		public Characters.Character Character;
		FlockBehaviour _flock;


		public void Initialize ()
		{
			Behavior = GetComponent<BehaviorTree> ().Root;
		}

		void Start ()
		{
			Walk = GetComponent<Characters.Walk> ();
			Character = GetComponent<Characters.Character> ();
			_flock = GetComponent<FlockBehaviour> ();
		}
		Vector3 lastPos;
		public bool MoveTowards (Vector3 pos, float thresh = 10f, float minDist = 3f)
		{
			float dist = Vector3.Distance (pos, transform.position);
			float moveDelta = Vector3.Distance (pos, lastPos);
			lastPos = pos;
			if (dist < minDist) {
				Walk.SetDir (Vector2.zero);
				return false;
			}
			if (moveDelta < 0.001f) {
				if (dist < thresh) {
					if (finishedMove) {
						Walk.SetDir (Vector2.zero);
						return false;
					}
					if (!finishedMove && !isTiming) {
						StartCoroutine (stopTimer (dist));
					}
				} else {
					if (isTiming) {
						StopCoroutine ("stopTimer");
						isTiming = false;
					}
					finishedMove = false;

				}
			} else {
				StopCoroutine ("stopTimer");
			}
			var direction = (pos - transform.position).normalized;
			Vector3 force = _flock.CalculateForce ();
			force = force * Time.deltaTime + direction;
			Walk.SetDir (force);
			return false;
		}

		bool finishedMove;
		bool isTiming;
		IEnumerator stopTimer (float dist)
		{
			isTiming = true;
			yield return new WaitForSeconds (.15f * dist);
			isTiming = false;
			finishedMove = true;
		}

		public void StopMoving ()
		{
			Walk.SetDir (Vector2.zero);
		}
	}
}