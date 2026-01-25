// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using Xunit;

namespace Invicta.Numerics.Tests
{
    public sealed class QuaternionDDTests
    {
        // A test for Dot (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDDotTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(5.0f, 6.0f, 7.0f, 8.0f);

            float expected = 70.0f;
            float actual;

            actual = QuaternionD.Dot(a, b);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Dot did not return the expected value: expected {expected} actual {actual}");
        }

        [Theory]
        [InlineData(0.0f, 1.0f, 0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 1.0f, 0.0f)]
        [InlineData(3.1434343f, 1.1234123f, 0.1234123f, -0.1234123f)]
        [InlineData(1.0000001f, 0.0000001f, 2.0000001f, 0.0000002f)]
        public void QuaternionDIndexerGetTest(float x, float y, float z, float w)
        {
            var quaternion = new QuaternionD(x, y, z, w);

            Assert.Equal(x, quaternion[0]);
            Assert.Equal(y, quaternion[1]);
            Assert.Equal(z, quaternion[2]);
            Assert.Equal(w, quaternion[3]);
        }

        [Theory]
        [InlineData(0.0f, 1.0f, 0.0f, 1.0f)]
        [InlineData(1.0f, 0.0f, 1.0f, 0.0f)]
        [InlineData(3.1434343f, 1.1234123f, 0.1234123f, -0.1234123f)]
        [InlineData(1.0000001f, 0.0000001f, 2.0000001f, 0.0000002f)]
        public void QuaternionDIndexerSetTest(float x, float y, float z, float w)
        {
            var quaternion = new QuaternionD(0.0f, 0.0f, 0.0f, 0.0f);

            quaternion[0] = x;
            quaternion[1] = y;
            quaternion[2] = z;
            quaternion[3] = w;

            Assert.Equal(x, quaternion[0]);
            Assert.Equal(y, quaternion[1]);
            Assert.Equal(z, quaternion[2]);
            Assert.Equal(w, quaternion[3]);
        }

