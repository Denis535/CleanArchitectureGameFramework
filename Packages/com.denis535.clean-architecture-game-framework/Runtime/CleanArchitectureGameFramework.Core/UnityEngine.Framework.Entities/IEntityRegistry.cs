#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IEntityRegistry {

        void RegisterEntity(EntityBase entity);
        void UnregisterEntity(EntityBase entity);

    }
}
