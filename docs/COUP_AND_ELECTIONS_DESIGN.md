# Caudillo Bay - Elections, Coups, and Regime Crises Design (Sprint 2.4)

## 1. Overview
This system introduces the periodic challenge of maintaining democratic (or pseudo-democratic) legitimacy through elections and surviving violent power struggles (coups) when legitimacy or loyalty fails.

## 2. Core Concepts

### 2.1 Legitimacy (0-100)
- **Definition:** The perceived right of El Presidente to rule.
- **Modifiers:**
  - High loyalty of factions (+).
  - Winning fair elections (+).
  - Successful economic growth (+).
  - Fraudulent elections (-).
  - Unmet promises (-).
  - High repression (-).
- **Effects:** High legitimacy reduces coup risk and increases tax efficiency. Low legitimacy (>30) triggers coup checks.

### 2.2 Elections
- **Cycle:** Occurs every 48-60 months (4-5 years).
- **Campaign Phase:**
  - **Spending:** Allocate treasury funds to boost specific faction support.
  - **Promises:** Select 1-3 decrees to enact within the next term.
- **Results:** Calculated based on `(Faction Loyalty * Support Base) + Campaign Boost`.
- **Fraud:** Option to "Adjust" results. Boosts win chance but heavily reduces Legitimacy and foreign relations.

### 2.3 Military System (Minimal)
- **Military Strength:** Total "power" calculated from the number of `Barracks` and technology modifiers.
- **Army Loyalty:** Influenced by the Nationalists faction and military spending.
- **Function:** Primary defense against coups and requirement for "Forceful Suppression".

### 2.4 Coups (Violent Crises)
- **Triggers:**
  - Legitimacy < 30.
  - At least one faction has Loyalty < 20.
  - Foreign superpower support (optional).
- **Phases:**
  1. **Outbreak:** Warning message identifying the instigator.
  2. **Player Decision:**
     - *Negotiate:* Sacrifice resources/decrees to stop it.
     - *Suppress:* Military vs Putschists strength check.
     - *Flee:* Game Over (Swiss account victory if funds exist).
  3. **Resolution:** Success (retained power, loss of loyalty/infrastructure) or Failure (Game Over).

## 3. Technical Architecture

### 3.1 Class Diagram
```text
[ ElectionManager ]
      |-- nextElectionDate
      |-- currentPromises
      |-- HoldElection()
      |-- ApplyFraud()

[ LegitimacySystem ]
      |-- currentValue
      |-- Modify(delta)

[ MilitaryManager ]
      |-- totalStrength
      |-- armyLoyalty
      |-- SuppressionCheck()

[ CoupManager ]
      |-- activeCoup
      |-- CheckConditions()
      |-- StartCoup()
      |-- Resolve(choice)
```

### 3.2 Sequence: Election Execution
1. `ElectionManager` triggers 1-month warning.
2. Player enters "Campaign UI" to set promises/spending.
3. Election day: Manager aggregates `FactionData` and calculates vote share.
4. Results displayed. Loyalty and Legitimacy updated.

---
*Note: This sprint adds the ultimate "Game Over" conditions and the cyclical pressure of political survival.*
