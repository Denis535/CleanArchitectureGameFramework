# Overview
This package is an add-on to Addressables and gives you the ability to reference your assets and labels in a very convenient way with compile-time checking.

# How to generate source codes
You should create an "AddressablesSourceGenerator" asset.
And then call the "Generate" method of this asset.
```
var generator = AssetDatabase.LoadAssetAtPath<AddressableSourceGenerator>( AssetDatabase.GUIDToAssetPath( AssetDatabase.FindAssets( "t:AddressableSourceGenerator" ).Single() ) );
generator.Generate();
```
Or you can create an "ResourcesSourceGenerator" and "LabelsSourceGenerator" objects.
And then call the "Generate" method of this objects.
```
var settings = AddressableAssetSettingsDefaultObject.Settings;
new ResourcesSourceGenerator().Generate( settings, "Assets/UnityEngine.AddressableAssets/R.cs", "UnityEngine.AddressableAssets", "R" );
new LabelsSourceGenerator().Generate( settings, "Assets/UnityEngine.AddressableAssets/L.cs", "UnityEngine.AddressableAssets", "L" );
```

# How to use generated source codes
You can reference your assets and labels very easily:
```
var address = R.MyProject.Scenes.MainScene;
var label = L.Scene;
```

# Example of generated source codes
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

# Reference
###### AddressablesSourceGenerator
- ``void Generate()``

###### AddressablesResourcesSourceGenerator
- ``void Generate(AddressableAssetSettings settings, string path, string @namespace, string class)``

###### AddressablesLabelsSourceGenerator
- ``void Generate(AddressableAssetSettings settings, string path, string @namespace, string class)``

# Media
- ![1](https://github.com/Denis535/UnityFramework/assets/7755015/a0cf834c-30cb-450b-bbc8-e3f5659b1950)
- ![2](https://github.com/Denis535/UnityFramework/assets/7755015/5c1d0ac7-c94d-4c06-bf03-75817e6294f7)
- ![3](https://github.com/Denis535/UnityFramework/assets/7755015/44e22d4f-58d4-4df0-92c7-4bc5f4ebe5f9)

# Links
- https://denis535.github.io
- https://github.com/Denis535/UnityFramework/

- https://www.nuget.org/profiles/Denis535
- https://openupm.com/packages/?sort=downloads&q=denis535

- https://assetstore.unity.com/publishers/90787
- https://denis535.itch.io/

- https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg

# If you want to support me
If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos.
