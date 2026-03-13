# Caudillo Bay - Logistics and Citizens Design

## 1. Citizen Simulation
Citizens are the lifeblood of the island. They provide labor, consume goods, and drive the political climate.

### 1.1 Citizen Data & State
- **Identity**: ID, Satisfaction (0-100).
- **Needs**: Food, Housing, Employment.
- **States**: `Idle`, `GoingToWork`, `Working`, `GoingHome`, `SeekingFood`.
- **Logic**: Citizens check their needs periodically. If hungry, they search for the nearest `Market` or `Storage`. If unemployed, they look for open slots in `ProducerBuilding` or other workplaces within range.

### 1.2 PopulationManager
A singleton responsible for:
- Spawning citizens into `ResidentialBuilding` when capacity allows.
- Calculating global satisfaction metrics (unemployment rate, average happiness).
- Passing data to `FactionManager` to affect political loyalty.

## 2. Logistics System
Resources no longer "teleport" between buildings. They are physically moved by vehicles.

### 2.1 LogisticsManager
Central brain for the island's transport:
- **TransportOrder**: A task containing Source, Destination, ResourceType, and Amount.
- **Logic**: Scans `ProducerBuilding` inputs (needs) and `StorageBuilding` stocks (supplies). Generates orders and assigns them to available `Vehicle` agents.

### 2.2 Vehicle Simulation
- **Types**: Trucks (initial), Trains/Ships (future).
- **Logic**: Vehicles use `NavMeshAgent` for pathfinding. They cycle through `Loading`, `Moving`, and `Unloading` states.

## 3. Revised Economic Flow
1. **ProducerBuilding** signals `LogisticsManager` that its input buffer is low.
2. **LogisticsManager** finds a `StorageBuilding` with the required resource and generates an order.
3. A **Vehicle** picks up the order, drives to the source, loads the goods, and delivers them to the producer.
4. Once inputs are delivered, the **ProducerBuilding** executes its production cycle.
5. Residents (Citizens) physically visit `Markets` (specialized Storages) to pull food for their household.
