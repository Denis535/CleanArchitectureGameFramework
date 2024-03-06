# Overview
This package provides you with the architecture game framework that helping you to develop your project following the best practices (like Clean Architecture and Uber Ribs).

# Reference
## Framework
- **IDependencyContainer** -
This interface allows you to resolve your dependencies.
- **Program** -
This class is responsible for the startup and global logic.

## Framework.UI
- **UIAudioTheme** -
This class is responsible for the audio theme.
- **UIScreen** -
This class is responsible for the user interface.
The user interface consists of the hierarchy of logical (business) units and the hierarchy of visual units.
- **UIWidget** -
This class is responsible for the business logic of ui unit.
This may contain (or not contain) the view.
- **UIView** -
This class is responsible for the visual (view) logic of ui unit.
This just contains the VisualElement, so it's essentially a wrapper for VisualElement.
- **UIRouter** -
This class is responsible for the application state.

## Framework.App
- **Application** -
This class is responsible for the application logic.
- **Globals** -
This class provides you with the global values.

## Framework.Entities
- **Game** -
This class is responsible for the game rules and states.
- **Player** -
This class is responsible for the player rules and states.
- **World** -
This class is responsible for the world.
- **WorldView** -
This class is responsible for the world's visual and audible aspects.
- **Entity** -
This class is responsible for the scene's entity (player's avatar, AI agent or any other object).
- **EntityView** -
This class is responsible for the entity's visual and audible aspects.
- **EntityBody** -
This class is responsible for the entity's physical aspects.

# Media
- [![YouTube](https://img.youtube.com/vi/JQobAqfakJQ/0.jpg)](https://youtu.be/JQobAqfakJQ)

# Example
- https://denis535.github.io/#clean-architecture-game-template

# Links
- https://denis535.github.io
- https://openupm.com/packages/?sort=downloads&q=denis535
- https://assetstore.unity.com/publishers/90787
- https://github.com/Denis535/CleanArchitectureGameFramework/tree/master/Packages/com.denis535.clean-architecture-game-framework
