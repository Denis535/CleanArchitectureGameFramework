#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework.App;
    using UnityEngine.Framework.Game;
    using UnityEngine.Framework.UI;

    public static class IDependencyContainerExtensions {

        // Base
        public static IDependencyContainer GetDependencyContainer(this MonoBehaviour monoBehaviour) {
            return IDependencyContainer.Instance;
        }

        // Program
        public static IDependencyContainer GetDependencyContainer(this ProgramBase program) {
            return IDependencyContainer.Instance;
        }

        // UI
        public static IDependencyContainer GetDependencyContainer(this UIAudioThemeBase theme) {
            return IDependencyContainer.Instance;
        }
        public static IDependencyContainer GetDependencyContainer(this UIScreenBase screen) {
            return IDependencyContainer.Instance;
        }
        public static IDependencyContainer GetDependencyContainer(this UIWidgetBase widget) {
            return IDependencyContainer.Instance;
        }
        public static IDependencyContainer GetDependencyContainer(this UIRouterBase router) {
            return IDependencyContainer.Instance;
        }

        // App
        public static IDependencyContainer GetDependencyContainer(this ApplicationBase application) {
            return IDependencyContainer.Instance;
        }

        // Game
        public static IDependencyContainer GetDependencyContainer(this GameBase game) {
            return IDependencyContainer.Instance;
        }
        public static IDependencyContainer GetDependencyContainer(this PlayerBase player) {
            return IDependencyContainer.Instance;
        }
        public static IDependencyContainer GetDependencyContainer(this WorldBase world) {
            return IDependencyContainer.Instance;
        }
        public static IDependencyContainer GetDependencyContainer(this WorldViewBase worldView) {
            return IDependencyContainer.Instance;
        }
        public static IDependencyContainer GetDependencyContainer(this EntityBase entity) {
            return IDependencyContainer.Instance;
        }
        public static IDependencyContainer GetDependencyContainer(this EntityViewBase entityView) {
            return IDependencyContainer.Instance;
        }
        public static IDependencyContainer GetDependencyContainer(this EntityBodyBase entityBody) {
            return IDependencyContainer.Instance;
        }

    }
}
