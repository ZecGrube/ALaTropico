# NUCLEAR WEAPONS DESIGN

## 1. Overview
Nuclear weapons represent the ultimate military power in Caudillo Bay. They provide a massive strategic advantage and deterrent but come with immense costs, risks, and diplomatic consequences.

## 2. Core Components

### 2.1 Resources
- **Uranium Ore**: Raw material mined from specialized deposits.
- **Enriched Uranium**: Produced from Uranium Ore in an Enrichment Plant. Required for nuclear warheads and reactors.
- **Plutonium**: Byproduct of nuclear reactors. Used for high-yield warheads.

### 2.2 Buildings
- **Uranium Mine**: Extracts raw uranium.
- **Enrichment Facility**: Processes Uranium Ore into Enriched Uranium.
- **Nuclear Reactor**: Generates massive power. Can be configured to produce Plutonium.
- **Nuclear Lab**: Dedicated research facility for nuclear technologies.
- **Missile Silo**: Stores and launches ICBMs.
- **Nuclear Test Site**: Required to conduct nuclear tests.
- **Radar Station**: Detects incoming strategic threats.

### 2.3 Units (Strategic)
- **ICBM (Intercontinental Ballistic Missile)**: High range, high damage, stationary (in Silo).
- **Nuclear Bomber**: Deployable aircraft from Airbases.
- **Ballistic Missile Submarine (SSBN)**: Stealthy maritime launch platform.

## 3. Systems

### 3.1 Nuclear Deterrence (MAD)
Calculated based on the player's nuclear triad (Silos, Bombers, Subs). High deterrence reduces the likelihood of direct attack by Superpowers but increases global tension.

### 3.2 Nuclear Tension
A global metric (0-100).
- **Increases**: Nuclear tests, building silos, deploying bombers, aggressive rhetoric.
- **Decreases**: Disarmament treaties, non-proliferation agreements, peaceful cooperation.
- **Thresholds**:
  - 50: Sanctions from non-allied nations.
  - 80: High risk of pre-emptive strike or Coup d'etat.
  - 100: DEFCON 1 - Global Nuclear War (Potential Game Over).

### 3.3 Nuclear Tests
- **Underground**: Low detection risk, low tension increase, moderate reliability gain.
- **Atmospheric**: High detection, high tension, high reliability gain, local radiation.
- **Underwater**: Moderate detection, moderate tension, risk of ecological disaster.

### 3.4 Radiation Zones
A map overlay that tracks radioactive contamination.
- **Effects**: Prevents building construction, kills citizens over time, spreads to adjacent tiles (slowly), reduces soil quality.
- **Decontamination**: Expensive process using specialized engineering units.

## 4. International Treaties
- **NPT (Non-Proliferation Treaty)**: Prevents nuclear weapon development in exchange for economic/tech aid.
- **START (Strategic Arms Reduction Treaty)**: Limits the number of warheads to reduce global tension.
- **CTBT (Comprehensive Test Ban Treaty)**: Bans nuclear testing.

## 5. Implementation Plan
1. Create `NuclearManager` to track global tension, treaties, and deterrence.
2. Implement `NuclearWarhead` and `DeliverySystem` logic.
3. Add `RadiationZone` system for post-strike effects.
4. Integrate with `EconomyManager`, `GlobalMapManager`, and `MilitaryManager`.
5. UI for Nuclear Program management.
