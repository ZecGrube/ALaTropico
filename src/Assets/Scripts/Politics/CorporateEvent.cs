using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Economy;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class CorporateEvent : GameEvent
    {
        public Corporation targetCorporation;
        public float wealthThreshold; // Trigger if corp wealth > threshold

        public void CheckOligarchInfluence()
        {
            if (targetCorporation != null && targetCorporation.treasury > wealthThreshold)
            {
                // Trigger oligarch demand event
                Core.EventManager.Instance.TriggerEvent(this);
            }
        }
    }
}
