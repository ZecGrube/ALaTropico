# Caudillo Bay - Task Backlog

## Core Mechanics

### M1: Island Generation
- **User Story:** As a player, I want to start a new game with a procedurally generated island so that every playthrough feels unique.
- **Tasks:**
  - [ ] Implement Perlin noise-based heightmap generation.
  - [ ] Generate resource deposits (gold, iron, coal, etc.) based on terrain type.
  - [ ] Create shorelines and initial foliage placement.
- **Acceptance Criteria:**
  - Island dimensions and resource richness can be configured.
  - All essential resources for early-game progression are reachable.

### M2: Building System
- **User Story:** As El Presidente, I want to place buildings and roads so that I can grow my nation.
- **Tasks:**
  - [ ] Implement a grid-based building placement system.
  - [ ] Create a road-building tool with pathfinding integration.
  - [ ] Implement building costs (money and resources).
- **Acceptance Criteria:**
  - Buildings cannot overlap.
  - Roads connect buildings to allow resource transport.

### M3: Resource & Production Chains
- **User Story:** As an economic strategist, I want to build production chains (e.g., Sugar -> Rum) so that I can export high-value goods.
- **Tasks:**
  - [ ] Implement a resource storage and transport system.
  - [ ] Create factory logic for consuming inputs and producing outputs.
  - [ ] Develop an export/import mechanic via the port.
- **Acceptance Criteria:**
  - Factories only operate if inputs are available and workers are present.
  - Exporting goods generates revenue in the national treasury.

## Buildings

### B1: Residential
- **Tasks:**
  - [ ] Implement Chabola (Hut), Casa de Madera, Bloque de Apartamentos.
  - [ ] Implement housing quality and capacity mechanics.

### B2: Agriculture & Industry
- **Tasks:**
  - [ ] Implement Banana/Sugar/Tobacco Plantations.
  - [ ] Implement Sawmill, Brick Factory, Rum Distillery.

### B3: Government & Services
- **Tasks:**
  - [ ] Implement Presidential Palace (starting building).
  - [ ] Implement Police Station, Clinic, Church.

## Politics & Global Relations

### P1: Factions System
- **User Story:** As a leader, I want to manage the loyalty of different factions (Capitalists, Communists, etc.) so that I don't get overthrown.
- **Tasks:**
  - [ ] Implement loyalty and influence metrics for each faction.
  - [ ] Create a "Decree" system where policies affect faction loyalty.
- **Acceptance Criteria:**
  - Faction leaders periodically issue demands.
  - Low loyalty leads to protests or coup attempts.

### P2: Global Map & Superpowers
- **User Story:** As a diplomat, I want to interact with the USA and USSR to gain financial and military aid.
- **Tasks:**
  - [ ] Implement the Global Map 2D UI.
  - [ ] Create superpower relationship tracking.
  - [ ] Implement mission-based interactions (e.g., "Export 100 Bananas to USA").

## Characters & Agents

### C1: Agent System (ECS)
- **User Story:** As a developer, I want to simulate thousands of citizens (workers, tourists) efficiently using ECS.
- **Tasks:**
  - [ ] Implement citizen pathfinding and job assignment.
  - [ ] Create unique attributes for agents (loyalty, wealth, happiness).

### C2: Bodyguards & Spies
- **Tasks:**
  - [ ] Implement unique "El Presidente Bodyguards" with special abilities.
  - [ ] Create a "Spy School" and global mission system for agents.

---
## Priority Roadmap (MVP)
1. **P0:** Basic Island Gen, Grid Building, Banana Export Chain, Simple Economy.
2. **P1:** Factions (Communist/Capitalist), Housing, Basic Infrastructure (Roads/Electricity).
3. **P2:** Global Map, Advanced Industry (Rum/Cigars), Tourism.
4. **P3:** Bodyguards, High-Tech Era, Nuclear Program.
