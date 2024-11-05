# Overview
This package is an add-on to Addressables and gives you the ability to reference your assets and labels in a very convenient way with compile-time checking.

# How to generate source codes
You can use an `AddressablesSourceGenerator` asset.
```
var generator = AssetDatabase.LoadAssetAtPath<AddressableSourceGenerator>( AssetDatabase.GUIDToAssetPath( AssetDatabase.FindAssets( "t:AddressableSourceGenerator" ).Single() ) );
generator.Generate();
```
Or you can use an `ResourcesSourceGenerator` and `LabelsSourceGenerator` classes.
```
var settings = AddressableAssetSettingsDefaultObject.Settings;
new ResourcesSourceGenerator().Generate( "Assets/UnityEngine.AddressableAssets/R.cs", "UnityEngine.AddressableAssets", "R", settings );
new LabelsSourceGenerator().Generate( "Assets/UnityEngine.AddressableAssets/L.cs", "UnityEngine.AddressableAssets", "L", settings );
```

# Example of generated source codes
It generates source codes that looks something like this:
```
namespace UnityEngine.AddressableAssets {
    public static class @R {
        public static class @MyProject {
            public static class @Scenes {
                public const string @MainScene = "MyProject/Scenes/MainScene.unity";
                public const string @GameScene = "MyProject/Scenes/GameScene.unity";
            }
        }
    }
    public static class @L {
        public const string @default = "default";
        public const string @Scene = "scene";
    }
}
```

# How to use generated source codes
You can use your assets very easily:
```
var address = R.MyProject.Scenes.MainScene;
var label = L.Scene;
```

# Reference
###### AddressablesSourceGenerator
- ``void Generate()``

###### ResourcesSourceGenerator
- ``void Generate(string path, string @namespace, string class, AddressableAssetSettings settings)``

###### LabelsSourceGenerator
- ``void Generate(string path, string @namespace, string class, AddressableAssetSettings settings)``

# Media
- ![1](https://github.com/Denis535/UnityFramework/assets/7755015/a0cf834c-30cb-450b-bbc8-e3f5659b1950)
- ![2](https://github.com/Denis535/UnityFramework/assets/7755015/5c1d0ac7-c94d-4c06-bf03-75817e6294f7)
- ![3](https://github.com/Denis535/UnityFramework/assets/7755015/44e22d4f-58d4-4df0-92c7-4bc5f4ebe5f9)

# Links
- https://denis535.github.io
- https://www.nuget.org/profiles/Denis535
- https://openupm.com/packages/?sort=downloads&q=denis535
- https://www.fab.com/sellers/Denis535
- https://assetstore.unity.com/publishers/90787
- https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg
- https://www.udemy.com/user/denis-84102

# If you want to support me
If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos.
