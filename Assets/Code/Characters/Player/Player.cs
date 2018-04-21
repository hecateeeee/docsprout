﻿using Code.Characters.Doods;
using UnityEngine;

namespace Code.Characters.Player
{
    [RequireComponent(typeof(CameraController))]
    public class Player : MonoBehaviour
    {
        public enum PlayerState
        {
            Walking,
            Selecting
        }

        [HideInInspector] public PlayerState state;

        CameraController _camController;
        private Movement _movement;


        private void Start () {
            _camController = GetComponent<CameraController>();
            _movement = GetComponent<Movement>();

            if (Game.Ctx != null) { Game.Ctx.SetPlayer(this); }
        }

        void Update () {
            switch (state) {
                case PlayerState.Walking:
                    float x = Game.Sesh.Input.Monitor.LeftH;
                    float y = Game.Sesh.Input.Monitor.LeftV;
                    if (x * x + y * y < 0.01 || Game.Sesh.Input.Monitor.LT >= 0.1f) {
                        _movement.SetDirection(Vector3.zero);
                        break;
                    }

                    Vector3 forwards = Vector3.Cross(_camController.camera.transform.right, Vector3.up).normalized;
                    Vector2 dir = new Vector2(x * _camController.camera.transform.right.x + y * forwards.x,
                        x * _camController.camera.transform.right.z + y * forwards.z);
                    _movement.SetDirection(dir);
                    break;
            }

            var camX = Game.Sesh.Input.Monitor.RightH;
            var camY = Game.Sesh.Input.Monitor.RightV;
            if (camX * camX + camY * camY > 0.01) {
                _camController.moveCamera(camX * Time.deltaTime, camY * Time.deltaTime);
            }
        }
    }
}