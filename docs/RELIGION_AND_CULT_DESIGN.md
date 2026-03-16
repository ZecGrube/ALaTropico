# Religion and Cult of Personality Design

## Overview
The Religion and Cult of Personality systems provide tools for managing the populace through spiritual and ideological means. While Religion focuses on the "Religious" faction and traditional faith, the Cult of Personality focuses on the deification of El Presidente.

## Core Managers

### ReligionManager.cs (Singleton)
- **Goal**: Manage the influence of faith on the island and the Religious faction's power.
- **Fields**:
  - `float religiousInfluence`: Global metric affected by the number and type of religious buildings.
  - `List<ReligiousBuilding> activeTemples`: Tracked religious buildings.
  - `ReligiousLeader currentLeader`: The primary religious figure on the island.
- **Methods**:
  - `CalculateInfluence()`: Aggregates influence from buildings and leaders.
  - `SpawnLeader()`: Logic to introduce a unique religious leader when influence thresholds are met.
  - `ProcessMonthlyUpdate()`: Modifies Religious faction loyalty and support base.

### PersonalityCultManager.cs (Singleton)
- **Goal**: Manage the ideological worship of El Presidente.
- **Fields**:
  - `float cultLevel`: 0-100 scale representing the strength of the cult.
  - `List<CultBuilding> cultAssets`: Statues, mausoleums, etc.
- **Methods**:
  - `AddCultPoints(float amount)`: Increases cult level through events or building construction.
  - `ApplyCultEffects()`: Grants bonuses to Nationalist and Religious loyalty, while potentially penalizing Liberal/Technocratic sentiments (if implemented).

## Buildings

### Religious Buildings (base: ReligiousBuilding.cs)
1. **Church (Iglesia)**: Medium capacity, provides basic spiritual satisfaction and influence.
2. **Cathedral (Catedral)**: Large capacity, significant attractiveness boost for tourism, high influence.

### Cult Buildings (base: CultBuilding.cs)
1. **Statue of El Presidente**: Basic cult asset, increases cult level in a radius.
2. **Mausoleum (Mausoleo)**: Unique building, provides massive global cult bonus and legitimacy.

## Integration
- **Politics**: Religious faction loyalty is highly dependent on `ReligionManager` metrics. Nationalists favor high Cult levels.
- **Tourism**: Cathedrals and cultural landmarks (Mausoleum) increase the `totalAttractiveness` parameter in `TouristManager`.
- **Crime**: High religious influence provides a passive reduction to global crime rates (moral policing).

## Characters
- **ReligiousLeader.cs**: Inherits from `Agent`. Has unique stats like `Faith` and `Influence`. Can be a staunch ally or a vocal critic of the regime.
