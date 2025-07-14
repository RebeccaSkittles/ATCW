# Card Wars - Version 1.12.8 Release Notes

## 🎮 What's New

### UI Improvements & Screen Compatibility
- **NEW**: Added `UILogoStartScreenSizer.cs` - Dynamic logo scaling system that automatically adjusts logos to fit different screen sizes with a 10% buffer to eliminate black bars
- **NEW**: Added `UIFlashText.cs` - Text animation component for creating flashing effects with multiple animation types (Smooth, Pulse, Blink)
- **NEW**: Added `GameStartLoadStartScene.cs` - Scene transition manager with smooth fade effects for loading the start scene

### Enhanced Logo Display
- Fixed logo scaling issues in the AdventureTime scene where logos wouldn't properly scale to screen dimensions
- Logos now automatically scale to fill the screen while maintaining aspect ratio
- Eliminated black bars on the sides of logos during scene transitions

### Scene Management
- Improved scene loading with proper fade transitions
- Added support for additive scene loading with cleanup
- Enhanced user input handling for scene transitions (click/tap support)

### Technical Improvements
- Updated project settings for better compatibility
- Added new utility scripts for UI management
- Improved screen size adaptation across different resolutions

---

## 🐛 Known Issues

### Gameplay Issues
- **Land Card Placement**: Land cards need to be placed on the edge of the board to properly position on the very left and right slots. This is a known interaction issue that may require precise positioning.

### UI Compatibility
- **Screen Edge Compatibility**: UI elements may not perfectly align on all screen sizes, particularly around the edges where safe area views are involved. Some UI elements may appear cut off or improperly positioned on certain device aspect ratios.
- **Safe Area Support**: The game may not fully respect safe area boundaries on devices with notches or unusual screen configurations.

---

## 🔧 Technical Details

### New Scripts Added
- `UILogoStartScreenSizer.cs` - Handles dynamic logo resizing
- `UIFlashText.cs` - Provides text animation capabilities  
- `GameStartLoadStartScene.cs` - Manages scene transitions with fade effects

### Unity Version
- Built with Unity 2022.3.33f
- Compatible with Unity 2017.4.40f (legacy builds)

### Platform Support
- **Windows**: Fully supported ✅
- **Android**: Supported with minor UI edge issues ⚠️
- **macOS**: In development 🚧
- **iOS**: In development 🚧
- **Linux**: In development 🚧

---

## 📋 Changelog

### Added
- Dynamic logo scaling system for better screen compatibility
- Text flashing animation component with multiple effect types
- Scene transition system with fade effects
- Improved input handling for scene management
- Enhanced UI scaling utilities

### Fixed
- Logo display issues where images wouldn't fill screen properly
- Scene transition black bars and scaling problems
- Screen size adaptation for logos and backgrounds

### Changed
- Updated project settings for better Unity compatibility
- Improved scene loading workflow
- Enhanced UI responsiveness across different screen sizes

### Known Issues
- Land card placement requires edge positioning for leftmost/rightmost slots
- UI elements may not align perfectly on all screen configurations
- Safe area compatibility needs improvement for modern devices

---

## 🎯 Future Improvements

- Complete safe area support for modern devices
- Fix land card placement interaction
- Enhanced UI scaling for ultra-wide and unusual aspect ratios
- Improved touch input handling across all screen sizes
- Additional platform releases (iOS, macOS, Linux)

---

**Game Version**: 1.12.8  
**Build**: 1  
**Release Date**: July 2025  
**Unity Version**: 2022.3.33f
