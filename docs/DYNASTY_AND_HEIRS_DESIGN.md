# Dynasty and Heirs System Design

## 1. Dynasty Mechanics
The Dynasty system allows the legacy of El Presidente to continue through successive generations. It introduces character-driven politics and long-term planning.

### 1.1 DynastyManager
- **Singleton**: Manages the family tree and succession state.
- **Ruler State**: Tracks the current `Heir` acting as El Presidente.
- **Aging**: All family members age by 1 year every 12 in-game months.
- **Death**: Natural death or event-driven demise triggers the Succession sequence.

### 1.2 The Heir Class
A specialized agent with dynastic properties:
- **Skills**: Charisma, Intelligence, Military, Cruelty.
- **Faction Support**: A mapping of how much each political faction likes this specific heir.
- **Loyalty**: Relation between the heir and the current ruler (low loyalty increases coup/assassination risk).

## 2. Succession and Intrigue

### 2.1 Succession Sequence
When the ruler dies:
1. **Selection**: If multiple heirs exist, the one with the highest `Legitimacy` or specified in a "Successor Decree" is chosen.
2. **Conflict**: If heirs have similar support, a civil war or coup event may trigger.
3. **Transition**: Faction loyalties are partially reset based on the new ruler's traits.

### 2.2 Palace Intrigue
Events triggered by heir characteristics:
- **Assassination Plots**: Low loyalty + high Cruelty heirs may attempt to seize power.
- **Family Scandals**: Intelligence/Charisma checks determine if scandals are suppressed or leaked.

## 3. Integration
- **Politics**: FactionManager updates support for heirs monthly based on their alignment with faction goals.
- **Global Map**: Marriage missions can introduce heirs with traits aligned with superpowers (USA/USSR).
- **SaveSystem**: All heir stats and the family tree structure are persisted.
