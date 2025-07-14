# DevTutorialSkipper Usage Guide

## Overview
The `DevTutorialSkipper` script is a development tool that allows you to skip tutorials in Card Wars for testing purposes. It can be attached to any GameObject in the scene (typically the EventSystem or a persistent GameObject).

## How It Works
- **When Enabled**: Marks all tutorials as completed in the player's save data, tricking the game into thinking you've finished all tutorials
- **When Disabled**: Restores the original tutorial completion state from backup
- **Backup System**: Automatically backs up your original tutorial progress before making changes

## Setup Instructions

### Method 1: Attach to EventSystem
1. Find the `EventSystem` GameObject in your scene hierarchy
2. Add the `DevTutorialSkipper` component to it
3. Configure the settings in the inspector

### Method 2: Create a Dedicated GameObject
1. Create a new empty GameObject in the scene
2. Name it "DevTutorialSkipper" or similar
3. Add the `DevTutorialSkipper` component to it
4. Configure the settings in the inspector

## Inspector Settings

### Tutorial Skip Settings
- **Skip Tutorials**: Main toggle - check this to enable tutorial skipping
- **Use Debug Flags**: Also sets the `DebugFlags.stopTutorial` flag when enabled

### Debug Info
- **Currently Skipping**: Read-only field showing current skip state

## Usage Methods

### In Inspector
Simply check/uncheck the "Skip Tutorials" checkbox in the inspector during runtime.

### Via Context Menu (Right-click in Inspector)
- **Toggle Tutorial Skip**: Switches between enabled/disabled
- **Enable Tutorial Skip**: Forces tutorial skipping on
- **Disable Tutorial Skip**: Forces tutorial skipping off
- **Debug: Print Tutorial Status**: Logs current status to console

### Via Code/Events
You can call these public methods from other scripts or UI buttons:
```csharp
DevTutorialSkipper skipper = GetComponent<DevTutorialSkipper>();

skipper.ToggleTutorialSkip();           // Toggle on/off
skipper.ForceEnableTutorialSkip();      // Force enable
skipper.ForceDisableTutorialSkip();     // Force disable
skipper.PrintTutorialStatus();          // Debug info
```

## Safety Features

### Automatic Backup
- The script automatically backs up your original tutorial progress
- This backup is used to restore your state when you disable skipping
- The backup is created the first time you enable tutorial skipping

### Non-Destructive
- Your original tutorial progress is preserved
- Disabling the script restores your exact original state
- No permanent changes are made to your save data

### Runtime-Only
- Changes are applied in real-time during gameplay
- No need to restart the game or reload scenes

## Console Logging
The script provides helpful console output:
- When tutorial skipping is enabled/disabled
- When backups are created
- Current tutorial status when debugging

## Example Use Cases

### Testing Late-Game Features
Enable tutorial skipping to immediately access late-game content without playing through tutorials.

### UI/UX Testing
Skip tutorials to test user interfaces that are normally locked behind tutorial completion.

### Bug Reproduction
Quickly get to specific game states for testing and debugging.

### Development Workflow
Toggle on when developing/testing, toggle off when you want to experience tutorials normally.

## Notes
- Changes take effect immediately when you toggle the setting
- The script works with the existing tutorial system without modifying core game files
- Safe to use in development builds
- Should be removed or disabled in production builds
