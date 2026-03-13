# Caudillo Bay - Tutorial, Achievements, and Localization Design

## 1. Localization System
To support global audiences, *Caudillo Bay* uses a key-based localization system.

### 1.1 LocalizationManager
- **Method**: `GetText(string key)` returns the translated string for the current language.
- **Data**: JSON files stored in `Resources/Localization/`.
- **Component**: `LocalizedText` automatically updates `TMP_Text` components when the language changes.

## 2. Achievement System
Encourages replayability and rewards player milestones.

### 2.1 Achievement (ScriptableObject)
- **Fields**: ID, TitleKey, DescriptionKey, Icon, Type (e.g., `AccumulateWealth`), TargetValue.
- **AchievementManager**: Listens to game events (e.g., `OnBuildingPlaced`, `OnCoupSurvived`) and evaluates progress.
- **Persistence**: Unlocked achievement IDs are stored in the global save file.

## 3. Interactive Tutorial
Guides new leaders through the complexities of island management.

### 3.1 TutorialStep (ScriptableObject)
- **Content**: Title, Description, and an optional UI target to highlight.
- **Triggers**: Condition that activates the step (e.g., `FirstFarmBuilt`).
- **TutorialManager**: Manages the sequence of steps, handles UI masking/highlighting, and can be skipped by the player.

## 4. Input & UX Improvements
- **InputManager**: Centralizes all keyboard shortcuts (e.g., 'B' for Build Menu, 'M' for Map).
- **TooltipSystem**: Enhanced to pull localized strings and provide context-sensitive help for buildings and resources.
