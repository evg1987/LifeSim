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
    public double EnergyAtStart = 20.0;

    /// <summary>
    /// Maximum amount of energy (limit)
    /// </summary>
    public double EnergyMax = 200.0;

    /// <summary>
    /// How many energy points does agent give to cell on die
    /// </summary>
    public double EnergyOfCorpse = 10.0;

    /// <summary>
    /// How many energy points does it need for child spawn
    /// </summary>
    public double EnergyForChildSpawn = 40.0;

    /// <summary>
    /// How many energy points does agent drain from cell
    /// </summary>
    public double EnergyConsumptionFromCellSpeed = 2.0;

    /// <summary>
    /// How many energy points does it need for movement to another cell
    /// </summary>
    public double EnergyForMovement = 1.0;

    /// <summary>
    /// How many energy points require for stabile state (group of agents)
    /// </summary>
    public double EnergyForStabileState = 0.01f;

    /// <summary>
    /// How many energy points require for non-stabile state (outside of group of agents)
    /// </summary>
    public double EnergyForNonStabileState = 0.25f;

    /// <summary>
    /// Number of neighbours of same team for stabile state (group of agents)
    /// </summary>
    public int NeighboursForStability = 3;
}
