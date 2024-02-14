# Overview
This package provides you with the architecture game framework that helping you to develop your project following the best practices (like Clean Architecture and Uber Ribs).

# Reference
This framework contains the classes defining the architecture of your game.

## Framework
- **IDependencyContainer** -
this interface allows you to resolve your dependencies.
- **Program** -
this class is responsible for a startup and global logic.

## Framework.UI
- **UIAudioTheme** -
this class responsible for the audio theme.
- **UIScreen** -
this class responsible for the user interface. The user interface consists of hierarchy of logical units and hierarchy of visual units.
- **UIWidget** -
this class represents the logical (business) unit of visual interface. This contains a business logic. And also this may contain (or not contain) the view.
- **UIView** -
this class represents the visual unit of visual interface.
This just contains the VisualElement, so it's essentially a wrapper for VisualElement.
- **UIRouter** -
this class is responsible for an application state.

## Framework.App
- **Application** -
this class represents the application.
- **Globals** -
this class responsible for a global values.

## Framework.Entities
- **Game** -
this class represents the game rules and states.
- **Player** -
this class represents the player rules and states.
- **World** -
this class represents the world.
- **WorldView** -
this class responsible for visual and audible aspects.
- **Entity** -
this class represents the scene's entity (independent entity or player's avatar).
- **EntityView** -
this class responsible for visual and audible aspects.
- **EntityBody** -
this class responsible for physical aspects.

# Links
- https://assetstore.unity.com/publishers/90787
