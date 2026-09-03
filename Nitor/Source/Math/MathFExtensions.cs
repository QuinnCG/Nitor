namespace Nitor.Math;

public static class MathFExtensions
{
	extension(MathF)
	{
		public static int Min(int a, int b)
		{
			if (b < a)
			{
				return b;
			}
			else
			{
				return a;
			}
		}

		public static int Max(int a, int b)
		{
			if (b > a)
			{
				return b;
			}
			else
			{
				return a;
			}
		}
	}
}
