using System;
namespace LifeSim.LifeSimulation.Settings;

/// <summary>
/// Simulation settings for world excluding living species (agents)
/// </summary>
public sealed class World
{
	/// <summary>
	/// Minimum amount of cell energy at simulation start
	/// </summary>
	public double CellEnergyMin = 5.0;

    /// <summary>
    /// Maximum amount of cell energy at simulation start
    /// </summary>
    public double CellEnergyMax = 20.0;
}
