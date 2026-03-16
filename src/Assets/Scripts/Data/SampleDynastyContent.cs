using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Core;
using CaudilloBay.Politics;

namespace CaudilloBay.Data
{
    public static class SampleDynastyContent
    {
        public static void CreateSampleDynastyEvents(EventManager manager)
        {
            // Event: Royal Birth
            GameEvent birth = ScriptableObject.CreateInstance<GameEvent>();
            birth.eventId = "royal_birth";
            birth.title = "A Royal Birth!";
            birth.description = "A new heir to the dynasty has been born. The nation celebrates!";

            EventChoice declareHoliday = new EventChoice {
                choiceText = "Declare a National Holiday",
                outcomeText = "Happiness increases, but production slows.",
                legitimacyChange = 10f,
                mandateChange = 5
            };

            birth.choices.Add(declareHoliday);
            manager.allEvents.Add(birth);

            // Event: Palace Intrigue
            GameEvent intrigue = ScriptableObject.CreateInstance<GameEvent>();
            intrigue.eventId = "palace_intrigue";
            intrigue.title = "Palace Intrigue";
            intrigue.description = "A rival heir is gathering support among the military.";

            EventChoice arrest = new EventChoice {
                choiceText = "Arrest the Conspirator",
                outcomeText = "The threat is neutralized, but legitimacy suffers.",
                legitimacyChange = -15f
            };

            intrigue.choices.Add(arrest);
            manager.allEvents.Add(intrigue);
        }
    }
}
