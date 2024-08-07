﻿#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    internal interface IAssetListHandle<TThis, T> {

        string[] Keys { get; }
        bool IsValid { get; }
        bool IsDone { get; }
        bool IsSucceeded { get; }
        bool IsFailed { get; }

        TThis Load();

        void Wait();
        ValueTask WaitAsync(CancellationToken cancellationToken);

        IReadOnlyList<T> GetValues();
        ValueTask<IReadOnlyList<T>> GetValuesAsync(CancellationToken cancellationToken);

        void Release();
        void ReleaseSafe();

    }
}
