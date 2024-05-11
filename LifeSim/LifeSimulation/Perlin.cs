using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LifeSim.LifeSimulation;

// https://stackoverflow.com/questions/8659351/2d-perlin-noise

/// <summary>
/// Perlin 2d noise
/// </summary>
public static class Perlin
{
    private static Random random;
    private static int[] permutation;
    private static Vector2[] gradients;

    static Perlin()
    {
        Reseed();
        CalculateGradients(out gradients);
    }

    private static void CalculatePermutation(out int[] p, int seed)
    {
        p = Enumerable.Range(0, 256).ToArray();

        random = new Random(seed);

        /// shuffle the array
        for (var i = 0; i < p.Length; i++)
        {
            var source = random.Next(p.Length);

            var t = p[i];
            p[i] = p[source];
            p[source] = t;
        }
    }

    /// <summary>
    /// Generate a new permutation.
    /// </summary>
    public static void Reseed(int seed = 0)
    {
        if (seed == 0)
        {
            var r = new Random();
            seed = r.Next();
        }

        CalculatePermutation(out permutation, seed);
    }

    private static void CalculateGradients(out Vector2[] grad)
    {
        grad = new Vector2[256];
        for (var i = 0; i < grad.Length; i++)
        {
            Vector2 gradient;

            do
            {
                gradient = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
            }
            while (gradient.LengthSquared() >= 1);

            gradient.Normalize();
            grad[i] = gradient;
        }

    }

    private static float Drop(float t)
    {
        t = Math.Abs(t);
        return 1f - t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float Q(float u, float v)
    {
        return Drop(u) * Drop(v);
    }

    /// <summary>
    /// Generate noise value at X Y
    /// </summary>
    public static float Noise(float x, float y)
    {
        Vector2 cell = new Vector2((float)Math.Floor(x), (float)Math.Floor(y));
        float total = 0f;
        Vector2[] corners = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

        foreach (Vector2 n in corners)
        {
            Vector2 ij = cell + n;
            Vector2 uv = new Vector2(x - ij.X, y - ij.Y);

            int index = permutation[(int)ij.X % permutation.Length];
            index = permutation[(index + (int)ij.Y) % permutation.Length];

            Vector2 grad = gradients[index % gradients.Length];

            total += Q(uv.X, uv.Y) * Vector2.Dot(grad, uv);
        }

        return Math.Clamp(total, -1.0f, 1.0f);
    }
}