using UnityEngine;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class HotkeySettingsPanel : MonoBehaviour
    {
        public void RebindAction(string actionName, string keyName)
        {
            if (System.Enum.TryParse(actionName, out GameAction action) &&
                System.Enum.TryParse(keyName, out KeyCode key))
            {
                InputManager.Instance.Rebind(action, key);
                PlayerPrefs.SetInt("Key_" + actionName, (int)key);
                Debug.Log($"Rebound {actionName} to {keyName}");
            }
        }

        public void LoadBindings()
        {
            foreach (GameAction action in System.Enum.GetValues(typeof(GameAction)))
            {
                if (PlayerPrefs.HasKey("Key_" + action.ToString()))
                {
                    KeyCode key = (KeyCode)PlayerPrefs.GetInt("Key_" + action.ToString());
                    InputManager.Instance.Rebind(action, key);
                }
            }
        }
    }
}
