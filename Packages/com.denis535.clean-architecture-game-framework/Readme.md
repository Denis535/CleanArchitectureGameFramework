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
    - IDependencyContainer - this allows you to resolve your dependencies.
    - Program              - this is responsible for the startup and global logic.
- Framework.UI - the presentation (user interface) module.
    - UIAudioTheme - the audio theme.
    - UIScreen     - the user interface. The user interface consists of the hierarchy of business units and the hierarchy of visual units.
    - UIWidget     - the business unit of ui. This may contain (or not contain) the view.
    - UIView       - the visual unit of ui. This just contains the VisualElement, so it's essentially a wrapper for VisualElement.
    - UIRouter     - the state router.
- Framework.App - the application module.
    - Application - The application.
    - Globals     - this class contains global values.
- Framework.Entities - the domain (entities) module.
    - Game       - the game's rules and states.
    - Player     - the player's rules and states.
    - Level      - the level.
    - LevelView  - the level's visual and audible aspects.
    - World      - the world.
    - WorldView  - the world's visual and audible aspects.
    - Entity     - the entity (player's character/avatar, NPC/bot or any other scene's object).
    - EntityView - the entity's visual and audible aspects.
    - EntityBody - the entity's physical aspects.

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
