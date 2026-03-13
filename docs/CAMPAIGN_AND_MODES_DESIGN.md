# Caudillo Bay - Campaign and Game Modes Design

## 1. Game Modes Overview
Players can engage with *Caudillo Bay* through three primary modes:

- **Campaign**: A series of narrative-driven missions with specific objectives and starting conditions.
- **Sandbox**: A free-form mode where players configure their island's size, resources, and starting difficulty.
- **Online (Placeholder)**: A stub for future multiplayer features.

## 2. Core Components

### 2.1 CampaignMission (ScriptableObject)
Defines the parameters for a specific story mission:
- **Briefing**: Title, description, briefing/debriefing text.
- **Map Parameters**: Fixed island generation seed or specific layout.
- **Starting State**: Pre-built buildings, initial treasury, and faction loyalty offsets.
- **Objectives**: A list of `CampaignObjective` objects.

### 2.2 CampaignObjective
Tracks progress toward mission completion:
- **Types**: `BuildBuilding`, `ReachPopulation`, `AccumulateWealth`, `SurviveTime`, `FactionLoyalty`.
- **Status**: Target value vs. Current value.

### 2.3 SandboxSettings
A data structure containing player-selected parameters:
- **Island Size**: Small, Medium, Large.
- **Resource Richness**: Sparse, Normal, Abundant.
- **Political Difficulty**: Affects mandate generation and coup chance.

## 3. Systems Interaction
- **CampaignManager**: Monitors objectives during the monthly tick. Triggers the victory or defeat UI.
- **GameStateManager**: Stores the selected `GameMode` and passes `SandboxSettings` or `CampaignMission` data to the `IslandGenerator` upon scene load.
- **SaveSystem**: Serializes the current mode, mission ID, and objective progress.

## 4. Navigation Flow
1. **Main Menu** -> "New Game"
2. **Mode Selection** -> Choose "Campaign" or "Sandbox"
3. **Setup Screen** -> (List of missions for Campaign / Configuration sliders for Sandbox)
4. **Game Start** -> Scene load + Island Gen with custom parameters.
