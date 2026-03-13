using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Core;

namespace CaudilloBay.Data
{
    public static class SampleCrimeContent
    {
        public static void CreateSampleCrimeEvents(EventManager manager)
        {
            // Event: Prison Break
            GameEvent prisonBreak = ScriptableObject.CreateInstance<GameEvent>();
            prisonBreak.eventId = "prison_break";
            prisonBreak.title = "Prison Break!";
            prisonBreak.description = "A massive breakout has occurred at the central prison. Crime is spiking!";

            EventChoice sendArmy = new EventChoice {
                choiceText = "Send the Army",
                outcomeText = "The streets are secured, but the people are afraid.",
                legitimacyChange = -5f,
                mandateChange = -2
            };

            EventChoice letPoliceHandle = new EventChoice {
                choiceText = "Let Police Handle It",
                outcomeText = "Chaos ensues for a few days. Crime remains high.",
                mandateChange = -5
            };

            prisonBreak.choices.Add(sendArmy);
            prisonBreak.choices.Add(letPoliceHandle);

            manager.allEvents.Add(prisonBreak);
        }
    }
}
