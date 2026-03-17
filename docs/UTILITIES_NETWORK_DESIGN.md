# Engineering Infrastructure: Utilities Network Design

## Overview
This document outlines the architecture for the Power Grid and Water Network systems in «Caudillo Bay». These systems manage the generation, transmission, and consumption of essential utilities required for building functionality and citizen satisfaction.

## Core Concepts
- **UtilityNode**: A base class for any object that interacts with the utility networks.
- **UtilityConnection**: A physical link (Power Line or Water Pipe) between two UtilityNodes.
- **UtilityNetwork**: A graph structure representing a connected set of nodes and edges.
- **Load Balancing**: The process of distributing resource production across all connected consumers, accounting for transmission losses.

## Class Diagram

### 1. UtilityNode (Abstract)
Base component for buildings and poles.
- `NodeType type` (Producer, Consumer, Storage, Junction)
- `float capacity` (Max production or storage)
- `float currentLoad` (Current usage/output)
- `List<UtilityConnection> connections`
- `bool IsPowered()` / `bool HasWater()`

### 2. UtilityConnection
Represents a Power Line or Water Pipe.
- `UtilityNode nodeA, nodeB`
- `ConnectionType type` (Power, Water)
- `float maxThroughput`
- `float length`
- `float efficiency` (Loss per unit length)

### 3. PowerGridManager / WaterNetworkManager (Singletons)
Responsible for:
- Mapping nodes into independent sub-networks.
- Calculating total supply vs demand per network.
- Distributing utilities and flagging nodes as "Underpowered" or "Water Shortage".
- Handling graph changes (adding/removing lines).

## Algorithms

### Network Identification
Uses a Breadth-First Search (BFS) to group connected `UtilityNodes` into discrete `UtilityNetwork` objects.

### Resource Distribution
1. Sum all `Producer` capacity in the network.
2. Sum all `Consumer` demand in the network.
3. Apply transmission losses based on connection lengths.
4. If `Supply >= Demand`, all consumers are satisfied.
5. If `Supply < Demand`, priority-based distribution or brownout logic is applied.

## Infrastructure Buildings

### Power
- **PowerPlant**: Primary producer.
- **PowerPole**: Passive junction to extend range.
- **TransformerStation**: High-throughput node for large industrial clusters.
- **BatteryStorage**: Stores excess production for peak demand or emergencies.

### Water
- **WaterPumpingStation**: Extracts water from sources.
- **WaterTower**: Gravity-fed storage that maintains pressure.
- **WaterPipe**: Linear connection between nodes.
- **WaterTreatmentPlant**: Improves water quality, reducing health risks.

## Integration
- **Buildings**: `RequiresPower` and `RequiresWater` flags. Functional efficiency drops to 0% if requirements are not met.
- **Citizens**: Satisfaction penalty for "Lack of Utilities" in residential zones. Increased disease risk without clean water.
- **Economy**: Maintenance costs for every meter of pipe and cable.

## Performance Optimization
- Recalculate network logic only when the graph topology changes (building built/destroyed, line added/removed).
- Cache network memberships.
- Use simplified line rendering for thousands of connections.
