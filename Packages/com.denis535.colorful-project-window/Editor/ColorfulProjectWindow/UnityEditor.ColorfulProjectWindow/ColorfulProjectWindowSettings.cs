#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ColorfulProjectWindowSettings {

        private static readonly Color DefaultAssemblyColor = HSVA( 000, 1f, 1.0f, 0.3f );
        private static readonly Color DefaultAssetsColor = HSVA( 060, 1f, 1.0f, 0.3f );
        private static readonly Color DefaultResourcesColor = HSVA( 060, 1f, 1.0f, 0.3f );
        private static readonly Color DefaultSourcesColor = HSVA( 120, 1f, 1.0f, 0.3f );
        private static ColorfulProjectWindowSettings? instance;

        // Instance
        public static ColorfulProjectWindowSettings Instance => instance ??= new ColorfulProjectWindowSettings();

        // Colors
        public Color AssemblyColor { get; internal set; }
        public Color AssetsColor { get; internal set; }
        public Color ResourcesColor { get; internal set; }
        public Color SourcesColor { get; internal set; }

        // Constructor
        private ColorfulProjectWindowSettings() {
            Load();
        }

        // Load
        public void Load() {
            AssemblyColor = LoadColor( "ColorfulProjectWindowSettings.AssemblyColor", DefaultAssemblyColor );
            AssetsColor = LoadColor( "ColorfulProjectWindowSettings.AssetsColor", DefaultAssetsColor );
            ResourcesColor = LoadColor( "ColorfulProjectWindowSettings.ResourcesColor", DefaultResourcesColor );
            SourcesColor = LoadColor( "ColorfulProjectWindowSettings.SourcesColor", DefaultSourcesColor );
        }

        // Save
        public void Save() {
            SaveColor( "ColorfulProjectWindowSettings.AssemblyColor", AssemblyColor );
            SaveColor( "ColorfulProjectWindowSettings.AssetsColor", AssetsColor );
            SaveColor( "ColorfulProjectWindowSettings.ResourcesColor", ResourcesColor );
            SaveColor( "ColorfulProjectWindowSettings.SourcesColor", SourcesColor );
        }

        // Reset
        public void Reset() {
            AssemblyColor = DefaultAssemblyColor;
            AssetsColor = DefaultAssetsColor;
            ResourcesColor = DefaultResourcesColor;
            SourcesColor = DefaultSourcesColor;
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
