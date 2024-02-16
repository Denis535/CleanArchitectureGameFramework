# Overview
This package is addition to Addressables giving you the ability to reference assets in a very convenient way. For example: 'R.MyProject.Scenes.GameScene' or 'L.Scene'.

# How it works
This takes a list of all addressable assets and generates the source code with all asset addresses and labels constants.
For example:
```
namespace UnityEngine.AddressableAssets {
    public static class @R {
        public static class @MyProject {
            public static class @Scenes {
                public const string @Scene = "MyProject/Scenes/GameScene.unity";
            }
        }
    }
    public static class @L {
        public const string @default = "default";
        public const string Scenes = "scene";
    }
}
```

# How to use it
The first way is to create the 'AddressablesSourceGenerator' asset and press the 'Generate' button in the inspector. It will generate the 'R.cs' and 'L.cs' files next to your asset. 
The second way is to use the 'ResourcesSourceGenerator' and 'LabelsSourceGenerator':
```
var settings = AddressableAssetSettingsDefaultObject.Settings;
new ResourcesSourceGenerator().Generate( settings, "Assets/UnityEngine.AddressableAssets/R.cs", "UnityEngine.AddressableAssets", "R" );
new LabelsSourceGenerator().Generate( settings, "Assets/UnityEngine.AddressableAssets/L.cs", "UnityEngine.AddressableAssets", "L" );
```

# Reference
- **AddressablesSourceGenerator**
    - ``void Generate()``
- **ResourcesSourceGenerator**
    - ``void Generate(AddressableAssetSettings settings, string path, string @namespace, string name)``
- **LabelsSourceGenerator**
    - ``void Generate(AddressableAssetSettings settings, string path, string @namespace, string name)``

# Media
![1](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/a0cf834c-30cb-450b-bbc8-e3f5659b1950)
![2](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/5c1d0ac7-c94d-4c06-bf03-75817e6294f7)
![3](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/44e22d4f-58d4-4df0-92c7-4bc5f4ebe5f9)

# Links
- https://denis535.github.io
- https://github.com/Denis535/CleanArchitectureGameFramework/tree/master/Packages/com.denis535.addressables-source-generator
- https://openupm.com/packages/com.denis535.addressables-source-generator
- https://assetstore.unity.com/publishers/90787
