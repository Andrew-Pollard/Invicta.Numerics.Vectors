// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace Invicta.Numerics
{
    public static partial class Vector
    {
        /// <summary>Reinterprets a <see cref="QuaternionD" /> as a new <see cref="Vector4D" />.</summary>
        /// <param name="value">The quaternion to reinterpret.</param>
        /// <returns><paramref name="value" /> reinterpreted as a new <see cref="QuaternionD" />.</returns>
        [Intrinsic]
        public static Vector4D AsVector4D(this QuaternionD value) => Unsafe.BitCast<QuaternionD, Vector4D>(value);
    }
}
