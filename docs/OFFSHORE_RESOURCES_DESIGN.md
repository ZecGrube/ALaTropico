# Offshore Resources and Deep-Sea Mining Design

## Overview
This system expands the island's economy to the seabed, introducing offshore deposits (Oil, Gas, Rare Earths) and specialized infrastructure (Drilling Platforms, Pipelines, Research Vessels) to exploit them.

## Core Components

### 1. OffshoreDeposit
Represents a resource node on the sea floor.
- `Vector2 position`: Coordinates in the marine zone.
- `float depth`: Influences extraction cost and difficulty.
- `float quantity`: Remaining resources.
- `bool isDiscovered`: Visible on map only after research vessel scan.
- `bool isClaimed`: Jurisdiction status (Territorial vs. International).

### 2. OffshorePlatform (Inherits from Building)
Structures built on water to extract resources.
- `DrillingPlatform`: For Oil and Gas.
- `OffshoreMine`: For minerals and rare earth metals.
- `float riskFactor`: Probability of a monthly accident (spill, leak).
- Requires transport links (Pipelines or Tankers).

### 3. Marine Logistics
- **Pipelines**: Linear underwater connections for fluid resources (Oil/Gas).
- **OilTankers**: Specialized ships for transporting oil from platforms to shore terminals.
- **ShoreTerminal**: Coastal building for receiving offshore resources.

### 4. Research Vessel (Vehicle)
Mobile units that scan the seabed for new deposits.
- Discovery mechanic: Scans a radius around the ship and reveals `OffshoreDeposit` nodes.

### 5. OffshoreManager (Singleton)
- Procedurally generates initial deposits based on Perlin noise.
- Manages monthly extraction and risk assessments.
- Triggers oil spill events.

## Integration
- **Economy**: New high-value export commodities.
- **Ecology**: Spills significantly impact tourism attractiveness and coastal health.
- **Politics**: Claiming international deposits improves national prestige but damages relations with neighbors and superpowers.

## UI/UX
- **Seabed Overlay**: Toggles visibility of known underwater deposits.
- **Platform Panel**: Monitors extraction rates and accident risks.
- **Disaster Notifications**: Alerts player to leaks or spills.

## Persistence
Saves position, type, and remaining quantity of all discovered and undiscovered deposits, along with platform states.
