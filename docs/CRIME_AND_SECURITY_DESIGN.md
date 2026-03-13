# Crime and Security System Design

## 1. Crime Mechanics
Crime represents the level of lawlessness on the island. It is calculated globally and influenced by local factors.

### 1.1 Global Crime Rate Formula
`GlobalCrimeRate = (UnemploymentRate * 0.5) + (PovertyFactor * 0.3) + CriminogenicBuildingsFactor - PoliceEffectiveness`

- **UnemploymentRate**: Percentage of citizens without a workplace (from `PopulationManager`).
- **PovertyFactor**: 100 - (Average Resident Happiness).
- **CriminogenicBuildingsFactor**: Sum of contributions from casinos, nightclubs, etc.
- **PoliceEffectiveness**: Total reduction provided by active Police Stations, Prisons, and Courthouses.

### 1.2 Local Crime Rate
Determined by the proximity of `PoliceStations`. Areas outside the `coverageRadius` of any police station suffer a local crime penalty to citizen satisfaction.

## 2. Security Buildings

### 2.1 Police Station (`PoliceStation`)
- **Radius**: Coverage area for crime reduction.
- **Effectiveness**: Base reduction value to the global crime rate.
- **Requirements**: Workers (Police Officers).

### 2.2 Prison (`Prison`)
- **Function**: Provides a flat reduction to the Global Crime Rate and prevents recidivism.
- **Side Effect**: Negative loyalty effect for "Environmentalists" or "Liberals" (Religious/Human Rights focus).

### 2.3 Courthouse (`Courthouse`)
- **Function**: Boosts the effectiveness of all Police Stations by 20%.

## 3. Impact of Crime

### 3.1 Citizen Satisfaction
- High crime increases `fearOfCrime` in `Citizen` agents.
- `Happiness` in `ResidentialBuilding` is penalized by `LocalCrimeRate`.
- Extremely high fear can trigger `Emigrate()` in `PopulationManager`.

### 3.2 Tourism
- `TouristManager` uses `1.0 - (GlobalCrimeRate / 100)` as the `safetyFactor`.
- High crime significantly reduces tourist arrivals and income.

### 3.3 Politics
- **Capitalists**: Demand low crime; loyalty drops if crime > 30.
- **Nationalists**: Demand strong police presence.

## 4. Decrees and Events
- **Decree: Martial Law**: Drastically reduces crime but kills Tourism and Legitimacy.
- **Event: Prison Break**: Temporarily spikes crime rate and damages Prison building.
- **Event: Bank Heist**: Deducts treasury balance based on current crime rate.
