// Copyright (c) Andrew Pollard, .NET Foundation and Contributors.
// Licensed under the MIT license - see README.md for details.

using System.Globalization;
using System.Runtime.InteropServices;
using Xunit;

namespace Invicta.Numerics.Tests
{
    public class PlaneDDTests
    {
        // A test for Equals (PlaneD)
        [Fact]
        public void PlaneDEqualsTest1()
        {
            PlaneD a = new PlaneD(1.0d, 2.0d, 3.0d, 4.0d);
            PlaneD b = new PlaneD(1.0d, 2.0d, 3.0d, 4.0d);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.Normal = new Vector3D(10.0d, b.Normal.Y, b.Normal.Z);
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void PlaneDEqualsTest()
        {
            PlaneD a = new PlaneD(1.0d, 2.0d, 3.0d, 4.0d);
            PlaneD b = new PlaneD(1.0d, 2.0d, 3.0d, 4.0d);

            // case 1: compare between same values
            object obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.Normal = new Vector3D(10.0d, b.Normal.Y, b.Normal.Z);

            obj = b;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare between different types.
            obj = new QuaternionD();
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare against null.
            obj = null;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (PlaneD, PlaneD)
        [Fact]
        public void PlaneDInequalityTest()
        {
            PlaneD a = new PlaneD(1.0d, 2.0d, 3.0d, 4.0d);
            PlaneD b = new PlaneD(1.0d, 2.0d, 3.0d, 4.0d);

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.Normal = new Vector3D(10.0d, b.Normal.Y, b.Normal.Z);
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (PlaneD, PlaneD)
        [Fact]
        public void PlaneDEqualityTest()
        {
            PlaneD a = new PlaneD(1.0d, 2.0d, 3.0d, 4.0d);
            PlaneD b = new PlaneD(1.0d, 2.0d, 3.0d, 4.0d);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.Normal = new Vector3D(10.0d, b.Normal.Y, b.Normal.Z);
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for GetHashCode ()
        [Fact]
        public void PlaneDGetHashCodeTest()
        {
            PlaneD target = new PlaneD(1.0d, 2.0d, 3.0d, 4.0d);

            int expected = HashCode.Combine(target.Normal, target.D);
            int actual = target.GetHashCode();
            Assert.Equal(expected, actual);
        }

        // A test for PlaneD (double, double, double, double)
        [Fact]
        public void PlaneDConstructorTest1()
        {
            double a = 1.0d, b = 2.0d, c = 3.0d, d = 4.0d;
            PlaneD target = new PlaneD(a, b, c, d);

            Assert.True(
                target.Normal.X == a && target.Normal.Y == b && target.Normal.Z == c && target.D == d,
                "PlaneD.cstor did not return the expected value.");
        }

        // A test for PlaneD.CreateFromVertices
        [Fact]
        public void PlaneDCreateFromVerticesTest()
        {
            Vector3D point1 = new Vector3D(0.0d, 1.0d, 1.0d);
            Vector3D point2 = new Vector3D(0.0d, 0.0d, 1.0d);
            Vector3D point3 = new Vector3D(1.0d, 0.0d, 1.0d);

            PlaneD target = PlaneD.CreateFromVertices(point1, point2, point3);
            PlaneD expected = new PlaneD(new Vector3D(0, 0, 1), -1.0d);
            Assert.Equal(target, expected);
        }

        // A test for PlaneD.CreateFromVertices
        [Fact]
        public void PlaneDCreateFromVerticesTest2()
        {
            Vector3D point1 = new Vector3D(0.0d, 0.0d, 1.0d);
            Vector3D point2 = new Vector3D(1.0d, 0.0d, 0.0d);
            Vector3D point3 = new Vector3D(1.0d, 1.0d, 0.0d);

            PlaneD target = PlaneD.CreateFromVertices(point1, point2, point3);
            double invRoot2 = (double)(1 / Math.Sqrt(2));

            PlaneD expected = new PlaneD(new Vector3D(invRoot2, 0, invRoot2), -invRoot2);
            Assert.True(MathHelper.Equal(target, expected), "PlaneD.cstor did not return the expected value.");
        }

        // A test for PlaneD (Vector3Df, double)
        [Fact]
        public void PlaneDConstructorTest3()
        {
            Vector3D normal = new Vector3D(1, 2, 3);
            double d = 4;

            PlaneD target = new PlaneD(normal, d);
            Assert.True(
                target.Normal == normal && target.D == d,
                "PlaneD.cstor did not return the expected value.");
        }

        // A test for PlaneD (Vector4Df)
        [Fact]
        public void PlaneDConstructorTest()
        {
            Vector4D value = new Vector4D(1.0d, 2.0d, 3.0d, 4.0d);
            PlaneD target = new PlaneD(value);

            Assert.True(
                target.Normal.X == value.X && target.Normal.Y == value.Y && target.Normal.Z == value.Z && target.D == value.W,
                "PlaneD.cstor did not return the expected value.");
        }

        [Fact]
        public void PlaneDDotTest()
        {
            PlaneD target = new PlaneD(2, 3, 4, 5);
            Vector4D value = new Vector4D(5, 4, 3, 2);

            double expected = 10 + 12 + 12 + 10;
            double actual = PlaneD.Dot(target, value);
            Assert.True(MathHelper.Equal(expected, actual), "PlaneD.Dot returns unexpected value.");
        }

        [Fact]
        public void PlaneDDotCoordinateTest()
        {
            PlaneD target = new PlaneD(2, 3, 4, 5);
            Vector3D value = new Vector3D(5, 4, 3);

            double expected = 10 + 12 + 12 + 5;
            double actual = PlaneD.DotCoordinate(target, value);
            Assert.True(MathHelper.Equal(expected, actual), "PlaneD.DotCoordinate returns unexpected value.");
        }

        [Fact]
        public void PlaneDDotNormalTest()
        {
            PlaneD target = new PlaneD(2, 3, 4, 5);
            Vector3D value = new Vector3D(5, 4, 3);

            double expected = 10 + 12 + 12;
            double actual = PlaneD.DotNormal(target, value);
            Assert.True(MathHelper.Equal(expected, actual), "PlaneD.DotCoordinate returns unexpected value.");
        }

        [Fact]
        public void PlaneDNormalizeTest()
        {
            PlaneD target = new PlaneD(1, 2, 3, 4);

            double f = target.Normal.LengthSquared();
            double invF = 1.0d / (double)Math.Sqrt(f);
            PlaneD expected = new PlaneD(target.Normal * invF, target.D * invF);

            PlaneD actual = PlaneD.Normalize(target);
            Assert.True(MathHelper.Equal(expected, actual), "PlaneD.Normalize returns unexpected value.");

            // normalize, normalized normal.
            actual = PlaneD.Normalize(actual);
            Assert.True(MathHelper.Equal(expected, actual), "PlaneD.Normalize returns unexpected value.");
        }

        [Fact]
        // Transform by matrix
        public void PlaneDTransformTest1()
        {
            PlaneD target = new PlaneD(1, 2, 3, 4);
            target = PlaneD.Normalize(target);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            m.M41 = 10.0d;
            m.M42 = 20.0d;
            m.M43 = 30.0d;

            PlaneD expected = new PlaneD();
            Matrix4x4D inv;
            Matrix4x4D.Invert(m, out inv);
            Matrix4x4D itm = Matrix4x4D.Transpose(inv);
            double x = target.Normal.X, y = target.Normal.Y, z = target.Normal.Z, w = target.D;
            expected.Normal = new Vector3D(
                x * itm.M11 + y * itm.M21 + z * itm.M31 + w * itm.M41,
                x * itm.M12 + y * itm.M22 + z * itm.M32 + w * itm.M42,
                x * itm.M13 + y * itm.M23 + z * itm.M33 + w * itm.M43);
            expected.D = x * itm.M14 + y * itm.M24 + z * itm.M34 + w * itm.M44;

            PlaneD actual;
            actual = PlaneD.Transform(target, m);
            Assert.True(MathHelper.Equal(expected, actual), "PlaneD.Transform did not return the expected value.");
        }

        [Fact]
        // Transform by quaternion
        public void PlaneDTransformTest2()
        {
            PlaneD target = new PlaneD(1, 2, 3, 4);
            target = PlaneD.Normalize(target);

            Matrix4x4D m =
                Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0d)) *
                Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0d));
            QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

            PlaneD expected = new PlaneD();
            double x = target.Normal.X, y = target.Normal.Y, z = target.Normal.Z, w = target.D;
            expected.Normal = new Vector3D(
                x * m.M11 + y * m.M21 + z * m.M31 + w * m.M41,
                x * m.M12 + y * m.M22 + z * m.M32 + w * m.M42,
                x * m.M13 + y * m.M23 + z * m.M33 + w * m.M43);
            expected.D = x * m.M14 + y * m.M24 + z * m.M34 + w * m.M44;

            PlaneD actual;
            actual = PlaneD.Transform(target, q);
            Assert.True(MathHelper.Equal(expected, actual), "PlaneD.Transform did not return the expected value.");
        }

