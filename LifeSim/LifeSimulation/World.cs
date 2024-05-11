using System;
using Microsoft.Xna.Framework;

namespace LifeSim.LifeSimulation;

/// <summary>
/// The World contains cites (world cells) which contains agents (living species)
/// and responsible for updating the first and the second ones
/// </summary>
public class World : GridCellsLayout<Site>
{
	public static World Instance { get; private set; }

	/// <summary>
	/// Random generator seed
	/// </summary>
	public readonly int Seed;

	/// <summary>
	/// Shared random generator
	/// </summary>
	public readonly Random Random;

    /// <summary>
    /// Construct world and start simulation
    /// </summary>
    /// <param name="width">Number of cells (horizontal)</param>
    /// <param name="height">Number of cells (vertical)</param>
	/// <param name="seed">Seed for all random generators</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public World(int width, int height, int seed)
		: base(width, height)
	{
		if (width < 1 || height < 1)
		{
			throw new ArgumentOutOfRangeException("Bad world size");
		}

		Instance = this;

		Random = new Random(seed);
		Seed = seed;

		InitializeCells();
		InitCites();
		InitAgents();
	}

	/// <summary>
	/// Perform next simulation step
	/// </summary>
	public void Update()
	{
		foreach (Site site in EnumerateCells())
		{
			site.Update();
		}
    }

	private Site GetRandomSite()
	{
		Site site = Cells[Random.Next() % Width, Random.Next() % Height];
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
                Cells[w, h].Energy = Math.Clamp((int)(noise[n] * ENERGY_MAX), (int)ENERGY_MIN, (int)ENERGY_MAX);
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

    protected override Site ConstructCell(int x, int y)
    {
		return new Site(x, y);
    }
}
