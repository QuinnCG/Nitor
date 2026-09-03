namespace AssetPacker;

static class Program
{
	static void Main(string[] args)
	{
		foreach (var arg in args)
		{
			Console.WriteLine(arg);
			Packer.Pack();
		}
	}
}
