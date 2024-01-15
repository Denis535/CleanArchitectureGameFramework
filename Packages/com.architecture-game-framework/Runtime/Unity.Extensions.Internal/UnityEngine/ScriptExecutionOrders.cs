#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ScriptExecutionOrders {

        // Program
        public const int Program = 13_000;
        // UI
        public const int UIAudioTheme = 12_200;
        public const int UIScreen = 12_100;
        public const int UIRouter = 12_000;
        // App
        public const int App = 11_000;
        // Entities
        public const int Game = 10_300;
        public const int Player = 10_200;
        public const int World = 10_100;
        public const int Entity = 10_000;

    }
}
