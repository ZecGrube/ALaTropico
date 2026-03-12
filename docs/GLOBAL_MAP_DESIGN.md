# Caudillo Bay - Global Map & Espionage Design (Sprint 2.3)

## 1. Overview
The Global Map system represents the world outside the island. It allows El Presidente to engage in diplomacy with superpowers and conduct covert operations to gain an advantage.

## 2. Core Components

### 2.1 Superpowers (ScriptableObject)
- **USA (Capitalist Bloc):** Favors trade, exports, and democratic stability.
- **USSR (Communist Bloc):** Favors nationalization, social equality, and propaganda.
- **Properties:**
  - `string powerName`, `Sprite flagIcon`.
  - `float relations` (-100 to 100).
  - `List<GlobalMission> missionTemplates`.

### 2.2 Global Missions & Spy Operations
- **GlobalMission:** Overt tasks (e.g., "Export 100 Bananas").
- **MissionOperation:** Covert tasks (e.g., "Steal Tech", "Sabotage Factory").
- **Properties:**
  - `string title`, `string description`.
  - `float baseDifficulty`.
  - `float duration` (Game time).
  - `Rewards`: Relation boost, Resources, Unlocked Tech.

### 2.3 Agents (Spies & Bodyguards)
- **Skills:**
  - `Stealth`: Essential for operations in hostile territory.
  - `Combat`: Reduces risk of agent death in failed missions.
  - `Charisma`: Better success in diplomatic missions.
  - `Tech`: Required for stealing blueprints/technologies.
- **Bodyguards:** Unique agents with fixed high stats and special abilities.

## 3. Technical Architecture

### 3.1 GlobalMapManager (Singleton)
- Tracks active missions and available agents.
- Handles the "Mission Loop":
  1. Agent assigned to Mission.
  2. Agent enters `OnMission` state (unavailable for island tasks).
  3. Timer counts down.
  4. Success check based on Agent skills vs Mission difficulty.
  5. Apply Rewards/Penalties.

### 3.2 Buildings
- **Ministry of Foreign Affairs:** Required to access the Global Map. Generates "Influence Points".
- **Spy School:** Training facility to hire and improve non-unique agents.

## 4. Interaction Diagram
```text
[ Ministry of Foreign Affairs ] --- (Unlocks) ---> [ Global Map UI ]
                                                          |
                                           [ Select Superpower/Mission ]
                                                          |
[ Agent Pool ] <--- (Select Agent) -----------------------+
      |
      V
[ Active Mission ] --(Timer)--> [ Success Check ] --(Result)--> [ Rewards/Events ]
                                                                       |
                                                                       +--> [ Faction Loyalty (Politics) ]
                                                                       +--> [ New Tech (Technology) ]
                                                                       +--> [ Resources (Economy) ]
```

---
*Note: The Global Map adds a high-stakes layer where international failures can trigger internal political crises.*
