using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class LegitimacySystem : MonoBehaviour
    {
        public static LegitimacySystem Instance { get; private set; }

        [Range(0, 100)]
        public float currentLegitimacy = 70f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ModifyLegitimacy(float delta)
        {
            currentLegitimacy = Mathf.Clamp(currentLegitimacy + delta, 0f, 100f);
            Debug.Log($"Legitimacy updated: {currentLegitimacy}");
        }
    }
}
