using System;
namespace LifeSim.LifeSimulation;

/// <summary>
/// Site is world cell which may contain useful resources and agent
/// </summary>
public class Site
{
	Agent agent;

	/// <summary>
	/// Site coordinate X
	/// </summary>
	public readonly int X;

	/// <summary>
	/// Site coordinate Y
	/// </summary>
	public readonly int Y;

	public int Energy;

	/// <summary>
	/// Agent that occupies this site
	/// </summary>
	public Agent Agent {
		get { return agent; }
		set
		{
			if (agent != value)
			{
				if (agent != null)
				{
					agent.X = -1;
					agent.Y = -1;
					agent = null;
				}
			}

			agent = value;

			if (agent != null)
			{
				agent.X = X;
				agent.Y = Y;
			}
		}
	}

    /// <summary>
    /// Initialize Site
    /// </summary>
    /// <param name="x">Site coordinate X</param>
    /// <param name="y">Site coordinate Y</param>
    public Site(int x, int y)
	{
		X = x;
		Y = y;
	}

	public void Update()
	{
		if (Agent != null)
		{
			if (Agent.Energy > 0)
			{
				// alive
				Agent.Update();
            }
			else
			{
				// dead
				Energy += World.Instance.Settings.Agent.EnergyOfCorpse;
				Agent = null;
			}
        }
	}
}
