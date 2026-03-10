# Caudillo Bay - Technical Q&A and Ask Phase

This document provides detailed answers to key technical questions for the development of «Caudillo Bay».

---

## 1. Tile System Implementation: Grid/Tilemap vs. Custom System
**Question:** How to correctly implement a tile system with building placement? Should I use Unity's `Grid` and `Tilemap` or create a custom grid?

**Answer:**
For «Caudillo Bay», a **hybrid custom grid system** is recommended over Unity's built-in `Tilemap`.
- **Custom Grid (Recommended):** Use a `NativeArray` or a 2D array of `TileData` structs. This is highly performant with **Unity ECS** and allows you to store custom simulation data (like soil fertility, faction influence, or pollution) directly on each tile.
- **Unity Tilemap:** Best for visual 2D games. While it handles visuals well, it can be harder to integrate with ECS and complex 3D placement logic.
- **Hybrid Approach:** Use a custom data structure for logic and a `MeshRenderer` or `Graphics.RenderMeshIndirect` for visuals to achieve the "thousands of tiles" performance.

---

## 2. Runtime NavMesh Baking
**Question:** How to bake NavMesh over a procedurally generated landscape at runtime?

**Answer:**
1. Use the **NavMeshComponents** package (specifically `NavMeshSurface`).
2. After `IslandGenerator` finishes creating the terrain:
   - Add a `NavMeshSurface` component to the island parent object.
   - Set the `Collect Objects` property to `Children` or `Current Object Hierarchy`.
   - Call `navMeshSurface.BuildNavMesh()` in your code.
3. For dynamic building placement, use `NavMeshModifier` or update the NavMesh periodically. However, for ECS-based agents, consider using **Unity Physics** or a custom **A* Pathfinding** for massive agent counts.

---

## 3. AI Builder Patterns for Crowding
**Question:** What patterns exist for AI builders so they don't interfere with each other?

**Answer:**
- **Steering Behaviors:** Use separation forces (RVO - Reciprocal Velocity Obstacles) so agents push away from each other while moving.
- **Slot/Reservation System:** Buildings and resource nodes should have "Interaction Slots" (e.g., only 4 builders can work on a 2x2 building at once). Builders "reserve" a slot before moving.
- **Staggered Task Selection:** When a building is placed, don't assign all builders at once. Space out task assignments to prevent a "conga line" effect.
- **Flow Fields:** For thousands of agents going to the same target, Flow Fields are more efficient than individual A* paths.

---

## 4. Building Data Storage
**Question:** How to organize building data (prefabs, costs, resources) in Unity?

**Answer:**
- **ScriptableObjects (Best for Designers):** Create a `BuildingSettings` ScriptableObject. It allows easy editing in the Inspector and provides a strong reference to prefabs.
- **JSON (Best for Modding/ECS):** For a data-oriented approach, store settings in JSON files. At runtime, parse these into ECS components.
- **Recommendation:** Use **ScriptableObjects** for primary development to leverage the Unity Editor, but ensure your code can serialize/deserialize to a flat data format for ECS.

---

## 5. Builder Inventory and Resource Gathering
**Question:** How to implement a simple builder inventory and resource collection?

**Answer:**
1. **Component-Based Inventory:** Add a `LocalInventory` component to the builder entity (a simple struct with `resourceId` and `amount`).
2. **Harvesting Logic:**
   - Builder reaches `ResourceNode`.
   - Every `tick`, the `GatheringSystem` reduces `Amount` in the node and increases `Amount` in the builder's `LocalInventory`.
3. **Delivery Logic:**
   - When `LocalInventory` is full, set target to the nearest `StorageBuilding`.
   - Upon arrival, subtract from `LocalInventory` and add to a global `Treasury` or building-specific `Inventory`.

---
*End of Ask Phase Document.*
