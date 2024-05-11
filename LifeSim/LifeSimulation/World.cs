using System;
using Microsoft.Xna.Framework;

namespace LifeSim.LifeSimulation;

/// <summary>
/// The World contains cites (world cells) which contains agents (living species)
/// and responsible for updating the first and the second ones
/// </summary>
public class World
{
	public static World Instance { get; private set; }

	/// <summary>
	/// Number of cells (horizontal)
	/// </summary>
	public readonly int Width;

	/// <summary>
	/// Number of cells (vertical)
	/// </summary>
	public readonly int Height;

	/// <summary>
	/// Random generator seed
	/// </summary>
	public readonly int Seed;

	/// <summary>
	/// Shared random generator
	/// </summary>
	public readonly Random Random;

	/// <summary>
	/// World cells where agents are living
	/// </summary>
    public Site[,] Sites { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="width">Number of cells (horizontal)</param>
    /// <param name="height">Number of cells (vertical)</param>
	/// <param name="seed">Seed for all random generators</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public World(int width, int height, int seed)
	{
		if (width < 1 || height < 1)
		{
			throw new ArgumentOutOfRangeException("Bad world size");
		}

		Instance = this;

		Width = width;
		Height = height;
		Random = new Random(seed);
		Seed = seed;

		// init cites
		Sites = new Site[Width, Height];
		for (int w = 0; w < Width; w++)
		{
			for (int h = 0; h < Height; h++)
			{
				Sites[w, h] = new Site(w, h);
			}
		}

		InitCites();
		InitAgents();
	}

	/// <summary>
	/// Perform next simulation step
	/// </summary>
	public void Update()
	{
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
				Sites[w, h].Update();
            }
        }
    }

	/// <summary>
	/// Check if cite coordinates within world bounds
	/// </summary>
	public bool IsValidCiteCoords(int x, int y)
	{
		return x >= 0 && x < Width && y >= 0 && y < Height;
	}

    /// <summary>
    /// Check if cite coordinates within world bounds
    /// </summary>
    public bool IsValidCiteCoords(Point citeCoords)
	{
		return IsValidCiteCoords(citeCoords.X, citeCoords.Y);
	}

	private Site GetRandomSite()
	{
		Site site = Sites[Random.Next() % Width, Random.Next() % Height];
		return site;
	}

    private void InitCites()
	{
        const float NOISE_STEP = 0.1f;
		const float ENERGY_MIN = 20.0f;
		const float ENERGY_MAX = 50.0f;

        Perlin.Reseed(Seed);

		// Noise at range of [-1, +1],
		int n = 0;
		float[] noise = new float[Width * Height];
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++, n++)
            {
				noise[n] = Perlin.Noise(w * NOISE_STEP, h * NOISE_STEP);
				noise[n] *= 8.0f;
            }
        }

		n = 0;
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++, n++)
            {
                Sites[w, h].Energy = Math.Clamp((int)(noise[n] * ENERGY_MAX), (int)ENERGY_MIN, (int)ENERGY_MAX);
            }
        }
    }

	private void InitAgents()
	{
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Site site = GetRandomSite();
				if (site != null && site.Agent == null)
				{
					site.Agent = new Agent()
					{
						Team = j
					};
				}
			}
		}
	}
}
