# Procedural Global Events Design

## Overview
The Procedural Global Events system dynamically generates events occurring in neighboring island states and global superpowers. These events are driven by the internal state of these nations (economy, stability, relations) and require the player's intervention or response.

## Core Components

### 1. GlobalEventTemplate (ScriptableObject)
Defines the structure of a potential event.
- `string eventId`: Unique identifier.
- `string titleTemplate`: e.g., "Economic Crisis in {country}"
- `string descriptionTemplate`: e.g., "The government of {country} is struggling with inflation and requested a loan of ${amount}."
- `EventType type`: (Economic, Political, Disaster, Tech, Military)
- `List<EventRequirement> requirements`: Conditions for the country (e.g., relations < 0, poverty > 50).
- `List<GlobalEventOptionTemplate> options`: Potential player responses.

### 2. GlobalEventInstance
A specific event currently active on the map.
- `GlobalEventTemplate template`
- `string resolvedTitle`, `resolvedDescription` (placeholders replaced)
- `Country targetCountry` (NeighborState or Superpower)
- `float remainingDuration`
- `float severity`

### 3. GlobalEventGenerator
Responsible for the monthly tick that spawns new events.
- `List<GlobalEventTemplate> allTemplates`
- `void Tick()`: Checks each country against templates and rolls for spawn chance.
- `GlobalEventInstance CreateInstance(GlobalEventTemplate template, Country target)`

### 4. GlobalEventEffect
Logic for applying consequences to the player or international relations.
- `EffectType`: (AddFunds, SpawnRefugees, ChangeRelations, AddTechnology, etc.)
- `float value`

## Event Flow
1. **Monthly Tick**: The generator checks conditions for neighbors and superpowers.
2. **Spawn**: If an event triggers, a notification appears on the Global Map.
3. **Player Interaction**: Clicking the event icon opens the UI.
4. **Resolution**: The player chooses an option, effects are applied, and the event is removed (or continues if it has duration).

## Integration
- **NeighborState**: Events can change the stability or relations of neighbors.
- **Superpower**: Events can trigger sanctions or aid offers from major powers.
- **MigrationManager**: Disasters or wars in neighboring states can spawn refugees on the player's island.
- **EconomyManager**: Financial aid or trade deals impact the national treasury.

## Persistence
`SaveSystem` tracks all `GlobalEventInstance` objects, ensuring current world events are restored upon loading.
