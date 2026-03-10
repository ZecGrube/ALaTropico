# Caudillo Bay - Data Schemas

## 1. Resource Definitions
Resources are the building blocks of the island's economy.

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "Resource",
  "type": "object",
  "properties": {
    "id": { "type": "string", "description": "Unique resource ID (e.g., 'sugar', 'bananas')" },
    "displayName": { "type": "string", "description": "Localized name" },
    "category": { "enum": ["agriculture", "forestry", "mineral", "intermediate", "final", "luxury", "strategic"] },
    "baseValue": { "type": "number", "description": "Base export value in local currency (e.g., pesos)" },
    "weight": { "type": "number", "description": "Transport weight/bulk factor" },
    "storageType": { "enum": ["dry", "liquid", "perishable", "bulk", "security"] }
  },
  "required": ["id", "displayName", "category", "baseValue"]
}
```

## 2. Production Chains (Factory Logic)
How inputs are converted into outputs in a building.

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "ProductionRecipe",
  "type": "object",
  "properties": {
    "id": { "type": "string" },
    "buildingId": { "type": "string", "description": "Building where this recipe is performed" },
    "inputs": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "resourceId": { "type": "string" },
          "quantity": { "type": "number" }
        }
      }
    },
    "outputs": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "resourceId": { "type": "string" },
          "quantity": { "type": "number" }
        }
      }
    },
    "cycleTime": { "type": "number", "description": "Time in game seconds to complete one production cycle" },
    "requiredWorkers": { "type": "integer" }
  },
  "required": ["id", "buildingId", "inputs", "outputs", "cycleTime"]
}
```

## 3. Building Definitions
Static properties of all structures.

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "Building",
  "type": "object",
  "properties": {
    "id": { "type": "string" },
    "category": { "enum": ["residential", "industrial", "agricultural", "government", "infrastructure", "service", "tourism", "unique"] },
    "cost": {
      "type": "object",
      "properties": {
        "money": { "type": "integer" },
        "resources": {
          "type": "array",
          "items": {
            "type": "object",
            "properties": {
              "resourceId": { "type": "string" },
              "quantity": { "type": "number" }
            }
          }
        }
      }
    },
    "era": { "type": "integer", "minimum": 1, "maximum": 4 },
    "footprint": {
      "type": "object",
      "properties": {
        "width": { "type": "integer" },
        "height": { "type": "integer" }
      }
    },
    "loyaltyEffects": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "factionId": { "type": "string" },
          "mod": { "type": "number" }
        }
      }
    }
  },
  "required": ["id", "category", "cost", "era", "footprint"]
}
```

---
*Note: These schemas will be used to generate C# data structures and JSON-based configuration files for the game engine.*
