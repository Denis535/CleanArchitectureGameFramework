# Overview
This package makes the project window much more convenient. This highlights the special folders (modules, assets, resources and sources) in different colors, thus making the project window easier to use and greatly reducing cognitive load.

# How it works
This identifies special folders according to the following rules:
- Module folder must contain '*.asmdef' or '*.asmref' files.
- Assets folder must start with 'Assets' or 'Assets.*' prefix.
- Resources folder must start with 'Resources' or 'Resources.*' prefix.
- Source folder is just any other folder.

# How to use it
```
#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public class ColorfulProjectWindow2 : ColorfulProjectWindow {

        // Constructor
        static ColorfulProjectWindow2() {
            new ColorfulProjectWindow2();
        }

        // Constructor
        public ColorfulProjectWindow2() {
        }

        // OnGUI
        protected override void OnGUI(string guid, Rect rect) {
            base.OnGUI( guid, rect );
        }
        protected override void OnGUI(Rect rect, string path, string module, string content) {
            base.OnGUI( rect, path, module, content );
        }

    }
}
#endif
```

# Setup
You can setup the colors in the 'Preferences/Colorful Project Window'.

# Media
- ![1](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/e825a503-0649-474d-8f4e-2f770dc1fb5a)
- ![2](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/74f55fd5-39f9-4b1b-a662-71d52e02cae0)
- ![3](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/e3a77f0e-1c00-4382-b9df-bd3313dfc305)

# Links
- https://github.com/Denis535/CleanArchitectureGameFramework/

- https://denis535.github.io
- https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg

- https://assetstore.unity.com/publishers/90787
- https://denis535.itch.io/

- https://www.nuget.org/profiles/Denis535
- https://openupm.com/packages/?sort=downloads&q=denis535

# If you want to support me
If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos.
