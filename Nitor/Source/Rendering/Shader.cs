using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;
using Nitor.AssetManagement;
using Nitor.Math;
using Silk.NET.OpenGL;
using static Silk.NET.Core.Native.WinString;

namespace Nitor.Rendering;

public class Shader : Asset, IBindable
{
	public uint Handle { get; private set; }
	public IEnumerable<string> Uniforms => _uniforms;

	private readonly HashSet<string> _uniforms = [];

	protected override void OnLoad(byte[] data)
	{
		var gl = Renderer.OpenGL;

		Handle = gl.CreateProgram();
		uint vs, fs;

		if (Asset.AreAssetsPacked)
		{
			const int intSize = sizeof(int);
			var span = data.AsSpan();

			int vertSectionEnd = intSize + BitConverter.ToInt32(span[..intSize]);
			var vertData = span[intSize..vertSectionEnd];
			int fragSectionSize = BitConverter.ToInt32(span[vertSectionEnd..(vertSectionEnd + intSize)]);
			var fragData = span[(vertSectionEnd + intSize)..];

			vs = CreateShader(ShaderType.VertexShader, vertData);
			fs = CreateShader(ShaderType.FragmentShader, fragData);
		}
		else
		{
			string source = Encoding.Default.GetString(data);
			var parts = source.Split("// Fragment");

			if (parts.Length != 2)
			{
				throw new Exception("Must have vertex section and fragment section separated by '// Fragment'!");
			}

			vs = CreateShader(ShaderType.VertexShader, parts[0]);
			fs = CreateShader(ShaderType.FragmentShader, parts[1]);
		}

		gl.AttachShader(Handle, vs);
		gl.AttachShader(Handle, fs);
		gl.LinkProgram(Handle);
		gl.ValidateProgram(Handle);

		string info = gl.GetProgramInfoLog(Handle);
		if (!string.IsNullOrWhiteSpace(info))
		{
			gl.DeleteShader(vs);
			gl.DeleteShader(fs);
			throw new Exception(info);
		}

		gl.DeleteShader(vs);
		gl.DeleteShader(fs);

		gl.GetProgram(Handle, ProgramPropertyARB.ActiveUniforms, out int uniformCount);

		for (uint i = 0; i < uniformCount; i++)
		{
			string name = gl.GetActiveUniform(Handle, i, out int size, out UniformType type);
			_uniforms.Add(name);
		}
	}

	protected override void OnUnload()
	{
		Renderer.OpenGL.DeleteShader(Handle);
	}

	public void Bind()
	{
		Renderer.OpenGL.UseProgram(Handle);
	}

	public bool HasUniform(string name)
	{
		return _uniforms.Contains(name);
	}

	public int GetUniformAddress(string name)
	{
		return Renderer.OpenGL.GetUniformLocation(Handle, name);
	}

	public void SetUniform(string name, float value)
	{
		Renderer.OpenGL.Uniform1(GetUniformAddress(name), value);
	}
	public void SetUniform(string name, int value)
	{
		Renderer.OpenGL.Uniform1(GetUniformAddress(name), value);
	}
	public void SetUniform(string name, double value)
	{
		Renderer.OpenGL.Uniform1(GetUniformAddress(name), value);
	}
	public void SetUniform(string name, bool value)
	{
		Renderer.OpenGL.Uniform1(GetUniformAddress(name), value ? 1 : 0);
	}
	public void SetUniform(string name, Vec2 value)
	{
		Renderer.OpenGL.Uniform2(GetUniformAddress(name), value.X, value.Y);
	}
	public void SetUniform(string name, Vec2i value)
	{
		Renderer.OpenGL.Uniform2(GetUniformAddress(name), value.X, value.Y);
	}
	public void SetUniform(string name, Vec3 value)
	{
		Renderer.OpenGL.Uniform3(GetUniformAddress(name), value.X, value.Y, value.Z);
	}
	public void SetUniform(string name, Vec4 value)
	{
		Renderer.OpenGL.Uniform4(GetUniformAddress(name), value.X, value.Y, value.Z, value.W);
	}
	public void SetUniform(string name, Color value)
	{
		Renderer.OpenGL.Uniform4(GetUniformAddress(name), value.R, value.G, value.B, value.A);
	}
	public unsafe void SetUniform(string name, Matrix4x4 value)
	{
		Renderer.OpenGL.UniformMatrix4(GetUniformAddress(name), false, new ReadOnlySpan<float>(&value.M11, 16));
	}

	public float GetUniformFloat(string name)
	{
		Renderer.OpenGL.GetUniform(Handle, GetUniformAddress(name), out float value);
		return value;
	}
	public int GetUniformInt(string name)
	{
		Renderer.OpenGL.GetUniform(Handle, GetUniformAddress(name), out int value);
		return value;
	}
	public double GetUniformDouble(string name)
	{
		Renderer.OpenGL.GetUniform(Handle, GetUniformAddress(name), out double value);
		return value;
	}
	public bool GetUniformBool(string name)
	{
		Renderer.OpenGL.GetUniform(Handle, GetUniformAddress(name), out int value);
		return value > 0;
	}

	private static uint CreateShader(ShaderType type, string source)
	{
		var gl = Renderer.OpenGL;

		var shader = gl.CreateShader(type);
		gl.ShaderSource(shader, source);
		gl.CompileShader(shader);

		string info = gl.GetShaderInfoLog(shader);
		if (!string.IsNullOrWhiteSpace(info))
		{
			gl.DeleteShader(shader);
			throw new Exception(info);
		}

		return shader;
	}
	private static unsafe uint CreateShader(ShaderType type, Span<byte> source)
	{
		var gl = Renderer.OpenGL;

		var shader = gl.CreateShader(type);
		gl.ShaderBinary(new ReadOnlySpan<uint>([shader]), ShaderBinaryFormat.ShaderBinaryFormatSpirV, source);
		gl.SpecializeShader(shader, "main", 0, null, null);

		return shader;
	}
}
