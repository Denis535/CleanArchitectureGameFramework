# Overview
This package makes the project window much more convenient. This highlights the special folders (packages, assemblies, content) in different colors, thus making the project window easier to use and greatly reducing cognitive load.

# How it works
It identifies special folders according to the following rules:
- Package folder is folder containing 'package.json' file.
- Assembly folder is folder containing '*.asmdef' or '*.asmref' file.
- Content folder is any folder within assembly folder.
- Assets folder is content folder starting with 'Assets.*' prefix.
- Resources folder is content folder starting with 'Resources.*' prefix.
- Source folder is just any other content folder.

# How to use it
```
[InitializeOnLoad]
public class ProjectWindow2 : ProjectWindow {

    static ProjectWindow2() {
        new ProjectWindow2();
    }

    public ProjectWindow2() {
    }

    protected override void OnGUI(Rect rect, string path) {
        base.OnGUI( rect, path );
    }

}
```

# Setup
You can setup the colors in the 'Preferences/Colorful Project Window' window.

# Media
- ![1](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/e825a503-0649-474d-8f4e-2f770dc1fb5a)
- ![2](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/74f55fd5-39f9-4b1b-a662-71d52e02cae0)
- ![3](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/e3a77f0e-1c00-4382-b9df-bd3313dfc305)

# Links
- https://github.com/denis535/CleanGameExample
- https://denis535.github.io
- https://www.nuget.org/profiles/Denis535
- https://openupm.com/packages/?sort=downloads&q=denis535
- https://assetstore.unity.com/publishers/90787
- https://denis535.itch.io/
- https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg

# If you want to support me
If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos.
