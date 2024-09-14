#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIThemeBase : Disposable {

        // AudioSource
        protected internal AudioSource AudioSource { get; }
        // Stateful
        private Stateful<UIPlayListBase> Stateful { get; } = new Stateful<UIPlayListBase>();
        // PlayList
        protected internal UIPlayListBase? PlayList => Stateful.State;

        // Constructor
        public UIThemeBase(AudioSource audioSource) {
            AudioSource = audioSource;
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            base.Dispose();
        }

        // SetPlayList
        protected virtual void SetPlayList(UIPlayListBase? playList, object? argument = null) {
            Stateful.SetState( playList, argument );
        }

    }
}
