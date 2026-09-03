using System.Runtime.InteropServices;

namespace Nitor.Math;

[StructLayout(LayoutKind.Sequential)]
public struct Vec2 : IEquatable<Vec2>
{
	public static Vec2 Zero { get; } = new();
	public static Vec2 Up { get; } = new(0f, 1f);
	public static Vec2 Down { get; } = new(0f, -1f);
	public static Vec2 Left { get; } = new(-1f, 0f);
	public static Vec2 Right { get; } = new(1f, 0f);

	public float X;
	public float Y;

	public readonly float MagnitudeSquared
	{
		get
		{
			return MathF.Pow(X, 2) + MathF.Pow(Y, 2);
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
	public readonly Vec2 Normalized
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

	public Vec2(float v)
	{
		X = Y = v;
	}
	public Vec2(float x, float y)
	{
		X = x;
		Y = y;
	}

	public static implicit operator Vec2(System.Numerics.Vector2 v)
	{
		return new(v.X, v.Y);
	}

	public static bool operator ==(Vec2 left, Vec2 right)
	{
		return (left.X == right.X && left.Y == right.Y);
	}
	public static bool operator !=(Vec2 left, Vec2 right)
	{
		return (left.X != right.X && left.Y != right.Y);
	}
	public static Vec2 operator +(Vec2 left, Vec2 right)
	{
		return new(left.X + right.X, left.Y + right.Y);
	}
	public static Vec2 operator -(Vec2 left, Vec2 right)
	{
		return new(left.X - right.X, left.Y - right.Y);
	}
	public static Vec2 operator *(Vec2 v, float f)
	{
		return new(v.X * f, v.Y * f);
	}
	public static Vec2 operator /(Vec2 v, float f)
	{
		return new(v.X / f, v.Y / f);
	}

	public override readonly bool Equals(object? obj) => obj is Vec2 v && v == this;
	public override readonly int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

	public Vec2 WithX(float x)
	{
		X = x;
		return this;
	}
	public Vec2 WithY(float y)
	{
		Y = y;
		return this;
	}

	public readonly bool Equals(Vec2 other)
	{
		return other == this;
	}
}

