# Overview
The package provides you with a more convenient project window, which highlights the most important files and folders in special colors.
This highlights packages, assemblies, assets, resources and sources in their appropriate colors.
Thus, this makes working with the project window much more convenient and faster.

# How to use it
```
[InitializeOnLoad]
public class ProjectWindow2 : ProjectWindow {

    static ProjectWindow2() {
        new ProjectWindow2();
    }

    public ProjectWindow2() {
    }

    protected override void DrawElement(Rect rect, string path) {
        base.DrawElement( rect, path );
    }
    protected override void DrawPackage(Rect rect, string path) {
        base.DrawPackage( rect, path );
    }
    protected override void DrawAssembly(Rect rect, string path) {
        base.DrawAssembly( rect, path );
    }

}
```

# Setup
You can set up the colors in the 'Preferences/Colorful Project Window' window.

# Media
- ![1](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/e825a503-0649-474d-8f4e-2f770dc1fb5a)
- ![2](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/74f55fd5-39f9-4b1b-a662-71d52e02cae0)
- ![3](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/e3a77f0e-1c00-4382-b9df-bd3313dfc305)

# Links
- https://denis535.github.io
- https://www.nuget.org/profiles/Denis535
- https://openupm.com/packages/?sort=downloads&q=denis535
- https://assetstore.unity.com/publishers/90787
- https://www.fab.com/sellers/Denis535
- https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg

# If you want to support me
If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos.
