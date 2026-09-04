using System.Reflection.Metadata;
using Nitor.AssetManagement;
using Silk.NET.OpenGL;
using StbiSharp;

namespace Nitor.Rendering;

public class Texture : Asset, IBindable
{
	public uint Handle { get; private set; }

	protected override void OnLoad(byte[] data)
	{
		var gl = Renderer.OpenGL;

		uint width, height;
		ReadOnlySpan<byte> pixels;
		StbiImage? imageData = null;

		if (Asset.AreAssetsPacked)
		{
			using var reader = new BinaryReader(new MemoryStream(data));
			width = reader.ReadUInt32();
			height = reader.ReadUInt32();
			int pixelDataLength = reader.ReadInt32();
			pixels = new(reader.ReadBytes(pixelDataLength));
		}
		else
		{
			Stbi.SetFlipVerticallyOnLoad(true);
			imageData = Stbi.LoadFromMemory(new ReadOnlySpan<byte>(data), 4);

			width = (uint)imageData.Width;
			height = (uint)imageData.Height;
			pixels = imageData.Data;
		}

		Handle = gl.CreateTexture(TextureTarget.Texture2D);
		Bind();

		gl.TextureParameter(Handle, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
		gl.TextureParameter(Handle, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
		gl.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
		gl.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

		gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
		gl.GenerateMipmap(TextureTarget.Texture2D);

		imageData?.Dispose();
	}

	protected override void OnUnload()
	{
		Renderer.OpenGL.DeleteTexture(Handle);
	}

	public void Bind()
	{
		Renderer.OpenGL.ActiveTexture(TextureUnit.Texture0);
		Renderer.OpenGL.BindTexture(TextureTarget.Texture2D, Handle);
	}
}
