#nullable enable
namespace UnityEngine.Framework.Game.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IEntityRegistry {

        protected internal void RegisterEntity(EntityBase entity);
        protected internal void UnregisterEntity(EntityBase entity);

    }
}
