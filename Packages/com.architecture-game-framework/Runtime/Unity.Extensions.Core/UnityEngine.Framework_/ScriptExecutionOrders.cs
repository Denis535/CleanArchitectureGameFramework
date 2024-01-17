#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ScriptExecutionOrders {

        // Program
        public const int Program = 13_000;
        // UI
        public const int UIAudioTheme = 12_200;
        public const int UIScreen     = 12_100;
        public const int UIRouter     = 12_000;
        // Application
        public const int Application = 11_000;
        // Game
        public const int Game        = 10_600;
        public const int Player      = 10_500;
        public const int World       = 10_400;
        public const int World_View  = 10_300;
        public const int Entity      = 10_200;
        public const int Entity_View = 10_100;
        public const int Entity_Body = 10_000;

    }
}
