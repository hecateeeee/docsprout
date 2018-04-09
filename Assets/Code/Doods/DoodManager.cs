﻿using System.Collections.Generic;
using UnityEngine;

namespace Code.Doods {
	public class DoodManager : IContextManager {
		static Object Prefab;

		readonly List<Dood> _doods = new List<Dood> ();

		public void Initialize ()
		{
			Prefab = Resources.Load ("Sphere Dood");
			for (float x = 0; x < 8; x+= 1.2f) {
				for (float y = 0; y < 12f; y+= 1.2f) {

					MakeDood (new Vector3 (x, 0f, y));
				}
			}
		}

		public void ShutDown () { }


		void MakeDood (Vector3 pos)
		{
			var go = (GameObject)Object.Instantiate (Prefab, pos, Quaternion.Euler(-90f, 0f, 0f));
			var dood = go.GetComponent<Dood> ();
			dood.Initialize ();

			_doods.Add (dood);
		}
	}
}