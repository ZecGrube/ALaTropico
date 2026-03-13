# Health and Epidemics System Design

## 1. Health Mechanics
Health represents the physical well-being of the island's population. It is tracked both as a global average and as an individual attribute for each citizen.

### 1.1 Global Average Health Level
`GlobalHealthLevel = Average(All Citizens' health)`

### 1.2 Health Factors
- **Medical Coverage**: Availability of Clinics and Hospitals.
- **Pollution**: High pollution levels penalize health.
- **Crime**: High crime rates cause stress and reduce health.
- **Nutrition**: Food availability (from existing systems).
- **Epidemics**: Random or sanitation-based events that rapidly deplete health.

## 2. Medical Buildings

### 2.1 Clinic (`Clinic`)
- **Function**: Basic healthcare, treats mild cases.
- **Effect**: Slows down health decay for citizens in range.
- **Requirement**: Doctors (qualified workers).

### 2.2 Hospital (`Hospital`)
- **Function**: Advanced healthcare, required for treating serious conditions and epidemics.
- **Effect**: Actively restores health for citizens; requires Medicine.

### 2.3 Pharmacy (`Pharmacy`)
- **Function**: Produces Medicine from chemical components.
- **Effect**: Medicine is consumed by Hospitals to increase treatment efficiency.

### 2.4 Medical Lab (`MedicalLab`)
- **Function**: Researches vaccines and improves global medical efficiency.
- **Effect**: Reduces Epidemic duration and lethality.

## 3. Epidemics
Epidemics are triggered by low global health or high population density without adequate sanitation.
- **Spread**: Increases health decay globally.
- **Mortality**: Citizens with health <= 0 are removed from the simulation (Die).
- **Management**: Requires quarantine decrees, vaccine research at Medical Labs, and high Hospital capacity.

## 4. Systemic Impact
- **Productivity**: Sick workers have reduced efficiency in `ProducerBuilding`.
- **Satisfaction**: Low health significantly penalizes citizen happiness.
- **Politics**: "Medical Professionals" faction demands high healthcare spending.

## 5. UI and Persistence
- **UI**: Added "Health" indicator to the HUD.
- **SaveSystem**: `globalHealthLevel` and active `Epidemic` states are serialized in `GameSaveData`.
