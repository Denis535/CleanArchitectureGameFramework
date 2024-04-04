#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class IDependencyContainerExtensions {

        public static IDependencyContainer GetDependencyContainer(this object? @object) {
            return IDependencyContainer.Instance;
        }

    }
}
