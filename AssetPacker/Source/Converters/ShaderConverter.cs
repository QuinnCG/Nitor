using System.Runtime.InteropServices;
using System.Text;
using Silk.NET.Core.Native;
using Silk.NET.Shaderc;

namespace AssetPacker;

unsafe class ShaderConverter : AssetConverter
{
	private static Shaderc Shaderc = null!;
	private static Compiler* Compiler;
	private static CompileOptions* Options;

	public override string[] Extensions { get; } = ["shader", "glsl"];

	public override void OnInitialize()
	{
		Shaderc = Silk.NET.Shaderc.Shaderc.GetApi();

		Compiler = Shaderc.CompilerInitialize();
		Options = Shaderc.CompileOptionsInitialize();

		Shaderc.CompileOptionsSetTargetEnv(Options, TargetEnv.Opengl, 0);
		//Shaderc.CompileOptionsSetOptimizationLevel(Options, OptimizationLevel.Performance);
		Shaderc.CompileOptionsSetGenerateDebugInfo(Options);
	}

	public override async Task<byte[]> ConvertAsync(FileStream stream)
	{
		var reader = new StreamReader(stream);
		var source = reader.ReadToEnd();
		var parts = source.Split("// Fragment");

		if (parts.Length != 2)
		{
			throw new Exception("Shader source files must have vertex and fragment in the same file separated by a '// Fragment' comment!");
		}

		var vert = CompileShader(ShaderKind.VertexShader, parts[0]);
		var frag = CompileShader(ShaderKind.FragmentShader, parts[1]);

		var vertDataSize = BitConverter.GetBytes(vert.Length);
		var fragDataSize = BitConverter.GetBytes(frag.Length);

		return [..vertDataSize, .. vert, ..fragDataSize, .. frag];

		// Vertex section size (4 bytes)
		// Vertex data
		// Fragment section size (4 bytes)
		// Fragment data
	}

	private static byte[] CompileShader(ShaderKind kind, string source)
	{
		byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
		var result = Shaderc.CompileIntoSpv(Compiler, sourceBytes, (nuint)sourceBytes.Length, kind, "shader", "main", Options);

		if (Shaderc.ResultGetCompilationStatus(result) is not CompilationStatus.Success)
		{
			var errMsg = Shaderc.ResultGetErrorMessageS(result);
			Shaderc.ResultRelease(result);
			throw new Exception(errMsg);
		}

		var bytes = new Span<byte>(Shaderc.ResultGetBytes(result), (int)Shaderc.ResultGetLength(result)).ToArray();
		Shaderc.ResultRelease(result);
		return bytes;
	}

	public override void OnCleanUp()
	{
		Shaderc.CompileOptionsRelease(Options);
		Shaderc.CompilerRelease(Compiler);
		Shaderc.Dispose();
	}
}
