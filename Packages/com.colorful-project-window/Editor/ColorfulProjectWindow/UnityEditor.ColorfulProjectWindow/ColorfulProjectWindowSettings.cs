#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ColorfulProjectWindowSettings {

        private static ColorfulProjectWindowSettings? instance;

        public static ColorfulProjectWindowSettings Instance {
            get {
                instance ??= new ColorfulProjectWindowSettings();
                return instance;
            }
        }

        public Color ModuleColor { get; set; } = HSVA( 000, 1, 1, 0.3f );
        public Color AssetsColor { get; set; } = HSVA( 060, 1f, 1.0f, 0.3f );
        public Color ResourcesColor { get; set; } = HSVA( 060, 1f, 1.0f, 0.3f );
        public Color SourcesColor { get; set; } = HSVA( 120, 1f, 1.0f, 0.3f );

        // Constructor
        public ColorfulProjectWindowSettings() {
        }

        //// Load
        //public void Load() {
        //}

        //// Save
        //public void Save() {
        //}

        // Reset
        public void Reset() {
            ModuleColor = HSVA( 000, 1, 1, 0.3f );
            AssetsColor = HSVA( 060, 1f, 1.0f, 0.3f );
            ResourcesColor = HSVA( 060, 1f, 1.0f, 0.3f );
            SourcesColor = HSVA( 120, 1f, 1.0f, 0.3f );
        }

        // Helpers
        private static Color HSVA(int h, float s, float v, float a) {
            var color = Color.HSVToRGB( h / 360f, s, v );
            color.a = a;
            return color;
        }

    }
}
