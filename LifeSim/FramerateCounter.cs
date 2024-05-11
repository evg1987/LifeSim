using System;
namespace LifeSim;

public class FramerateCounter
{
	public static FramerateCounter Instance { get; private set; }

	/// <summary>
	/// Measured frame rate
	/// </summary>
	public int Framerate { get; private set; }

	int frameCounter;
	float elapsedTime;

	public static void Initialize()
	{
		Instance = new FramerateCounter();
	}

	public void Update(float delta)
	{
		elapsedTime += delta;
		if (elapsedTime >= 1.0f)
		{
			elapsedTime = 0.0f;
			Framerate = frameCounter;
			frameCounter = 0;
		}
	}
}
