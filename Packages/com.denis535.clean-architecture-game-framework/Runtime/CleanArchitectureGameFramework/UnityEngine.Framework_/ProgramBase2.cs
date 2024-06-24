#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ProgramBase2 : ProgramBase, IDependencyContainer {

        // IDependencyContainer
        Option<object?> IDependencyContainer.GetValue(Type type, object? argument) {
            return GetValue( type, argument );
        }
        protected abstract Option<object?> GetValue(Type type, object? argument);

    }
}
