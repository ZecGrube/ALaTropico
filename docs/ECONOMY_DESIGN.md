# Caudillo Bay - Economy & Resource System Design

## 1. Resource Foundation
The economy is based on discrete resources defined by `ResourceType` (ScriptableObjects).

### 1.1 Resource Types
- **Raw:** Bananas, Sugar Cane, Logs, Iron Ore.
- **Processed:** Sugar, Rum, Planks, Steel.
- **Strategic:** Gold, Mandate, Science.

## 2. Inventory System
Every building, storage, and builder has an `Inventory`.

### 2.1 Core Methods
- `AddResource(ResourceType type, float amount)`: Adds resource to the list. Returns overflow if any.
- `RemoveResource(ResourceType type, float amount)`: Removes amount if available. Returns success.
- `HasResource(ResourceType type, float amount)`: Checks for availability.
- `GetTotalWeight()`: Calculates current load for logistics.

## 3. Building Roles
### 3.1 Base Building
All buildings inherit from `Building`.
- `BuildCost`: List of resources required for construction.
- `BuildProgress`: 0-100% completion.
- `IsFunctioning`: Only true when construction is finished and workers are present.

### 3.2 Producers (IProducer)
- `Recipe`: Defines inputs, outputs, and cycle time.
- `Logic`: Consumes inputs from internal inventory, waits for cycle time, then deposits outputs to internal inventory.

### 3.3 Consumers (IConsumer)
- Consumes resources (e.g., Food, Water) periodically to maintain citizen happiness or building state.

### 3.4 Storage (StorageBuilding)
- Large capacity.
- Acts as a destination for logistics delivery.
- Resources here are "Global" and can be reserved for construction.

## 4. Logistics & Task Management
### 4.1 Task Manager
- Maintains a queue of `ConstructionTasks` and `LogisticsTasks`.
- `ConstructionTask`: Requires builders to bring resources to a site and then "build".
- `LogisticsTask`: Moving goods from a producer to a warehouse.

### 4.2 Builder AI Logic
1. **Idle:** Query TaskManager for a job.
2. **Construction Job:**
   - If site needs resources: Find nearest `StorageBuilding` containing them.
   - Go to Storage -> Collect -> Go to Site -> Deposit.
   - Once resources are met: Perform "Build" action until 100%.
3. **Gathering Job:**
   - Go to Producer -> Collect Output -> Go to Storage -> Deposit.

## 5. Sequence Diagram: Construction
```text
Player -> PlacementSystem: Place Building
PlacementSystem -> TaskManager: Create ConstructionTask
TaskManager -> BuilderSystem: Assign Task to Builder
Builder -> StorageBuilding: Collect Resources
Builder -> ConstructionSite: Deliver Resources
Builder -> ConstructionSite: Perform "Build"
ConstructionSite -> Building: Update Progress
Building -> Player: Construction Complete
```

## 6. Class Relationships (Simplified)
```text
[ ScriptableObject ] <--- (describes) --- [ BuildingSettings ]
      |                                       |
      +--- (defines) --- [ ResourceType ]     | (instantiates)
                              |               V
[ Inventory ] <--- (manages) --- [ Building ] (Base)
      ^                           /      \
      |            [ ProducerBuilding ]  [ StorageBuilding ]
      |                   (IProducer)
      |
[ BuilderAI ] --- (moves to) ---> [ ConstructionSite ]
      |                                 |
      +--- (queries) ---> [ TaskManager ] <--- (populates) --- [ PlacementSystem ]
```

---
*Note: This architecture ensures a clear flow of goods and labor, essential for the "Tropico-style" simulation.*
