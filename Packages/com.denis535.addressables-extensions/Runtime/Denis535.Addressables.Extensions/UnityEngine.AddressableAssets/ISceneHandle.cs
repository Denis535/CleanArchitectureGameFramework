#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal interface ISceneHandle<TThis> {

        string Key { get; }
        bool IsValid { get; }
        bool IsDone { get; }
        bool IsSucceeded { get; }
        bool IsFailed { get; }

        TThis Load(LoadSceneMode loadMode, bool activateOnLoad);

        ValueTask WaitAsync();

        ValueTask<Scene> GetValueAsync();

        ValueTask<Scene> ActivateAsync();

        void Unload();
        ValueTask UnloadAsync();

        void UnloadSafe();
        ValueTask UnloadSafeAsync();

    }
}
