# Overview
This package provides you with the architecture game framework that helping you to develop your project following the best practices (like Domain Driven Design, Clean Architecture and Uber Ribs).

# Reference
This package contains classes that define the entire architecture of your game project and some other utilities.

## Assemblies
- CleanArchitectureGameFramework - the additional logic.
- CleanArchitectureGameFramework.Core - the main logic.
- CleanArchitectureGameFramework.Internal - utilities and helpers.

## Namespaces
- Framework - the root module.
    - IDependencyContainer - this interface allows you to resolve your dependencies.
    - Program - this class is responsible for the startup and global logic.
- Framework.UI - the presentation (user interface) module.
    - UIAudioTheme - this class is responsible for the audio theme.
    - UIScreen - this class is responsible for the user interface. The user interface consists of the hierarchy of logical (business) units and the hierarchy of visual units.
    - UIWidget - this class is responsible for the business logic of ui unit. This may contain (or not contain) the view.
    - UIView - this class is responsible for the visual (view) logic of ui unit. This just contains the VisualElement, so it's essentially a wrapper for VisualElement.
    - UIRouter - this class is responsible for the application state.
- Framework.App - the application module.
    - Application - This class is responsible for the application logic.
    - Globals - this class contains global values.
- Framework.Entities - the domain (entities) module.
    - Game - this class is responsible for the game rules and states.
    - Player - this class is responsible for the player rules and states.
    - Level - this class is responsible for the level rules and states.
    - World - this class is responsible for the world.
    - WorldView - this class is responsible for the world's visual and audible aspects.
    - Entity - this class is responsible for the scene's entity (player's character/avatar, NPC/bot or any other object).
    - EntityView - this class is responsible for the entity's visual and audible aspects.
    - EntityBody - this class is responsible for the entity's physical aspects.

# Media
- [![YouTube](https://img.youtube.com/vi/JQobAqfakJQ/0.jpg)](https://youtu.be/JQobAqfakJQ)

# Example
- https://denis535.github.io/#clean-architecture-game-template

# Links
- https://denis535.github.io
- https://assetstore.unity.com/publishers/90787
- https://denis535.itch.io/
- https://openupm.com/packages/?sort=downloads&q=denis535
- https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg
- https://github.com/Denis535/CleanArchitectureGameFramework/

# If you want to support me
If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos.
