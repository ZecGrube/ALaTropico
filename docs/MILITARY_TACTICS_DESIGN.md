# Military Tactics and Mass Battles Design

## Overview
This system replaces the abstract "Military Strength" with a detailed tactical simulation. It introduces specific unit types (Infantry, Cavalry, Artillery), formations, generals, and a battle resolution engine.

## Core Components

### 1. UnitType (ScriptableObject)
Defines the base statistics for a specific type of soldier.
- `string unitId`
- `float baseAttack`, `float baseDefense`, `float baseSpeed`
- `float baseUpkeep`
- `ResourceCost recruitmentCost`

### 2. MilitaryUnit (Class)
An instance of a unit within an army.
- `UnitType type`
- `float health`, `float morale`, `float experience`
- `void BattleTick()`: Updates unit status during combat.

### 3. Army (Class)
A collection of units under a single command.
- `string armyName`
- `List<MilitaryUnit> units`
- `Formation currentFormation`
- `General commander`
- `float supplyLevel` (Ammunition and Rations)
- `Vector2 gridPosition` (on the Global Map)

### 4. General (Class)
A specialized leader agent.
- `float leadership`: Boosts morale and cohesion.
- `float tacticalSkill`: Enhances specific formation effectiveness.
- `float loyalty`: Risk factor for military coups.

### 5. Formations (Enum)
- **Line**: Balanced, high firepower.
- **Column**: Fast movement, vulnerable to flanking.
- **Square**: Anti-cavalry defense, very slow.
- **Skirmish**: Reduced damage from artillery, lower overall defense.

## Battle Resolution Algorithm
1. **Engagement**: Triggered when two armies overlap.
2. **Phase Calculation**:
   - `AttackPower = Σ(Unit.Attack * FormationModifier * SupplyFactor * GeneralBonus)`
   - `DefensePower = Σ(Unit.Defense * TerrainModifier * CohesionFactor)`
3. **Loss Calculation**: Losses are distributed across units based on the power differential.
4. **Morale Check**: If morale falls below a threshold, the army retreats or routs.

## Infrastructure
- **Barracks**: Trains Infantry units.
- **Stable**: Trains Cavalry units.
- **Artillery Foundry**: Produces Artillery.
- **Fort**: Provides significant defense bonuses to garrisoned armies.
- **Military Academy**: Trains and levels up Generals.

## Integration
- **Economy**: Constant upkeep cost and supply production (Ammunition/Rations).
- **Logistics**: Supplies must be transported to armies in the field.
- **Politics**: Army loyalty directly impacts the probability and success of coups.

## Persistence
- Saves individual army compositions, unit experience levels, and general stats.
