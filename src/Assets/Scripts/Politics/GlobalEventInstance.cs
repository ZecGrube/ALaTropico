using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class GlobalEventInstance
    {
        public string templateId;
        public string resolvedTitle;
        public string resolvedDescription;
        public string targetCountryName;
        public float remainingDuration;
        public float severity;

        // References for logic
        [System.NonSerialized] public GlobalEventTemplate template;
        [System.NonSerialized] public NeighborState neighborRef;
        [System.NonSerialized] public Superpower superpowerRef;

        public GlobalEventInstance(GlobalEventTemplate t, string countryName)
        {
            template = t;
            templateId = t.eventId;
            targetCountryName = countryName;
            remainingDuration = t.duration;
            severity = Random.Range(0.5f, 1.5f);

            ResolveTemplates();
        }

        public void ResolveTemplates()
        {
            resolvedTitle = template.titleTemplate.Replace("{country}", targetCountryName);
            resolvedDescription = template.descriptionTemplate.Replace("{country}", targetCountryName);

            // Further resolution for {amount} etc could be added here
        }
    }
}
