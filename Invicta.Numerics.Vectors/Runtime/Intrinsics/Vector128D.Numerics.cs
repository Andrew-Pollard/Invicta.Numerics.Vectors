// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Invicta.Runtime.Intrinsics
{
    public static class Vector128D
    {
        extension(Vector128)
        {
            /// <inheritdoc cref="Vector4D.All(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool All(Vector2D vector, double value) => vector.AsVector128() == Vector2D.Create(value).AsVector128();

            /// <inheritdoc cref="Vector4D.All(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool All(Vector3D vector, double value) => vector.AsVector128() == Vector3D.Create(value).AsVector128();

            /// <inheritdoc cref="Vector4D.AllWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool AllWhereAllBitsSet(Vector2D vector) => vector.AsVector128().AsInt32() == Vector2D.AllBitsSet.AsVector128().AsInt32();

            /// <inheritdoc cref="Vector4D.AllWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool AllWhereAllBitsSet(Vector3D vector) => vector.AsVector128().AsInt32() == Vector3D.AllBitsSet.AsVector128().AsInt32();

            /// <inheritdoc cref="Vector4D.Any(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool Any(Vector2D vector, double value) => Vector128.EqualsAny(vector.AsVector128(), Vector128.Create(value, value, -1, -1));

            /// <inheritdoc cref="Vector4D.Any(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool Any(Vector3D vector, double value) => Vector128.EqualsAny(vector.AsVector128(), Vector128.Create(value, value, value, -1));

            /// <inheritdoc cref="Vector4D.AnyWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool AnyWhereAllBitsSet(Vector2D vector) => Vector128.EqualsAny(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet);

            /// <inheritdoc cref="Vector4D.AnyWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool AnyWhereAllBitsSet(Vector3D vector) => Vector128.EqualsAny(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet);

            /// <inheritdoc cref="Vector4D.Count(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int Count(Vector2D vector, double value) => BitOperations.PopCount(Vector128.Equals(vector.AsVector128(), Vector128.Create(value, value, -1, -1)).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.Count(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int Count(Vector3D vector, double value) => BitOperations.PopCount(Vector128.Equals(vector.AsVector128(), Vector128.Create(value, value, value, -1)).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.CountWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int CountWhereAllBitsSet(Vector2D vector) => BitOperations.PopCount(Vector128.Equals(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.CountWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int CountWhereAllBitsSet(Vector3D vector) => BitOperations.PopCount(Vector128.Equals(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.IndexOf(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int IndexOf(Vector2D vector, double value)
            {
                int result = BitOperations.TrailingZeroCount(Vector128.Equals(vector.AsVector128(), Vector128.Create(value, value, -1, -1)).ExtractMostSignificantBits());
                return (result != 32) ? result : -1;
            }

            /// <inheritdoc cref="Vector4D.IndexOf(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int IndexOf(Vector3D vector, double value)
            {
                int result = BitOperations.TrailingZeroCount(Vector128.Equals(vector.AsVector128(), Vector128.Create(value, value, value, -1)).ExtractMostSignificantBits());
                return (result != 32) ? result : -1;
            }

            /// <inheritdoc cref="Vector4D.IndexOfWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int IndexOfWhereAllBitsSet(Vector2D vector)
            {
                int result = BitOperations.TrailingZeroCount(Vector128.Equals(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet).ExtractMostSignificantBits());
                return (result != 32) ? result : -1;
            }

            /// <inheritdoc cref="Vector4D.IndexOfWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int IndexOfWhereAllBitsSet(Vector3D vector)
            {
                int result = BitOperations.TrailingZeroCount(Vector128.Equals(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet).ExtractMostSignificantBits());
                return (result != 32) ? result : -1;
            }

            /// <inheritdoc cref="Vector4D.LastIndexOf(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int LastIndexOf(Vector2D vector, double value) => 31 - BitOperations.LeadingZeroCount(Vector128.Equals(vector.AsVector128(), Vector128.Create(value, value, -1, -1)).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.LastIndexOf(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int LastIndexOf(Vector3D vector, double value) => 31 - BitOperations.LeadingZeroCount(Vector128.Equals(vector.AsVector128(), Vector128.Create(value, value, value, -1)).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.LastIndexOfWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int LastIndexOfWhereAllBitsSet(Vector2D vector) => 31 - BitOperations.LeadingZeroCount(Vector128.Equals(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.LastIndexOfWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int LastIndexOfWhereAllBitsSet(Vector3D vector) => 31 - BitOperations.LeadingZeroCount(Vector128.Equals(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.None(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool None(Vector2D vector, double value) => !Vector128.EqualsAny(vector.AsVector128(), Vector128.Create(value, value, -1, -1));

            /// <inheritdoc cref="Vector4D.None(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool None(Vector3D vector, double value) => !Vector128.EqualsAny(vector.AsVector128(), Vector128.Create(value, value, value, -1));

            /// <inheritdoc cref="Vector4D.NoneWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool NoneWhereAllBitsSet(Vector2D vector) => !Vector128.EqualsAny(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet);

            /// <inheritdoc cref="Vector4D.NoneWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool NoneWhereAllBitsSet(Vector3D vector) => !Vector128.EqualsAny(vector.AsVector128().AsInt32(), Vector128<int>.AllBitsSet);
        }
        
        /// <summary>Reinterprets a <see langword="Vector128&lt;Double&gt;" /> as a new <see cref="PlaneD" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="PlaneD" />.</returns>
        [Intrinsic]
        public static PlaneD AsPlaneD(this Vector128<double> value) => Unsafe.BitCast<Vector128<double>, PlaneD>(value);

        /// <summary>Reinterprets a <see langword="Vector128&lt;Double&gt;" /> as a new <see cref="QuaternionD" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="QuaternionD" />.</returns>
        [Intrinsic]
        public static QuaternionD AsQuaternionD(this Vector128<double> value) => Unsafe.BitCast<Vector128<double>, QuaternionD>(value);

        /// <summary>Reinterprets a <see cref="PlaneD" /> as a new <see langword="Vector128&lt;Double&gt;" />.</summary>
        /// <param name="value">The plane to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector128&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector128<double> AsVector128(this PlaneD value) => Unsafe.BitCast<PlaneD, Vector128<double>>(value);

        /// <summary>Reinterprets a <see cref="QuaternionD" /> as a new <see langword="Vector128&lt;Double&gt;" />.</summary>
        /// <param name="value">The quaternion to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector128&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector128<double> AsVector128(this QuaternionD value) => Unsafe.BitCast<QuaternionD, Vector128<double>>(value);

        /// <summary>Reinterprets a <see langword="Vector2D" /> as a new <see cref="Vector128&lt;Double&gt;" /> with the new elements zeroed.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector128&lt;Double&gt;" /> with the new elements zeroed.</returns>
        [Intrinsic]
        public static Vector128<double> AsVector128(this Vector2D value) => Vector4D.Create(value, 0, 0).AsVector128();

        /// <summary>Reinterprets a <see langword="Vector3D" /> as a new <see cref="Vector128&lt;Double&gt;" /> with the new elements zeroed.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector128&lt;Double&gt;" /> with the new elements zeroed.</returns>
        [Intrinsic]
        public static Vector128<double> AsVector128(this Vector3D value) => Vector4D.Create(value, 0).AsVector128();

        /// <summary>Reinterprets a <see langword="Vector4D" /> as a new <see cref="Vector128&lt;Double&gt;" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector128&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector128<double> AsVector128(this Vector4D value) => Unsafe.BitCast<Vector4D, Vector128<double>>(value);

        /// <summary>Reinterprets a <see cref="Vector{T}" /> as a new <see cref="Vector128{T}" />.</summary>
        /// <typeparam name="T">The type of the elements in the vector.</typeparam>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector128{T}" />.</returns>
        /// <exception cref="NotSupportedException">The type of <paramref name="value" /> (<typeparamref name="T" />) is not supported.</exception>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<T> AsVector128<T>(this Vector<T> value)
        {
            Debug.Assert(Vector<T>.Count >= Vector128<T>.Count);
            ThrowHelper.ThrowForUnsupportedIntrinsicsVector128BaseType<T>();

            ref byte address = ref Unsafe.As<Vector<T>, byte>(ref value);
            return Unsafe.ReadUnaligned<Vector128<T>>(ref address);
        }

        /// <summary>Reinterprets a <see langword="Vector2D" /> as a new <see cref="Vector128&lt;Double&gt;" />, leaving the new elements undefined.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector128&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector128<double> AsVector128Unsafe(this Vector2D value)
        {
            // This relies on us stripping the "init" flag from the ".locals"
            // declaration to let the upper bits be uninitialized.

            Unsafe.SkipInit(out Vector128<double> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<double>, byte>(ref result), value);
            return result;
        }

        /// <summary>Reinterprets a <see langword="Vector3D" /> as a new <see cref="Vector128&lt;Double&gt;" />, leaving the new elements undefined.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector128&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector128<double> AsVector128Unsafe(this Vector3D value)
        {
            // This relies on us stripping the "init" flag from the ".locals"
            // declaration to let the upper bits be uninitialized.

            Unsafe.SkipInit(out Vector128<double> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector128<double>, byte>(ref result), value);
            return result;
        }

        /// <summary>Reinterprets a <see langword="Vector128&lt;Double&gt;" /> as a new <see cref="Vector2D" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector2D" />.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D AsVector2D(this Vector128<double> value)
        {
            ref byte address = ref Unsafe.As<Vector128<double>, byte>(ref value);
            return Unsafe.ReadUnaligned<Vector2D>(ref address);
        }

        /// <summary>Reinterprets a <see langword="Vector128&lt;Double&gt;" /> as a new <see cref="Vector3D" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector3D" />.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D AsVector3D(this Vector128<double> value)
        {
            ref byte address = ref Unsafe.As<Vector128<double>, byte>(ref value);
            return Unsafe.ReadUnaligned<Vector3D>(ref address);
        }

        /// <summary>Reinterprets a <see langword="Vector128&lt;Double&gt;" /> as a new <see cref="Vector4D" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector4D" />.</returns>
        [Intrinsic]
        public static Vector4D AsVector4D(this Vector128<double> value) => Unsafe.BitCast<Vector128<double>, Vector4D>(value);

        /// <summary>Reinterprets a <see cref="Vector128{T}" /> as a new <see cref="Vector{T}" />.</summary>
        /// <typeparam name="T">The type of the elements in the vector.</typeparam>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector128{T}" />.</returns>
        /// <exception cref="NotSupportedException">The type of <paramref name="value" /> (<typeparamref name="T" />) is not supported.</exception>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<T> AsVector<T>(this Vector128<T> value)
        {
            Debug.Assert(Vector<T>.Count >= Vector128<T>.Count);
            ThrowHelper.ThrowForUnsupportedIntrinsicsVector128BaseType<T>();

            Vector<T> result = default;
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector<T>, byte>(ref result), value);
            return result;
        }
    }
}
