using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Nitor.Rendering;

public class SpriteRenderSystem : ISystem, ILateUpdateSystem
{
	public static float BatchDestroyDelay { get; set; } = 60f;

	struct BatchKey : IEquatable<BatchKey>
	{
		public required Shader Shader { get; init; }
		public required BlendMode BlendMode { get; init; }
		public required RenderLayer Layer { get; init; }

		public bool Equals(BatchKey other)
		{
			if (Shader.Handle != other.Shader.Handle) return false;
			if (BlendMode != other.BlendMode) return false;
			if (Layer != other.Layer) return false;

			return true;
		}

		public override bool Equals(object? obj)
		{
			return obj is BatchKey && Equals((BatchKey)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Shader.Handle.GetHashCode(), BlendMode.GetHashCode(), Layer.GetHashCode());
		}
	}

	class Batch : IRenderable
	{
		public required Shader Shader { get; init; }
		public required BlendMode BlendMode { get; init; }
		public required RenderLayer Layer { get; init; }

		/// <summary>
		/// True if enough time has passed since we ran out of children. This will reset if any child is added before the delay is elapsed.
		/// </summary>
		public bool WantsToBeDestroyed => Time.Now >= _destroyTime;

		private float _destroyTime = float.PositiveInfinity;
		private readonly List<SpriteRenderer> _sprites = [];

		// TODO: Remake buffer if not enough room
		// TODO: Shrink buffer is too much room for too long.

		private uint _ssbo;

		public unsafe void Create()
		{
			Renderer.Register(this);

			var gl = Renderer.OpenGL;
			_ssbo = gl.CreateBuffer();
			gl.BindBuffer(BufferTargetARB.ShaderStorageBuffer, _ssbo);
			gl.BufferData(BufferTargetARB.ShaderStorageBuffer, (nuint)(SpriteInstance.StructSize * 5), null, BufferUsageARB.DynamicDraw);
		}

		public unsafe void OnRender()
		{
			var gl = Renderer.OpenGL;

			gl.BindVertexArray(_vao);

			gl.BindBuffer(BufferTargetARB.ShaderStorageBuffer, _ssbo);

			var instances = new SpriteInstance[_sprites.Count];
			for (int i = 0; i < instances.Length; i++)
			{
				instances[i] = _sprites[i].GetRenderData();
			}
			gl.BufferSubData(BufferTargetARB.ShaderStorageBuffer, 0, (nuint)(SpriteInstance.StructSize * instances.Length), instances.AsSpan());
			gl.BindBufferBase(BufferTargetARB.ShaderStorageBuffer, 0, _ssbo);

			gl.DrawElementsInstanced(PrimitiveType.Triangles, (uint)6, DrawElementsType.UnsignedInt, null, (uint)_sprites.Count);
		}

		public void Destroy()
		{
			Renderer.Unregister(this);
			Renderer.OpenGL.DeleteBuffer(_ssbo);
		}

		public bool Add(SpriteRenderer sprite)
		{
			if (_sprites.Contains(sprite)) return false;

			_sprites.Add(sprite);
			_destroyTime = float.PositiveInfinity;

			return true;
		}

		public void Remove(SpriteRenderer sprite)
		{
			_sprites.Remove(sprite);

			if (_sprites.Count == 0)
			{
				_destroyTime = Time.Now + BatchDestroyDelay;
			}
		}
	}

	private static uint _vao, _vbo, _ibo;
	private static readonly Dictionary<BatchKey, Batch> _batches = [];
	private static readonly Dictionary<SpriteRenderer, BatchKey> _spriteToBatchKey = [];
	private readonly List<BatchKey> _toRemove = [];

	public void OnCreate()
	{
		var gl = Renderer.OpenGL;

		_vao = gl.CreateVertexArray();
		gl.BindVertexArray(_vao);

		var vertices = new float[]
		{
			// Pos			// UV
			-0.5f, -0.5f,	0f, 0f,
			-0.5f,  0.5f,	0f, 1f,
			 0.5f,  0.5f,	1f, 1f,
			 0.5f, -0.5f,	1f, 0f
		};

		var indices = new uint[]
		{
			0, 1, 2,
			3, 0, 2
		};

		_vbo = gl.CreateBuffer();
		gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
		gl.BufferData(BufferTargetARB.ArrayBuffer, vertices, BufferUsageARB.StaticDraw);

		_ibo = gl.CreateBuffer();
		gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ibo);
		gl.BufferData(BufferTargetARB.ElementArrayBuffer, indices, BufferUsageARB.StaticDraw);

		const uint stride = sizeof(float) * 4;

		gl.EnableVertexAttribArray(0);
		gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);

		gl.EnableVertexAttribArray(1);
		gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, sizeof(float) * 2);
	}

	public void OnLateUpdate()
	{
		foreach (var keyValue in _batches)
		{
			var batch = keyValue.Value;

			if (batch.WantsToBeDestroyed)
			{
				_toRemove.Add(keyValue.Key);
			}
		}

		foreach (var key in _toRemove)
		{
			var batch = _batches[key];
			_batches.Remove(key);
			batch.Destroy();
		}

		_toRemove.Clear();
	}

	public void OnDestroy()
	{
		foreach (var batch in _batches.Values)
		{
			batch.Destroy();
		}

		_batches.Clear();
		_spriteToBatchKey.Clear();

		var gl = Renderer.OpenGL;
		gl.DeleteVertexArray(_vao);
		gl.DeleteBuffer(_vbo);
		gl.DeleteBuffer(_ibo);
	}

	internal static void Register(SpriteRenderer sprite)
	{
		var key = new BatchKey()
		{
			Shader = sprite.Shader,
			BlendMode = sprite.BlendMode,
			Layer = sprite.Layer
		};

		if (_batches.TryGetValue(key, out var batch))
		{
			if (!batch.Add(sprite))
			{
				return;
			}
		}
		else
		{
			batch = new Batch()
			{
				Shader = key.Shader,
				BlendMode = key.BlendMode,
				Layer = key.Layer
			};

			batch.Create();

			batch.Add(sprite);
			_batches.Add(key, batch);
		}

		_spriteToBatchKey.Add(sprite, key);
	}

	internal static void Unregister(SpriteRenderer sprite)
	{
		if (_spriteToBatchKey.TryGetValue(sprite, out var key))
		{
			_spriteToBatchKey.Remove(sprite);
			var batch = _batches[key];
			batch.Remove(sprite);
		}
	}

	public static void Resort(SpriteRenderer sprite)
	{
		Unregister(sprite);
		Register(sprite);
	}
}
