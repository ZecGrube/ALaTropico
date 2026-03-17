using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Systems;

namespace CaudilloBay.UI
{
    public class UtilityOverlay : MonoBehaviour
    {
        public bool showPower = false;
        public bool showWater = false;

        public void TogglePower() { showPower = !showPower; }
        public void ToggleWater() { showWater = !showWater; }

        private void OnGUI()
        {
            if (showPower) GUILayout.Label("POWER GRID OVERLAY ACTIVE");
            if (showWater) GUILayout.Label("WATER NETWORK OVERLAY ACTIVE");
        }

        // In a real implementation, we would highlight UtilityNodes with colors
        // Green for satisfied, Red for unsatisfied
    }
}
