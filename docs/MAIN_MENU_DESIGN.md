# Caudillo Bay - Main Menu & Navigation Design

## 1. Overview
The Main Menu serves as the entry point for the player, providing access to new games, saved progress, global settings, and exiting the application. The system is designed to be modular, allowing for future expansions like "Campaign" or "Online" modes.

## 2. Scene Structure
- **MainMenu**: The primary scene containing the UI canvas for all menu operations.
- **GameScene**: The main gameplay scene (simulated in the prototype).

## 3. Key Classes

### 3.1 GameStateManager (Core)
A singleton manager responsible for cross-scene state and loading.
- `LoadScene(string sceneName)`: Asynchronous loading with a simulated loading screen.
- `StartNewGame()`: Resets relevant managers and loads `GameScene`.
- `QuitGame()`: Handles application exit logic.

### 3.2 MainMenuUI (UI)
The top-level controller for the Main Menu scene.
- Handles "New Game", "Load", "Options", and "Exit" button events.
- Manages the visibility of sub-panels (OptionsPanel, LoadPanel).

### 3.3 OptionsPanel (UI)
Manages persistent player settings.
- **Graphics**: Resolution, Fullscreen toggle.
- **Audio**: Master, Music, and SFX volume sliders.
- Uses `PlayerPrefs` for immediate persistence of user preferences.

### 3.4 LoadPanel (UI)
Interfaces with `SaveSystem` to display and load saved games.
- Lists files from `Application.persistentDataPath`.
- Provides "Load", "Delete", and "Back" functionality.

## 4. UI Hierarchy
```
Canvas
└── MainMenuPanel
    ├── Title
    ├── NewGameButton
    ├── LoadButton
    ├── OptionsButton
    └── ExitButton
└── OptionsPanel (Hidden)
    ├── ResolutionDropdown
    ├── FullscreenToggle
    ├── VolumeSliders
    └── BackButton
└── LoadPanel (Hidden)
    ├── SaveFileScrollList
    ├── LoadButton
    └── BackButton
└── LoadingOverlay (Hidden)
    └── ProgressBar
```

## 5. Sequence: Starting a New Game
1. User clicks `NewGameButton`.
2. `MainMenuUI` calls `GameStateManager.Instance.StartNewGame()`.
3. `GameStateManager` triggers `LoadingOverlay`.
4. `GameStateManager` calls `SceneManager.LoadSceneAsync("GameScene")`.
5. Upon completion, `LoadingOverlay` is hidden and game initialization begins.
