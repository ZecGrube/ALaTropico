# Caudillo Bay - Political System Design (MVP)

## 1. Overview
The political system manages the relationship between El Presidente and the various power blocks of the island. In the MVP, we focus on a single faction: the **Peasants (Pobres)**.

## 2. Core Concepts

### 2.1 Factions
- **FactionType:** Enum (Peasants, Capitalists, Communists, etc.).
- **Loyalty:** A value from 0 to 100.
  - > 70: Supportive (Bonuses to production/Mandate).
  - < 30: Hostile (Risks of strikes, protests).
- **Support Base:** For Peasants, this is the count of residents in low-tier housing and workers in plantations.

### 2.2 Mandate (Power Points)
- A resource used to issue Decrees.
- **Generation:** Generated monthly.
  - Base: +5 per month.
  - Bonus: +1 for every 10 points of loyalty above 50.
  - Penalty: -1 for every 10 points of loyalty below 50.

### 2.3 Decrees
ScriptableObjects defining direct interventions.
- **Properties:** Cost (Mandate), Loyalty Effects, Economic Modifiers (Tax, Wage).
- **Examples:**
  - *Food Subsidies:* -10 Mandate, +15 Peasant Loyalty, -50$ per month.
  - *Extra Work Hours:* +5 Mandate, -20 Peasant Loyalty, +20% Production.

### 2.4 Political Events
Random or triggered occurrences.
- **Trigger:** Random check every month or specific conditions (e.g., "Food shortage").
- **Effects:** Instant loyalty change or temporary modifiers.

## 3. Architecture

### 3.1 FactionManager (Singleton)
- Tracks all active factions and their loyalty.
- Monthly Tick logic:
  1. Collect satisfaction data from buildings.
  2. Update Loyalty based on satisfaction, active modifiers, and events.
  3. Generate Mandate.
  4. Roll for Random Events.

### 3.2 Integration with Economy
- **Building Satisfaction:** `ResidentialBuilding` provides a `Happiness` value based on resource availability in its inventory (e.g., Corn, Bananas).
- **Economic Modifiers:** A global `PolicySettings` (managed by `FactionManager`) stores current tax and wage multipliers applied by `ProducerBuilding`.

## 4. Class Diagram (Text-based)
```text
[ ScriptableObject ]
      |
      +-- [ Decree ]
      +-- [ PoliticalEvent ]

[ FactionManager ]
      |
      +-- List<FactionData> (Runtime state)
      +-- ApplyDecree(Decree)
      +-- TriggerEvent(PoliticalEvent)
      +-- MonthlyTick()

[ Building ] (Existing)
      ^
      |
[ ResidentialBuilding ]
      +-- float GetHappiness()

[ PoliticsUI ]
      +-- Display Mandate/Loyalty
      +-- List Decrees
```

---
*Note: This MVP focuses on a single-faction feedback loop: Economy -> Satisfaction -> Loyalty -> Mandate -> Decrees -> Economy.*
