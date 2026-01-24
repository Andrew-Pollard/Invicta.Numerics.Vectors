// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Runtime.CompilerServices;

namespace Invicta.Numerics
{
    public static partial class Vector
    {
        /// <summary>Reinterprets a <see cref="PlaneD" /> as a new <see cref="Vector4D" />.</summary>
        /// <param name="value">The plane to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="Vector4D" />.</returns>
        [Intrinsic]
        public static Vector4D AsVector4D(this PlaneD value) => Unsafe.BitCast<PlaneD, Vector4D>(value);
    }
}
