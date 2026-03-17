# Eras and Quests System Design

## Overview
The "Eras" system replaces abstract tech point accumulation with a structured historical progression. Players advance through Eras (Colonial, Industrial, Modern, Future) by completing Main Quests.

## Core Components

### 1. Era (ScriptableObject)
Represents a historical period.
- `string eraName`, `string description`
- `int eraIndex`
- `List<Technology> technologiesUnlocked`
- `List<BuildingData> buildingsUnlocked`
- `List<Quest> mainQuests`: Required for transition.
- `Era nextEra`

### 2. Quest (ScriptableObject)
A specific task assigned to the player.
- `string questId`, `string questName`, `string description`
- `QuestType type`: (BuildBuilding, ReachPopulation, ResourceThreshold, ResearchTech, FactionLoyalty)
- `string targetId`: (buildingId, resourceId, factionType, etc.)
- `int requiredAmount`
- `bool isMainQuest`: If true, blocks Era transition.
- `List<QuestReward> rewards`

### 3. QuestManager (Singleton)
Tracks progress for all active quests.
- `List<QuestInstance> activeQuests`
- `List<string> completedQuestIds`
- `void UpdateProgress(QuestType type, string id, int amount)`
- `void CompleteQuest(QuestInstance instance)`

### 4. EraManager (Singleton)
Manages historical progression.
- `Era currentEra`
- `void CheckTransition()`: Triggered when a Main Quest is completed.
- `void TransitionToNextEra()`: Unlocks new content and populates next Era's quests.

## Integration
- **TechnologyManager**: `CanResearch(tech)` checks if `tech.requiredEra == currentEra`.
- **PlacementSystem**: Building placement filtered by `building.requiredEra`.
- **UI**: New "Era HUD" showing current period and main objectives. Full-screen notification on transition.

## Persistence
- `currentEraIndex`
- `completedQuestIds`
- `activeQuestProgress` (Dictionary of ID -> current progress)
