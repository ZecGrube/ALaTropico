# Dynamic Events & Global Expansion Design

## 1. Dynamic Event System

### GameEvent (ScriptableObject)
- **ID**: Unique identifier.
- **Title/Description**: Localized strings.
- **Weight**: Probability weight for random selection.
- **Conditions**: List of requirements (e.g., Faction loyalty < 30, Pollution > 50).
- **Choices**: List of player options for interactive events.
  - **Effects**: Immediate changes (Mandate, Legitimacy, Resources).
  - **Modifiers**: Temporary buffs/debuffs added to `ModifierManager`.

### EventManager
- Handles random monthly event checks.
- Manages the active event queue and UI triggers.
- Dispatches events to `EventNotificationUI`.

## 2. Global Expansion & Diplomacy

### International Organizations
- Groups of superpowers or regional blocs.
- Provide global bonuses if membership requirements are met.
- Can impose collective sanctions.

### Sanctions
- Global modifiers that affect trade prices or aid.
- Triggered by low relations with superpowers or high human rights violations (low Legitimacy).

### Alliances
- Formal agreements with Superpowers.
- Provides military protection (prevents coups/invasions) and unique building unlocks.
- Costs monthly Mandate or Resources.

## 3. Building Health & Repair
- **Current Health / Max Health**: Buildings can be damaged by events (hurricanes, riots).
- **TakeDamage(float amount)**: Reduces health. Zero health disables building functionality.
- **Repair()**: Consumes resources (Steel, Wood, Cash) to restore health.

## 4. Modifier System
- **ModifierManager**: Tracks temporary effects with expiration dates.
- **Modifiers**: Can affect production efficiency, citizen happiness, or resource costs.
- **Save/Load**: Persisted in `SaveSystem`.
