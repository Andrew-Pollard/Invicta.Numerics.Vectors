// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Invicta.Numerics
{
    public static unsafe partial class Vector
    {
        /// <summary>Reinterprets a <see cref="Vector2D" /> to a new <see cref="Vector3D" /> with the new elements zeroed.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted to a new <see cref="Vector3D" /> with the new elements zeroed.</returns>
        public static Vector3D AsVector3D(this Vector2D value) => value.AsVector128().AsVector3D();

        /// <summary>Reinterprets a <see cref="Vector2D" /> to a new <see cref="Vector3D" /> with the new elements undefined.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted to a new <see cref="Vector3D" /> with the new elements undefined.</returns>
        public static Vector3D AsVector3DUnsafe(this Vector2D value) => value.AsVector128Unsafe().AsVector3D();

        /// <summary>Reinterprets a <see cref="Vector2D" /> to a new <see cref="Vector4D" /> with the new elements zeroed.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted to a new <see cref="Vector4D" /> with the new elements zeroed.</returns>
        public static Vector4D AsVector4D(this Vector2D value) => value.AsVector128().AsVector4D();

        /// <summary>Reinterprets a <see cref="Vector2D" /> to a new <see cref="Vector4D" /> with the new elements undefined.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted to a new <see cref="Vector4D" /> with the new elements undefined.</returns>
        public static Vector4D AsVector4DUnsafe(this Vector2D value) => value.AsVector128Unsafe().AsVector4D();

        /// <inheritdoc cref="ExtractMostSignificantBits(Vector4D)" />
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ExtractMostSignificantBits(this Vector2D vector) => vector.AsVector128().ExtractMostSignificantBits();

        /// <inheritdoc cref="GetElement(Vector4D, int)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetElement(this Vector2D vector, int index)
        {
            if ((uint)index >= Vector2D.ElementCount)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
            }
            return vector.AsVector128Unsafe().GetElement(index);
        }

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The destination at which <paramref name="source" /> will be stored.</param>
        [CLSCompliant(false)]
        public static void Store(this Vector2D source, float* destination) => source.StoreUnsafe(ref *destination);

        /// <summary>Stores a vector at the given 8-byte aligned destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The aligned destination at which <paramref name="source" /> will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 8-byte aligned.</exception>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreAligned(this Vector2D source, float* destination)
        {
            if (((nuint)destination % (uint)(Vector2D.Alignment)) != 0)
            {
                ThrowHelper.ThrowAccessViolationException();
            }

            *(Vector2D*)destination = source;
        }

        /// <summary>Stores a vector at the given 8-byte aligned destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The aligned destination at which <paramref name="source" /> will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 8-byte aligned.</exception>
        /// <remarks>This method may bypass the cache on certain platforms.</remarks>
        [CLSCompliant(false)]
        public static void StoreAlignedNonTemporal(this Vector2D source, float* destination) => source.StoreAligned(destination);

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The destination at which <paramref name="source" /> will be stored.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreUnsafe(this Vector2D source, ref float destination)
        {
            ref byte address = ref Unsafe.As<float, byte>(ref destination);
            Unsafe.WriteUnaligned(ref address, source);
        }

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The destination to which <paramref name="elementOffset" /> will be added before the vector will be stored.</param>
        /// <param name="elementOffset">The element offset from <paramref name="destination" /> from which the vector will be stored.</param>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreUnsafe(this Vector2D source, ref float destination, nuint elementOffset)
        {
            destination = ref Unsafe.Add(ref destination, (nint)elementOffset);
            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref destination), source);
        }

        /// <inheritdoc cref="ToScalar(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToScalar(this Vector2D vector) => vector.AsVector128Unsafe().ToScalar();

        /// <inheritdoc cref="WithElement(Vector4D, int, float)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D WithElement(this Vector2D vector, int index, float value)
        {
            if ((uint)index >= Vector2D.ElementCount)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
            }
            return vector.AsVector128Unsafe().WithElement(index, value).AsVector2D();
        }
    }
}
