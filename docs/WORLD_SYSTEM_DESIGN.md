# Caudillo Bay - World System Design

## 1. Map Data Structure: Tile-Based Grid (Hybrid approach)

For «Caudillo Bay», we will use a **Tile-Based Grid (1x1 units)**.

### 1.1 Decision: Tilemap over Mesh Terrain
- **Performance (Unity ECS):** A grid-based structure is easier to represent in ECS. Each tile can be an entity or part of a dense array in a singleton component.
- **Building Placement:** Grid-based placement (e.g., a building takes 2x2 or 3x3 tiles) is simpler to implement and more intuitive for city-builders.
- **Modification:** Changing a tile (e.g., adding a road, clearing forest) is faster than re-generating a mesh or updating a collider.
- **Hybrid Visuals:** While the underlying data is a 1x1 grid, the visual representation can still be a smooth mesh for the landscape (using a custom shader or a specialized tool like MicroSplat) to maintain the "tropical paradise" aesthetic.

### 1.2 Coordinate System
- **Global Origin (0,0,0):** The center of the primary island.
- **Tile Scaling:** 1 Unity Unit = 1 Tile.
- **Grid Layout:** A 2D array or a `NativeArray` for ECS, representing the entire playable area.

## 2. Island Generator Architecture

The `IslandGenerator` is a high-level system (likely a `SystemBase` in ECS) responsible for initializing the world state.

### 2.1 Coordinate System & Grid
- **Scale:** 1.0f units = 1 tile.
- **Orientation:** XZ plane (X = North/South, Z = East/West).
- **Coordinate Conversion:**
  - `WorldToGrid(float3 position) -> int2 gridPos`
  - `GridToWorld(int2 gridPos) -> float3 position`

### 2.2 Component Data
- `TileData`: A component for each tile containing:
  - `TerrainType` (Deep Water, Shallow Water, Sand, Grass, Forest, Mountain).
  - `Height` (float).
  - `IsOccupied` (bool).
  - `OccupierEntity` (Entity).
- `IslandSettings`: Global configuration (Seed, SizeX, SizeZ, NoiseScale).

### 2.3 Generator Methods (Pseudo-code)
- `GenerateHeightMap(IslandSettings settings)`: Calculates a `NativeArray<float>` for the grid.
- `ApplyThresholds(NativeArray<float> heights)`: Maps heights to `TerrainType`.
- `DistributeResources(TerrainType target, float density)`: Spawns `ResourceNode` entities in the designated biome.
- `IdentifyStartZone()`: Searches for a 10x10 flat area of `TerrainType.Grass` near a `TerrainType.Sand` tile.

## 3. Builder AI & Interaction

Builders are the primary agents of the economy. They are implemented as ECS entities with a behavior state machine managed by the `BuilderSystem`.

### 3.1 State Machine
- **IDLE:**
  - *Action:* Query `GlobalTaskQueue` for the nearest available task.
  - *Transition:* If task found, set target and move to `MOVE_TO_TARGET`.
- **MOVE_TO_TARGET:**
  - *Action:* Request path from `NavigationSystem`. Update `Translation` based on `MovementSpeed`.
  - *Transition:* Upon reaching `GridPosition` of target, switch to `GATHER` or `BUILD` based on task type.
- **GATHER:**
  - *Action:* Call `IResourceNode.Harvest(rate)` periodically. Fill `LocalInventory`.
  - *Transition:* If `LocalInventory` is full or `IResourceNode.IsExhausted`, switch to `MOVE_TO_TARGET` (Target: `StorageBuilding`).
- **BUILD:**
  - *Action:* Call `IBuildable.AddProgress(rate)` periodically.
  - *Transition:* If `IBuildable.IsConstructed`, switch to `IDLE`.
- **DELIVER:**
  - *Action:* Transfer `LocalInventory` to `StorageBuilding`.
  - *Transition:* Switch to `IDLE` or back to `MOVE_TO_TARGET` if gathering task is incomplete.

### 3.2 Class/System Diagram (Text-based)
```text
[ GlobalTaskQueue ] <--- (reads) --- [ BuilderSystem (ECS) ]
        ^                                     |
        |                                     | (Interacts with)
        +--- (populates) --- [ ProductionSystem ] | [ ConstructionSystem ]
                                     |        |          |
                                     V        V          V
                              [ IResourceNode ] [ IBuildable ] [ IPlaceable ]
                                     |                |               |
                               (Tree/Rock)       (House/Factory)   (Road/Warehouse)
```

### 3.3 Integration with Navigation
The `NavigationSystem` maintains a `NavGrid` updated by `BuildingSystem` whenever an `IPlaceable` is added or removed. Builders use A* or simple Flow-fields (optimized for ECS) to traverse the grid.

## 4. Technical Implementation Specification

### 4.1 IslandGenerator System
```csharp
public class IslandGenerator : SystemBase {
    // 1. Setup IslandSettings (Seed, Size)
    // 2. Parallel Job: Generate Perlin Heightmap
    // 3. Parallel Job: Instantiate Tile Entities with TileData
    // 4. Distribution Job: Cluster ResourceNodes (NativeArray + Random)
}
```

### 4.2 BuilderSystem (State Update)
```csharp
public partial struct BuilderJob : IJobEntity {
    public float DeltaTime;
    public void Execute(ref BuilderState state, ref LocalInventory inventory, ref Translation translation) {
        switch (state.Current) {
            case State.IDLE: // Query GlobalTaskQueue (Singleton)
                break;
            case State.MOVE_TO_TARGET: // Lerp Translation to GridPosition(state.Target)
                break;
            case State.GATHER: // If timer > 1s, call node.Harvest(rate)
                break;
            // ... other states
        }
    }
}
```

---
*Note: This design prioritizes scalability for large islands and high agent counts.*
