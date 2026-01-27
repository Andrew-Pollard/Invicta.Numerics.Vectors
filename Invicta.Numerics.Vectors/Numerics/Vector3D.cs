// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Invicta.Numerics
{
    /// <summary>Represents a vector with three  double-precision floating-point values.</summary>
    /// <remarks><format type="text/markdown"><![CDATA[
    /// The <xref:System.Numerics.Vector3D> structure provides support for hardware acceleration.
    /// [!INCLUDE[vectors-are-rows-paragraph](~/includes/system-numerics-vectors-are-rows.md)]
    /// ]]></format></remarks>
    [Intrinsic]
    public partial struct Vector3D : IEquatable<Vector3D>, IFormattable
    {
        /// <summary>Specifies the alignment of the vector as used by the <see cref="LoadAligned(double*)" /> and <see cref="Vector.StoreAligned(Vector3D, double*)" /> APIs.</summary>
        /// <remarks>
        ///     <para>
        ///       Different environments all have their own concepts of alignment/packing.
        ///       For example, a <c>Vector3D</c> in .NET is 4-byte aligned and 12-bytes in size,
        ///       in GLSL a <c>vec3</c> is 16-byte aligned and 16-byte sized, while in HLSL a
        ///       <c>float3</c> is functionally 8-byte aligned and 12-byte sized. These differences
        ///       make it impossible to define a "correct" alignment; additionally, the nuance
        ///       in environments like HLSL where size is not a multiple of alignment introduce complications.
        ///     </para>
        ///     <para>
        ///       For the purposes of the <c>LoadAligned</c> and <c>StoreAligned</c> APIs we
        ///       therefore pick a value that allows for a broad range of compatibility while
        ///       also allowing more optimal codegen for various target platforms.
        ///     </para>
        /// </remarks>
        internal const int Alignment = 8;

        /// <summary>The X component of the vector.</summary>
        public double X;

        /// <summary>The Y component of the vector.</summary>
        public double Y;

        /// <summary>The Z component of the vector.</summary>
        public double Z;

        internal const int ElementCount = 3;

        /// <summary>Creates a new <see cref="Vector3D" /> object whose three elements have the same value.</summary>
        /// <param name="value">The value to assign to all three elements.</param>
        [Intrinsic]
        public Vector3D(double value)
        {
            this = Create(value);
        }

        /// <summary>Creates a   new <see cref="Vector3D" /> object from the specified <see cref="Vector2D" /> object and the specified value.</summary>
        /// <param name="value">The vector with two elements.</param>
        /// <param name="z">The additional value to assign to the <see cref="Z" /> field.</param>
        [Intrinsic]
        public Vector3D(Vector2D value, double z)
        {
            this = Create(value, z);
        }

        /// <summary>Creates a vector whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
        /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
        [Intrinsic]
        public Vector3D(double x, double y, double z)
        {
            this = Create(x, y, z);
        }

        /// <summary>Constructs a vector from the given <see cref="ReadOnlySpan{Double}" />. The span must contain at least 3 elements.</summary>
        /// <param name="values">The span of elements to assign to the vector.</param>
        [Intrinsic]
        public Vector3D(ReadOnlySpan<double> values)
        {
            this = Create(values);
        }

        /// <inheritdoc cref="Vector4D.AllBitsSet" />
        public static Vector3D AllBitsSet
        {
            [Intrinsic]
            get => Vector128<double>.AllBitsSet.AsVector3D();
        }

        /// <inheritdoc cref="Vector4D.E" />
        public static Vector3D E
        {
            [Intrinsic]
            get => Create(double.E);
        }

        /// <inheritdoc cref="Vector4D.Epsilon" />
        public static Vector3D Epsilon
        {
            [Intrinsic]
            get => Create(double.Epsilon);
        }

        /// <inheritdoc cref="Vector4D.NaN" />
        public static Vector3D NaN
        {
            [Intrinsic]
            get => Create(double.NaN);
        }

        /// <inheritdoc cref="Vector4D.NegativeInfinity" />
        public static Vector3D NegativeInfinity
        {
            [Intrinsic]
            get => Create(double.NegativeInfinity);
        }

        /// <inheritdoc cref="Vector4D.NegativeZero" />
        public static Vector3D NegativeZero
        {
            [Intrinsic]
            get => Create(double.NegativeZero);
        }

        /// <inheritdoc cref="Vector4D.One" />
        public static Vector3D One
        {
            [Intrinsic]
            get => Create(1.0d);
        }

        /// <inheritdoc cref="Vector4D.Pi" />
        public static Vector3D Pi
        {
            [Intrinsic]
            get => Create(double.Pi);
        }

        /// <inheritdoc cref="Vector4D.PositiveInfinity" />
        public static Vector3D PositiveInfinity
        {
            [Intrinsic]
            get => Create(double.PositiveInfinity);
        }

        /// <inheritdoc cref="Vector4D.Tau" />
        public static Vector3D Tau
        {
            [Intrinsic]
            get => Create(double.Tau);
        }

        /// <summary>Gets the vector (1,0,0).</summary>
        /// <value>The vector <c>(1,0,0)</c>.</value>
        public static Vector3D UnitX
        {
            [Intrinsic]
            get => CreateScalar(1.0d);
        }

        /// <summary>Gets the vector (0,1,0).</summary>
        /// <value>The vector <c>(0,1,0)</c>.</value>
        public static Vector3D UnitY
        {
            [Intrinsic]
            get => Create(0.0d, 1.0d, 0.0d);
        }

        /// <summary>Gets the vector (0,0,1).</summary>
        /// <value>The vector <c>(0,0,1)</c>.</value>
        public static Vector3D UnitZ
        {
            [Intrinsic]
            get => Create(0.0d, 0.0d, 1.0d);
        }

        /// <inheritdoc cref="Vector4D.Zero" />
        public static Vector3D Zero
        {
            [Intrinsic]
            get => default;
        }

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <returns>The the element at <paramref name="index" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> was less than zero or greater than the number of elements.</exception>
        public double this[int index]
        {
            [Intrinsic]
            readonly get => this.GetElement(index);

            [Intrinsic]
            set
            {
                this = this.WithElement(index, value);
            }
        }

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        /// <remarks>The <see cref="op_Addition" /> method defines the addition operation for <see cref="Vector3D" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator +(Vector3D left, Vector3D right) => (left.AsVector128Unsafe() + right.AsVector128Unsafe()).AsVector3D();

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        /// <remarks>The <see cref="Vector3D.op_Division" /> method defines the division operation for <see cref="Vector3D" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator /(Vector3D left, Vector3D right) => (left.AsVector128Unsafe() / right.AsVector128Unsafe()).AsVector3D();

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="value1">The vector.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        /// <remarks>The <see cref="Vector3D.op_Division" /> method defines the division operation for <see cref="Vector3D" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator /(Vector3D value1, double value2) => (value1.AsVector128Unsafe() / value2).AsVector3D();

        /// <summary>Returns a value that indicates whether each pair of elements in two specified vectors is equal.</summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="Vector3D" /> objects are equal if each element in <paramref name="left" /> is equal to the corresponding element in <paramref name="right" />.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3D left, Vector3D right) => left.AsVector128() == right.AsVector128();

        /// <summary>Returns a value that indicates whether two specified vectors are not equal.</summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
        [Intrinsic]
        public static bool operator !=(Vector3D left, Vector3D right) => !(left == right);

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        /// <remarks>The <see cref="Vector3D.op_Multiply" /> method defines the multiplication operation for <see cref="Vector3D" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator *(Vector3D left, Vector3D right) => (left.AsVector128Unsafe() * right.AsVector128Unsafe()).AsVector3D();

        /// <summary>Multiplies the specified vector by the specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        /// <remarks>The <see cref="Vector3D.op_Multiply" /> method defines the multiplication operation for <see cref="Vector3D" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator *(Vector3D left, double right) => (left.AsVector128Unsafe() * right).AsVector3D();

        /// <summary>Multiplies the scalar value by the specified vector.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        /// <remarks>The <see cref="Vector3D.op_Multiply" /> method defines the multiplication operation for <see cref="Vector3D" /> objects.</remarks>
        [Intrinsic]
        public static Vector3D operator *(double left, Vector3D right) => right * left;

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        /// <remarks>The <see cref="op_Subtraction" /> method defines the subtraction operation for <see cref="Vector3D" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator -(Vector3D left, Vector3D right) => (left.AsVector128Unsafe() - right.AsVector128Unsafe()).AsVector3D();

        /// <summary>Negates the specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        /// <remarks>The <see cref="op_UnaryNegation" /> method defines the unary negation operation for <see cref="Vector3D" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator -(Vector3D value) => (-value.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.op_BitwiseAnd(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator &(Vector3D left, Vector3D right) => (left.AsVector128Unsafe() & right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.op_BitwiseOr(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator |(Vector3D left, Vector3D right) => (left.AsVector128Unsafe() | right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.op_ExclusiveOr(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator ^(Vector3D left, Vector3D right) => (left.AsVector128Unsafe() ^ right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.op_LeftShift(Vector4D, int)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator <<(Vector3D value, int shiftAmount) => (value.AsVector128Unsafe() << shiftAmount).AsVector3D();

        /// <inheritdoc cref="Vector4D.op_OnesComplement(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator ~(Vector3D value) => (~value.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.op_RightShift(Vector4D, int)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator >>(Vector3D value, int shiftAmount) => (value.AsVector128Unsafe() >> shiftAmount).AsVector3D();

        /// <inheritdoc cref="Vector4D.op_UnaryPlus(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator +(Vector3D value) => value;

        /// <inheritdoc cref="Vector4D.op_UnsignedRightShift(Vector4D, int)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D operator >>>(Vector3D value, int shiftAmount) => (value.AsVector128Unsafe() >>> shiftAmount).AsVector3D();

        /// <summary>Returns a vector whose elements are the absolute values of each of the specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The absolute value vector.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Abs(Vector3D value) => Vector128.Abs(value.AsVector128Unsafe()).AsVector3D();

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        [Intrinsic]
        public static Vector3D Add(Vector3D left, Vector3D right) => left + right;

        /// <inheritdoc cref="Vector4D.All(Vector4D, double)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All(Vector3D vector, double value) => Vector128.All(vector, value);

        /// <inheritdoc cref="Vector4D.AllWhereAllBitsSet(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllWhereAllBitsSet(Vector3D vector) => Vector128.AllWhereAllBitsSet(vector);

        /// <inheritdoc cref="Vector4D.AndNot(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D AndNot(Vector3D left, Vector3D right) => Vector128.AndNot(left.AsVector128Unsafe(), right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.Any(Vector4D, double)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any(Vector3D vector, double value) => Vector128.Any(vector, value);

        /// <inheritdoc cref="Vector4D.AnyWhereAllBitsSet(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyWhereAllBitsSet(Vector3D vector) => Vector128.AnyWhereAllBitsSet(vector);

        /// <inheritdoc cref="Vector4D.BitwiseAnd(Vector4D, Vector4D)" />
        [Intrinsic]
        public static Vector3D BitwiseAnd(Vector3D left, Vector3D right) => left & right;

        /// <inheritdoc cref="Vector4D.BitwiseOr(Vector4D, Vector4D)" />
        [Intrinsic]
        public static Vector3D BitwiseOr(Vector3D left, Vector3D right) => left | right;

        /// <inheritdoc cref="Vector4D.Clamp(Vector4D, Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Clamp(Vector3D value1, Vector3D min, Vector3D max) => Vector128.Clamp(value1.AsVector128Unsafe(), min.AsVector128Unsafe(), max.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.ClampNative(Vector4D, Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D ClampNative(Vector3D value1, Vector3D min, Vector3D max) => Vector128.ClampNative(value1.AsVector128Unsafe(), min.AsVector128Unsafe(), max.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.ConditionalSelect(Vector4D, Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D ConditionalSelect(Vector3D condition, Vector3D left, Vector3D right) => Vector128.ConditionalSelect(condition.AsVector128Unsafe(), left.AsVector128Unsafe(), right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.CopySign(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D CopySign(Vector3D value, Vector3D sign) => Vector128.CopySign(value.AsVector128Unsafe(), sign.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.Cos(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Cos(Vector3D vector) => Vector128.Cos(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.Count(Vector4D, double)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count(Vector3D vector, double value) => Vector128.Count(vector, value);

        /// <inheritdoc cref="Vector4D.CountWhereAllBitsSet(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountWhereAllBitsSet(Vector3D vector) => Vector128.CountWhereAllBitsSet(vector);

        /// <summary>Creates a new <see cref="Vector3D" /> object whose three elements have the same value.</summary>
        /// <param name="value">The value to assign to all three elements.</param>
        /// <returns>A new <see cref="Vector3D" /> whose three elements have the same value.</returns>
        [Intrinsic]
        public static Vector3D Create(double value) => Vector128.Create(value).AsVector3D();

        /// <summary>Creates a new <see cref="Vector3D" /> object from the specified <see cref="Vector2D" /> object and a Z and a W component.</summary>
        /// <param name="vector">The vector to use for the X and Y components.</param>
        /// <param name="z">The Z component.</param>
        /// <returns>A new <see cref="Vector3D" /> from the specified <see cref="Vector2D" /> object and a Z and a W component.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Create(Vector2D vector, double z)
        {
            return vector.AsVector128Unsafe()
                         .WithElement(2, z)
                         .AsVector3D();
        }

        /// <summary>Creates a vector whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
        /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
        /// <returns>A new <see cref="Vector3D" /> whose elements have the specified values.</returns>
        [Intrinsic]
        public static Vector3D Create(double x, double y, double z) => Vector128.Create(x, y, z, 0).AsVector3D();

        /// <summary>Constructs a vector from the given <see cref="ReadOnlySpan{Double}" />. The span must contain at least 3 elements.</summary>
        /// <param name="values">The span of elements to assign to the vector.</param>
        /// <returns>A new <see cref="Vector3D" /> whose elements have the specified values.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Create(ReadOnlySpan<double> values)
        {
            if (values.Length < ElementCount)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.values);
            }
            return Unsafe.ReadUnaligned<Vector3D>(ref Unsafe.As<double, byte>(ref MemoryMarshal.GetReference(values)));
        }

        /// <summary>Creates a vector with <see cref="X" /> initialized to the specified value and the remaining elements initialized to zero.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <returns>A new <see cref="Vector3D" /> with <see cref="X" /> initialized <paramref name="x" /> and the remaining elements initialized to zero.</returns>
        [Intrinsic]
        public static Vector3D CreateScalar(double x) => Vector128.CreateScalar(x).AsVector3D();

        /// <summary>Creates a vector with <see cref="X" /> initialized to the specified value and the remaining elements left uninitialized.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <returns>A new <see cref="Vector3D" /> with <see cref="X" /> initialized <paramref name="x" /> and the remaining elements left uninitialized.</returns>
        [Intrinsic]
        public static Vector3D CreateScalarUnsafe(double x) => Vector128.CreateScalarUnsafe(x).AsVector3D();

        /// <summary>Computes the cross product of two vectors.</summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
            // This implementation is based on the DirectX Math Library XMVector3Cross method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl

            Vector128<double> v1 = vector1.AsVector128Unsafe();
            Vector128<double> v2 = vector2.AsVector128Unsafe();

            Vector128<double> temp1 = Vector128.Shuffle(v1, Vector128.Create(1, 2, 0, 0)) * Vector128.Shuffle(v2, Vector128.Create(2, 0, 1, 0));
            Vector128<double> temp2 = Vector128.Shuffle(v1, Vector128.Create(2, 0, 1, 0)) * Vector128.Shuffle(v2, Vector128.Create(1, 2, 0, 0));

            return (temp1 - temp2).AsVector3D();
        }

        /// <inheritdoc cref="Vector4D.DegreesToRadians(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D DegreesToRadians(Vector3D degrees) => Vector128.DegreesToRadians(degrees.AsVector128Unsafe()).AsVector3D();

        /// <summary>Computes the Euclidean distance between the two given points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance.</returns>
        [Intrinsic]
        public static double Distance(Vector3D value1, Vector3D value2) => double.Sqrt(DistanceSquared(value1, value2));

        /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance squared.</returns>
        [Intrinsic]
        public static double DistanceSquared(Vector3D value1, Vector3D value2) => (value1 - value2).LengthSquared();

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector resulting from the division.</returns>
        [Intrinsic]
        public static Vector3D Divide(Vector3D left, Vector3D right) => left / right;

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="divisor">The scalar value.</param>
        /// <returns>The vector that results from the division.</returns>
        [Intrinsic]
        public static Vector3D Divide(Vector3D left, double divisor) => left / divisor;

        /// <summary>Returns the dot product of two vectors.</summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The dot product.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Vector3D vector1, Vector3D vector2) => Vector128.Dot(vector1.AsVector128(), vector2.AsVector128());

        /// <inheritdoc cref="Vector4D.Exp(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Exp(Vector3D vector) => Vector128.Exp(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.Equals(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Equals(Vector3D left, Vector3D right) => Vector128.Equals(left.AsVector128Unsafe(), right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.EqualsAll(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsAll(Vector3D left, Vector3D right) => Vector128.EqualsAll(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector4D.EqualsAny(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsAny(Vector3D left, Vector3D right) => Vector128.EqualsAny(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector128.MultiplyAddEstimate(Vector128{double}, Vector128{double}, Vector128{double})" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D FusedMultiplyAdd(Vector3D left, Vector3D right, Vector3D addend) => Vector128.FusedMultiplyAdd(left.AsVector128Unsafe(), right.AsVector128Unsafe(), addend.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.GreaterThan(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D GreaterThan(Vector3D left, Vector3D right) => Vector128.GreaterThan(left.AsVector128Unsafe(), right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.GreaterThanAll(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanAll(Vector3D left, Vector3D right) => Vector128.GreaterThanAll(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector4D.GreaterThanAny(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanAny(Vector3D left, Vector3D right) => Vector128.GreaterThanAny(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector4D.GreaterThanOrEqual(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D GreaterThanOrEqual(Vector3D left, Vector3D right) => Vector128.GreaterThanOrEqual(left.AsVector128Unsafe(), right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.GreaterThanOrEqualAll(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrEqualAll(Vector3D left, Vector3D right) => Vector128.GreaterThanOrEqualAll(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector4D.GreaterThanOrEqualAny(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrEqualAny(Vector3D left, Vector3D right) => Vector128.GreaterThanOrEqualAny(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector4D.Hypot(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Hypot(Vector3D x, Vector3D y) => Vector128.Hypot(x.AsVector128Unsafe(), y.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IndexOf(Vector4D, double)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(Vector3D vector, double value) => Vector128.IndexOf(vector, value);

        /// <inheritdoc cref="Vector4D.IndexOfWhereAllBitsSet(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfWhereAllBitsSet(Vector3D vector) => Vector128.IndexOfWhereAllBitsSet(vector);

        /// <inheritdoc cref="Vector4D.IsEvenInteger(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsEvenInteger(Vector3D vector) => Vector128.IsEvenInteger(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsFinite(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsFinite(Vector3D vector) => Vector128.IsFinite(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsInfinity(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsInfinity(Vector3D vector) => Vector128.IsInfinity(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsInteger(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsInteger(Vector3D vector) => Vector128.IsInteger(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsNaN(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsNaN(Vector3D vector) => Vector128.IsNaN(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsNegative(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsNegative(Vector3D vector) => Vector128.IsNegative(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsNegativeInfinity(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsNegativeInfinity(Vector3D vector) => Vector128.IsNegativeInfinity(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsNormal(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsNormal(Vector3D vector) => Vector128.IsNormal(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsOddInteger(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsOddInteger(Vector3D vector) => Vector128.IsOddInteger(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsPositive(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsPositive(Vector3D vector) => Vector128.IsPositive(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsPositiveInfinity(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsPositiveInfinity(Vector3D vector) => Vector128.IsPositiveInfinity(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsSubnormal(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsSubnormal(Vector3D vector) => Vector128.IsSubnormal(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.IsZero(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D IsZero(Vector3D vector) => Vector128.IsZero(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.LastIndexOf(Vector4D, double)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOf(Vector3D vector, double value) => Vector128.LastIndexOf(vector, value);

        /// <inheritdoc cref="Vector4D.LastIndexOfWhereAllBitsSet(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfWhereAllBitsSet(Vector3D vector) => Vector128.LastIndexOfWhereAllBitsSet(vector);

        /// <inheritdoc cref="Vector4D.Lerp(Vector4D, Vector4D, double)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Lerp(Vector3D value1, Vector3D value2, double amount) => Lerp(value1, value2, Create(amount));

        /// <inheritdoc cref="Vector4D.Lerp(Vector4D, Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Lerp(Vector3D value1, Vector3D value2, Vector3D amount) => Vector128.Lerp(value1.AsVector128Unsafe(), value2.AsVector128Unsafe(), amount.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.LessThan(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D LessThan(Vector3D left, Vector3D right) => Vector128.LessThan(left.AsVector128Unsafe(), right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.LessThanAll(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanAll(Vector3D left, Vector3D right) => Vector128.LessThanAll(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector4D.LessThanAny(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanAny(Vector3D left, Vector3D right) => Vector128.LessThanAny(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector4D.LessThanOrEqual(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D LessThanOrEqual(Vector3D left, Vector3D right) => Vector128.LessThanOrEqual(left.AsVector128Unsafe(), right.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.LessThanOrEqualAll(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrEqualAll(Vector3D left, Vector3D right) => Vector128.LessThanOrEqualAll(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector4D.LessThanOrEqualAny(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrEqualAny(Vector3D left, Vector3D right) => Vector128.LessThanOrEqualAny(left.AsVector128Unsafe(), right.AsVector128Unsafe());

        /// <inheritdoc cref="Vector4D.Load(double*)" />
        [Intrinsic]
        [CLSCompliant(false)]
        public static unsafe Vector3D Load(double* source) => LoadUnsafe(in *source);

        /// <inheritdoc cref="Vector4D.LoadAligned(double*)" />
        [Intrinsic]
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector3D LoadAligned(double* source)
        {
            if (((nuint)(source) % Alignment) != 0)
            {
                ThrowHelper.ThrowAccessViolationException();
            }

            return *(Vector3D*)source;
        }

        /// <inheritdoc cref="Vector4D.LoadAlignedNonTemporal(double*)" />
        [Intrinsic]
        [CLSCompliant(false)]
        public static unsafe Vector3D LoadAlignedNonTemporal(double* source) => LoadAligned(source);

        /// <inheritdoc cref="Vector128.LoadUnsafe{T}(ref readonly T)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D LoadUnsafe(ref readonly double source)
        {
            ref readonly byte address = ref Unsafe.As<double, byte>(ref Unsafe.AsRef(in source));
            return Unsafe.ReadUnaligned<Vector3D>(in address);
        }

        /// <inheritdoc cref="Vector4D.LoadUnsafe(ref readonly double, nuint)" />
        [Intrinsic]
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D LoadUnsafe(ref readonly double source, nuint elementOffset)
        {
            ref readonly byte address = ref Unsafe.As<double, byte>(ref Unsafe.Add(ref Unsafe.AsRef(in source), (nint)elementOffset));
            return Unsafe.ReadUnaligned<Vector3D>(in address);
        }

        /// <inheritdoc cref="Vector4D.Log(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Log(Vector3D vector) => Vector128.Log(Vector4D.Create(vector, 1.0d).AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.Log2(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Log2(Vector3D vector) => Vector128.Log2(Vector4D.Create(vector, 1.0d).AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.Max(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Max(Vector3D value1, Vector3D value2) => Vector128.Max(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.MaxMagnitude(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MaxMagnitude(Vector3D value1, Vector3D value2) => Vector128.MaxMagnitude(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.MaxMagnitudeNumber(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MaxMagnitudeNumber(Vector3D value1, Vector3D value2) => Vector128.MaxMagnitudeNumber(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.MaxNative(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MaxNative(Vector3D value1, Vector3D value2) => Vector128.MaxNative(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.MaxNumber(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MaxNumber(Vector3D value1, Vector3D value2) => Vector128.MaxNumber(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.Min(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Min(Vector3D value1, Vector3D value2) => Vector128.Min(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.MinMagnitude(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MinMagnitude(Vector3D value1, Vector3D value2) => Vector128.MinMagnitude(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.MinMagnitudeNumber(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MinMagnitudeNumber(Vector3D value1, Vector3D value2) => Vector128.MinMagnitudeNumber(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.MinNative(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MinNative(Vector3D value1, Vector3D value2) => Vector128.MinNative(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.MinNumber(Vector4D, Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MinNumber(Vector3D value1, Vector3D value2) => Vector128.MinNumber(value1.AsVector128Unsafe(), value2.AsVector128Unsafe()).AsVector3D();

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        [Intrinsic]
        public static Vector3D Multiply(Vector3D left, Vector3D right) => left * right;

        /// <summary>Multiplies a vector by a specified scalar.</summary>
        /// <param name="left">The vector to multiply.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        [Intrinsic]
        public static Vector3D Multiply(Vector3D left, double right) => left * right;

        /// <summary>Multiplies a scalar value by a specified vector.</summary>
        /// <param name="left">The scaled value.</param>
        /// <param name="right">The vector.</param>
        /// <returns>The scaled vector.</returns>
        [Intrinsic]
        public static Vector3D Multiply(double left, Vector3D right) => left * right;

        /// <inheritdoc cref="Vector128.MultiplyAddEstimate(Vector128{double}, Vector128{double}, Vector128{double})" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D MultiplyAddEstimate(Vector3D left, Vector3D right, Vector3D addend) => Vector128.MultiplyAddEstimate(left.AsVector128Unsafe(), right.AsVector128Unsafe(), addend.AsVector128Unsafe()).AsVector3D();

        /// <summary>Negates a specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        [Intrinsic]
        public static Vector3D Negate(Vector3D value) => -value;

        /// <inheritdoc cref="Vector4D.None(Vector4D, double)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None(Vector3D vector, double value) => Vector128.None(vector, value);

        /// <inheritdoc cref="Vector4D.NoneWhereAllBitsSet(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NoneWhereAllBitsSet(Vector3D vector) => Vector128.NoneWhereAllBitsSet(vector);

        /// <summary>Returns a vector with the same direction as the specified vector, but with a length of one.</summary>
        /// <param name="value">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        [Intrinsic]
        public static Vector3D Normalize(Vector3D value) => value / value.Length();

        /// <inheritdoc cref="Vector4D.OnesComplement(Vector4D)" />
        [Intrinsic]
        public static Vector3D OnesComplement(Vector3D value) => ~value;

        /// <inheritdoc cref="Vector4D.RadiansToDegrees(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D RadiansToDegrees(Vector3D radians) => Vector128.RadiansToDegrees(radians.AsVector128Unsafe()).AsVector3D();

        /// <summary>Returns the reflection of a vector off a surface that has the specified normal.</summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">The normal of the surface being reflected off.</param>
        /// <returns>The reflected vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Reflect(Vector3D vector, Vector3D normal)
        {
            // This implementation is based on the DirectX Math Library XMVector3Reflect method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl

            Vector3D tmp = Create(Dot(vector, normal));
            tmp += tmp;
            return MultiplyAddEstimate(-tmp, normal, vector);
        }

        /// <inheritdoc cref="Vector4D.Round(Vector4D)" />
        [Intrinsic]
        public static Vector3D Round(Vector3D vector) => Vector128.Round(vector.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.Round(Vector4D, MidpointRounding)" />
        [Intrinsic]
        public static Vector3D Round(Vector3D vector, MidpointRounding mode) => Vector128.Round(vector.AsVector128Unsafe(), mode).AsVector3D();

        /// <summary>Creates a new vector by selecting values from an input vector using a set of indices.</summary>
        /// <param name="vector">The input vector from which values are selected.</param>
        /// <param name="xIndex">The index used to select a value from <paramref name="vector" /> to be used as the value of <see cref="X" /> in the result.</param>
        /// <param name="yIndex">The index used to select a value from <paramref name="vector" /> to be used as the value of <see cref="Y" /> in the result</param>
        /// <param name="zIndex">The index used to select a value from <paramref name="vector" /> to be used as the value of <see cref="Z" /> in the result</param>
        /// <returns>A new vector containing the values from <paramref name="vector" /> selected by the given indices.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Shuffle(Vector3D vector, byte xIndex, byte yIndex, byte zIndex)
        {
            // We do `AsVector128` instead of `AsVector128Unsafe` so that indices which
            // are out of range for Vector3D but in range for Vector128 still produce 0
            return Vector128.Shuffle(vector.AsVector128(), Vector128.Create(xIndex, yIndex, zIndex, 3)).AsVector3D();
        }

        /// <inheritdoc cref="Vector4D.Sin(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Sin(Vector3D vector) => Vector128.Sin(vector.AsVector128()).AsVector3D();

        /// <inheritdoc cref="Vector4D.SinCos(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector3D Sin, Vector3D Cos) SinCos(Vector3D vector)
        {
            (Vector128<double> sin, Vector128<double> cos) = Vector128.SinCos(vector.AsVector128());
            return (sin.AsVector3D(), cos.AsVector3D());
        }

        /// <summary>Returns a vector whose elements are the square root of each of a specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The square root vector.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D SquareRoot(Vector3D value) => Vector128.Sqrt(value.AsVector128Unsafe()).AsVector3D();

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The difference vector.</returns>
        [Intrinsic]
        public static Vector3D Subtract(Vector3D left, Vector3D right) => left - right;

        /// <inheritdoc cref="Vector4D.Sum(Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sum(Vector3D value) => Vector128.Sum(value.AsVector128());

        /// <summary>Transforms a vector by a specified 4x4 matrix.</summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Transform(Vector3D position, Matrix4x4D matrix) => Vector4D.Transform(position, matrix).AsVector3D();

        /// <summary>Transforms a vector by the specified QuaternionD rotation value.</summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D Transform(Vector3D value, QuaternionD rotation) => Vector4D.Transform(value, rotation).AsVector3D();

        /// <summary>Transforms a vector normal by the given 4x4 matrix.</summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3D TransformNormal(Vector3D normal, Matrix4x4D matrix)
        {
            Vector4D result = matrix.X * normal.X;

            result = Vector4D.MultiplyAddEstimate(matrix.Y, Vector4D.Create(normal.Y), result);
            result = Vector4D.MultiplyAddEstimate(matrix.Z, Vector4D.Create(normal.Z), result);

            return result.AsVector3D();
        }

        /// <inheritdoc cref="Vector4D.Truncate(Vector4D)" />
        [Intrinsic]
        public static Vector3D Truncate(Vector3D vector) => Vector128.Truncate(vector.AsVector128Unsafe()).AsVector3D();

        /// <inheritdoc cref="Vector4D.Xor(Vector4D, Vector4D)" />
        [Intrinsic]
        public static Vector3D Xor(Vector3D left, Vector3D right) => left ^ right;

        /// <summary>Copies the elements of the vector to a specified array.</summary>
        /// <param name="array">The destination array.</param>
        /// <remarks><paramref name="array" /> must have at least three elements. The method copies the vector's elements starting at index 0.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(double[] array)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons

            if (array.Length < ElementCount)
            {
                ThrowHelper.ThrowArgumentException_DestinationTooShort();
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref array[0]), this);
        }

        /// <summary>Copies the elements of the vector to a specified array starting at a specified index position.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The index at which to copy the first element of the vector.</param>
        /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the three vector elements. In other words, elements <paramref name="index" />, <paramref name="index" /> + 1, and <paramref name="index" /> + 2 must already exist in <paramref name="array" />.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than zero.
        /// -or-
        /// <paramref name="index" /> is greater than or equal to the array length.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(double[] array, int index)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons

            if ((uint)index >= (uint)array.Length)
            {
                ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLess();
            }

            if ((array.Length - index) < ElementCount)
            {
                ThrowHelper.ThrowArgumentException_DestinationTooShort();
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref array[index]), this);
        }

        /// <summary>Copies the vector to the given <see cref="Span{T}" />. The length of the destination span must be at least 3.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <exception cref="ArgumentException">If number of elements in source vector is greater than those available in destination span.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<double> destination)
        {
            if (destination.Length < ElementCount)
            {
                ThrowHelper.ThrowArgumentException_DestinationTooShort();
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref MemoryMarshal.GetReference(destination)), this);
        }

        /// <summary>Attempts to copy the vector to the given <see cref="Span{Double}" />. The length of the destination span must be at least 3.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <returns><see langword="true" /> if the source vector was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(Span<double> destination)
        {
            if (destination.Length < ElementCount)
            {
                return false;
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref MemoryMarshal.GetReference(destination)), this);
            return true;
        }

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Vector3D" /> object and their corresponding elements are equal.</remarks>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is Vector3D other) && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another vector are equal.</summary>
        /// <param name="other">The other vector.</param>
        /// <returns><see langword="true" /> if the two vectors are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two vectors are equal if their <see cref="X" />, <see cref="Y" />, and <see cref="Z" /> elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Vector3D other) => this.AsVector128().Equals(other.AsVector128());

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);

        /// <summary>Returns the length of this vector object.</summary>
        /// <returns>The vector's length.</returns>
        /// <altmember cref="LengthSquared" />
        [Intrinsic]
        public readonly double Length() => double.Sqrt(LengthSquared());

        /// <summary>Returns the length of the vector squared.</summary>
        /// <returns>The vector's length squared.</returns>
        /// <remarks>This operation offers better performance than a call to the <see cref="Length" /> method.</remarks>
        /// <altmember cref="Length" />
        [Intrinsic]
        public readonly double LengthSquared() => Dot(this, this);

        /// <summary>Returns the string representation of the current instance using default formatting.</summary>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        public override readonly string ToString() => ToString("G", CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements and the specified format provider to define culture-specific formatting.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <param name="formatProvider">A format provider that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "&lt;" and "&gt;" characters are used to begin and end the string, and the format provider's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
        {
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return $"<{X.ToString(format, formatProvider)}{separator} {Y.ToString(format, formatProvider)}{separator} {Z.ToString(format, formatProvider)}>";
        }
    }
}
