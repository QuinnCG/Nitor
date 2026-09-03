namespace AssetPacker;

static class Program
{
	static async Task Main(string[] args)
	{
		if (args.Length != 2)
		{
			string suffix = args.Length > 2 ? "Too many args!" : "Too few args!";
			throw new ArgumentException("Must have two arguments: source asset path & target build path! " + suffix);
		}

		await Packer.PackAsync(args[0], args[1]);
	}
}
