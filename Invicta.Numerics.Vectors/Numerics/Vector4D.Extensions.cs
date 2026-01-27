// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Invicta.Numerics
{
    public static unsafe partial class Vector
    {
        /// <summary>Reinterprets a <see cref="Vector4D" /> as a new <see cref="PlaneD" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="PlaneD" />.</returns>
        public static PlaneD AsPlaneD(this Vector4D value) => Unsafe.BitCast<Vector4D, PlaneD>(value);

        /// <summary>Reinterprets a <see cref="Vector4D" /> as a new <see cref="QuaternionD" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="QuaternionD" />.</returns>
        public static QuaternionD AsQuaternionD(this Vector4D value) => Unsafe.BitCast<Vector4D, QuaternionD>(value);

        /// <summary>Reinterprets a <see cref="Vector4D" /> as a new <see cref="Vector2D" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector2D" />.</returns>
        public static Vector2D AsVector2D(this Vector4D value) => value.AsVector256().AsVector2D();

        /// <summary>Reinterprets a <see cref="Vector4D" /> as a new <see cref="Vector3D" />.</summary>
        /// <param name="value">The vector to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector3D" />.</returns>
        public static Vector3D AsVector3D(this Vector4D value) => value.AsVector256().AsVector3D();

        /// <inheritdoc cref="Vector256.ExtractMostSignificantBits{T}(Vector256{T})" />
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ExtractMostSignificantBits(this Vector4D vector) => vector.AsVector256().ExtractMostSignificantBits();

        /// <inheritdoc cref="Vector256.GetElement{T}(Vector256{T}, int)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetElement(this Vector4D vector, int index) => vector.AsVector256().GetElement(index);

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The destination at which <paramref name="source" /> will be stored.</param>
        [CLSCompliant(false)]
        public static void Store(this Vector4D source, double* destination) => source.AsVector256().Store(destination);

        /// <summary>Stores a vector at the given 16-byte aligned destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The aligned destination at which <paramref name="source" /> will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 16-byte aligned.</exception>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreAligned(this Vector4D source, double* destination) => source.AsVector256().StoreAligned(destination);

        /// <summary>Stores a vector at the given 16-byte aligned destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The aligned destination at which <paramref name="source" /> will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 16-byte aligned.</exception>
        /// <remarks>This method may bypass the cache on certain platforms.</remarks>
        [CLSCompliant(false)]
        public static void StoreAlignedNonTemporal(this Vector4D source, double* destination) => source.AsVector256().StoreAlignedNonTemporal(destination);

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The destination at which <paramref name="source" /> will be stored.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreUnsafe(this Vector4D source, ref double destination) => source.AsVector256().StoreUnsafe(ref destination);

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="source">The vector that will be stored.</param>
        /// <param name="destination">The destination to which <paramref name="elementOffset" /> will be added before the vector will be stored.</param>
        /// <param name="elementOffset">The element offset from <paramref name="destination" /> from which the vector will be stored.</param>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreUnsafe(this Vector4D source, ref double destination, nuint elementOffset) => source.AsVector256().StoreUnsafe(ref destination, elementOffset);

        /// <inheritdoc cref="Vector256.ToScalar{T}(Vector256{T})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToScalar(this Vector4D vector) => vector.AsVector256().ToScalar();

        /// <inheritdoc cref="Vector256.WithElement{T}(Vector256{T}, int, T)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D WithElement(this Vector4D vector, int index, double value) => vector.AsVector256().WithElement(index, value).AsVector4D();
    }
}
