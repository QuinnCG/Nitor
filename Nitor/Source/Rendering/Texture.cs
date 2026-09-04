using System.Reflection.Metadata;
using Nitor.AssetManagement;
using Silk.NET.OpenGL;
using StbiSharp;

namespace Nitor.Rendering;

public class Texture : Asset, IBindable
{
	public static Texture NoTexture { get; } = new();

	public uint Handle { get; private set; }
	/// <summary>
	/// This is only valid if <see cref="Renderer.GLExtensions.UsesBindlessTextures"/> is true.
	/// </summary>
	public ulong BindlessHandle { get; private set; }

	protected override void OnLoad(byte[] data)
	{
		var gl = Renderer.OpenGL;

		using var reader = new BinaryReader(new MemoryStream(data));

		uint width = reader.ReadUInt32();
		uint height = reader.ReadUInt32();
		int pixelDataLength = reader.ReadInt32();
		var pixels = new ReadOnlySpan<byte>(reader.ReadBytes(pixelDataLength));

		Handle = gl.CreateTexture(TextureTarget.Texture2D);
		Bind();

		gl.TextureParameter(Handle, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
		gl.TextureParameter(Handle, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
		gl.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
		gl.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

		gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
		gl.GenerateMipmap(TextureTarget.Texture2D);

		var extensions = Renderer.Extensions;
		if (extensions.UsesBindlessTextures)
		{
			BindlessHandle = extensions.Bindless.GetTextureHandle(Handle);
			extensions.Bindless.MakeTextureHandleResident(BindlessHandle);
		}
	}

	protected override void OnUnload()
	{
		var extensions = Renderer.Extensions;
		if (extensions.UsesBindlessTextures)
		{
			extensions.Bindless.MakeTextureHandleNonResident(BindlessHandle);
			BindlessHandle = 0;
		}

		Renderer.OpenGL.DeleteTexture(Handle);
		Handle = 0;
	}

	public void Bind()
	{
		Renderer.OpenGL.ActiveTexture(TextureUnit.Texture0);
		Renderer.OpenGL.BindTexture(TextureTarget.Texture2D, Handle);
	}
}
