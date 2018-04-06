﻿using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Menus
{
    public class MainMenu : Menu
    {
        private static int _sceneTarget = 4; // Magic constant for the demo scene. TODO bit of a hack.

        public override void CreateGameObject () {
            GO = UIUtils.MakeUIPrefab(UIPrefab.MainMenu);
            InitializeButtons();
        }

        private void InitializeButtons () {
            var buttons = GO.transform.Find("Buttons");

            UIUtils.FindUICompOfType<Button>(buttons, "Start").onClick.AddListener(StartNewGame);
            UIUtils.FindUICompOfType<Button>(buttons, "Load").onClick.AddListener(LoadGame);
            UIUtils.FindUICompOfType<Button>(buttons, "Options").onClick.AddListener(OpenOptions);
            UIUtils.FindUICompOfType<Button>(buttons, "Quit").onClick.AddListener(QuitGame);
            UIUtils.FindUICompOfType<Button>(GO.transform, "Acknowledgements").onClick
                .AddListener(LoadAcknowledgements);
#if UNITY_EDITOR
            // if we are in the editor, add in the secret button to load one of the dev scenes
            var secret = GO.transform.Find("Development");
            secret.gameObject.SetActive(true);
#endif
        }

        private void StartNewGame () {
#if UNITY_EDITOR
            var drop = UIUtils.FindUICompOfType<Dropdown>(GO.transform, "Development/Dropdown");
            _sceneTarget = drop.value + 1;
#endif
            Game.Sesh.StartGame(_sceneTarget);
        }

        private void LoadGame () {
            Debug.Log("Implement me!!"); // todo :3
        }


        private OptionsMenu _options;

        private void OpenOptions () {
            _options = new OptionsMenu(this);
            _options.CreateGameObject();
        }

        // todo this is not grace... dialogs manager here? smart stack?
        public void CloseOptions () {
            _options.RemoveGameObject();
            _options = null;
        }

        private void QuitGame () {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // Complains about dead code. Boo.
#endif
        }

        private void LoadAcknowledgements () {
            Debug.Log("Implement me!"); // todo :3
        }
    }
}