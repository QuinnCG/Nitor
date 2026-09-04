using Nitor.Math;

namespace Nitor.Rendering;

[RequireSibling<Transform>]
public class SpriteRenderer : Component
{
	public RenderLayer Layer { get; set; } = RenderLayer.Default;
	public BlendMode BlendMode { get; set; } = BlendMode.Opaque;
	public Color Color { get; set; } = Color.White;

	public required Shader Shader { get; set; }
	public required Sprite Sprite { get; set; }

	public Vec2 Pivot { get; set; }
	public Vec2 Scale { get; set; } = Vec2.One;

	private Transform _transform = null!;

	protected override void OnCreate()
	{
		_transform = GetFirstSibling<Transform>();
		SpriteRenderSystem.Register(this);
	}

	protected override void OnDestroy()
	{
		SpriteRenderSystem.Unregister(this);
	}

	internal SpriteInstance GetRenderData()
	{
		return new(_transform.Position - (Pivot / 2f * Scale), Scale, -_transform.Rotation * MathF.DegToRad, Sprite.UVMinMax, Sprite.Texture.BindlessHandle, Color);
	}
}
