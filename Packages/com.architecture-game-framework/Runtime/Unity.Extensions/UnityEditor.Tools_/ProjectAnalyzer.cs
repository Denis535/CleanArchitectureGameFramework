#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.Tools_ {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using UnityEngine.Framework;
    using UnityEngine.Framework.App;
    using UnityEngine.Framework.Game;
    using UnityEngine.Framework.UI;

    public abstract class ProjectAnalyzer {

        // Analyze
        public virtual void Analyze() {
            foreach (var assembly in Compilation.CompilationPipeline.GetAssemblies().Where( IsSupported )) {
                Analyze( assembly );
            }
            foreach (var script in MonoImporter.GetAllRuntimeMonoScripts().Where( IsSupported )) {
                Analyze( script );
            }
        }
        public abstract void Analyze(Compilation.Assembly assembly);
        public abstract void Analyze(MonoScript script);

        // IsSupported
        public virtual bool IsSupported(Compilation.Assembly assembly) {
            return
                !assembly.name.Equals( "Unity" ) && !assembly.name.StartsWith( "Unity." ) &&
                !assembly.name.Equals( "UnityEngine" ) && !assembly.name.StartsWith( "UnityEngine." ) &&
                !assembly.name.Equals( "UnityEditor" ) && !assembly.name.StartsWith( "UnityEditor." ) &&
                !assembly.name.Equals( "PPv2URPConverters" );
        }
        public virtual bool IsSupported(MonoScript script) {
            return AssetDatabase.GetAssetPath( script ).StartsWith( "Assets/" );
        }

    }
    public class ProjectAnalyzer2 : ProjectAnalyzer {

        // Analyze
        public override void Analyze() {
            base.Analyze();
        }
        public override void Analyze(Compilation.Assembly assembly) {
            foreach (var type in Assembly.LoadFrom( assembly.outputPath ).DefinedTypes) {
                Analyze( type );
            }
        }
        public override void Analyze(MonoScript script) {
        }

        // Analyze
        public virtual void Analyze(Type type) {
            Analyze_Program( type, "Project" );
            Analyze_UI( type, "Project" );
            Analyze_App( type, "Project" );
            Analyze_Game( type, "Project" );
        }
        public virtual void Analyze_Program(Type type, string @namespace) {
            // Program
            if (type.CanAnalyze( typeof( ProgramBase ) )) {
                type.Namespace( @namespace );
                type.Name( "Program" );
            }
        }
        public virtual void Analyze_UI(Type type, string @namespace) {
            // Audible
            if (type.CanAnalyze( typeof( UIAudioThemeBase ) )) {
                type.Namespace( "UnityEngine.Framework.UI", $"{@namespace}.UI" );
                type.Name( "*Theme" );
            }
            // Logical
            if (type.CanAnalyze( typeof( UIScreenBase ) )) {
                type.Namespace( "UnityEngine.Framework.UI", $"{@namespace}.UI" );
                type.Name( "*Screen" );
            }
            if (type.CanAnalyze( typeof( UIWidgetBase ) )) {
                type.Namespace( "UnityEngine.Framework.UI", $"{@namespace}.UI" );
                type.Name( "*Widget" );
            }
            // Visual
            if (type.CanAnalyze( typeof( UIViewBase ) )) {
                type.Namespace( "UnityEngine.Framework.UI", $"{@namespace}.UI" );
                type.Name( "*View" );
            }
            if (type.CanAnalyze( typeof( UIScreenViewBase ) )) {
                type.Namespace( "UnityEngine.Framework.UI", $"{@namespace}.UI" );
                type.Name( "*ScreenView" );
            }
            if (type.CanAnalyze( typeof( UIWidgetViewBase ) )) {
                type.Namespace( "UnityEngine.Framework.UI", $"{@namespace}.UI" );
                type.Name( "*WidgetView" );
            }
            // Misc
            if (type.CanAnalyze( typeof( UIRouterBase ) )) {
                type.Namespace( "UnityEngine.Framework.UI", $"{@namespace}.UI" );
                type.Name( "UIRouter" );
            }
        }
        public virtual void Analyze_App(Type type, string @namespace) {
            // App
            if (type.CanAnalyze( typeof( ApplicationBase ) )) {
                type.Namespace( "UnityEngine.Framework.App", $"{@namespace}.App" );
                type.Name( "Application" );
            }
            // Globals
            if (type.CanAnalyze( typeof( GlobalsBase ) )) {
                type.Namespace( "UnityEngine.Framework.App", $"{@namespace}.App" );
            }
        }
        public virtual void Analyze_Game(Type type, string @namespace) {
            // Game
            if (type.CanAnalyze( typeof( GameBase ) )) {
                type.Namespace( "UnityEngine.Framework.Game", $"{@namespace}.Domain", $"{@namespace}.Entities", $"{@namespace}.Game" );
                type.Name( "*Game" );
            }
            if (type.CanAnalyze( typeof( PlayerBase ) )) {
                type.Namespace( "UnityEngine.Framework.Game", $"{@namespace}.Domain", $"{@namespace}.Entities", $"{@namespace}.Game" );
                type.Name( "*Player" );
            }
            // World
            if (type.CanAnalyze( typeof( WorldBase ) )) {
                type.Namespace( "UnityEngine.Framework.Game", $"{@namespace}.Domain", $"{@namespace}.Entities", $"{@namespace}.Game" );
                type.Name( "*World" );
            }
            if (type.CanAnalyze( typeof( WorldViewBase ) )) {
                type.Namespace( "UnityEngine.Framework.Game", $"{@namespace}.Domain", $"{@namespace}.Entities", $"{@namespace}.Game" );
                type.Name( "*View" );
            }
            // Entity
            if (type.CanAnalyze( typeof( EntityBase ) )) {
                type.Namespace( "UnityEngine.Framework.Game", $"{@namespace}.Domain", $"{@namespace}.Entities", $"{@namespace}.Game" );
                type.Name( "*Entity" );
            }
            if (type.CanAnalyze( typeof( EntityViewBase ) )) {
                type.Namespace( "UnityEngine.Framework.Game", $"{@namespace}.Domain", $"{@namespace}.Entities", $"{@namespace}.Game" );
                type.Name( "*View" );
            }
            if (type.CanAnalyze( typeof( EntityBodyBase ) )) {
                type.Namespace( "UnityEngine.Framework.Game", $"{@namespace}.Domain", $"{@namespace}.Entities", $"{@namespace}.Game" );
                type.Name( "*Body" );
            }
        }

    }
    public static class ProjectAnalyzerUtils {

        // CanAnalyze
        public static bool CanAnalyze(this Type type, Type pattern) {
            return type.Is( pattern ) || type.IsDescendentOf( pattern ) || type.IsImplementationOf( pattern );
        }
        public static bool CanAnalyze(this Type type, string pattern) {
            return type.GetSimpleName().IsNameMatch( pattern );
        }
        // CanAnalyze
        public static bool CanAnalyze(this Type type, params Type[] patterns) {
            return patterns.Any( pattern => type.CanAnalyze( pattern ) );
        }
        public static bool CanAnalyze(this Type type, params string[] patterns) {
            return patterns.Any( pattern => type.CanAnalyze( pattern ) );
        }

        // Namespace
        public static void Namespace(this Type type, string pattern) {
            // Namespace || Namespace.
            var @namespace = type.Namespace.TrimEnd( '_' );
            if (!@namespace.IsNamespaceMatch( pattern )) {
                Debug.LogWarningFormat( "Type '{0}' must be within namespace '{1}'", type.FullName, pattern );
            }
        }
        public static void Namespace(this Type type, params string[] patterns) {
            // Namespace || Namespace.
            var @namespace = type.Namespace.TrimEnd( '_' );
            if (!@namespace.IsNamespaceMatch( patterns )) {
                Debug.LogWarningFormat( "Type '{0}' must be within namespace '{1}'", type.FullName, string.Join( ", ", patterns ) );
            }
        }

        // Name
        public static void Name(this Type type, string pattern) {
            // *Name* || Name* || *Name || Name
            var name = type.GetSimpleName().TrimEnd( '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' ).TrimEnd( "Base" );
            if (!name.IsNameMatch( pattern )) {
                Debug.LogWarningFormat( "Type '{0}' must have name '{1}'", name, pattern );
            }
        }
        public static void Name(this Type type, params string[] patterns) {
            // *Name* || Name* || *Name || Name
            var name = type.GetSimpleName().TrimEnd( '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' ).TrimEnd( "Base" );
            if (!name.IsNameMatch( patterns )) {
                Debug.LogWarningFormat( "Type '{0}' must have name '{1}'", name, string.Join( ", ", patterns ) );
            }
        }

        // Helpers
        private static bool IsNamespaceMatch(this string value, string pattern) {
            return value.Equals( pattern ) || value.StartsWith( pattern + '.' );
        }
        private static bool IsNamespaceMatch(this string value, string[] patterns) {
            return patterns.Any( i => value.IsNamespaceMatch( i ) );
        }
        // Helpers
        private static bool IsNameMatch(this string value, string pattern) {
            if (pattern.StartsWith( '*' ) && pattern.EndsWith( '*' )) {
                return value.Contains( pattern.Trim( '*' ) );
            }
            if (pattern.StartsWith( '*' )) {
                return value.EndsWith( pattern.TrimStart( '*' ) );
            }
            if (pattern.EndsWith( '*' )) {
                return value.StartsWith( pattern.TrimEnd( '*' ) );
            }
            {
                return value.Equals( pattern );
            }
        }
        private static bool IsNameMatch(this string value, string[] patterns) {
            return patterns.Any( i => value.IsNameMatch( i ) );
        }

    }
}
#endif
