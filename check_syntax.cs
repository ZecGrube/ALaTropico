using System;
using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Data;
using CaudilloBay.Politics;
using CaudilloBay.Core;
using CaudilloBay.AI;
using CaudilloBay.Economy;

public class SyntaxCheck {
    public void Test() {
        Debug.Log("Checking references...");
        var stats = typeof(StatsManager);
        var building = typeof(Building);
        var save = typeof(SaveSystem);
        var ai = typeof(BuilderAI);
        var factions = typeof(FactionManager);
        var tech = typeof(TechnologyManager);
        var map = typeof(GlobalMapManager);
        var tourism = typeof(TouristManager);
    }
}
