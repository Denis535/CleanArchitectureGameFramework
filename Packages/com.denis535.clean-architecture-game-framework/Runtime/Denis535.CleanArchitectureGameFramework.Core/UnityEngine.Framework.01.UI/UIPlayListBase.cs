#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
        // Context
        protected UIThemeBase Context { get; }
        // IsPlaying
        protected bool IsPlaying {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return Context.IsPlaying;
            }
        }
        protected bool IsCompleted {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be playing" ).Valid( IsPlaying );
                return Context.IsCompleted;
            }
        }
        protected bool IsPaused {
            set {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be playing" ).Valid( IsPlaying );
                Context.IsPaused = value;
            }
        }
        protected bool Mute {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be playing" ).Valid( IsPlaying );
                return Context.Mute;
            }
            set {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be playing" ).Valid( IsPlaying );
                Context.Mute = value;
            }
        }
        protected float Volume {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be playing" ).Valid( IsPlaying );
                return Context.Volume;
            }
            set {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be playing" ).Valid( IsPlaying );
                Context.Volume = value;
            }
        }
        protected float Pitch {
            get {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be playing" ).Valid( IsPlaying );
                return Context.Pitch;
            }
            set {
                Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"PlayList {this} must be playing" ).Valid( IsPlaying );
                Context.Pitch = value;
            }
        }

        // Constructor
        public UIPlayListBase(UIThemeBase context) {
            Context = context;
        }
        public virtual void Dispose() {
            Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"PlayList {this} must be deactivating or inactive" ).Valid( State is State_.Deactivating or State_.Inactive );
            disposeCancellationTokenSource?.Cancel();
            IsDisposed = true;
            Assert.Operation.Message( $"PlayList {this} must be non-playing" ).Valid( !Context.IsPlaying );
        }

        // Play
        protected void Play(AudioClip clip) {
            Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"PlayList {this} must be activating or active" ).Valid( State is State_.Activating or State_.Active );
            Assert.Operation.Message( $"PlayList {this} must be non-playing" ).Valid( !IsPlaying );
            Context.Play( clip );
        }
        protected void Stop() {
            Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Context.Stop();
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
