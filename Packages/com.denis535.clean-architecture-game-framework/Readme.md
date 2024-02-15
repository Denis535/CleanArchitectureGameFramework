# Overview
This package provides you with the architecture game framework that helping you to develop your project following the best practices (like Clean Architecture and Uber Ribs).

# Reference
This framework contains the classes defining the architecture of your game.

## Framework
**IDependencyContainer**
This interface allows you to resolve your dependencies.
**Program**
This class is responsible for a startup and global logic.

## Framework.UI
**UIAudioTheme**
This class responsible for the audio theme.
**UIScreen**
This class responsible for the user interface. The user interface consists of hierarchy of logical units and hierarchy of visual units.
**UIWidget**
This class represents the logical (business) unit of visual interface. This contains a business logic. And also this may contain (or not contain) the view.
**UIView**
This class represents the visual unit of visual interface.
This just contains the VisualElement, so it's essentially a wrapper for VisualElement.
**UIRouter**
This class is responsible for an application state.

## Framework.App
**Application**
This class represents the application.
**Globals**
This class responsible for a global values.

## Framework.Entities
**Game**
This class represents the game rules and states.
**Player**
This class represents the player rules and states.
**World**
This class represents the world.
**WorldView**
This class responsible for visual and audible aspects.
**Entity**
This class represents the scene's entity (independent entity or player's avatar).
**EntityView**
This class responsible for visual and audible aspects.
**EntityBody**
This class responsible for physical aspects.

# Media
[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/YOUTUBE_VIDEO_ID_HERE/0.jpg)](https://www.youtube.com/watch?v=lva7KKOQ71k)
https://youtu.be/lva7KKOQ71k

# Links
- https://github.com/Denis535/CleanArchitectureGameFramework/tree/master/Packages/com.denis535.clean-architecture-game-framework
- https://openupm.com/packages/com.denis535.clean-architecture-game-framework
- https://assetstore.unity.com/publishers/90787
