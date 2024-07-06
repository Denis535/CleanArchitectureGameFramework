#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class PlayerPrefs2 {

        // GetValue
        public static bool GetBool(string key, bool @default) {
            var result = PlayerPrefs.GetString( key, null );
            return result != null && bool.TryParse( result, out var result2 ) ? result2 : @default;
        }
        public static decimal GetDecimal(string key, decimal @default) {
            var result = PlayerPrefs.GetString( key, null );
            return result != null && decimal.TryParse( result, out var result2 ) ? result2 : @default;
        }
        public static Enum GetEnum(string key, Enum @default, Type enumType) {
            var result = PlayerPrefs.GetString( key, null );
            return result != null && Enum.TryParse( enumType, result, true, out var result2 ) ? (Enum) result2 : @default;
        }

        // SetValue
        public static void SetBool(string key, bool value) {
            PlayerPrefs.SetString( key, value.ToString() );
        }
        public static void SetDecimal(string key, decimal value) {
            PlayerPrefs.SetString( key, value.ToString() );
        }
        public static void SetEnum(string key, Enum value) {
            PlayerPrefs.SetString( key, value.ToString() );
        }

    }
}
