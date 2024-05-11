using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LifeSim.LifeSimulation;

/// <summary>
/// 
/// </summary>
public class Agent
{
	/// <summary>
	/// Allowed movement directions relative to current site coordinates
	/// </summary>
	readonly Point[] movementMatrix;

	/// <summary>
	/// Team defines relationship between other agents
	/// </summary>
	public int Team = 0;

	/// <summary>
	/// Generation increases every time when parent spawns child
	/// </summary>
	public int Generation = 0;

	/// <summary>
	/// World cite coordinate X
	/// </summary>
	public int X = -1;

	/// <summary>
	/// World cite coordinate Y
	/// </summary>
	public int Y = -1;

	/// <summary>
	/// Current amount of energy.
	/// Energy is used for useful actions and is decreased every simulation step.
	/// If energy is 0 - agent dies
	/// </summary>
	public int Energy = 100;

	/// <summary>
	/// How many energy remains when agent dies
	/// </summary>
	public int CorpseEnergy = 50;

	/// <summary>
	/// How many energy required for spawn child agent
	/// </summary>
	public int SpawnEnergy = 200;

	/// <summary>
	/// Site occupied by agent
	/// </summary>
	public Site Site
	{
		get
		{
			return World.Instance.IsValidCoords(X, Y)
				? World.Instance.Cells[X, Y]
				: null;
		}
	}

	public Agent()
	{
		movementMatrix = new Point[8];
		movementMatrix[0] = new Point(0, -1);  // up
		movementMatrix[1] = new Point(1, -1);  // up right
		movementMatrix[2] = new Point(1, 0);   // right
		movementMatrix[3] = new Point(1, 1);   // down right
		movementMatrix[4] = new Point(0, 1);   // down
		movementMatrix[5] = new Point(-1, 1);  // down left
		movementMatrix[6] = new Point(-1, 0);  // left
		movementMatrix[7] = new Point(-1, -1); // up left
    }

	public void Update()
	{

		if (Energy >= SpawnEnergy)
		{
			// spawn child
			int childEnergy = Energy / 2;
			Agent child = SpawnChild();
			if (child != null)
			{
				// the child has been spawned
				child.Energy = childEnergy + child.Site.Energy;
				Energy -= childEnergy;
				child.Site.Energy = 0;
			}
		}
		else
		{
			if (GetNumNeighbours() < 3)
			{
				Energy--;
				MoveToAnotherSite();
            }
        }
    }

	private List<Site> GetPossibleDestinations()
	{
		var result = new List<Site>();
		foreach (Point dir in movementMatrix)
		{
			// where agent wants to move
			Point c = new Point(X + dir.X, Y + dir.Y);
			if (CanMoveIntoSite(c.X, c.Y))
			{
				result.Add(World.Instance.Cells[c.X, c.Y]);
			}
		}
		return result;
	}

	private List<Site> GetUnoccupiedSites()
	{
        var result = new List<Site>();
        foreach (Point dir in movementMatrix)
        {
            Point c = new Point(X + dir.X, Y + dir.Y);
            if (World.Instance.IsValidCoords(c.X, c.Y))
            {
				Site site = World.Instance.Cells[c.X, c.Y];
				if (site.Agent == null)
				{
					result.Add(site);
				}
            }
        }
        return result;
    }

	private bool CanMoveIntoSite(int x, int y)
	{
		return World.Instance.IsValidCoords(x, y) && CanMoveIntoSite(World.Instance.Cells[x, y]);
	}

	private bool CanMoveIntoSite(Site site)
	{
		// check is site occupied by another agent
		if (site.Agent != null)
		{
			return false;
		}

		return true;
	}

	private Site SelectDestinationSite(List<Site> sites)
	{
		return sites
			.Shuffle(World.Instance.Random)
			.OrderByDescending(s => s.Energy)
			.FirstOrDefault();
	}

	private bool MoveToAnotherSite()
	{
        List<Site> possibleDestinations = GetPossibleDestinations();
		if (possibleDestinations.Count > 0)
		{
			Site destination = SelectDestinationSite(possibleDestinations);
			if (destination != null)
			{
				// success
				Site.Agent = null;
				destination.Agent = this;

				// consume site energy
				Energy += Math.Min(2, Site.Energy);
				Site.Energy = Math.Max(0, Site.Energy - 2);

				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
    }

	private Agent SpawnChild()
	{
		List<Site> unoccupiedSites = GetUnoccupiedSites();
		if (unoccupiedSites.Count > 0)
		{
			Site site = SelectDestinationSite(unoccupiedSites);
			if (site != null)
			{
				Agent child = new Agent();
				child.Team = Team;
				child.Generation = Generation + 1;
				site.Agent = child;

				return child;
			}
			else
			{
				return null;
			}
		}
		else
		{
			return null;
		}
	}

	private int GetNumNeighbours()
	{
		int result = 0;
        foreach (Point dir in movementMatrix)
        {
            Point c = new Point(X + dir.X, Y + dir.Y);
            if (World.Instance.IsValidCoords(c.X, c.Y))
            {
                Site site = World.Instance.Cells[c.X, c.Y];
                if (site.Agent != null && site.Agent.Team == Team)
                {
					result++;
                }
            }
        }

		return result;
    }
}