        // A test for PlaneD comparison involving NaN values
        [Fact]
        public void PlaneDEqualsNaNTest()
        {
            PlaneD a = new PlaneD(double.NaN, 0, 0, 0);
            PlaneD b = new PlaneD(0, double.NaN, 0, 0);
            PlaneD c = new PlaneD(0, 0, double.NaN, 0);
            PlaneD d = new PlaneD(0, 0, 0, double.NaN);

            Assert.False(a == new PlaneD(0, 0, 0, 0));
            Assert.False(b == new PlaneD(0, 0, 0, 0));
            Assert.False(c == new PlaneD(0, 0, 0, 0));
            Assert.False(d == new PlaneD(0, 0, 0, 0));

            Assert.True(a != new PlaneD(0, 0, 0, 0));
            Assert.True(b != new PlaneD(0, 0, 0, 0));
            Assert.True(c != new PlaneD(0, 0, 0, 0));
            Assert.True(d != new PlaneD(0, 0, 0, 0));

            Assert.False(a.Equals(new PlaneD(0, 0, 0, 0)));
            Assert.False(b.Equals(new PlaneD(0, 0, 0, 0)));
            Assert.False(c.Equals(new PlaneD(0, 0, 0, 0)));
            Assert.False(d.Equals(new PlaneD(0, 0, 0, 0)));

            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.True(c.Equals(c));
            Assert.True(d.Equals(d));
        }

