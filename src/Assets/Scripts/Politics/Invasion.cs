using UnityEngine;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class Invasion
    {
        public SuperpowerType invader;
        public float invasionStrength;
        public string target;
        public int durationMonths;
        public bool isNaval;
        public float reparationsCost;

        public Invasion(SuperpowerType invaderType, float strength, string invasionTarget, int duration, bool naval)
        {
            invader = invaderType;
            invasionStrength = strength;
            target = invasionTarget;
            durationMonths = duration;
            isNaval = naval;
            reparationsCost = Mathf.Max(100f, strength * 3f);
        }
    }
}
