# Transport Revolution: Multimodal Logistics Design

## Overview
This system expands the logistics framework from simple truck-based deliveries to a multimodal network involving Trains, Ships, and Aircraft. It introduces high-capacity, specialized infrastructure and passenger transport mechanics.

## Vehicle Hierarchy
- **Vehicle.cs (Abstract Base)**: Contains state machine (Idle, Moving, Loading, etc.), cargo inventory, and pathfinding interface.
  - **Truck.cs**: Uses standard NavMesh (Roads). Moderate speed/capacity.
  - **Train.cs**: Moves strictly along `RailwaySegment` paths. High capacity, high speed, low flexibility.
  - **Ship.cs**: Uses Water NavMesh. Extremely high capacity, low speed.
  - **Plane.cs**: Linear interpolation between `Airports`. Highest speed, moderate capacity, high cost.

## Infrastructure
- **RailwaySegment**: Linear path data connecting `TrainStations`. Built using a node-to-node system.
- **TrainStation**: Hub for cargo and passenger loading. Requires proximity to railways.
- **Harbor**: Coastal building for maritime logistics. Extends island trade capacity.
- **Airport**: Hub for long-distance trade and high-tier tourism.

## Production Buildings
- **Train Depot**: Produces and maintains the train fleet.
- **Shipyard**: Constructs maritime vessels.
- **Hangar**: Houses and produces aircraft.

## Logistics Management (TransportManager)
- **Order Routing**: `LogisticsManager` now filters orders by distance and volume.
  - Heavy/Bulk (e.g., Coal, Iron) -> Preferred for Trains/Ships.
  - Urgent/Light (e.g., Medicine, Electronics) -> Preferred for Planes.
- **Multimodal Hubs**: Resources can be transferred between different vehicle types at stations/ports.

## Passenger Transport
- **Citizen Commuting**: Citizens check for public transport routes if their workplace is beyond walking distance.
- **Ticket Revenue**: Passenger vehicles generate income for the national treasury per trip.
- **Tourist Arrival**: `Airports` and `Harbors` increase the cap on maximum tourists and arrival rates.

## Persistence
- **Rail Networks**: Serialized as a list of connected nodes.
- **Vessel State**: Positions, fuel, and assigned routes/orders are persisted in `SaveSystem.json`.
