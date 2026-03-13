using UnityEngine;
using System.Collections.Generic;
using System;

namespace CaudilloBay.Core
{
    public enum GameAction { OpenBuildMenu, OpenPolitics, OpenGlobalMap, QuickSave, QuickLoad, Pause }

    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private Dictionary<GameAction, KeyCode> keyBindings = new Dictionary<GameAction, KeyCode>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDefaults();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeDefaults()
        {
            keyBindings[GameAction.OpenBuildMenu] = KeyCode.B;
            keyBindings[GameAction.OpenPolitics] = KeyCode.O;
            keyBindings[GameAction.OpenGlobalMap] = KeyCode.M;
            keyBindings[GameAction.QuickSave] = KeyCode.F5;
            keyBindings[GameAction.QuickLoad] = KeyCode.F9;
            keyBindings[GameAction.Pause] = KeyCode.P;
        }

        public bool GetActionDown(GameAction action)
        {
            if (keyBindings.ContainsKey(action))
                return Input.GetKeyDown(keyBindings[action]);
            return false;
        }

        public void Rebind(GameAction action, KeyCode newKey)
        {
            keyBindings[action] = newKey;
        }
    }
}
