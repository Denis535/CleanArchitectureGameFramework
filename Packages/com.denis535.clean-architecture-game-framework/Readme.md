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
[![YouTube](https://img.youtube.com/vi/lva7KKOQ71k/0.jpg)](https://youtu.be/lva7KKOQ71k)
![CleanArchitectureGameTemplate-638435363375865274](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/f1ac68b5-e925-4621-ab10-d52586d4c559)
![CleanArchitectureGameTemplate-638435363615105446](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/a3e2988f-eb4f-40b3-b743-6587cef92950)
![CleanArchitectureGameTemplate-638435363730608344](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/c0344129-61d7-4797-94e5-951cd44a9d2b)
![CleanArchitectureGameTemplate-638435363835706904](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/94937e69-2729-46ba-8692-d20589c524d4)
![CleanArchitectureGameTemplate-638435363985585039](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/bc7930b2-bd16-4961-807b-390ca72d7dc8)
![CleanArchitectureGameTemplate-638435364085742612](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/d030fa10-3643-4912-810b-b43e08033585)
![1](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/c02726e0-ff87-42e3-85ff-f870217a151e)
![2](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/9319a8e7-26fb-48e7-aa2a-0e58b96c9074)

# Links
- https://denis535.github.io
- https://openupm.com/packages/?sort=downloads&q=denis535
- https://assetstore.unity.com/publishers/90787
- https://github.com/Denis535/CleanArchitectureGameFramework/tree/master/Packages/com.denis535.clean-architecture-game-framework
