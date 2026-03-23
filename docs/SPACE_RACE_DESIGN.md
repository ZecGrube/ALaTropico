# SPACE RACE DESIGN

## 1. Overview
The Space Program represents the pinnacle of technological achievement for Caudillo Bay. It allows the player to launch satellites, conduct crewed missions, and eventually colonize the Moon and Mars, providing massive global bonuses and prestige.

## 2. Infrastructure & Resources

### 2.1 Buildings
- **Spaceport**: The central hub for all space activities. Includes launch pads and mission control.
- **Rocket Factory**: Produces complex rocket components (Engines, Fuel Tanks, Payloads).
- **Space Tourism Center**: Generates high revenue by sending wealthy tourists on suborbital flights.
- **Lunar Logistics Hub**: Required for managing supply runs to the Moon.

### 2.2 Advanced Resources
- **Titanium**: High-strength metal for rocket structures.
- **Carbon Composite**: Lightweight material for advanced payloads.
- **Rocket Fuel**: Specialized chemical propellant.
- **Helium-3**: Rare isotope harvested from the Moon.

## 3. Mission System

### 3.1 Mission Types
- **Satellite Launch**: Deploys a functional satellite (Communication, Nav, Recon, Science, Weather, Military).
- **Crewed Orbit**: First step in human spaceflight. Massive prestige boost.
- **Lunar Landing**: Historic milestone. Unlocks Lunar Base projects.
- **Mars Expedition**: The final endgame goal.

### 3.2 Reliability & Risk
Every launch has a failure chance based on:
- Technology level.
- Quality of components.
- Investment in "Safety Protocols".
- Weather conditions.

## 4. Satellite Network
Satellites provide permanent global modifiers:
- **Communication**: +15% Logistics Efficiency, +10% Tourism.
- **Navigation**: +20% Transport Efficiency, -10% Fuel Consumption.
- **Reconnaissance**: Reveals hidden resources, +15% Espionage Success.
- **Science**: +20% Research Speed.
- **Weather**: -50% Damage from natural disasters (early warning).
- **Military**: +10% Defense Strength, detects stealth armies.

## 5. Space Race Milestones
The `SpaceRaceManager` tracks who reaches milestones first (Player vs USSR vs USA).
- **First Satellite**: +100 Influence, +20 Prestige.
- **First Human in Space**: +150 Influence, +30 Prestige.
- **Moon Landing**: +300 Influence, +50 Prestige.
- **Mars Colony**: Automatic Victory (Scientific/Prestige).

## 6. Moon & Mars Bases
Special projects that act as separate resource pools.
- **Lunar Base**: Produces Helium-3, requires monthly shipments of Water and Oxygen (Food/Tools).
- **Mars Colony**: Extremely expensive maintenance, but provides ultimate prestige.
