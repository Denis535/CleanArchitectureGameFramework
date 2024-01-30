# Overview
This package "Addressables Source Generator" gives you an ability to generate a source code with hierarchical list of all addressable assets and its labels.

# How to use it
The first way is to create the 'AddressablesSourceGenerator' asset and press 'Generate' button in the inspector.
The seconds way is to write a script which will generate the source code.
```
var settings = AddressableAssetSettingsDefaultObject.Settings;
new ResourcesSourceGenerator().Generate( settings, "Assets/UnityEngine.AddressableAssets/R.cs", "UnityEngine.AddressableAssets", "R" );
new LabelsSourceGenerator().Generate( settings, "Assets/UnityEngine.AddressableAssets/L.cs", "UnityEngine.AddressableAssets", "L" );
```

# Example (Assets)
```
namespace UnityEngine.AddressableAssets {
    public static class @R {
        public static class @Global {
            public static class @Aaa {
                public const string @Asset = "Global/Aaa/Asset.txt";
                public const string @Asset_1 = "Global/Aaa/Asset 1.txt";
                public const string @Asset_2 = "Global/Aaa/Asset 2.txt";
            }
            public static class @Bbb {
                public const string @Asset = "Global/Bbb/Asset.txt";
                public const string @Asset_1 = "Global/Bbb/Asset 1.txt";
                public const string @Asset_2 = "Global/Bbb/Asset 2.txt";
            }
            public const string @Asset = "Global/Asset.txt";
            public const string @Asset_1 = "Global/Asset 1.txt";
            public const string @Asset_2 = "Global/Asset 2.txt";
        }
        public static class @MyProject {
            public static class @Aaa {
                public const string @Asset = "MyProject.Aaa/Asset.txt";
                public const string @Asset_1 = "MyProject.Aaa/Asset 1.txt";
                public const string @Asset_2 = "MyProject.Aaa/Asset 2.txt";
            }
            public static class @Bbb {
                public const string @Asset = "MyProject.Bbb/Asset.txt";
                public const string @Asset_1 = "MyProject.Bbb/Asset 1.txt";
                public const string @Asset_2 = "MyProject.Bbb/Asset 2.txt";
            }
            public const string @Asset = "MyProject/Asset.txt";
            public const string @Asset_1 = "MyProject/Asset 1.txt";
            public const string @Asset_2 = "MyProject/Asset 2.txt";
        }
    }
}
```

# Example (Labels)
```
namespace UnityEngine.AddressableAssets {
    public static class @L {
        public const string @default = "default";
    }
}
```

# Links
- https://u3d.as/39eY
- https://youtu.be/tardralpxdA
