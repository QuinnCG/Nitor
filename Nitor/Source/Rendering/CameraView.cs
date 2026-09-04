using System.Numerics;
using Nitor.Math;

namespace Nitor.Rendering;

[RequireSibling<Transform>]
public class CameraView : Component
{
	public static CameraView? Current { get; private set; }

	public float OrthographicScale { get; set; } = 5f;

	private Transform _transform = null!;

	public Matrix4x4 Matrix
	{
		get
		{
			var scale = OrthographicScale;
			float ratio = Application.WindowRatio;

			var pos = _transform.Position;
			var rot = _transform.Rotation;

			return
				Matrix4x4.CreateTranslation(-pos.X, -pos.Y, -1f) *
				Matrix4x4.CreateRotationZ(-rot * MathF.DegToRad) *
				Matrix4x4.CreateOrthographic(OrthographicScale * ratio, OrthographicScale, 0.1f, 5f);
		}
	}

	protected override void OnCreate()
	{
		Current ??= this;
		_transform = GetFirstSibling<Transform>();
	}

	protected override void OnDestroy()
	{
		if (Current == this)
		{
			Current = null;
		}
	}

	public void SetCurrent()
	{
		Current = this;
	}

	public static void ClearCurrent()
	{
		Current = null;
	}
}
