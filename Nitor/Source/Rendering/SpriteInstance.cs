using System.Runtime.InteropServices;
using Nitor.Math;

namespace Nitor.Rendering;

[StructLayout(LayoutKind.Sequential)]
public readonly struct SpriteInstance
{
	public static readonly int StructSize;

	static SpriteInstance()
	{
		StructSize = Marshal.SizeOf<SpriteInstance>();
	}

	public readonly Vec2 Position;
	public readonly Vec2 Size;
	public readonly Vec4 UVMinMax;
	public readonly Color Color;
	public readonly ulong TextureHandle;
	public readonly float Rotation;

	public SpriteInstance(Vec2 position, Vec2 size, float rotation, Vec4 uVMinMax, ulong textureHandle, Color color)
	{
		Position = position;
		Size = size;
		Rotation = rotation;
		UVMinMax = uVMinMax;
		TextureHandle = textureHandle;
		Color = color;
	}
}
