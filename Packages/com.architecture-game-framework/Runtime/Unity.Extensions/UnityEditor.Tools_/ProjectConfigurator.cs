#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.Tools_ {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Framework;
    using UnityEngine.Framework.App;
    using UnityEngine.Framework.Game;
    using UnityEngine.Framework.UI;

    public abstract class ProjectConfigurator {

        // Configure
        public virtual void Configure() {
            foreach (var assembly in Compilation.CompilationPipeline.GetAssemblies().Where( IsSupported )) {
                Configure( assembly );
            }
            foreach (var script in MonoImporter.GetAllRuntimeMonoScripts().Where( IsSupported )) {
                Configure( script );
            }
        }
        public abstract void Configure(Compilation.Assembly assembly);
        public abstract void Configure(MonoScript script);

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
    public class ProjectConfigurator2 : ProjectConfigurator {

        // Configure
        public override void Configure() {
            base.Configure();
        }
        public override void Configure(Compilation.Assembly assembly) {
        }
        public override void Configure(MonoScript script) {
            var order = GetExecutionOrder( script );
            if (order.HasValue && order != MonoImporter.GetExecutionOrder( script )) {
                MonoImporter.SetExecutionOrder( script, order.Value );
            }
        }

        // GetExecutionOrder
        public virtual int? GetExecutionOrder(MonoScript script) {
            return GetExecutionOrder_Program( script ) ?? GetExecutionOrder_UI( script ) ?? GetExecutionOrder_App( script ) ?? GetExecutionOrder_Game( script );
        }
        public virtual int? GetExecutionOrder_Program(MonoScript script) {
            // Program
            if (script.CanConfigure( typeof( ProgramBase ) )) return ScriptExecutionOrders.Program;
            return null;
        }
        public virtual int? GetExecutionOrder_UI(MonoScript script) {
            // Audible
            if (script.CanConfigure( typeof( UIAudioThemeBase ) )) return ScriptExecutionOrders.UIAudioTheme;
            // Logical
            if (script.CanConfigure( typeof( UIScreenBase ) )) return ScriptExecutionOrders.UIScreen;
            // Misc
            if (script.CanConfigure( typeof( UIRouterBase ) )) return ScriptExecutionOrders.UIRouter;
            return null;
        }
        public virtual int? GetExecutionOrder_App(MonoScript script) {
            // App
            if (script.CanConfigure( typeof( ApplicationBase ) )) return ScriptExecutionOrders.App;
            return null;
        }
        public virtual int? GetExecutionOrder_Game(MonoScript script) {
            // Game
            if (script.CanConfigure( typeof( GameBase ) )) return ScriptExecutionOrders.Game + 00;
            if (script.CanConfigure( typeof( PlayerBase ) )) return ScriptExecutionOrders.Player + 00;
            // World
            if (script.CanConfigure( typeof( WorldBase ) )) return ScriptExecutionOrders.World + 10;
            if (script.CanConfigure( typeof( WorldViewBase ) )) return ScriptExecutionOrders.World + 00;
            // Entity
            if (script.CanConfigure( typeof( EntityBase ) )) return ScriptExecutionOrders.Entity + 20;
            if (script.CanConfigure( typeof( EntityViewBase ) )) return ScriptExecutionOrders.Entity + 10;
            if (script.CanConfigure( typeof( EntityBodyBase ) )) return ScriptExecutionOrders.Entity + 00;
            return null;
        }

    }
    public static class ProjectConfiguratorUtils {

        // CanConfigure
        public static bool CanConfigure(this MonoScript script, Type pattern) {
            if (script.GetClass() != null) {
                return script.GetClass().Is( pattern ) || script.GetClass().IsDescendentOf( pattern ) || script.GetClass().IsImplementationOf( pattern );
            }
            return false;
        }
        public static bool CanConfigure(this MonoScript script, string pattern) {
            if (script.GetClass() != null) {
                return script.GetClass().GetSimpleName().IsMatch( pattern );
            }
            return false;
        }
        // CanConfigure
        public static bool CanConfigure(this MonoScript script, params Type[] patterns) {
            return patterns.Any( pattern => script.CanConfigure( pattern ) );
        }
        public static bool CanConfigure(this MonoScript script, params string[] patterns) {
            return patterns.Any( pattern => script.CanConfigure( pattern ) );
        }

        // Helpers
        private static bool IsMatch(this string value, string pattern) {
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

    }
}
#endif
