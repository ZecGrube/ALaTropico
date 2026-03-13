# Education and Skills System Design

## 1. Education Mechanics
Education represents the collective intellectual and technical capacity of the island's population. It is tracked both as a global average and as an individual attribute for each citizen.

### 1.1 Global Average Education Level
`GlobalEducationLevel = Average(All Citizens' educationLevel)`

### 1.2 Education Growth
Citizens increase their `educationLevel` when attending educational buildings.
- **Base Growth**: `growth = (SchoolBonus + HighSchoolBonus + UniversityBonus) / PopulationSize`

## 2. Educational Buildings

### 2.1 Elementary School (`School`)
- **Function**: Boosts the base education of the entire population, especially children/new spawns.
- **Effect**: +5% to Global Education Growth per school.

### 2.2 High School (`HighSchool`)
- **Function**: Required for reaching a mid-tier education level (up to 60%).
- **Effect**: +10% to Global Education Growth.

### 2.3 Vocational School (`VocationalSchool`)
- **Function**: Specifically targets industrial workers.
- **Effect**: +15% to production efficiency of factories.

### 2.4 University (`University`)
- **Function**: Required for top-tier education (up to 100%) and critical for research.
- **Effect**: +20% to Global Education Growth; boosts `TechnologyManager` RP generation.

## 3. Systemic Impact of Education

### 3.1 Production Efficiency (`ProducerBuilding`)
`EfficiencyMultiplier = 1.0 + (GlobalEducationLevel / 200)`
- At 100% education, production efficiency is boosted by 50%.

### 3.2 Research Speed (`TechnologyManager`)
`ResearchPointRate = BaseRate * (1.0 + (GlobalEducationLevel / 100))`
- High education doubles the base research speed.

### 3.3 Crime Reduction (`CrimeManager`)
`EducationCrimeFactor = - (GlobalEducationLevel * 0.2)`
- High education provides a flat reduction to the global crime rate.

### 3.4 Citizen Satisfaction
- Education increases expectations. Higher education levels lead to higher needs for healthcare and luxury items.

## 4. UI and Persistence
- **UI**: Added "Education" indicator to the HUD.
- **SaveSystem**: `globalEducationLevel` is serialized in `GameSaveData`.
