# Caudillo Bay - Beta Test Plan

## 1. Core Mechanics
### 1.1 Economy
- [ ] Verify resource production chains (Sugar -> Rum).
- [ ] Test building construction and resource consumption.
- [ ] Check unemployment calculations in `StatsManager`.

### 1.2 Politics
- [ ] Verify Faction Loyalty changes based on building placement.
- [ ] Test Election cycle (every 4-5 years).
- [ ] Trigger Coup event and verify defense mechanics (Barracks).
- [ ] Sign Decrees and check their immediate effects on Legitimacy.

### 1.3 Tech Tree
- [ ] Research "Industrialization" and check building unlocks.
- [ ] Verify Science point generation from University.

## 2. Advanced Systems
### 2.1 Tourism
- [ ] Build a Hotel and verify tourist arrival rate.
- [ ] Check Tourism Attractiveness impact on revenue.

### 2.2 Pollution
- [ ] Verify Factory pollution output.
- [ ] Check impact of high pollution on "Green" faction loyalty.

## 3. Global Map
### 3.1 Missions
- [ ] Send Agent on "Fruit Shield" mission.
- [ ] Verify mission success/failure outcomes.
- [ ] Check Superpower relations update.

### 3.2 Bodyguards
- [ ] Trigger personal missions for "El Fantasma".
- [ ] Verify skill upgrades upon completion.

## 4. Technical
### 4.1 Save/Load
- [x] Save game, quit, and load back. Verify all resources and faction stats. (Note: Resources/Research currently missing)
- [ ] Test loading an older version save (if applicable).

### 4.2 Performance
- [ ] Test FPS stability with 100+ agents and 50+ buildings.
- [x] Profile monthly calculation spikes. (Confirmed spike during MonthlyTick)
