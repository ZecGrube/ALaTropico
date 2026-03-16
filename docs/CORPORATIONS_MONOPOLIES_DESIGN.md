# Industrial Monopolies and Corporations Design

## Overview
The Corporations system introduces a new layer of economic and political depth. It allows for the grouping of buildings into corporate entities that can be owned by the state, private individuals (oligarchs), or foreign powers. Corporations influence the economy through industrial scaling and the political landscape through lobbying and power struggles.

## Core Classes

### Corporation.cs
Represents an individual corporate entity.
- **Fields**:
  - `string corporationName`: Unique identifier.
  - `IndustryType industry`: Agriculture, Mining, Industry, Tourism, Transport.
  - `CorporationType type`: StateMonopoly, Private, Foreign.
  - `List<Building> ownedBuildings`: Assets managed by the corporation.
  - `float treasury`: Accumulated profit.
  - `int totalShares`: Total shares issued.
  - `float sharePrice`: Current valuation based on profit and assets.
  - `OwnershipStructure ownership`: Distribution between State, Private, Foreign.
  - `Dictionary<FactionType, float> lobbyingPower`: Influence per faction.
- **Methods**:
  - `CalculateMonthlyProfit()`: Aggregates income/expenses from `ownedBuildings`.
  - `PayDividends()`: Distributes profit to shareholders.
  - `IssueShares(int amount)`: Raises capital.
  - `BuyShares(int amount, Entity buyer)`: Transfers ownership.
  - `Lobby(Faction faction, float amount)`: Spends treasury to increase faction loyalty.

### CorporationManager.cs
Singleton managing the corporate ecosystem.
- **Responsibilities**:
  - Maintains `List<Corporation> corporations`.
  - Handles the creation/merging of corporations.
  - Orchestrates monthly financial updates.
  - Provides an interface for the UI to query corporate data.
- **Integration**:
  - Triggered by `EconomyManager` during the monthly cycle.
  - Interface with `FactionManager` for lobbying effects.

## Types of Corporations
1. **State Monopoly**: 100% state-owned. Profits go to the national treasury. Highly stable but may have efficiency penalties.
2. **Private Corporation**: Owned by local oligarchs. Pays taxes to the state. Increases the influence of the Capitalist faction. May lobby for favorable decrees or tax breaks.
3. **Foreign Corporation**: Branch of a superpower (USA/USSR). Provides immediate investment (cash/tech) but increases foreign influence and may lead to dependency.

## Political Impact
- **Lobbying**: Corporations can spend their own funds to influence political factions, making certain decrees easier to pass or keeping factions happy.
- **Oligarchy**: If private corporations become too wealthy compared to the state, "Oligarchs Demand Power" events trigger, potentially leading to a coup or forced privatization.
- **Corruption**: Managers can skim funds, leading to a "Shadow Economy" that interacts with the `CorruptionManager`.

## Integration with Existing Systems
- **Building.cs**: Add `Corporation ownerCorporation`.
- **EconomyManager.cs**: Redirect building profits to `ownerCorporation.treasury`. State takes a tax cut from Private/Foreign corporations.
- **FactionManager.cs**: Capitalists' loyalty is tied to the performance of Private corporations.
- **SaveSystem.cs**: Serialize all corporate data, including ownership and share prices.

## UI Requirements
- **Corporation Overview**: List of all corporations, their health, and profitability.
- **Stock Market**: Interface to buy/sell shares (for the player's personal Swiss bank account or the state).
- **Lobbying Panel**: Spend corporate funds to influence politics.
