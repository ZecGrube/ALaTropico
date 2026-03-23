# DIPLOMATIC VICTORY DESIGN

## 1. Overview
The Diplomatic Victory is an alternative win condition where the player achieves the status of a global leader through international cooperation, crisis management, and prestige, rather than pure military or economic dominance.

## 2. Key Metrics

### 2.1 Global Influence (0-1000)
A weighted score representing the nation's total weight on the world stage.
- **Economic (30%)**: GDP, trade balance, currency stability.
- **Military (20%)**: Army size, nuclear status (provides massive influence but prestige penalty).
- **Cultural (20%)**: Tourism levels, UNESCO sites, sports achievements.
- **Diplomatic (20%)**: Alliances, organization memberships, signed treaties.
- **Scientific (10%)**: Tech tier, space program progress.

### 2.2 International Prestige (0-100)
Represents how much other nations trust and respect the player's leadership.
- **Increases**: Solving global crises, hosting summits, humanitarian aid.
- **Decreases**: Human rights violations, breaking treaties, unprovoked wars, nuclear testing.
- **Effect**: High prestige grants extra votes in international organizations and easier alliance formation.

## 3. Global Crisis System
Periodic massive events that affect the whole world.
- **Crisis Types**:
  - *Economic Recession*: Global trade yields -50%.
  - *Pandemic*: High citizen mortality, closed borders.
  - *Climate Crisis*: Rapid sea-level rise, farm yield penalties.
  - *Nuclear Brinkmanship*: Rapid tension increase between superpowers.
- **Resolution**: Players can commit resources (funds, troops, tech) to solve the crisis. Success grants massive Influence and Prestige.

## 4. International Voting & Resolutions
Occurs periodically within organizations (e.g., UN, Regional Blocs).
- **Resolutions**:
  - *Global Trade Pact*: +10% export prices for all.
  - *Environmental Accord*: -20% pollution, but -10% industrial output.
  - *Sanctions*: Targeted economic penalties against a specific country.
  - *Peacekeeping*: Deployment of international troops to a conflict zone.
- **Voting Logic**: Total votes = (Influence * Prestige Multiplier) + Alliance support.

## 5. Global Summits
A high-level event hosted by the player.
- **Requirements**: High Prestige (>70), significant funds, and a peaceful state.
- **Outcome**: A "Summit Declaration" providing permanent global buffs and a permanent Influence boost.

## 6. Victory Conditions
To trigger a Diplomatic Victory, the player must:
1. Reach 800/1000 Global Influence.
2. Hold a "World Leader" title (voted in an international organization).
3. Have solved at least 3 Global Crises.
4. Maintain peace with all Superpowers for 10 consecutive years.

## 7. Integration
- **NuclearManager**: Nuclear status adds +150 Influence but -30 Prestige.
- **EconomyManager**: GDP growth scales the Economic Influence component.
- **CultureManager**: UNESCO sites provide passive Prestige and Cultural Influence.
- **MilitaryManager**: Army strength scales Military Influence.
