﻿using System;
using Code.Interaction;
using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public class DoodStatus : MonoBehaviour, IApproachable
    {
        private const float MAX_HAPPINESS = 100f;
        private const float MAGNITUDE = 5f;

        public float Happiness;
        public Waterable Waterable;

        private DoodColor _doodColor;
        private StatusDisplay _display;

        private void Start () {
            Waterable = GetComponent<Waterable>();
            _doodColor = GetComponent<DoodColor>();
            _doodColor.IsHighlighted = true;
            _display = new StatusDisplay(gameObject);
        }

        private void CalculateHappiness () {
            var delta = (Waterable.Status == 0 ? -1f : 1f) * MAGNITUDE * Time.deltaTime;
            Happiness = Mathf.Clamp(Happiness + delta, 0f, MAX_HAPPINESS);
        }

        private void Update () {
            CalculateHappiness();
            if (_doodColor) {
                _doodColor.Happiness = Happiness / MAX_HAPPINESS;
            }
        }

        // todo if you stand near a dood, their status display won't change
        public void OnApproach () { _display.Show(Waterable.Status); }
        public void OnDepart () { _display.Hide(); }
    }

    public class StatusDisplay
    {
        private readonly GameObject _needMore;
        private readonly GameObject _needLess;

        public StatusDisplay (GameObject go) {
            _needMore = go.transform.Find("StatusDisplay/NeedWater").gameObject;
            _needLess = go.transform.Find("StatusDisplay/StopWater").gameObject;
        }

        public void Show (int status) {
            // lol this could be a bit manipulation // but m'clarity
            switch (status) {
                case 0:
                    SetDisplays(false, false);
                    break;
                case 1:
                    SetDisplays(false, true);
                    break;
                case -1:
                    SetDisplays(true, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Hide () { SetDisplays(false, false); }

        private void SetDisplays (bool under, bool over) {
            _needMore.SetActive(under);
            _needLess.SetActive(over);
        }
    }
}