# Caudillo Bay - Expanded Political System Design (Sprint 2.1)

## 1. Overview
This document outlines the expansion of the political system from a single faction (Peasants) to five distinct factions, each with their own leadership, requirements, and inter-faction relationships.

## 2. Core Components

### 2.1 Factions
The five core factions are:
1. **Capitalists (El Capital):** Want low taxes, high exports, and luxury goods.
2. **Communists (Los Comunistas):** Want high employment, social services, and low inequality.
3. **Nationalists (La Patria):** Want strong military presence and low foreign influence.
4. **Religious (La Iglesia):** Want churches, high "morality" (low crime), and conservative decrees.
5. **Environmentalists (Los Verdes):** Want low pollution and preservation of nature.

### 2.2 Faction Leadership
- **FactionLeader (ScriptableObject):**
  - `string leaderName`, `Texture2D portrait`, `Personality personality`.
  - Personalities (Aggressive, Moderate, Charismatic) affect the frequency and intensity of demands.

### 2.3 Faction Relations
- A 2D matrix (or `Dictionary<FactionType, float>`) stored within each `FactionData`.
- High relations between two factions mean helping one also helps the other.
- Low relations (Enmity) mean helping one angers the other (e.g., Capitalists vs. Communists).

### 2.4 Faction Needs & Statistics
- Faction loyalty is calculated based on "Satisfiers":
  - **Global Stats:** Tax rate, unemployment, budget surplus.
  - **Building Presence:** Count of specific buildings (e.g., Churches for Religious, Casinos for Capitalists).
  - **Resource Availability:** Stockpiles of specific goods.

### 2.5 Demands System
- **Demand:** A temporary objective issued by a faction leader.
- **Types:** `BuildBuilding`, `EnactDecree`, `ReachResourceStockpile`, `MaintainStat`.
- **States:** `Pending`, `Accepted`, `Completed`, `Failed`, `Expired`.

## 3. Technical Architecture

### 3.1 Class Relationship Diagram
```text
[ ScriptableObject ]
      |
      +-- [ FactionLeader ]
      +-- [ Decree ] (Updated for multi-faction effects)
      +-- [ DemandTemplate ]

[ FactionData ] (Expanded)
      |-- FactionType type
      |-- FactionLeader leader
      |-- Dictionary<FactionType, float> relations
      |-- List<Demand> activeDemands
      |-- float loyalty

[ FactionManager ] (Expanded)
      |-- List<FactionData> factions
      |-- StatsManager stats (Helper to aggregate building data)
      |-- UpdateAllFactions()
      |-- CheckDemands()
      |-- ModifyRelations(FactionType, FactionType, delta)

[ Building ] (Existing)
      |-- List<FactionType> favoredFactions
      |-- List<FactionType> dislikedFactions
```

### 3.2 Monthly Update Loop (Sequence)
1. **Gather Stats:** `StatsManager` scans all buildings and inventories.
2. **Calculate Needs:** Each faction updates its internal "Satisfaction" score based on stats.
3. **Apply Relations:** Loyalty is adjusted based on how "allied" or "enemy" factions are doing.
4. **Process Demands:**
   - Check completion of `activeDemands`.
   - Check if new demands should be generated (if loyalty < 30).
5. **Generate Mandate:** Sum of global loyalty and faction support bases.
6. **Update UI:** Push new data to `PoliticsUI`.

---
*Note: This system allows for emergent gameplay where favoring one faction inevitably alienates another, forcing El Presidente to balance or suppress.*
