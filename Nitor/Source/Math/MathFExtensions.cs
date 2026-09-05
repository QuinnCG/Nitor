namespace Nitor;

public static class MathFExtensions
{
	extension(MathF)
	{
		public static float RadToDeg => 180f / MathF.PI;
		public static float DegToRad => MathF.PI / 180f;
	}
}
