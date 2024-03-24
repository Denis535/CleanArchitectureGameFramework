#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ScriptExecutionOrders {

        // Program
        public const int Program      = 15_000;
        // UI
        public const int UIAudioTheme = 14_200;
        public const int UIScreen     = 14_100;
        public const int UIRouter     = 14_000;
        // Application
        public const int Application  = 13_000;
        // Game
        public const int Game         = 12_200;
        public const int Player       = 12_100;
        public const int Level        = 12_000;
        // Game
        public const int World        = 11_100;
        public const int World_View   = 11_000;
        // Game
        public const int Entity       = 10_200;
        public const int Entity_View  = 10_100;
        public const int Entity_Body  = 10_000;

    }
}
