# Caudillo Bay - Pollution & Ecology Design (Sprint 2.5)

## 1. Overview
The ecology system tracks the environmental impact of industrialization. High pollution levels damage citizen health and anger the Environmentalist faction.

## 2. Core Components

### 2.1 Pollution Level (0-100)
- **Global Parameter:** Managed by `StatsManager`.
- **Regional Parameter:** (Optional for future) Different zones of the island can have different levels.

### 2.2 Sources & Sinks
- **Sources (Increase):**
  - `ProducerBuilding` (Factories, Power Plants).
  - `Mining` (Iron/Coal mines).
  - High population density.
- **Sinks (Decrease):**
  - `GreenPark`: Small passive reduction.
  - `WasteTreatmentPlant`: Large active reduction, requires workers and power.
  - `Solar/Wind Power`: Replacement for polluting coal plants.

## 3. Effects

### 3.1 Citizen Health & Happiness
- **Residential Happiness:** Direct penalty based on `PollutionLevel`.
- **Death Rate/Health:** High pollution increases the chance of agents becoming "Sick" or leaving the island.

### 3.2 Political Impact
- **Environmentalists (Los Verdes):** Loyalty drops significantly as pollution rises.
- **Demands:** They will demand the construction of waste treatment or the passage of "Clean Air" decrees.

## 4. Implementation Specification
- `Building` class gets a `float pollutionOutput` field.
- `StatsManager.RefreshStats()` aggregates all outputs and applies passive decay.
- UI displays a "Smog" overlay or a status icon when pollution is critical.

---
*Note: This system forces a choice between rapid industrial growth and long-term sustainability.*
