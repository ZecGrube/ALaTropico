# Zoning and Landmarks System Design

## Overview
This system allows players to group buildings into functional Districts (Zones) and apply specific Policies to them. It also introduces Landmarks—unique buildings with powerful global bonuses.

## Core Components

### 1. District
A collection of buildings within a specified area.
- `string districtName`
- `Color districtColor`
- `List<Building> buildings`
- `List<Policy> activePolicies`
- `float attractiveness`, `float pollution`, `float crimeRate` (aggregated stats)

### 2. DistrictManager (Singleton)
Manages the lifecycle of all districts.
- `List<District> activeDistricts`
- `District CreateDistrict(string name, List<Vector2Int> gridPositions)`
- `void AssignBuildingToDistrict(Building b, District d)`
- `void ApplyPolicyToDistrict(District d, Policy p)`

### 3. Policy (ScriptableObject)
A set of modifiers applied to all buildings in a district.
- `string policyName`, `string description`
- `float maintenanceCost`
- `Dictionary<BuildingType, float> modifiers` (e.g., +20% production for Industrial)
- `float attractivenessBonus`, `float crimeModifier`

### 4. Landmark (Inherits from Building)
Unique structures with global effects.
- `bool isUnique = true`
- `PlacementConditions conditions` (e.g., minimum population, tech level)
- `GlobalEffect globalEffect` (e.g., +15% Tourism, +10% Logistics)

## Selection Tool
The `DistrictTool` allows players to click and drag a rectangular area on the grid. All buildings within this rectangle are grouped into a new or existing district.

## Integration
- **Economy**: District policies like "Industrial Zone" increase production but also maintenance and pollution.
- **Tourism**: "Historical District" policy boosts attractiveness and revenue from hotels.
- **Citizens**: Happiness is influenced by the "Eco-District" policy (improves health) or "Police Presence" (reduces crime).
- **Politics**: Policies affect faction loyalty (e.g., "Industrial Zone" pleases Capitalists, "Eco-District" pleases Environmentalists).

## Persistence
Districts are saved as a list of names, colors, active policy IDs, and the grid boundaries/coordinates they cover. Landmarks are saved as unique instances.
