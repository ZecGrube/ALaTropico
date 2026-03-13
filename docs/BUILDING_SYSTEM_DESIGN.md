# Caudillo Bay - Building System Design

## 1. Overview
The building system is the core of *Caudillo Bay*'s gameplay. Buildings are the primary drivers of the economy, political influence, and physical island development. This document describes the data-oriented architecture used to define, place, and simulate buildings.

## 2. Data Architecture: BuildingData (ScriptableObject)
All building types are defined as `BuildingData` assets. This allows for easy balance tweaks without modifying code.

### 2.1 Core Parameters
- **Identity**: ID, Name, Description, Category (Enum), Icon.
- **Physical**: Prefab (Visual/Collision), Footprint (Grid size).
- **Construction**: Build Costs (Array of Resource/Amount), Build Time.
- **Economic**:
    - `workersRequired`: Jobs provided.
    - `production`: Monthly output.
    - `consumption`: Monthly input.
    - `maintenanceCost`: Monthly gold cost.
    - `storageCapacity`: For warehouses.
- **Political**:
    - `loyaltyEffects`: Immediate/Monthly impact on specific factions.
    - `touristAttraction`: Influence on tourist flow.
- **Environmental**: `pollutionOutput`.
- **Technological**: `requiredTech` (Reference to Technology asset).

## 3. Building Categories
- **Residential**: Provides housing, consumes food, generates happiness.
- **Industrial**: Processes resources (e.g., Sawmill, Rum Distillery).
- **Agricultural**: Extracts raw resources (e.g., Banana Plantation).
- **Infrastructure**: Power, Water, Roads, Docks.
- **Tourism**: Hotels, Casinos, Beach Clubs.
- **Government**: Palace, Police, Ministry.
- **Decorative**: Parks, Statues (Esthetic only).

## 4. Monthly Cycle Integration
The `EconomyManager` executes a monthly tick that iterates through all active buildings:
1. **Consumption**: Attempt to pull inputs from local or global inventory.
2. **Production**: If inputs and workers are available, generate outputs.
3. **Pollution**: Add to the island's global pollution metric.
4. **Maintenance**: Subtract from the treasury.
5. **Politics**: Apply monthly loyalty modifiers based on building status.

## 5. Technology Gating
The `PlacementSystem` queries `TechnologyManager` before allowing a building to be placed. If `BuildingData.requiredTech` is not researched, the building remains locked in the construction menu.
