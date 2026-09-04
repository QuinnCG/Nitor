using System.Runtime.InteropServices;

namespace Nitor.Math;

[StructLayout(LayoutKind.Sequential)]
public struct Vec3 : IEquatable<Vec3>
{
	public static Vec3 Zero { get; } = new();
	public static Vec3 One { get; } = new(1f, 1f, 1f);

	public static Vec3 Up { get; } = new(0f, 1f);
	public static Vec3 Down { get; } = new(0f, -1f);
	public static Vec3 Left { get; } = new(-1f, 0f);
	public static Vec3 Right { get; } = new(1f, 0f);
	public static Vec3 Forward { get; } = new(0f, 0f, 1f);
	public static Vec3 Backward { get; } = new(0f, 0f, -1f);

	public float X ;
	public float Y ;
	public float Z;

	public readonly float MagnitudeSquared
	{
		get
		{
			return MathF.Pow(X, 2) + MathF.Pow(Y, 2) + MathF.Pow(Z, 2);
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
	public readonly Vec3 Normalized
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

	public Vec3(float v)
	{
		X = Y = Z = v;
	}
	public Vec3(float x, float y)
	{
		X = x;
		Y = y;
	}
	public Vec3(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
	}
	public Vec3(Vec2 v)
	{
		X = v.X;
		Y = v.Y;
	}

	public static implicit operator Vec3(System.Numerics.Vector3 v)
	{
		return new(v.X, v.Y, v.Z);
	}

	public static bool operator ==(Vec3 left, Vec3 right)
	{
		return (left.X == right.X && left.Y == right.Y && left.Z == right.Z);
	}
	public static bool operator !=(Vec3 left, Vec3 right)
	{
		return (left.X != right.X && left.Y != right.Y && left.Z != right.Z);
	}
	public static Vec3 operator +(Vec3 left, Vec3 right)
	{
		return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
	}
	public static Vec3 operator -(Vec3 left, Vec3 right)
	{
		return new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
	}
	public static Vec3 operator *(Vec3 v, float f)
	{
		return new(v.X * f, v.Y * f, v.Z * f);
	}
	public static Vec3 operator /(Vec3 v, float f)
	{
		return new(v.X / f, v.Y / f, v.Z / f);
	}

	public static Vec3 operator +(Vec3 left, Vec2 right)
	{
		return new(left.X + right.X, left.Y + right.Y, left.Z);
	}
	public static Vec3 operator -(Vec3 left, Vec2 right)
	{
		return new(left.X - right.X, left.Y - right.Y, left.Z);
	}

	public override readonly bool Equals(object? obj) => obj is Vec3 v && v == this;
	public override readonly int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode());

	public Vec3 WithX(float x)
	{
		X = x;
		return this;
	}
	public Vec3 WithY(float y)
	{
		Y = y;
		return this;
	}
	public Vec3 WithZ(float z)
	{
		Z = z;
		return this;
	}

	public readonly bool Equals(Vec3 other)
	{
		return other == this;
	}
}
