# Caudillo Bay - Game Architecture (Hybrid Approach)

## 1. Overview
Caudillo Bay utilizes a hybrid architecture designed for both rapid iteration and high performance.

### Prototype Phase (Current)
The current foundational layer is implemented using **MonoBehaviours** for ease of development, debugging, and integration with high-level Unity systems like NavMesh and UI. This allows for quick prototyping of game mechanics such as the Political System and Economy.

### Scalability Strategy (Target)
As the simulation grows to handle thousands of individual agents (builders, tourists, cars), performance-critical logic is slated for migration to **Unity ECS (Entity Component System)** and **DOTS**.

- **Logic Separation:** Managers and data-heavy systems (e.g., `ProductionSystem`, `StatsManager`) are designed to be easily transitioned to ECS Systems.
- **Data Driven:** The use of ScriptableObjects and serializable data structures facilitates the eventual move to pure Component data.

## 2. System Design (Transitioning to Components)
Data is separated from logic. Key components include:

### 2.1 Citizen Components
- `Agent`: Core ID and state (loyalty, wealth, happiness).
- `Pathfinding`: Target destination and current path.
- `JobInfo`: Associated building ID and work shift.
- `HomeInfo`: Residential building ID.

### 2.2 Building Components
- `BuildingData`: ID, category, era.
- `ProductionState`: Current cycle progress, input/output buffer.
- `EmploymentState`: Current worker IDs and capacity.
- `StorageState`: Total resource capacity and current inventory.

### 2.3 Resource Components
- `ResourceInventory`: A buffer of key-value pairs (resource ID -> quantity) attached to buildings.
- `TransportRequest`: Used to signal that a building needs or has resources for transport.

## 3. Systems Architecture
Logic is organized into dedicated systems:

### 3.1 Economic Systems
- `ProductionSystem`: Updates production cycles in factories and plantations.
- `InventorySystem`: Manages resource flow between buildings.
- `TreasurySystem`: Tracks national funds and export/import revenue.

### 3.2 Political Systems
- `FactionLoyaltySystem`: Calculates global and per-agent loyalty based on current conditions and building proximity.
- `DecreeSystem`: Applies temporary or permanent modifiers to global game variables.

### 3.3 Agent Simulation
- `NavigationSystem`: Calculates paths for agents on the grid.
- `JobAssignmentSystem`: Assigns citizens to open positions based on distance and education.
- `SatisfactionSystem`: Updates citizen happiness based on housing, services, and local events.

## 4. Communication & Data Flow
- **Data-Driven Configuration:** Buildings and resources are defined in JSON files, which are parsed and turned into component data at runtime.
- **Event Bus:** Used for infrequent high-level events (e.g., "A coup has started!", "New Era reached").
- **UI Interaction:** A hybrid approach using traditional Unity UI (uGUI or UI Toolkit) that reads from ECS state via a bridge system.

---
*Note: This architecture aims for high modularity and scalability for PC and console targets.*
