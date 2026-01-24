// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Invicta.Numerics
{
    /// <summary>Represents a plane in three-dimensional space.</summary>
    /// <remarks><format type="text/markdown"><![CDATA[
    /// [!INCLUDE[vectors-are-rows-paragraph](~/includes/system-numerics-vectors-are-rows.md)]
    /// ]]></format></remarks>
    [Intrinsic]
    public struct PlaneD : IEquatable<PlaneD>
    {
        /// <summary>The normal vector of the plane.</summary>
        public Vector3D Normal;

        /// <summary>The distance of the plane along its normal from the origin.</summary>
        public float D;

        /// <summary>Initializes a <see cref="PlaneD" /> from the X, Y, and Z components of its normal, and its distance from the origin on that normal.</summary>
        /// <param name="x">The X component of the normal.</param>
        /// <param name="y">The Y component of the normal.</param>
        /// <param name="z">The Z component of the normal.</param>
        /// <param name="d">The distance of the plane along its normal from the origin.</param>
        [Intrinsic]
        public PlaneD(float x, float y, float z, float d)
        {
            this = Create(x, y, z, d);
        }

        /// <summary>Initializes a <see cref="PlaneD" /> from a specified normal and the distance along the normal from the origin.</summary>
        /// <param name="normal">The plane's normal vector.</param>
        /// <param name="d">The plane's distance from the origin along its normal vector.</param>
        [Intrinsic]
        public PlaneD(Vector3D normal, float d)
        {
            this = Create(normal, d);
        }

        /// <summary>Initializes a <see cref="PlaneD" /> from a specified four-dimensional vector.</summary>
        /// <param name="value">A vector whose first three elements describe the normal vector, and whose <see cref="Vector4.W" /> defines the distance along that normal from the origin.</param>
        [Intrinsic]
        public PlaneD(Vector4 value)
        {
            this = value.AsPlane();
        }

        /// <summary>Creates a <see cref="PlaneD" /> from a specified four-dimensional vector.</summary>
        /// <param name="value">A vector whose first three elements describe the normal vector, and whose <see cref="Vector4.W" /> defines the distance along that normal from the origin.</param>
        /// <remarks>A <see cref="PlaneD" /> created using <paramref name="value" />.</remarks>
        [Intrinsic]
        public static PlaneD Create(Vector4 value) => value.AsPlane();

        /// <summary>Creates a <see cref="PlaneD" /> from a specified normal and the distance along the normal from the origin.</summary>
        /// <param name="normal">The plane's normal vector.</param>
        /// <param name="d">The plane's distance from the origin along its normal vector.</param>\
        /// <returns>A <see cref="PlaneD" /> created from a specified normal and the distance along the normal from the origin.</returns>
        [Intrinsic]
        public static PlaneD Create(Vector3D normal, float d) => Vector4.Create(normal, d).AsPlane();

        /// <summary>Creates a <see cref="PlaneD" /> from the X, Y, and Z components of its normal, and its distance from the origin on that normal.</summary>
        /// <param name="x">The X component of the normal.</param>
        /// <param name="y">The Y component of the normal.</param>
        /// <param name="z">The Z component of the normal.</param>
        /// <param name="d">The distance of the plane along its normal from the origin.</param>
        /// <returns>A <see cref="PlaneD" /> created from the X, Y, and Z components of its normal, and its distance from the origin on that normal.</returns>
        [Intrinsic]
        public static PlaneD Create(float x, float y, float z, float d) => Vector128.Create(x, y, z, d).AsPlane();

        /// <summary>Creates a <see cref="PlaneD" /> object that contains three specified points.</summary>
        /// <param name="point1">The first point defining the plane.</param>
        /// <param name="point2">The second point defining the plane.</param>
        /// <param name="point3">The third point defining the plane.</param>
        /// <returns>The plane containing the three points.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PlaneD CreateFromVertices(Vector3D point1, Vector3D point2, Vector3D point3)
        {
            // This implementation is based on the DirectX Math Library XMPlaneFromPoints method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            Vector3D normal = Vector3D.Normalize(Vector3D.Cross(point2 - point1, point3 - point1));

            return Create(
                normal,
                -Vector3D.Dot(normal, point1)
            );
        }

        /// <summary>Calculates the dot product of a plane and a 4-dimensional vector.</summary>
        /// <param name="plane">The plane.</param>
        /// <param name="value">The four-dimensional vector.</param>
        /// <returns>The dot product.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(PlaneD plane, Vector4 value) => Vector128.Dot(plane.AsVector128(), value.AsVector128());

        /// <summary>Returns the dot product of a specified three-dimensional vector and the normal vector of this plane plus the distance (<see cref="D" />) value of the plane.</summary>
        /// <param name="plane">The plane.</param>
        /// <param name="value">The 3-dimensional vector.</param>
        /// <returns>The dot product.</returns>
        public static float DotCoordinate(PlaneD plane, Vector3D value)
        {
            // This implementation is based on the DirectX Math Library XMPlaneDotCoord method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            return Dot(plane, Vector4.Create(value, 1.0f));
        }

        /// <summary>Returns the dot product of a specified three-dimensional vector and the <see cref="Normal" /> vector of this plane.</summary>
        /// <param name="plane">The plane.</param>
        /// <param name="value">The three-dimensional vector.</param>
        /// <returns>The dot product.</returns>
        public static float DotNormal(PlaneD plane, Vector3D value)
        {
            // This implementation is based on the DirectX Math Library XMPlaneDotNormal method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            return Vector3D.Dot(plane.Normal, value);
        }

        /// <summary>Creates a new <see cref="PlaneD" /> object whose normal vector is the source plane's normal vector normalized.</summary>
        /// <param name="value">The source plane.</param>
        /// <returns>The normalized plane.</returns>
        public static PlaneD Normalize(PlaneD value)
        {
            // This implementation is based on the DirectX Math Library XMPlaneNormalize method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            Vector128<float> lengthSquared = Vector128.Create(value.Normal.LengthSquared());

            return Vector128.AndNot(
                (value.AsVector128() / Vector128.Sqrt(lengthSquared)),
                Vector128.Equals(lengthSquared, Vector128.Create(float.PositiveInfinity))
            ).AsPlane();
        }

        /// <summary>Transforms a normalized plane by a 4x4 matrix.</summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="matrix">The transformation matrix to apply to <paramref name="plane" />.</param>
        /// <returns>The transformed plane.</returns>
        /// <remarks><paramref name="plane" /> must already be normalized so that its <see cref="Normal" /> vector is of unit length before this method is called.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PlaneD Transform(PlaneD plane, Matrix4x4D matrix)
        {
            Matrix4x4D.Invert(matrix, out Matrix4x4D inverseMatrix);
            return Vector4.Transform(plane.AsVector4(), Matrix4x4D.Transpose(inverseMatrix)).AsPlane();
        }

        /// <summary>Transforms a normalized plane by a QuaternionD rotation.</summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="rotation">The QuaternionD rotation to apply to the plane.</param>
        /// <returns>A new plane that results from applying the QuaternionD rotation.</returns>
        /// <remarks><paramref name="plane" /> must already be normalized so that its <see cref="Normal" /> vector is of unit length before this method is called.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PlaneD Transform(PlaneD plane, QuaternionD rotation) => Vector4.Transform(plane.AsVector4(), rotation).AsPlane();

        /// <summary>Returns a value that indicates whether two planes are equal.</summary>
        /// <param name="value1">The first plane to compare.</param>
        /// <param name="value2">The second plane to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="value1" /> and <paramref name="value2" /> are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="PlaneD" /> objects are equal if their <see cref="Normal" /> and <see cref="D" /> fields are equal.
        /// The <see cref="op_Equality" /> method defines the operation of the equality operator for <see cref="PlaneD" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PlaneD value1, PlaneD value2) => value1.AsVector128() == value2.AsVector128();

        /// <summary>Returns a value that indicates whether two planes are not equal.</summary>
        /// <param name="value1">The first plane to compare.</param>
        /// <param name="value2">The second plane to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="value1" /> and <paramref name="value2" /> are not equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>The <see cref="op_Inequality" /> method defines the operation of the inequality operator for <see cref="PlaneD" /> objects.</remarks>
        [Intrinsic]
        public static bool operator !=(PlaneD value1, PlaneD value2) => !(value1 == value2);

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="PlaneD" /> object and their <see cref="Normal" /> and <see cref="D" /> fields are equal.</remarks>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is PlaneD other) && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another plane object are equal.</summary>
        /// <param name="other">The other plane.</param>
        /// <returns><see langword="true" /> if the two planes are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="PlaneD" /> objects are equal if their <see cref="Normal" /> and <see cref="D" /> fields are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(PlaneD other) => this.AsVector128().Equals(other.AsVector128());

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(Normal, D);

        /// <summary>Returns the string representation of this plane object.</summary>
        /// <returns>A string that represents this <see cref="PlaneD" /> object.</returns>
        /// <remarks>The string representation of a <see cref="PlaneD" /> object use the formatting conventions of the current culture to format the numeric values in the returned string. For example, a <see cref="PlaneD" /> object whose string representation is formatted by using the conventions of the en-US culture might appear as <c>{Normal:&lt;1.1, 2.2, 3.3&gt; D:4.4}</c>.</remarks>
        public override readonly string ToString() => $"{{Normal:{Normal} D:{D}}}";
    }
}
