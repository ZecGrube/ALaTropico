# Culture and Landmarks Design

## 1. Culture Mechanics
Culture represents the island's soft power, historical heritage, and entertainment value.

### 1.1 Culture Level
`CultureLevel = (Sum of Culture Buildings' output) / PopulationSize`

### 1.2 Impact
- **Tourism**: Directly increases island attractiveness.
- **Satisfaction**: High culture level boosts citizen happiness.
- **Research**: Indirectly boosts RP generation via "Intellectual Environment" modifier.

## 2. Cultural Buildings

### 2.1 Theatre (`Theatre`)
- **Effect**: Increases `CultureLevel` and tourist attractiveness. Favorite of the "Intelligentsia".

### 2.2 Stadium (`Stadium`)
- **Effect**: Massive boost to national pride and general happiness. Lowers crime rate in the surrounding area during events.

### 2.3 Museum (`Museum`)
- **Effect**: Generates Culture. Requires `Artifacts` resource to function at 100% efficiency.

### 2.4 Amusement Park (`AmusementPark`)
- **Effect**: Increases resident satisfaction (especially families) and generates profit.

### 2.5 Landmark (`Landmark`)
- **Effect**: Unique, expensive structures (e.g., Statue of El Presidente) that provide massive global bonuses to Loyalty or Tourism.

## 3. Cultural Resources and Events
- **Resource: Artifacts**: Collected via global map missions or produced by Museums (archaeology).
- **Event: National Festival**: Temporary boost to Culture and Tourism; costs treasury.

## 4. Integration
- **Economy**: High tourist revenue from cultural sites.
- **SaveSystem**: `globalCultureLevel` is persisted in `GameSaveData`.
