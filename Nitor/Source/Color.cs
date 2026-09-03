using System.Globalization;
using System.Runtime.InteropServices;

namespace Nitor;

[StructLayout(LayoutKind.Sequential)]
public struct Color
{
	public static Color Transparent { get; } = new(0f, 0f);

	public static Color Black { get; } = new(0f, 1f);
	public static Color White { get; } = new(1f, 1f);

	public static Color Red { get; } = new(1f, 0f, 0f);
	public static Color Green { get; } = new(0f, 1f, 0f);
	public static Color Blue { get; } = new(0f, 0f, 1f);

	public static Color Yellow { get; } = new(1f, 1f, 0f);
	public static Color Magenta { get; } = new(1f, 0f, 1f);
	public static Color Cyan { get; } = new(0f, 1f, 1f);

	public static Color Orange { get; } = new(0f, 1f, 1f);
	public static Color Purple { get; } = new(0f, 1f, 1f);

	/// <summary>
	/// Red channel.
	/// </summary>
	public float R;
	/// <summary>
	/// Green channel.
	/// </summary>
	public float G;
	/// <summary>
	/// Blue channel.
	/// </summary>
	public float B;
	/// <summary>
	/// Alpha channel.
	/// </summary>
	public float A;

	public readonly (byte R, byte G, byte B, byte A) Bytes
	{
		get
		{
			return ((byte)(R * 255f), (byte)(G * 255f), (byte)(B * 255f), (byte)(A * 255f));
		}
	}

	public Color(float v)
	{
		R = G = B = v;
		A = 1f;
	}
	public Color(float v, float a)
	{
		R = G = B = v;
		A = a;
	}
	public Color(float r, float g, float b)
	{
		R = r;
		G = g;
		B = b;
		A = 1f;
	}
	public Color(float r, float g, float b, float a)
	{
		R = r;
		G = g;
		B = b;
		A = a;
	}
	public Color(byte r, byte g, byte b)
	{
		R = r;
		G = g;
		B = b;
		A = 1f;
	}
	public Color(byte r, byte g, byte b, float a)
	{
		R = r;
		G = g;
		B = b;
		A = a;
	}

	/// <summary>
	/// Create an instance of <see cref="Color"/> from a parsed hexcode.<br/><br/>
	/// You can include or exclude the prefix '0x'.<br/>
	/// You can omit the last two characters representing alpha. In such cases, the alpha will default to 100%.<br/>
	/// Capitalization does not matter.<br/><br/>
	/// <strong>Example:</strong><br/>
	/// - 0xFF0000 = Red with no specified alpha.<br/>
	/// - 00ff00ff = Green with an explicit alpha of 100%.
	/// - 0XFFFFFFff = White with an explicit alpha of 100%.
	/// </summary>
	/// <param name="hex">The hexcode.</param>
	/// <returns>The instance of the created color.</returns>
	/// <exception cref="ArgumentException">If the hex is given in the wrong form, this exception can be thrown.</exception>
	public static Color FromHex(string hex)
	{
		static int Parse(string hex, string channelDisplayName)
		{
			if (int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result))
			{
				return result;
			}

			throw new ArgumentException($"Failed to parse hex characters '{hex}' for'{channelDisplayName}'!");
		}

		hex = hex
			.Trim()
			.ToLower()
			.Replace("0x", string.Empty);

		if (hex.Length is not 6 and not 8)
		{
			throw new ArgumentException($"Hexcode '{hex}' must have 6 or 8 characters, excepting the prefix. This corrosponds to RGB or RGBA, respectively.");
		}

		bool usesAlpha = hex.Length is 8;
		const float maxByte = 255f;

		return new Color()
		{
			R = Parse(hex[0..2], "Red") / maxByte,
			G = Parse(hex[2..4], "Green") / maxByte,
			B = Parse(hex[4..6], "Blue") / maxByte,
			A = usesAlpha ? (Parse(hex[6..8], "Alpha") / maxByte) : 1f
		};
	}

	// TODO: From HSV.
	// TODO: Provide way to calculate and get HSV for given color instance.

	public override readonly string ToString()
	{
		return $"<{R:0.00}, {G:0.00}, {B:0.00}, {A:0.00}>";
	}

	public Color WithR(float r)
	{
		R = r;
		return this;
	}
	public Color WithG(float g)
	{
		G = g;
		return this;
	}
	public Color WithB(float b)
	{
		B = b;
		return this;
	}
	public Color WithA(float a)
	{
		A = a;
		return this;
	}

	public static Color operator*(Color c, float factor)
	{
		return new()
		{
			R = c.R * factor,
			G = c.G * factor,
			B = c.B * factor
		};
	}
}
