# Overview
This package provides you with the architecture game framework that helping you to develop your project following the best practices (like Domain Driven Design, Clean Architecture and Uber Ribs).

# Reference
This package contains classes that define the entire architecture of your game project and some other utilities.

## Assemblies
- CleanArchitectureGameFramework - the secondary module.
- CleanArchitectureGameFramework.Core - the primary module.
- CleanArchitectureGameFramework.Internal - the module with utilities and helpers.

## Namespaces
- Framework - the root module.
    - Program              - the entry point.
- Framework.UI - the user interface module.
    - UITheme              - the audio theme.
    - UIScreen             - the graphics user interface. The user interface consists of the widget tree and the visual element tree.
    - UIWidget             - the state unit of ui. This may contain (or not contain) the view.
    - UIView               - the visual unit of ui. This is just the VisualElement.
    - UIRouter             - the state manager.
- Framework.App - the application module.
    - Application          - the application.
    - Storage              - the values provider.
- Framework.Entities - the domain module.
    - Game                 - the game's states and rules.
    - Player               - the player's states.
    - Entity               - the entity (any scene's actor).

# Media
- [![YouTube](https://img.youtube.com/vi/WmLJHRg0EI4/0.jpg)](https://youtu.be/WmLJHRg0EI4?feature=shared)

# Example
- https://denis535.github.io/#clean-game-example

# Article
- https://habr.com/ru/articles/833532/

# Links
- https://denis535.github.io
- https://github.com/Denis535/CleanGameExample/
- https://github.com/Denis535/CleanArchitectureGameFramework/
- https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg
- https://www.nuget.org/profiles/Denis535
- https://openupm.com/packages/?sort=downloads&q=denis535
- https://assetstore.unity.com/publishers/90787
- https://denis535.itch.io/

# If you want to support me
If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos.
