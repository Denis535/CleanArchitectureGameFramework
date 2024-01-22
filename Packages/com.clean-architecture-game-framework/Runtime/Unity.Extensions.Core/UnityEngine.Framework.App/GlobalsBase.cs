#nullable enable
namespace UnityEngine.Framework.App {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public abstract class GlobalsBase {

        // Constructor
        public GlobalsBase() {
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
            var value = PlayerPrefs.GetString( key, @default );
            return value;
        }
        protected static int Load(string key, int @default) {
            var value = PlayerPrefs.GetInt( key, @default );
            return value;
        }
        protected static float Load(string key, float @default) {
            var value = PlayerPrefs.GetFloat( key, @default );
            return value;
        }
        protected static decimal Load(string key, decimal @default) {
            var value = PlayerPrefs.GetString( key, @default.ToString() );
            return decimal.TryParse( value, out var value2 ) ? value2 : @default;
        }
        protected static bool Load(string key, bool @default) {
            var value = PlayerPrefs.GetString( key, @default.ToString() );
            return bool.TryParse( value, out var value2 ) ? value2 : @default;
        }
        protected static T Load<T>(string key, T @default) where T : struct, Enum {
            var value = PlayerPrefs.GetString( key, @default.ToString() );
            return Enum.TryParse<T>( value, out var value2 ) ? value2 : @default;
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
        protected static void Save<T>(string key, T value) where T : Enum {
            PlayerPrefs.SetString( key, value.ToString() );
        }

    }
}
