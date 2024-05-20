#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ColorfulProjectWindowSettings {

        private static readonly Color DefaultModuleColor = HSVA( 000, 1, 1, 0.3f );
        private static readonly Color DefaultContentColor = HSVA( 120, 1f, 1.0f, 0.3f );
        private static readonly Color DefaultAssetsColor = HSVA( 060, 1f, 1.0f, 0.3f );
        private static readonly Color DefaultResourcesColor = HSVA( 060, 1f, 1.0f, 0.3f );
        private static ColorfulProjectWindowSettings? instance;

        // Instance
        public static ColorfulProjectWindowSettings Instance => instance ??= new ColorfulProjectWindowSettings();

        // Colors
        public Color ModuleColor { get; set; }
        public Color ContentColor { get; set; }
        public Color AssetsColor { get; set; }
        public Color ResourcesColor { get; set; }

        // Constructor
        private ColorfulProjectWindowSettings() {
            Load();
        }

        // Load
        public void Load() {
            ModuleColor = LoadColor( "ColorfulProjectWindowSettings.ModuleColor", DefaultModuleColor );
            ContentColor = LoadColor( "ColorfulProjectWindowSettings.ContentColor", DefaultContentColor );
            AssetsColor = LoadColor( "ColorfulProjectWindowSettings.AssetsColor", DefaultAssetsColor );
            ResourcesColor = LoadColor( "ColorfulProjectWindowSettings.ResourcesColor", DefaultResourcesColor );
        }

        // Save
        public void Save() {
            SaveColor( "ColorfulProjectWindowSettings.ModuleColor", ModuleColor );
            SaveColor( "ColorfulProjectWindowSettings.ContentColor", ContentColor );
            SaveColor( "ColorfulProjectWindowSettings.AssetsColor", AssetsColor );
            SaveColor( "ColorfulProjectWindowSettings.ResourcesColor", ResourcesColor );
        }

        // Reset
        public void Reset() {
            ModuleColor = DefaultModuleColor;
            ContentColor = DefaultContentColor;
            AssetsColor = DefaultAssetsColor;
            ResourcesColor = DefaultResourcesColor;
        }

        // Helpers
        private static Color LoadColor(string key, Color @default) {
            var result = EditorPrefs.GetString( key, ColorUtility.ToHtmlStringRGBA( @default ) );
            if (ColorUtility.TryParseHtmlString( $"#{result}", out var result2 )) {
                return result2;
            }
            return @default;
        }
        private static void SaveColor(string key, Color value) {
            EditorPrefs.SetString( key, ColorUtility.ToHtmlStringRGBA( value ) );
        }
        // Helpers
        private static Color HSVA(int h, float s, float v, float a) {
            var color = Color.HSVToRGB( h / 360f, s, v );
            color.a = a;
            return color;
        }

    }
}
