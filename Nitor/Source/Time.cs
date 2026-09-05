using System;
using System.Collections.Generic;
using System.Text;

namespace Nitor;

public static class Time
{
	public static int StepCount { get; private set; }

	public static float Now { get; private set; }
	public static float Delta { get; private set; }

	public static float NowUnscaled => Now;

	internal static void Step(float dt)
	{
		Delta = dt;
		Now += dt;
		StepCount++;
	}
}
