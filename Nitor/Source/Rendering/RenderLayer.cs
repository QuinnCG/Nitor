namespace Nitor.Rendering;

public class RenderLayer
{
	public static RenderLayer Default { get; } = new();

	public int Index => _layers.IndexOf(this);

	private static readonly List<RenderLayer> _layers = [Default];

	private RenderLayer() { }

	public static RenderLayer CreateAfter(RenderLayer target)
	{
		var layer = new RenderLayer();
		
		if (target.Index == _layers.Count - 1)
		{
			_layers.Add(layer);
		}
		else
		{
			_layers.Insert(target.Index + 1, layer);
		}

		return layer;
	}

	public static RenderLayer CreateBefore(RenderLayer target)
	{
		var layer = new RenderLayer();
		_layers.Insert(target.Index, layer);

		return layer;
	}

	public static RenderLayer CreateBehind()
	{
		var layer = new RenderLayer();
		_layers.Insert(0, layer);

		return layer;
	}

	public static RenderLayer CreateInFront()
	{
		var layer = new RenderLayer();
		_layers.Add(layer);

		return layer;
	}

	public override string ToString()
	{
		return $"Render Layer ({Index})";
	}
}