        /* Enable when size of Vector3D is correct
        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void PlaneDSizeofTest()
        {
            Assert.Equal(16, sizeof(PlaneD));
            Assert.Equal(32, sizeof(PlaneD_2x));
            Assert.Equal(20, sizeof(PlaneDPlusDouble));
            Assert.Equal(40, sizeof(PlaneDPlusDouble_2x));
        }
        */

        [Fact]
        public void PlaneDToStringTest()
        {
            PlaneD target = new PlaneD(1, 2, 3, 4);
            string expected = string.Format(
                CultureInfo.CurrentCulture,
                "{{Normal:{0:G} D:{1}}}",
                target.Normal,
                target.D);

            Assert.Equal(expected, target.ToString());
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PlaneD_2x
        {
            private PlaneD _a;
            private PlaneD _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PlaneDPlusDouble
        {
            private PlaneD _v;
            private double _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PlaneDPlusDouble_2x
        {
            private PlaneDPlusDouble _a;
            private PlaneDPlusDouble _b;
        }

        // A test to make sure the fields are laid out how we expect
        [Fact]
        public unsafe void PlaneDFieldOffsetTest()
        {
            PlaneD plane = new PlaneD();

            double* basePtr = &plane.Normal.X; // Take address of first element
            PlaneD* planePtr = &plane; // Take address of whole PlaneD

            Assert.Equal(new IntPtr(basePtr), new IntPtr(planePtr));

            Assert.Equal(new IntPtr(basePtr + 0), new IntPtr(&plane.Normal));
            Assert.Equal(new IntPtr(basePtr + 3), new IntPtr(&plane.D));
        }
    }
}
