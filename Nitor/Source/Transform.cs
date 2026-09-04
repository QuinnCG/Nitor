using Nitor.Math;

namespace Nitor;

public class Transform : Component
{
	public Vec2 Position { get; set; }
	public float Rotation { get; set; }
	public Vec2 Scale { get; set; } = Vec2.One;
}
