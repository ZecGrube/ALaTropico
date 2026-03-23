# International Organizations, Alliances, and Deep Espionage Design

## Overview
This system expands the Global Map with formal diplomatic entities (International Organizations), regional power blocs (Alliances), and a sophisticated intelligence layer (Spy Networks and Counter-Intelligence).

## Core Components

### 1. International Organizations (ScriptableObject)
Global or regional bodies like the UN or a Caribbean Trade Bloc.
- `List<Requirement> requirements`: Minimum economic/political stats to join.
- `List<Benefit> benefits`: Trade bonuses, military protection, or tech sharing.
- `List<Obligation> obligations`: Yearly dues, specific laws, or troop contributions.
- `void Join()` / `void Expel()`: Logic for membership state.

### 2. Alliances
Dynamic blocs formed between the player and neighbor states.
- `AllianceType`: Military, Trade, or Defensive.
- `float cohesion`: Measures members' commitment; affected by diplomatic relations.
- `Joint Actions`: High-impact missions like "Joint Naval Maneuvers" or "Customs Union".

### 3. Deep Espionage
- **Spy Networks**: Agents assigned to a country form a network, providing passive intel and increasing mission success rates over time.
- **Mission Types**:
    - **Industrial Sabotage**: Disables enemy production.
    - **Cyber Attack**: Impacts enemy utilities or police effectiveness.
    - **Recruitment**: Chance to convert a foreign agent.
    - **Assassination**: Targets generals to reduce enemy military morale.
    - **Disinformation**: Reduces local faction loyalty in target states.
- **Counter-Intelligence**: Passive defense provided by the `CounterIntelligenceHQ` building.

## Integration
- **Economy**: Alliances create free-trade zones, increasing export revenue. Organizations require yearly membership fees.
- **Military**: Alliances provide reinforcements during invasions.
- **Politics**: Global organizations can issue mandates that affect island legitimacy.

## UI/UX
- **Diplomacy HUB**: Central UI for managing memberships and viewing alliance status.
- **Intel Map**: Displays active spy networks and foreign agent activity.

## Persistence
Saves membership status, active alliances, spy network strength, and ongoing covert missions.
