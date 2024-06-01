#nullable enable
namespace UnityEngine.Framework.App {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public abstract class StorageBase : Disposable {

        // Constructor
        public StorageBase() {
        }

        // Helpers
        protected static bool HasCommandLineArgument(string name) {
            Assert.Argument.Message( $"Name {name} must start with '-'" ).Valid( name.StartsWith( '-' ) );
            var arguments = Environment.GetCommandLineArgs();
            var i = Array.IndexOf( arguments, name );
            return i != -1;
        }
        protected static string? GetCommandLineArgument(string name) {
            Assert.Argument.Message( $"Name {name} must start with '-'" ).Valid( name.StartsWith( '-' ) );
            var arguments = Environment.GetCommandLineArgs();
            var i = Array.IndexOf( arguments, name );
            if (i != -1) return arguments.Skip( i ).Skip( 1 ).TakeWhile( i => !i.StartsWith( '-' ) ).FirstOrDefault();
            return null;
        }
        protected static string[]? GetCommandLineArguments(string name) {
            Assert.Argument.Message( $"Name {name} must start with '-'" ).Valid( name.StartsWith( '-' ) );
            var arguments = Environment.GetCommandLineArgs();
            var i = Array.IndexOf( arguments, name );
            if (i != -1) return arguments.Skip( i ).Skip( 1 ).TakeWhile( i => !i.StartsWith( '-' ) ).ToArray().NullIfEmpty();
            return null;
        }
        // Helpers
        protected static string Load(string key, string @default) {
            var result = PlayerPrefs.GetString( key, @default );
            return result;
        }
        protected static int Load(string key, int @default) {
            var result = PlayerPrefs.GetInt( key, @default );
            return result;
        }
        protected static float Load(string key, float @default) {
            var result = PlayerPrefs.GetFloat( key, @default );
            return result;
        }
        protected static decimal Load(string key, decimal @default) {
            var result = PlayerPrefs.GetString( key, null );
            return result != null && decimal.TryParse( result, out var result2 ) ? result2 : @default;
        }
        protected static bool Load(string key, bool @default) {
            var result = PlayerPrefs.GetString( key, null );
            return result != null && bool.TryParse( result, out var result2 ) ? result2 : @default;
        }
        protected static Enum Load(string key, Enum @default, Type enumType) {
            var result = PlayerPrefs.GetString( key, null );
            return result != null && Enum.TryParse( enumType, result, true, out var result2 ) ? (Enum) result2 : @default;
        }
        // Helpers
        protected static void Save(string key, string value) {
            PlayerPrefs.SetString( key, value );
        }
        protected static void Save(string key, int value) {
            PlayerPrefs.SetInt( key, value );
        }
        protected static void Save(string key, float value) {
            PlayerPrefs.SetFloat( key, value );
        }
        protected static void Save(string key, decimal value) {
            PlayerPrefs.SetString( key, value.ToString() );
        }
        protected static void Save(string key, bool value) {
            PlayerPrefs.SetString( key, value.ToString() );
        }
        protected static void Save(string key, Enum value) {
            PlayerPrefs.SetString( key, value.ToString() );
        }

    }
}
