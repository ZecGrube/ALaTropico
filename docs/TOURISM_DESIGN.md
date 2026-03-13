# Caudillo Bay - Tourism System Design (Sprint 2.5)

## 1. Overview
Tourism is a late-game economic pillar. It relies on the island's beauty, safety, and specialized infrastructure to attract high-wealth visitors who spend money in local establishments.

## 2. Core Components

### 2.1 Tourist (Agent)
- **Type:** Temporary visitor entity.
- **Spending Power:** Determined by origin and current global events.
- **Satisfaction:** Influenced by building quality, safety, and pollution.

### 2.2 Tourism Statistics
- **Attractiveness:** Sum of all "Beauty" scores from buildings and nature.
- **Safety:** Inverse of crime/military presence (too much military scares tourists).
- **Service Quality:** Average level of tourism buildings.

### 2.3 Tourism Buildings
- **Hotel:** Increases maximum tourist capacity.
- **Restaurant:** Consumes Food, generates income per tourist.
- **Casino:** High income, high crime increase, consumes Luxury Goods.
- **Beach Club:** High attractiveness, requires clean water/shore.

## 3. Implementation Logic

### 3.1 TouristManager (Singleton)
- **Flow Calculation:** `BaseFlow * Attractiveness * SafetyFactor`.
- **Monthly Revenue:** `Sum of (TouristCount * BuildingEfficiency)`.
- **Faction Impact:**
  - Capitalists: +Loyalty (Profit).
  - Religious: -Loyalty (Moral decay from casinos).

### 3.2 Sequence: Tourist Arrival
1. `TouristManager` calculates available slots (Hotels).
2. New `Tourist` entities are simulated.
3. Tourists visit buildings, consuming services and resources.
4. Revenue is added to the treasury at the end of the month.

---
*Note: Tourism requires a balance between openness (to attract visitors) and control (to manage pollution and morality).*
