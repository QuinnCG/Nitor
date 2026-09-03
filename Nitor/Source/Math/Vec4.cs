using System.Runtime.InteropServices;

namespace Nitor.Math;

[StructLayout(LayoutKind.Sequential)]
public struct Vec4 : IEquatable<Vec4>
{
	public static Vec4 Zero { get; } = new();
	public static Vec4 Up { get; } = new(0f, 1f);
	public static Vec4 Down { get; } = new(0f, -1f);
	public static Vec4 Left { get; } = new(-1f, 0f);
	public static Vec4 Right { get; } = new(1f, 0f);
	public static Vec4 Forward { get; } = new(0f, 0f, 1f);
	public static Vec4 Backward { get; } = new(0f, 0f, -1f);

	public float X;
	public float Y;
	public float Z;
	public float W;

	public readonly float MagnitudeSquared
	{
		get
		{
			return MathF.Pow(X, 2) + MathF.Pow(Y, 2) + MathF.Pow(Z, 2) + MathF.Pow(W, 2);
		}
	}

	public readonly float Magnitude
	{
		get
		{
			float mag2 = MagnitudeSquared;

			if (mag2 == 0f)
			{
				return 0f;
			}
			else
			{
				return MathF.Sqrt(mag2);
			}
		}
	}
	public readonly Vec4 Normalized
	{
		get
		{
			float mag = Magnitude;

			if (mag == 0f)
			{
				return Zero;
			}
			else
			{
				return this / mag;
			}
		}
	}

	public Vec4(float v)
	{
		X = Y = Z = v;
	}
	public Vec4(float x, float y)
	{
		X = x;
		Y = y;
	}
	public Vec4(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
	}
	public Vec4(float x, float y, float z, float w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}
	public Vec4(Vec2 v)
	{
		X = v.X;
		Y = v.Y;
	}
	public Vec4(Vec3 v)
	{
		X = v.X;
		Y = v.Y;
		Z = v.Z;
	}

	public static implicit operator Vec4(System.Numerics.Vector4 v)
	{
		return new(v.X, v.Y, v.Z, v.W);
	}

	public static bool operator ==(Vec4 left, Vec4 right)
	{
		return (left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.W == right.W);
	}
	public static bool operator !=(Vec4 left, Vec4 right)
	{
		return (left.X != right.X && left.Y != right.Y && left.Z != right.Z && left.W == right.W);
	}
	public static Vec4 operator +(Vec4 left, Vec4 right)
	{
		return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
	}
	public static Vec4 operator -(Vec4 left, Vec4 right)
	{
		return new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
	}
	public static Vec4 operator *(Vec4 v, float f)
	{
		return new(v.X * f, v.Y * f, v.Z * f, v.W * f);
	}
	public static Vec4 operator /(Vec4 v, float f)
	{
		return new(v.X / f, v.Y / f, v.Z / f, v.W / f);
	}

	public static Vec4 operator +(Vec4 left, Vec2 right)
	{
		return new(left.X + right.X, left.Y + right.Y, left.Z, left.W);
	}
	public static Vec4 operator -(Vec4 left, Vec2 right)
	{
		return new(left.X - right.X, left.Y - right.Y, left.Z, left.W);
	}
	public static Vec4 operator +(Vec4 left, Vec3 right)
	{
		return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W);
	}
	public static Vec4 operator -(Vec4 left, Vec3 right)
	{
		return new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W);
	}

	public override readonly bool Equals(object? obj) => obj is Vec4 v && v == this;
	public override readonly int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode(), W.GetHashCode());

	public Vec4 WithX(float x)
	{
		X = x;
		return this;
	}
	public Vec4 WithY(float y)
	{
		Y = y;
		return this;
	}
	public Vec4 WithZ(float z)
	{
		Z = z;
		return this;
	}
	public Vec4 WithW(float w)
	{
		W = w;
		return this;
	}

	public readonly bool Equals(Vec4 other)
	{
		return other == this;
	}
}
