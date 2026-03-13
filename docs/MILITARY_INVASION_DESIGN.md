# Military and Invasion System Design

## 1. Military Mechanics
The military represents the island's ability to defend itself from external threats and maintain internal order.

### 1.1 Army Strength
`TotalMilitaryStrength = (Sum of Barracks Strength) * (TrainingLevel / 100) * (Readiness / 100)`

- **Barracks Strength**: Provided by active and staffed Barracks buildings.
- **TrainingLevel**: Increases over time if a Military Base is active.
- **Readiness**: Affected by funding and recent events.

### 1.2 Army Loyalty
Low loyalty can lead to a coup (existing system). High loyalty is required for effective defense.

## 2. Military Buildings

### 2.1 Barracks (`Barracks`)
- **Function**: Primary source of soldiers. Increases base `armyStrength`.
- **Requirement**: Staff (Soldiers).

### 2.2 Military Base (`MilitaryBase`)
- **Function**: Increases `trainingLevel` over time. Can host superpower troops via treaties.

### 2.3 Coastal Defense (`CoastalDefense`)
- **Function**: Specifically mitigates damage from sea-based invasions.

### 2.4 Airfield (`Airfield`)
- **Function**: Unlocks air defense and future air strike capabilities.

## 3. Invasions
Invasions are rare, high-stakes events triggered by hostile superpowers or regional instability.

### 3.1 Invasion Event (`Invasion`)
- **Force Strength**: The incoming army's power.
- **Duration**: Number of months the invasion lasts.
- **Resolution**:
  - **Military Victory**: If `TotalMilitaryStrength` > `InvasionForce`.
  - **Diplomatic Ceasefire**: Costly negotiation using treasury or alliances.
  - **Defeat**: Massive loss of Legitimacy, destruction of buildings, and reparations.

## 4. Integration
- **Politics**: Nationalists loyalty increases with military spending.
- **Economy**: High maintenance costs for all military structures.
- **Global Map**: Alliances with superpowers can prevent invasions via "Nuclear Umbrella" or direct intervention.
