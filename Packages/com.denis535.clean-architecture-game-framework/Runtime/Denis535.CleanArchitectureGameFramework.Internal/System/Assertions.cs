﻿#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public static class Assertions {
        // IAssertion
        private interface IAssertion {
            FormattableString? Message { get; }
        }
        // Argument
        public readonly struct Argument : IAssertion {

            public FormattableString? Message { get; }

            public Argument(FormattableString? message) {
                Message = message;
            }

            [MethodImpl( MethodImplOptions.AggressiveInlining )]
            public void Valid([DoesNotReturnIf( false )] bool isValid) {
                if (!isValid) throw Exceptions.GetException<ArgumentException>( Message );
            }
            [MethodImpl( MethodImplOptions.AggressiveInlining )]
            public void NotNull([DoesNotReturnIf( false )] bool isValid) {
                if (!isValid) throw Exceptions.GetException<ArgumentNullException>( Message );
            }
            [MethodImpl( MethodImplOptions.AggressiveInlining )]
            public void InRange([DoesNotReturnIf( false )] bool isValid) {
                if (!isValid) throw Exceptions.GetException<ArgumentOutOfRangeException>( Message );
            }

            public override string? ToString() {
                return Exceptions.GetMessageStringDelegate( Message );
            }

        }
        // Operation
        public readonly struct Operation : IAssertion {

            public FormattableString? Message { get; }

            public Operation(FormattableString? message) {
                Message = message;
            }

            [MethodImpl( MethodImplOptions.AggressiveInlining )]
            public void Valid([DoesNotReturnIf( false )] bool isValid) {
                if (!isValid) throw Exceptions.GetException<InvalidOperationException>( Message );
            }
            [MethodImpl( MethodImplOptions.AggressiveInlining )]
            public void Ready([DoesNotReturnIf( false )] bool isValid) {
                if (!isValid) throw Exceptions.GetException<ObjectNotReadyException>( Message );
            }
            [MethodImpl( MethodImplOptions.AggressiveInlining )]
            public void NotDisposed([DoesNotReturnIf( false )] bool isValid) {
                if (!isValid) throw Exceptions.GetException<ObjectDisposedException>( Message );
            }

            public override string? ToString() {
                return Exceptions.GetMessageStringDelegate( Message );
            }

        }
    }
}
