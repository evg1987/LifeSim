using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LifeSim
{
	public static class Extensions
	{
		public static Color Brighten(this Color color, int value)
		{
			return new Color(
				Math.Clamp(color.R + value, 0, 255),
				Math.Clamp(color.G + value, 0, 255),
				Math.Clamp(color.B + value, 0, 255),
				color.A
            );
		}

		public static List<T> Shuffle<T>(this List<T> list, Random random = null)
		{
			if (list.Count > 1)
			{
				if (random == null)
				{
					random = new Random();
				}

				var result = new List<T>(list.Count);
				var tempList = new List<T>(list);
                while (tempList.Count > 0)
				{
					int n = random.Next() % tempList.Count;
					result.Add(tempList[n]);
					tempList.RemoveAt(n);
				}

				return result;
			}
			else
			{
				// empty or one item
				return new List<T>(list);
			}
		}
    }
}
