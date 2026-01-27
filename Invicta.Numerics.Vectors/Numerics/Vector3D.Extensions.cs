// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Invicta.Numerics
{
    public static unsafe partial class Vector
    {
        /// <summary>Reinterprets a <see cref="Vector3D" /> as a new <see cref="Vector2D" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector2D" />.</returns>
        public static Vector2D AsVector2D(this Vector3D value) => value.AsVector128().AsVector2D();

        /// <summary>Converts a <see cref="Vector3D" /> to a new <see cref="Vector4D" /> with the new elements zeroed.</summary>
        /// <param name="value">The vector to convert.</param>
        /// <returns><paramref name="value" /> converted to a new <see cref="Vector4D" /> with the new elements zeroed.</returns>
        public static Vector4D AsVector4D(this Vector3D value) => value.AsVector128().AsVector4D();

        /// <summary>Converts a <see cref="Vector3D" /> to a new <see cref="Vector4D" /> with the new elements undefined.</summary>
        /// <param name="value">The vector to convert.</param>
        /// <returns><paramref name="value" /> converted to a new <see cref="Vector4D" /> with the new elements undefined.</returns>
        public static Vector4D AsVector4DUnsafe(this Vector3D value) => value.AsVector128Unsafe().AsVector4D();

        /// <inheritdoc cref="ExtractMostSignificantBits(Vector4D)" />
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ExtractMostSignificantBits(this Vector3D vector) => vector.AsVector128().ExtractMostSignificantBits();

        /// <inheritdoc cref="GetElement(Vector4D, int)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetElement(this Vector3D vector, int index)
        {
            if ((uint)index >= Vector3D.ElementCount)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
            }
            return vector.AsVector128Unsafe().GetElement(index);
        }

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The destination at which <paramref name="source" /> will be stored.</param>
        [CLSCompliant(false)]
        public static void Store(this Vector3D source, double* destination) => source.StoreUnsafe(ref *destination);

        /// <summary>Stores a vector at the given 8-byte aligned destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The aligned destination at which <paramref name="source" /> will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 8-byte aligned.</exception>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreAligned(this Vector3D source, double* destination)
        {
            if (((nuint)destination % (uint)(Vector3D.Alignment)) != 0)
            {
                ThrowHelper.ThrowAccessViolationException();
            }

            *(Vector3D*)destination = source;
        }

        /// <summary>Stores a vector at the given 8-byte aligned destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The aligned destination at which <paramref name="source" /> will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 8-byte aligned.</exception>
        /// <remarks>This method may bypass the cache on certain platforms.</remarks>
        [CLSCompliant(false)]
        public static void StoreAlignedNonTemporal(this Vector3D source, double* destination) => source.StoreAligned(destination);

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The destination at which <paramref name="source" /> will be stored.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreUnsafe(this Vector3D source, ref double destination)
        {
            ref byte address = ref Unsafe.As<double, byte>(ref destination);
            Unsafe.WriteUnaligned(ref address, source);
        }

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The destination to which <paramref name="elementOffset" /> will be added before the vector will be stored.</param>
        /// <param name="elementOffset">The element offset from <paramref name="destination" /> from which the vector will be stored.</param>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreUnsafe(this Vector3D source, ref double destination, nuint elementOffset)
        {
            destination = ref Unsafe.Add(ref destination, (nint)elementOffset);
            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref destination), source);
        }

        /// <inheritdoc cref="ToScalar(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToScalar(this Vector3D vector) => vector.AsVector128Unsafe().ToScalar();

        /// <inheritdoc cref="WithElement(Vector4D, int, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D WithElement(this Vector3D vector, int index, double value)
        {
            if ((uint)index >= Vector3D.ElementCount)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
            }
            return vector.AsVector128Unsafe().WithElement(index, value).AsVector3D();
        }
    }
}