        // A test for Length ()
        [Fact]
        public void QuaternionDLengthTest()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);

            float w = 4.0f;

            QuaternionD target = new QuaternionD(v, w);

            float expected = 5.477226f;
            float actual;

            actual = target.Length();

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Length did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for LengthSquared ()
        [Fact]
        public void QuaternionDLengthSquaredTest()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
            float w = 4.0f;

            QuaternionD target = new QuaternionD(v, w);

            float expected = 30.0f;
            float actual;

            actual = target.LengthSquared();

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.LengthSquared did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (QuaternionD, QuaternionD, float)
        [Fact]
        public void QuaternionDLerpTest()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0f));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0f));

            float t = 0.5f;

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(20.0f));
            QuaternionD actual;

            actual = QuaternionD.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Lerp did not return the expected value: expected {expected} actual {actual}");

            // Case a and b are same.
            expected = a;
            actual = QuaternionD.Lerp(a, a, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Lerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (QuaternionD, QuaternionD, float)
        // Lerp test when t = 0
        [Fact]
        public void QuaternionDLerpTest1()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0f));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0f));

            float t = 0.0f;

            QuaternionD expected = new QuaternionD(a.X, a.Y, a.Z, a.W);
            QuaternionD actual = QuaternionD.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Lerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (QuaternionD, QuaternionD, float)
        // Lerp test when t = 1
        [Fact]
        public void QuaternionDLerpTest2()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0f));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0f));

            float t = 1.0f;

            QuaternionD expected = new QuaternionD(b.X, b.Y, b.Z, b.W);
            QuaternionD actual = QuaternionD.Lerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Lerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (QuaternionD, QuaternionD, float)
        // Lerp test when the two quaternions are more than 90 degree (dot product <0)
        [Fact]
        public void QuaternionDLerpTest3()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0f));
            QuaternionD b = QuaternionD.Negate(a);

            float t = 1.0f;

            QuaternionD actual = QuaternionD.Lerp(a, b, t);
            // Note that in quaternion world, Q == -Q. In the case of quaternions dot product is zero,
            // one of the quaternion will be flipped to compute the shortest distance. When t = 1, we
            // expect the result to be the same as quaternion b but flipped.
            Assert.True(actual == a, $"QuaternionD.Lerp did not return the expected value: expected {a} actual {actual}");
        }

        // A test for Conjugate(QuaternionD)
        [Fact]
        public void QuaternionDConjugateTest1()
        {
            QuaternionD a = new QuaternionD(1, 2, 3, 4);

            QuaternionD expected = new QuaternionD(-1, -2, -3, 4);
            QuaternionD actual;

            actual = QuaternionD.Conjugate(a);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Conjugate did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Normalize (QuaternionD)
        [Fact]
        public void QuaternionDNormalizeTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);

            QuaternionD expected = new QuaternionD(0.182574168f, 0.365148336f, 0.5477225f, 0.7302967f);
            QuaternionD actual;

            actual = QuaternionD.Normalize(a);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Normalize did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Normalize (QuaternionD)
        // Normalize zero length quaternion
        [Fact]
        public void QuaternionDNormalizeTest1()
        {
            QuaternionD a = new QuaternionD(0.0f, 0.0f, -0.0f, 0.0f);

            QuaternionD actual = QuaternionD.Normalize(a);
            Assert.True(float.IsNaN(actual.X) && float.IsNaN(actual.Y) && float.IsNaN(actual.Z) && float.IsNaN(actual.W)
                , $"QuaternionD.Normalize did not return the expected value: expected {new QuaternionD(float.NaN, float.NaN, float.NaN, float.NaN)} actual {actual}");
        }

        // A test for Concatenate(QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDConcatenateTest1()
        {
            QuaternionD b = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD a = new QuaternionD(5.0f, 6.0f, 7.0f, 8.0f);

            QuaternionD expected = new QuaternionD(24.0f, 48.0f, 48.0f, -6.0f);
            QuaternionD actual;

            actual = QuaternionD.Concatenate(a, b);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Concatenate did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator - (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDSubtractionTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 6.0f, 7.0f, 4.0f);
            QuaternionD b = new QuaternionD(5.0f, 2.0f, 3.0f, 8.0f);

            QuaternionD expected = new QuaternionD(-4.0f, 4.0f, 4.0f, -4.0f);
            QuaternionD actual;

            actual = a - b;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator - did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator * (QuaternionD, float)
        [Fact]
        public void QuaternionDMultiplyTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            float factor = 0.5f;

            QuaternionD expected = new QuaternionD(0.5f, 1.0f, 1.5f, 2.0f);
            QuaternionD actual;

            actual = a * factor;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator * did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator * (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDMultiplyTest1()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(5.0f, 6.0f, 7.0f, 8.0f);

            QuaternionD expected = new QuaternionD(24.0f, 48.0f, 48.0f, -6.0f);
            QuaternionD actual;

            actual = a * b;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator * did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator / (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDDivisionTest1()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(5.0f, 6.0f, 7.0f, 8.0f);

            QuaternionD expected = new QuaternionD(-0.045977015f, -0.09195402f, -7.450581E-9f, 0.402298868f);
            QuaternionD actual;

            actual = a / b;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator / did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator + (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDAdditionTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(5.0f, 6.0f, 7.0f, 8.0f);

            QuaternionD expected = new QuaternionD(6.0f, 8.0f, 10.0f, 12.0f);
            QuaternionD actual;

            actual = a + b;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator + did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for QuaternionD (float, float, float, float)
        [Fact]
        public void QuaternionDConstructorTest()
        {
            float x = 1.0f;
            float y = 2.0f;
            float z = 3.0f;
            float w = 4.0f;

            QuaternionD target = new QuaternionD(x, y, z, w);

            Assert.True(MathHelper.Equal(target.X, x) && MathHelper.Equal(target.Y, y) && MathHelper.Equal(target.Z, z) && MathHelper.Equal(target.W, w),
                "QuaternionD.constructor (x,y,z,w) did not return the expected value.");
        }

        // A test for QuaternionD (Vector3Df, float)
        [Fact]
        public void QuaternionDConstructorTest1()
        {
            Vector3D v = new Vector3D(1.0f, 2.0f, 3.0f);
            float w = 4.0f;

            QuaternionD target = new QuaternionD(v, w);
            Assert.True(MathHelper.Equal(target.X, v.X) && MathHelper.Equal(target.Y, v.Y) && MathHelper.Equal(target.Z, v.Z) && MathHelper.Equal(target.W, w),
                "QuaternionD.constructor (Vector3Df,w) did not return the expected value.");
        }

        // A test for CreateFromAxisAngle (Vector3Df, float)
        [Fact]
        public void QuaternionDCreateFromAxisAngleTest()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            float angle = MathHelper.ToRadians(30.0f);

            QuaternionD expected = new QuaternionD(0.06917231f, 0.13834462f, 0.2075169f, 0.9659258f);
            QuaternionD actual;

            actual = QuaternionD.CreateFromAxisAngle(axis, angle);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.CreateFromAxisAngle did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for CreateFromAxisAngle (Vector3Df, float)
        // CreateFromAxisAngle of zero vector
        [Fact]
        public void QuaternionDCreateFromAxisAngleTest1()
        {
            Vector3D axis = new Vector3D();
            float angle = MathHelper.ToRadians(-30.0f);

            float cos = (float)System.Math.Cos(angle / 2.0f);
            QuaternionD actual = QuaternionD.CreateFromAxisAngle(axis, angle);

            Assert.True(actual.X == 0.0f && actual.Y == 0.0f && actual.Z == 0.0f && MathHelper.Equal(cos, actual.W)
                , "QuaternionD.CreateFromAxisAngle did not return the expected value.");
        }

        // A test for CreateFromAxisAngle (Vector3Df, float)
        // CreateFromAxisAngle of angle = 30 && 750
        [Fact]
        public void QuaternionDCreateFromAxisAngleTest2()
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            float angle1 = MathHelper.ToRadians(30.0f);
            float angle2 = MathHelper.ToRadians(750.0f);

            QuaternionD actual1 = QuaternionD.CreateFromAxisAngle(axis, angle1);
            QuaternionD actual2 = QuaternionD.CreateFromAxisAngle(axis, angle2);
            Assert.True(MathHelper.Equal(actual1, actual2), $"QuaternionD.CreateFromAxisAngle did not return the expected value: actual1 {actual1} actual2 {actual2}");
        }

        // A test for CreateFromAxisAngle (Vector3Df, float)
        // CreateFromAxisAngle of angle = 30 && 390
        [Fact]
        public void QuaternionDCreateFromAxisAngleTest3()
        {
            Vector3D axis = new Vector3D(1, 0, 0);
            float angle1 = MathHelper.ToRadians(30.0f);
            float angle2 = MathHelper.ToRadians(390.0f);

            QuaternionD actual1 = QuaternionD.CreateFromAxisAngle(axis, angle1);
            QuaternionD actual2 = QuaternionD.CreateFromAxisAngle(axis, angle2);
            actual1.X = -actual1.X;
            actual1.W = -actual1.W;

            Assert.True(MathHelper.Equal(actual1, actual2), $"QuaternionD.CreateFromAxisAngle did not return the expected value: actual1 {actual1} actual2 {actual2}");
        }

        [Fact]
        public void QuaternionDCreateFromYawPitchRollTest1()
        {
            float yawAngle = MathHelper.ToRadians(30.0f);
            float pitchAngle = MathHelper.ToRadians(40.0f);
            float rollAngle = MathHelper.ToRadians(50.0f);

            QuaternionD yaw = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, yawAngle);
            QuaternionD pitch = QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, pitchAngle);
            QuaternionD roll = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, rollAngle);

            QuaternionD expected = yaw * pitch * roll;
            QuaternionD actual = QuaternionD.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.QuaternionDCreateFromYawPitchRollTest1 did not return the expected value: expected {expected} actual {actual}");
        }

        // Covers more numeric rigions
        [Fact]
        public void QuaternionDCreateFromYawPitchRollTest2()
        {
            const float step = 35.0f;

            for (float yawAngle = -720.0f; yawAngle <= 720.0f; yawAngle += step)
            {
                for (float pitchAngle = -720.0f; pitchAngle <= 720.0f; pitchAngle += step)
                {
                    for (float rollAngle = -720.0f; rollAngle <= 720.0f; rollAngle += step)
                    {
                        float yawRad = MathHelper.ToRadians(yawAngle);
                        float pitchRad = MathHelper.ToRadians(pitchAngle);
                        float rollRad = MathHelper.ToRadians(rollAngle);

                        QuaternionD yaw = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, yawRad);
                        QuaternionD pitch = QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, pitchRad);
                        QuaternionD roll = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, rollRad);

                        QuaternionD expected = yaw * pitch * roll;
                        QuaternionD actual = QuaternionD.CreateFromYawPitchRoll(yawRad, pitchRad, rollRad);
                        Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.QuaternionDCreateFromYawPitchRollTest2 Yaw:{yawAngle} Pitch:{pitchAngle} Roll:{rollAngle} did not return the expected value: expected {expected} actual {actual}");
                    }
                }
            }
        }

        // A test for Slerp (QuaternionD, QuaternionD, float)
        [Fact]
        public void QuaternionDSlerpTest()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0f));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0f));

            float t = 0.5f;

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(20.0f));
            QuaternionD actual;

            actual = QuaternionD.Slerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");

            // Case a and b are same.
            expected = a;
            actual = QuaternionD.Slerp(a, a, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (QuaternionD, QuaternionD, float)
        // Slerp test where t = 0
        [Fact]
        public void QuaternionDSlerpTest1()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0f));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0f));

            float t = 0.0f;

            QuaternionD expected = new QuaternionD(a.X, a.Y, a.Z, a.W);
            QuaternionD actual = QuaternionD.Slerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (QuaternionD, QuaternionD, float)
        // Slerp test where t = 1
        [Fact]
        public void QuaternionDSlerpTest2()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0f));
            QuaternionD b = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0f));

            float t = 1.0f;

            QuaternionD expected = new QuaternionD(b.X, b.Y, b.Z, b.W);
            QuaternionD actual = QuaternionD.Slerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (QuaternionD, QuaternionD, float)
        // Slerp test where dot product is < 0
        [Fact]
        public void QuaternionDSlerpTest3()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0f));
            QuaternionD b = -a;

            float t = 1.0f;

            QuaternionD expected = a;
            QuaternionD actual = QuaternionD.Slerp(a, b, t);
            // Note that in quaternion world, Q == -Q. In the case of quaternions dot product is zero,
            // one of the quaternion will be flipped to compute the shortest distance. When t = 1, we
            // expect the result to be the same as quaternion b but flipped.
            Assert.True(actual == expected, $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (QuaternionD, QuaternionD, float)
        // Slerp test where the quaternion is flipped
        [Fact]
        public void QuaternionDSlerpTest4()
        {
            Vector3D axis = Vector3D.Normalize(new Vector3D(1.0f, 2.0f, 3.0f));
            QuaternionD a = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(10.0f));
            QuaternionD b = -QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0f));

            float t = 0.0f;

            QuaternionD expected = new QuaternionD(a.X, a.Y, a.Z, a.W);
            QuaternionD actual = QuaternionD.Slerp(a, b, t);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator - (QuaternionD)
        [Fact]
        public void QuaternionDUnaryNegationTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);

            QuaternionD expected = new QuaternionD(-1.0f, -2.0f, -3.0f, -4.0f);
            QuaternionD actual;

            actual = -a;

            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.operator - did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Inverse (QuaternionD)
        [Fact]
        public void QuaternionDInverseTest()
        {
            QuaternionD a = new QuaternionD(5.0f, 6.0f, 7.0f, 8.0f);

            QuaternionD expected = new QuaternionD(-0.0287356321f, -0.03448276f, -0.0402298868f, 0.04597701f);
            QuaternionD actual;

            actual = QuaternionD.Inverse(a);
            Assert.Equal(expected, actual);
        }

        // A test for Inverse (QuaternionD)
        // Invert zero length quaternion
        [Fact]
        public void QuaternionDInverseTest1()
        {
            QuaternionD a = new QuaternionD();

            QuaternionD expected = QuaternionD.Zero;
            QuaternionD actual = QuaternionD.Inverse(a);

            Assert.Equal(expected, actual);
        }

        // A test for ToString ()
        [Fact]
        public void QuaternionDToStringTest()
        {
            QuaternionD target = new QuaternionD(-1.0f, 2.2f, 3.3f, -4.4f);

            string expected = string.Format(CultureInfo.CurrentCulture
                , "{{X:{0} Y:{1} Z:{2} W:{3}}}"
                , -1.0f, 2.2f, 3.3f, -4.4f);

            string actual = target.ToString();
            Assert.Equal(expected, actual);
        }

        // A test for Add (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDAddTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(5.0f, 6.0f, 7.0f, 8.0f);

            QuaternionD expected = new QuaternionD(6.0f, 8.0f, 10.0f, 12.0f);
            QuaternionD actual;

            actual = QuaternionD.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDDivideTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(5.0f, 6.0f, 7.0f, 8.0f);

            QuaternionD expected = new QuaternionD(-0.045977015f, -0.09195402f, -7.450581E-9f, 0.402298868f);
            QuaternionD actual;

            actual = QuaternionD.Divide(a, b);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Divide did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Equals (object)
        [Fact]
        public void QuaternionDEqualsTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            object obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0f;
            obj = b;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare between different types.
            obj = new Vector4D();
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare against null.
            obj = null;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);
        }

        // A test for GetHashCode ()
        [Fact]
        public void QuaternionDGetHashCodeTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);

            int expected = HashCode.Combine(a.X, a.Y, a.Z, a.W);
            int actual = a.GetHashCode();
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (QuaternionD, float)
        [Fact]
        public void QuaternionDMultiplyTest2()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            float factor = 0.5f;

            QuaternionD expected = new QuaternionD(0.5f, 1.0f, 1.5f, 2.0f);
            QuaternionD actual;

            actual = QuaternionD.Multiply(a, factor);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Multiply did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Multiply (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDMultiplyTest3()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(5.0f, 6.0f, 7.0f, 8.0f);

            QuaternionD expected = new QuaternionD(24.0f, 48.0f, 48.0f, -6.0f);
            QuaternionD actual;

            actual = QuaternionD.Multiply(a, b);
            Assert.True(MathHelper.Equal(expected, actual), $"QuaternionD.Multiply did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Negate (QuaternionD)
        [Fact]
        public void QuaternionDNegateTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);

            QuaternionD expected = new QuaternionD(-1.0f, -2.0f, -3.0f, -4.0f);
            QuaternionD actual;

            actual = QuaternionD.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for Subtract (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDSubtractTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 6.0f, 7.0f, 4.0f);
            QuaternionD b = new QuaternionD(5.0f, 2.0f, 3.0f, 8.0f);

            QuaternionD expected = new QuaternionD(-4.0f, 4.0f, 4.0f, -4.0f);
            QuaternionD actual;

            actual = QuaternionD.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDInequalityTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0f;
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (QuaternionD, QuaternionD)
        [Fact]
        public void QuaternionDEqualityTest()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0f;
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert Identity matrix test
        [Fact]
        public void QuaternionDFromRotationMatrixTest1()
        {
            Matrix4x4D matrix = Matrix4x4D.Identity;

            QuaternionD expected = new QuaternionD(0.0f, 0.0f, 0.0f, 1.0f);
            QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
            Assert.True(MathHelper.Equal(expected, actual),
                $"QuaternionD.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
            Assert.True(MathHelper.Equal(matrix, m2),
                $"QuaternionD.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert X axis rotation matrix
        [Fact]
        public void QuaternionDFromRotationMatrixTest2()
        {
            for (float angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                Matrix4x4D matrix = Matrix4x4D.CreateRotationX(angle);

                QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);
                QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
                Assert.True(MathHelper.EqualRotation(expected, actual),
                    $"QuaternionD.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
                Assert.True(MathHelper.Equal(matrix, m2),
                    $"QuaternionD.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert Y axis rotation matrix
        [Fact]
        public void QuaternionDFromRotationMatrixTest3()
        {
            for (float angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                Matrix4x4D matrix = Matrix4x4D.CreateRotationY(angle);

                QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle);
                QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
                Assert.True(MathHelper.EqualRotation(expected, actual),
                    $"QuaternionD.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
                Assert.True(MathHelper.Equal(matrix, m2),
                    $"QuaternionD.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert Z axis rotation matrix
        [Fact]
        public void QuaternionDFromRotationMatrixTest4()
        {
            for (float angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                Matrix4x4D matrix = Matrix4x4D.CreateRotationZ(angle);

                QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle);
                QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
                Assert.True(MathHelper.EqualRotation(expected, actual),
                    $"QuaternionD.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
                Assert.True(MathHelper.Equal(matrix, m2),
                    $"QuaternionD.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Convert XYZ axis rotation matrix
        [Fact]
        public void QuaternionDFromRotationMatrixTest5()
        {
            for (float angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                Matrix4x4D matrix = Matrix4x4D.CreateRotationX(angle) * Matrix4x4D.CreateRotationY(angle) * Matrix4x4D.CreateRotationZ(angle);

                QuaternionD expected =
                    QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle) *
                    QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle) *
                    QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);

                QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
                Assert.True(MathHelper.EqualRotation(expected, actual),
                    $"QuaternionD.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
                Assert.True(MathHelper.Equal(matrix, m2),
                    $"QuaternionD.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // X axis is most large axis case
        [Fact]
        public void QuaternionDFromRotationMatrixWithScaledMatrixTest1()
        {
            float angle = MathHelper.ToRadians(180.0f);
            Matrix4x4D matrix = Matrix4x4D.CreateRotationY(angle) * Matrix4x4D.CreateRotationZ(angle);

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle) * QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle);
            QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
            Assert.True(MathHelper.EqualRotation(expected, actual),
                $"QuaternionD.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
            Assert.True(MathHelper.Equal(matrix, m2),
                $"QuaternionD.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Y axis is most large axis case
        [Fact]
        public void QuaternionDFromRotationMatrixWithScaledMatrixTest2()
        {
            float angle = MathHelper.ToRadians(180.0f);
            Matrix4x4D matrix = Matrix4x4D.CreateRotationX(angle) * Matrix4x4D.CreateRotationZ(angle);

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle) * QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);
            QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
            Assert.True(MathHelper.EqualRotation(expected, actual),
                $"QuaternionD.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
            Assert.True(MathHelper.Equal(matrix, m2),
                $"QuaternionD.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for CreateFromRotationMatrix (Matrix4x4D)
        // Z axis is most large axis case
        [Fact]
        public void QuaternionDFromRotationMatrixWithScaledMatrixTest3()
        {
            float angle = MathHelper.ToRadians(180.0f);
            Matrix4x4D matrix = Matrix4x4D.CreateRotationX(angle) * Matrix4x4D.CreateRotationY(angle);

            QuaternionD expected = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle) * QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);
            QuaternionD actual = QuaternionD.CreateFromRotationMatrix(matrix);
            Assert.True(MathHelper.EqualRotation(expected, actual),
                $"QuaternionD.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            Matrix4x4D m2 = Matrix4x4D.CreateFromQuaternion(actual);
            Assert.True(MathHelper.Equal(matrix, m2),
                $"QuaternionD.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for Equals (QuaternionD)
        [Fact]
        public void QuaternionDEqualsTest1()
        {
            QuaternionD a = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);
            QuaternionD b = new QuaternionD(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b.X = 10.0f;
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for Zero
        [Fact]
        public void QuaternionDZeroTest()
        {
            // A default value should be equal to a zero value.
            Assert.Equal(default(QuaternionD), QuaternionD.Zero);

            // A newly constructed value should be equal to a zero value.
            Assert.Equal(new QuaternionD(), QuaternionD.Zero);

            // A newly constructed value with (0, 0, 0, 0) should be equal to a zero value.
            Assert.Equal(new QuaternionD(0, 0, 0, 0), QuaternionD.Zero);
        }

        // A test for Identity
        [Fact]
        public void QuaternionDIdentityTest()
        {
            QuaternionD val = new QuaternionD(0, 0, 0, 1);
            Assert.Equal(val, QuaternionD.Identity);
        }

        // A test for IsIdentity
        [Fact]
        public void QuaternionDIsIdentityTest()
        {
            Assert.True(QuaternionD.Identity.IsIdentity);
            Assert.True(new QuaternionD(0, 0, 0, 1).IsIdentity);
            Assert.False(new QuaternionD(1, 0, 0, 1).IsIdentity);
            Assert.False(new QuaternionD(0, 1, 0, 1).IsIdentity);
            Assert.False(new QuaternionD(0, 0, 1, 1).IsIdentity);
            Assert.False(new QuaternionD(0, 0, 0, 0).IsIdentity);
        }

        // A test for QuaternionD comparison involving NaN values
        [Fact]
        public void QuaternionDEqualsNaNTest()
        {
            QuaternionD a = new QuaternionD(float.NaN, 0, 0, 0);
            QuaternionD b = new QuaternionD(0, float.NaN, 0, 0);
            QuaternionD c = new QuaternionD(0, 0, float.NaN, 0);
            QuaternionD d = new QuaternionD(0, 0, 0, float.NaN);

            Assert.False(a == new QuaternionD(0, 0, 0, 0));
            Assert.False(b == new QuaternionD(0, 0, 0, 0));
            Assert.False(c == new QuaternionD(0, 0, 0, 0));
            Assert.False(d == new QuaternionD(0, 0, 0, 0));

            Assert.True(a != new QuaternionD(0, 0, 0, 0));
            Assert.True(b != new QuaternionD(0, 0, 0, 0));
            Assert.True(c != new QuaternionD(0, 0, 0, 0));
            Assert.True(d != new QuaternionD(0, 0, 0, 0));

            Assert.False(a.Equals(new QuaternionD(0, 0, 0, 0)));
            Assert.False(b.Equals(new QuaternionD(0, 0, 0, 0)));
            Assert.False(c.Equals(new QuaternionD(0, 0, 0, 0)));
            Assert.False(d.Equals(new QuaternionD(0, 0, 0, 0)));

            Assert.False(a.IsIdentity);
            Assert.False(b.IsIdentity);
            Assert.False(c.IsIdentity);
            Assert.False(d.IsIdentity);

            Assert.True(a.Equals(a));
            Assert.True(b.Equals(b));
            Assert.True(c.Equals(c));
            Assert.True(d.Equals(d));
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void QuaternionDSizeofTest()
        {
            Assert.Equal(16, sizeof(QuaternionD));
            Assert.Equal(32, sizeof(QuaternionD_2x));
            Assert.Equal(20, sizeof(QuaternionDPlusFloat));
            Assert.Equal(40, sizeof(QuaternionDPlusFloat_2x));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct QuaternionD_2x
        {
            private QuaternionD _a;
            private QuaternionD _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct QuaternionDPlusFloat
        {
            private QuaternionD _v;
            private float _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct QuaternionDPlusFloat_2x
        {
            private QuaternionDPlusFloat _a;
            private QuaternionDPlusFloat _b;
        }

        // A test to make sure the fields are laid out how we expect
        [Fact]
        public unsafe void QuaternionDFieldOffsetTest()
        {
            QuaternionD quat = new QuaternionD();

            float* basePtr = &quat.X; // Take address of first element
            QuaternionD* quatPtr = &quat; // Take address of whole QuaternionD

            Assert.Equal(new IntPtr(basePtr), new IntPtr(quatPtr));

            Assert.Equal(new IntPtr(basePtr + 0), new IntPtr(&quat.X));
            Assert.Equal(new IntPtr(basePtr + 1), new IntPtr(&quat.Y));
            Assert.Equal(new IntPtr(basePtr + 2), new IntPtr(&quat.Z));
            Assert.Equal(new IntPtr(basePtr + 3), new IntPtr(&quat.W));
        }
    }
}
