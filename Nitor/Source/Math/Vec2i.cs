using System.Runtime.InteropServices;

namespace Nitor.Math;

[StructLayout(LayoutKind.Sequential)]
public struct Vec2i : IEquatable<Vec2i>
{
	public static Vec2i Zero { get; } = new();
	public static Vec2i Up { get; } = new(0, 1);
	public static Vec2i Down { get; } = new(0, -1);
	public static Vec2i Left { get; } = new(-1, 0);
	public static Vec2i Right { get; } = new(1, 0);

	public int X;
	public int Y;

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

	public Vec2i(int v)
	{
		X = Y = v;
	}
	public Vec2i(int x, int y)
	{
		X = x;
		Y = y;
	}

	public static bool operator ==(Vec2i left, Vec2i right)
	{
		return (left.X == right.X && left.Y == right.Y);
	}
	public static bool operator !=(Vec2i left, Vec2i right)
	{
		return (left.X != right.X && left.Y != right.Y);
	}
	public static Vec2i operator +(Vec2i left, Vec2i right)
	{
		return new(left.X + right.X, left.Y + right.Y);
	}
	public static Vec2i operator -(Vec2i left, Vec2i right)
	{
		return new(left.X - right.X, left.Y - right.Y);
	}

	public override readonly bool Equals(object? obj) => obj is Vec2i v && v == this;
	public override readonly int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

	public Vec2i WithX(int x)
	{
		X = x;
		return this;
	}
	public Vec2i WithY(int y)
	{
		Y = y;
		return this;
	}

	public readonly bool Equals(Vec2i other)
	{
		return other == this;
	}
}
