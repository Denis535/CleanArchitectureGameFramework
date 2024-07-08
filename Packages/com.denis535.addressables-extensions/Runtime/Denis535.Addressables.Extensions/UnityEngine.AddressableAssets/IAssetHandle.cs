#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    internal interface IAssetHandle<TThis, T> where T : notnull, UnityEngine.Object {

        string Key { get; }
        bool IsValid { get; }
        bool IsDone { get; }
        bool IsSucceeded { get; }
        bool IsFailed { get; }

        TThis Load();

        void Wait();
        ValueTask WaitAsync(CancellationToken cancellationToken);

        T GetValue();
        ValueTask<T> GetValueAsync(CancellationToken cancellationToken);

        void Release();
        void ReleaseSafe();

    }
}
