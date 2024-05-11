using System;
namespace LifeSim.LifeSimulation.Settings;

/// <summary>
/// Settings for each simulation element
/// </summary>
public sealed class Simulation
{
	public World World;

	public Agent Agent;

	public Simulation()
	{
		World = new World();
		Agent = new Agent();
	}
}
