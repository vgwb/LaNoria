# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed

- Public configuration fields are now private serializable.
- Non-static public fields that are referenced outside their containing classes have become properties.
- *HexFeatureCollection* has become a nested type inside *HexFeatureManager*.

### Fixed

- Units are no longer selectable in edit mode on startup, so no more unwanted pathfinding while editing.

## [2.0.0] - 2022-11-04

### Added

- Dependency on Universal RP package version 12.1.7 and created URP assets.
- URP package dependencies, Burst updated to version 1.8.1 and Mathematics to 1.2.6.
- Shader graphs that replace all terrain, water, and feature shaders.
- HLSL files to replace CGINC files, plus new files for shader graph custom function nodes.
- CHANGELOG file.
- Initial documentation for URP and materials.

### Changed

- Switched from Built-in RP to URP.
- Expanded README file.

### Removed

- Surface shaders for all terrain, water, and feature materials.
- All CGINC files.

### Fixed

- Set *Scale Factor* of *Save Load Menu* to 1, so it is no longer twice as large as the rest of the GUI.

## [1.0.0] - 2022-10-24

### Added

- Created Unity 2021 LTS project with Hex Map 27 unitypackage imported and input controls configured. 
