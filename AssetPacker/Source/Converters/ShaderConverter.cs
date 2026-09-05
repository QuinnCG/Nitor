using System;
using System.Collections.Generic;
using System.Text;

namespace AssetPacker.Converters;

class ShaderConverter : AssetConverter
{
	public override string[] Extensions { get; } = ["shader", "glsl"];

	private readonly Dictionary<string, string> _keywordToSpecialInclude = new()
	{
		{ "SpriteInstance",
			"""
			struct SpriteInstance
			{
				vec2 position;
				vec2 size;

				vec4 uvMinMax;
				vec4 color;

				uvec2 textureHandle;
				float rotation;
			};
			""" }
	};

	public override async Task<byte[]> ConvertAsync(FileStream stream)
	{
		var bytes = await ReadAllBytesAsync(stream);
		string source = Encoding.Default.GetString(bytes);

		string[] lines = source.Split('\n', StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < lines.Length; i++)
		{
			ResolveIncludeDirective(lines, i);
			ResolveInjectionDirective(lines, i);
		}

		return Encoding.Default.GetBytes(string.Join('\n', lines));
	}

	private void ResolveIncludeDirective(string[] lines, int index)
	{
		string line = lines[index].Trim();

		const string keyword = "#include";

		if (line.StartsWith(keyword))
		{
			string toInclude = line[(line.IndexOf(keyword) + keyword.Length)..].Trim();

			if (toInclude.StartsWith('"') && toInclude.EndsWith('"'))
			{
				toInclude = toInclude.Remove(0, 1).Remove(toInclude.Length - 2);

				if (Packer.DoesFileExistAtSource(toInclude))
				{
					string includeSource = Encoding.Default.GetString(Packer.ReadAllBytesAtSource(toInclude));
					lines[index] = includeSource;
				}
				else
				{
					throw new Exception($"Cannot include file at '{toInclude}' because file was not found!");
				}
			}
			else if (_keywordToSpecialInclude.TryGetValue(toInclude, out var injection))
			{
				lines[index] = injection;
			}
			else
			{
				throw new Exception($"Invalid include declaration '{toInclude}'!");
			}
		}
	}

	private static void ResolveInjectionDirective(string[] lines, int index)
	{
		string line = lines[index].Trim();
		const string keyword = "#inject";

		if (!line.StartsWith(keyword))
		{
			return;
		}

		line = line.Replace(keyword, string.Empty);
		var parts = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

		string layoutData = string.Empty;

		if (line.Contains('(') && line.Contains(')'))
		{
			layoutData = line[line.IndexOf('(')..(line.IndexOf(')') + 1)];
		}

		lines[index] =
				$$"""
				{{(string.IsNullOrWhiteSpace(layoutData) ? string.Empty : "layout")}} {{layoutData}} readonly buffer Buffer{{Guid.NewGuid():N}}
				{
				    {{parts[^2]}} {{parts[^1]}}[];
				};
				""";
	}
}
