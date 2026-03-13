using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Core;

namespace CaudilloBay.Data
{
    public static class SampleHealthContent
    {
        public static void CreateSampleHealthEvents(EventManager manager)
        {
            // Event: Plague
            GameEvent plague = ScriptableObject.CreateInstance<GameEvent>();
            plague.eventId = "plague_outbreak";
            plague.title = "Plague Outbreak!";
            plague.description = "A mysterious disease is spreading rapidly. Health is plummeting!";

            EventChoice mandatoryQuarantine = new EventChoice {
                choiceText = "Mandatory Quarantine",
                outcomeText = "The spread is contained, but the economy slows down.",
                legitimacyChange = -10f,
                mandateChange = -5
            };

            EventChoice distributeMedicine = new EventChoice {
                choiceText = "Distribute Medicine",
                outcomeText = "Treasury suffers, but the people are grateful.",
                mandateChange = 5
            };

            plague.choices.Add(mandatoryQuarantine);
            plague.choices.Add(distributeMedicine);

            manager.allEvents.Add(plague);
        }
    }
}
