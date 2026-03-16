# Corruption and Black Market Design

## 1. Corruption Mechanics
Corruption represents the diversion of state resources for personal or illicit gain. It is tracked globally and influenced by specific buildings and actions.

### 1.1 Global Corruption Rate Formula
`GlobalCorruptionRate = (CriminogenicBuildingsFactor * 2.0) + (ShadowBuildingsFactor) - AntiCorruptionEffort`

- **AntiCorruptionEffort**: Provided by `PoliceHQ` and `Courthouses`.
- **Impacts**:
  - Reduces tax efficiency (direct loss of treasury balance).
  - Increases construction costs.
  - Drops Legitimacy if discovered (scandals).

## 2. Shadow Economy

### 2.1 Black Market Money
A secondary, hidden currency used for:
- **Bribery**: Instant loyalty boost for factions.
- **Swiss Bank Account**: Personal score for the "Escape" victory condition.
- **Laundering**: Converting to official treasury (with a commission fee).

### 2.2 Smuggling
Selling resources via the Global Map without paying state taxes. Requires agents with high Stealth.

## 3. Shadow Buildings

### 3.1 Smuggling Den (`SmugglingDen`)
- **Function**: Unlocks smuggling missions on the Global Map.
- **Effect**: Generates a small amount of Black Market Money monthly.

### 3.2 Illegal Factory (`IllegalFactory`)
- **Function**: Produces goods without maintenance costs.
- **Side Effect**: High risk of scandal; increases corruption significantly.

### 3.3 Money Laundry (`MoneyLaundry`)
- **Function**: Converts Black Market Money to Official Treasury.
- **Efficiency**: 50-80% based on tech/upgrades.

### 3.4 Police HQ (`PoliceHQ`)
- **Function**: Major reduction to Corruption Rate.
- **Risk**: Can become "Double Agent" if army loyalty is low, increasing corruption instead.

## 4. Integration
- **Politics**: "Criminals" faction demands high corruption.
- **Events**: "Corruption Scandal" event triggers at >70% corruption, causing massive Legitimacy loss.
- **SaveSystem**: `globalCorruptionRate` and `blackMarketMoney` are persisted.
