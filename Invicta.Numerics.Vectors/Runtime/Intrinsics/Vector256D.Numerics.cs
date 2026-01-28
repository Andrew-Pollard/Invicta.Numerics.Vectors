// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Invicta.Runtime.Intrinsics
{
    public static partial class Vector256D
    {
        extension(Vector256)
        {
            /// <inheritdoc cref="Vector4D.All(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool All(Vector2D vector, double value) => vector.AsVector256() == Vector2D.Create(value).AsVector256();

            /// <inheritdoc cref="Vector4D.All(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool All(Vector3D vector, double value) => vector.AsVector256() == Vector3D.Create(value).AsVector256();

            /// <inheritdoc cref="Vector4D.AllWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool AllWhereAllBitsSet(Vector2D vector) => vector.AsVector256().AsInt64() == Vector2D.AllBitsSet.AsVector256().AsInt64();

            /// <inheritdoc cref="Vector4D.AllWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool AllWhereAllBitsSet(Vector3D vector) => vector.AsVector256().AsInt64() == Vector3D.AllBitsSet.AsVector256().AsInt64();

            /// <inheritdoc cref="Vector4D.Any(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool Any(Vector2D vector, double value) => Vector256.EqualsAny(vector.AsVector256(), Vector256.Create(value, value, -1, -1));

            /// <inheritdoc cref="Vector4D.Any(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool Any(Vector3D vector, double value) => Vector256.EqualsAny(vector.AsVector256(), Vector256.Create(value, value, value, -1));

            /// <inheritdoc cref="Vector4D.AnyWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool AnyWhereAllBitsSet(Vector2D vector) => Vector256.EqualsAny(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet);

            /// <inheritdoc cref="Vector4D.AnyWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool AnyWhereAllBitsSet(Vector3D vector) => Vector256.EqualsAny(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet);

            /// <inheritdoc cref="Vector4D.Count(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int Count(Vector2D vector, double value) => BitOperations.PopCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, -1, -1)).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.Count(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int Count(Vector3D vector, double value) => BitOperations.PopCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, value, -1)).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.CountWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int CountWhereAllBitsSet(Vector2D vector) => BitOperations.PopCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.CountWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int CountWhereAllBitsSet(Vector3D vector) => BitOperations.PopCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.IndexOf(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int IndexOf(Vector2D vector, double value)
            {
                int result = BitOperations.TrailingZeroCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, -1, -1)).ExtractMostSignificantBits());
                return (result != 32) ? result : -1;
            }

            /// <inheritdoc cref="Vector4D.IndexOf(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int IndexOf(Vector3D vector, double value)
            {
                int result = BitOperations.TrailingZeroCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, value, -1)).ExtractMostSignificantBits());
                return (result != 32) ? result : -1;
            }

            /// <inheritdoc cref="Vector4D.IndexOfWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int IndexOfWhereAllBitsSet(Vector2D vector)
            {
                int result = BitOperations.TrailingZeroCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());
                return (result != 32) ? result : -1;
            }

            /// <inheritdoc cref="Vector4D.IndexOfWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int IndexOfWhereAllBitsSet(Vector3D vector)
            {
                int result = BitOperations.TrailingZeroCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());
                return (result != 32) ? result : -1;
            }

            /// <inheritdoc cref="Vector4D.LastIndexOf(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int LastIndexOf(Vector2D vector, double value) => 31 - BitOperations.LeadingZeroCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, -1, -1)).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.LastIndexOf(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int LastIndexOf(Vector3D vector, double value) => 31 - BitOperations.LeadingZeroCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, value, -1)).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.LastIndexOfWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int LastIndexOfWhereAllBitsSet(Vector2D vector) => 31 - BitOperations.LeadingZeroCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.LastIndexOfWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static int LastIndexOfWhereAllBitsSet(Vector3D vector) => 31 - BitOperations.LeadingZeroCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());

            /// <inheritdoc cref="Vector4D.None(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool None(Vector2D vector, double value) => !Vector256.EqualsAny(vector.AsVector256(), Vector256.Create(value, value, -1, -1));

            /// <inheritdoc cref="Vector4D.None(Vector4D, double)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool None(Vector3D vector, double value) => !Vector256.EqualsAny(vector.AsVector256(), Vector256.Create(value, value, value, -1));

            /// <inheritdoc cref="Vector4D.NoneWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool NoneWhereAllBitsSet(Vector2D vector) => !Vector256.EqualsAny(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet);

            /// <inheritdoc cref="Vector4D.NoneWhereAllBitsSet(Vector4D)" />
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool NoneWhereAllBitsSet(Vector3D vector) => !Vector256.EqualsAny(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet);
        }
        
        /// <summary>Reinterprets a <see langword="Vector256&lt;Double&gt;" /> as a new <see cref="PlaneD" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="PlaneD" />.</returns>
        [Intrinsic]
        public static PlaneD AsPlaneD(this Vector256<double> value) => Unsafe.BitCast<Vector256<double>, PlaneD>(value);

        /// <summary>Reinterprets a <see langword="Vector256&lt;Double&gt;" /> as a new <see cref="QuaternionD" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="QuaternionD" />.</returns>
        [Intrinsic]
        public static QuaternionD AsQuaternionD(this Vector256<double> value) => Unsafe.BitCast<Vector256<double>, QuaternionD>(value);

        /// <summary>Reinterprets a <see cref="PlaneD" /> as a new <see langword="Vector256&lt;Double&gt;" />.</summary>
        /// <param name="value">The plane to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector256&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector256<double> AsVector256(this PlaneD value) => Unsafe.BitCast<PlaneD, Vector256<double>>(value);

        /// <summary>Reinterprets a <see cref="QuaternionD" /> as a new <see langword="Vector256&lt;Double&gt;" />.</summary>
        /// <param name="value">The quaternion to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector256&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector256<double> AsVector256(this QuaternionD value) => Unsafe.BitCast<QuaternionD, Vector256<double>>(value);

        /// <summary>Reinterprets a <see langword="Vector2D" /> as a new <see cref="Vector256&lt;Double&gt;" /> with the new elements zeroed.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector256&lt;Double&gt;" /> with the new elements zeroed.</returns>
        [Intrinsic]
        public static Vector256<double> AsVector256(this Vector2D value) => Vector4D.Create(value, 0, 0).AsVector256();

        /// <summary>Reinterprets a <see langword="Vector3D" /> as a new <see cref="Vector256&lt;Double&gt;" /> with the new elements zeroed.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector256&lt;Double&gt;" /> with the new elements zeroed.</returns>
        [Intrinsic]
        public static Vector256<double> AsVector256(this Vector3D value) => Vector4D.Create(value, 0).AsVector256();

        /// <summary>Reinterprets a <see langword="Vector4D" /> as a new <see cref="Vector256&lt;Double&gt;" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector256&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector256<double> AsVector256(this Vector4D value) => Unsafe.BitCast<Vector4D, Vector256<double>>(value);

        /// <summary>Reinterprets a <see cref="Vector{T}" /> as a new <see cref="Vector256{T}" />.</summary>
        /// <typeparam name="T">The type of the elements in the vector.</typeparam>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector256{T}" />.</returns>
        /// <exception cref="NotSupportedException">The type of <paramref name="value" /> (<typeparamref name="T" />) is not supported.</exception>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<T> AsVector256<T>(this Vector<T> value)
        {
            Debug.Assert(Vector<T>.Count >= Vector256<T>.Count);
            ThrowHelper.ThrowForUnsupportedIntrinsicsVector256BaseType<T>();

            ref byte address = ref Unsafe.As<Vector<T>, byte>(ref value);
            return Unsafe.ReadUnaligned<Vector256<T>>(ref address);
        }

        /// <summary>Reinterprets a <see langword="Vector2D" /> as a new <see cref="Vector256&lt;Double&gt;" />, leaving the new elements undefined.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector256&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector256<double> AsVector256Unsafe(this Vector2D value)
        {
            // This relies on us stripping the "init" flag from the ".locals"
            // declaration to let the upper bits be uninitialized.

            Unsafe.SkipInit(out Vector256<double> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<double>, byte>(ref result), value);
            return result;
        }

        /// <summary>Reinterprets a <see langword="Vector3D" /> as a new <see cref="Vector256&lt;Double&gt;" />, leaving the new elements undefined.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see langword="Vector256&lt;Double&gt;" />.</returns>
        [Intrinsic]
        public static Vector256<double> AsVector256Unsafe(this Vector3D value)
        {
            // This relies on us stripping the "init" flag from the ".locals"
            // declaration to let the upper bits be uninitialized.

            Unsafe.SkipInit(out Vector256<double> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<double>, byte>(ref result), value);
            return result;
        }

        /// <summary>Reinterprets a <see langword="Vector256&lt;Double&gt;" /> as a new <see cref="Vector2D" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector2D" />.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D AsVector2D(this Vector256<double> value)
        {
            ref byte address = ref Unsafe.As<Vector256<double>, byte>(ref value);
            return Unsafe.ReadUnaligned<Vector2D>(ref address);
        }

        /// <summary>Reinterprets a <see langword="Vector256&lt;Double&gt;" /> as a new <see cref="Vector3D" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector3D" />.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D AsVector3D(this Vector256<double> value)
        {
            ref byte address = ref Unsafe.As<Vector256<double>, byte>(ref value);
            return Unsafe.ReadUnaligned<Vector3D>(ref address);
        }

        /// <summary>Reinterprets a <see langword="Vector256&lt;Double&gt;" /> as a new <see cref="Vector4D" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector4D" />.</returns>
        [Intrinsic]
        public static Vector4D AsVector4D(this Vector256<double> value) => Unsafe.BitCast<Vector256<double>, Vector4D>(value);

        /// <summary>Reinterprets a <see cref="Vector256{T}" /> as a new <see cref="Vector{T}" />.</summary>
        /// <typeparam name="T">The type of the elements in the vector.</typeparam>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector256{T}" />.</returns>
        /// <exception cref="NotSupportedException">The type of <paramref name="value" /> (<typeparamref name="T" />) is not supported.</exception>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<T> AsVector<T>(this Vector256<T> value)
        {
            Debug.Assert(Vector<T>.Count >= Vector256<T>.Count);
            ThrowHelper.ThrowForUnsupportedIntrinsicsVector256BaseType<T>();

            Vector<T> result = default;
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector<T>, byte>(ref result), value);
            return result;
        }
    }
}
