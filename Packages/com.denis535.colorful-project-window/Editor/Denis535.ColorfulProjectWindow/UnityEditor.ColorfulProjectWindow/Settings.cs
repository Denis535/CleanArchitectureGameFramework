#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Settings {

        private static readonly Color DefaultPackageColor   = HSVA( 240, 1.0f, 1.0f, 0.3f );
        private static readonly Color DefaultAssemblyColor  = HSVA( 000, 1.0f, 1.0f, 0.3f );
        private static readonly Color DefaultAssetsColor    = HSVA( 060, 1.0f, 1.0f, 0.3f );
        private static readonly Color DefaultResourcesColor = HSVA( 060, 1.0f, 1.0f, 0.3f );
        private static readonly Color DefaultSourcesColor   = HSVA( 120, 1.0f, 1.0f, 0.3f );
        private static Settings? instance;

        // Instance
        public static Settings Instance => instance ??= new Settings();

        // Colors
        public Color PackageColor { get; internal set; }
        public Color AssemblyColor { get; internal set; }
        public Color AssetsColor { get; internal set; }
        public Color ResourcesColor { get; internal set; }
        public Color SourcesColor { get; internal set; }

        // Constructor
        private Settings() {
            Load();
        }

        // Load
        public void Load() {
            PackageColor   = GetColor( "ColorfulProjectWindow.Settings.PackageColor",   DefaultPackageColor );
            AssemblyColor  = GetColor( "ColorfulProjectWindow.Settings.AssemblyColor",  DefaultAssemblyColor );
            AssetsColor    = GetColor( "ColorfulProjectWindow.Settings.AssetsColor",    DefaultAssetsColor );
            ResourcesColor = GetColor( "ColorfulProjectWindow.Settings.ResourcesColor", DefaultResourcesColor );
            SourcesColor   = GetColor( "ColorfulProjectWindow.Settings.SourcesColor",   DefaultSourcesColor );
        }

        // Save
        public void Save() {
            SetColor( "ColorfulProjectWindow.Settings.PackageColor",   PackageColor );
            SetColor( "ColorfulProjectWindow.Settings.AssemblyColor",  AssemblyColor );
            SetColor( "ColorfulProjectWindow.Settings.AssetsColor",    AssetsColor );
            SetColor( "ColorfulProjectWindow.Settings.ResourcesColor", ResourcesColor );
            SetColor( "ColorfulProjectWindow.Settings.SourcesColor",   SourcesColor );
        }

        // Reset
        public void Reset() {
            PackageColor   = DefaultPackageColor;
            AssemblyColor  = DefaultAssemblyColor;
            AssetsColor    = DefaultAssetsColor;
            ResourcesColor = DefaultResourcesColor;
            SourcesColor   = DefaultSourcesColor;
        }

        // Helpers
        private static Color GetColor(string key, Color @default) {
            var result = EditorPrefs.GetString( key, ColorUtility.ToHtmlStringRGBA( @default ) );
            if (ColorUtility.TryParseHtmlString( $"#{result}", out var result2 )) {
                return result2;
            }
            return @default;
        }
        private static void SetColor(string key, Color value) {
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
