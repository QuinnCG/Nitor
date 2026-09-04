using System;
using System.Collections.Generic;
using System.Text;

namespace Nitor;

public static class Time
{
	public static float Now { get; private set; }

	internal static void Step(float dt)
	{
		Now += dt;
	}
}
