# Citizen Deep Simulation Design

## Overview
Transforming citizens from statistical data into individual agents with unique identities, social circles, and life goals. This system simulates a living society where micro-actions of thousands of agents drive the island's economy and political stability.

## Core Data Structures

### Citizen.cs (Expanded)
- **Identity**: `firstName`, `lastName`, `gender`, `age`, `birthDate`.
- **Personality Traits**: `aggressiveness`, `ambition`, `charisma`, `loyalty`, `greed`, `faith`, `curiosity`.
- **Socio-Economics**:
  - `socialClass`: Poor, Working, Middle, Rich.
  - `educationLevel`: None to Higher.
  - `personalWealth`: Liquid assets for consumption and savings.
- **Social**:
  - `Family family`: Reference to spouse and children.
  - `List<Citizen> friends`: Network for information and mood propagation.
- **Political**: `Dictionary<FactionType, float> politicalLeaning`.
- **Logic**:
  - `CalculateHappiness()`: Derived from needs, safety, wealth, and family.
  - `Consume()`: Logic for spending wealth at `CommercialBuildings`.

### Family.cs
- Manages groups of citizens living together.
- Handles inheritance of wealth and trait influence.

### Job.cs (Struct/Class)
- Links a citizen to a `ProducerBuilding`.
- Tracks `salary`, `jobTitle`, and `satisfaction`.

## Systems

### PopulationManager (Demographics & Migration)
- **Aging**: Monthly increment of age; death probability based on age/health.
- **Reproduction**: Birth logic for couples based on wealth and housing.
- **Migration**: Attractiveness-based inflow/outflow of agents.

### JobMarket.cs
- Matches vacancies in `ProducerBuildings` with unemployed citizens based on education requirements and location.

### Commercial System
- `CommercialBuilding.cs`: Sells goods (Food, Luxury, Services) to citizens.
- Generates revenue for the state via sales tax and profit transfer.

## Political & Economic Impact
- **Voting**: Individual citizens vote in elections based on political leaning and current happiness.
- **Consumption**: Citizen spending drives the internal economy, reducing reliance on exports.
- **Taxation**: Introduction of Personal Income Tax (PIT).

## Optimization
- **Batch Processing**: Citizens are divided into update groups (e.g., update 1/30th of population per day).
- **Spatial Grid**: For efficient local store/home searches.
- **Data Locality**: Using arrays of structs for personality and stats where possible to maximize cache hits.

## Persistence
- Use a flattened data structure in `SaveSystem.cs` to store citizen IDs, trait indices, and relationship links (spouse ID, child IDs) to ensure large populations are savable.
