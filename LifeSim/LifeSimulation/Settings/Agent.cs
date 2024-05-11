using System;
namespace LifeSim.LifeSimulation.Settings;

/// <summary>
/// Simulation settings for agents (living species)
/// </summary>
public sealed class Agent
{
    /// <summary>
    /// How many energy points does agent have on spawn 
    /// </summary>
    public int EnergyAtStart = 20;

    /// <summary>
    /// Maximum amount of energy (limit)
    /// </summary>
    public int EnergyMax = 200;

    /// <summary>
    /// How many energy points does agent give to cell on die
    /// </summary>
    public int EnergyOfCorpse = 10;

    /// <summary>
    /// How many energy points does it need for child spawn
    /// </summary>
    public int EnergyForChildSpawn = 40;

    /// <summary>
    /// How many energy points does agent drain from cell
    /// </summary>
    public int EnergyConsumptionFromCellSpeed = 2;

    /// <summary>
    /// How many energy points does it need for movement to another cell
    /// </summary>
    public int EnergyForMovement = 1;

    /// <summary>
    /// Number of neighbours of same team for stabile state
    /// </summary>
    public int NeighboursForStability = 3;
}
