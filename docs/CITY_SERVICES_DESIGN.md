# City Services and Public Utilities Design

## Overview
This system introduces the essential services required for a functioning city: waste management, street lighting, cleaning, and public transport. These services impact citizen happiness, health, safety, and the island's environmental status.

## Core Systems

### Waste Management (WasteManager.cs)
- **Goal**: Handle the lifecycle of garbage from production in buildings to disposal or recycling.
- **Mechanics**:
  - Buildings produce `garbageOutput` monthly.
  - `GarbageTruck` agents collect accumulated waste and transport it to disposal facilities.
  - **Facilities**:
    - `Landfill`: Cheap disposal, high pollution.
    - `Incinerator`: Burns waste for energy (optional), moderate pollution.
    - `Recycling Plant`: Converts waste into `Recyclables` (metal/plastic), low pollution, high cost.
- **Impact**: Uncollected garbage reduces district cleanliness, lowering health and happiness.

### Street Services
- **Street Lighting (StreetLight.cs)**:
  - Increases 'Lighting' coverage in a radius.
  - Improves safety (reduces local crime effect) and citizen satisfaction at night (abstractly).
- **Street Cleaning (StreetSweeperStation.cs)**:
  - Deploys sweepers to maintain `cleanliness` in a radius.
  - Counteracts the negative effects of garbage accumulation.

### Public Transport (LogisticsManager expansion)
- **Goal**: Enable citizens to commute to distant workplaces efficiently.
- **Infrastructure**:
  - `BusStop`: Defines pickup/drop-off points.
  - `BusDepot`: Produces and maintains the `Bus` fleet.
- **Mechanics**:
  - Citizens with workplaces > 100 units away prioritize homes/workplaces near `BusStops`.
  - Buses follow routes connecting stops.
  - Provides a happiness bonus for "Accessibility".

## Citizen Integration
- **Happiness Calculation**:
  - `Environment` factor: Average of cleanliness and lighting coverage.
  - `Accessibility` factor: Availability of public transport for long commutes.
- **Health**: Cleanliness affects the global health level in the district.

## UI Requirements
- **Overlays**: Data maps for Cleanliness, Lighting, and Transport Accessibility.
- **Service Panel**: Stats on garbage production vs. disposal capacity and fleet status.

## Persistence
- Garbage levels per building and active transport routes are serialized in `SaveSystem.json`.
