#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.StateMachine;
    using System.Threading;
    using UnityEngine;

    public abstract class UIPlayListBase : StateBase<UIPlayListBase>, IDisposable {

        private CancellationTokenSource? disposeCancellationTokenSource;

        // System
        public bool IsDisposed { get; private set; }
        public CancellationToken DisposeCancellationToken {
            get {
                if (disposeCancellationTokenSource == null) {
                    disposeCancellationTokenSource = new CancellationTokenSource();
                    if (IsDisposed) disposeCancellationTokenSource.Cancel();
                }
                return disposeCancellationTokenSource.Token;
            }
        }
        // Theme
        protected UIThemeBase? Theme => (UIThemeBase?) Stateful;
        // IsRunning
        protected bool IsRunning {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be activating or active or deactivating" ).Valid( Activity is Activity_.Activating or Activity_.Active or Activity_.Deactivating );
                return Theme!.IsRunning;
            }
        }
        protected bool IsPlaying {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be activating or active or deactivating" ).Valid( Activity is Activity_.Activating or Activity_.Active or Activity_.Deactivating );
                return Theme!.IsPlaying;
            }
        }
        protected bool IsPaused {
            set {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be activating or active or deactivating" ).Valid( Activity is Activity_.Activating or Activity_.Active or Activity_.Deactivating );
                Theme!.IsPaused = value;
            }
        }
        protected bool Mute {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be activating or active or deactivating" ).Valid( Activity is Activity_.Activating or Activity_.Active or Activity_.Deactivating );
                return Theme!.Mute;
            }
            set {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be activating or active or deactivating" ).Valid( Activity is Activity_.Activating or Activity_.Active or Activity_.Deactivating );
                Theme!.Mute = value;
            }
        }
        protected float Volume {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be activating or active or deactivating" ).Valid( Activity is Activity_.Activating or Activity_.Active or Activity_.Deactivating );
                return Theme!.Volume;
            }
            set {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be activating or active or deactivating" ).Valid( Activity is Activity_.Activating or Activity_.Active or Activity_.Deactivating );
                Theme!.Volume = value;
            }
        }
        protected float Pitch {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be activating or active or deactivating" ).Valid( Activity is Activity_.Activating or Activity_.Active or Activity_.Deactivating );
                return Theme!.Pitch;
            }
            set {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be activating or active or deactivating" ).Valid( Activity is Activity_.Activating or Activity_.Active or Activity_.Deactivating );
                Theme!.Pitch = value;
            }
        }

        // Constructor
        public UIPlayListBase() {
        }
        public virtual void Dispose() {
            Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"PlayList {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            disposeCancellationTokenSource?.Cancel();
            IsDisposed = true;
        }

        // Play
        protected void Play(AudioClip clip) {
            Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"PlayList {this} must be activating or active" ).Valid( Activity is Activity_.Activating or Activity_.Active );
            Theme!.Play( clip );
        }
        protected void Stop() {
            Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"PlayList {this} must be active or deactivating" ).Valid( Activity is Activity_.Active or Activity_.Deactivating );
            Theme!.Stop();
        }

        // Helpers
        protected static T[] Shuffle<T>(T[] array) {
            for (int i = 0, j = array.Length; i < array.Length; i++, j--) {
                var rnd = i + UnityEngine.Random.Range( 0, j );
                (array[ i ], array[ rnd ]) = (array[ rnd ], array[ i ]);
            }
            return array;
        }
        protected static T GetNext<T>(T[] array, T? value) {
            var index = Array.IndexOf( array, value );
            if (index != -1) {
                index = (index + 1) % array.Length;
                return array[ index ];
            }
            return array[ 0 ];
        }
        protected static T GetRandom<T>(T[] array, T? value) {
            var index = UnityEngine.Random.Range( 0, array.Length );
            if (index != -1) {
                if (ReferenceEquals( array[ index ], value ) && array.Length > 1) {
                    return GetRandom( array, value );
                }
                return array[ index ];
            }
            return array[ 0 ];
        }

    }
}
