using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeSim;

/// <summary>
/// Playboard cell
/// </summary>
public class Cell
{
	readonly SpriteBatch spriteBatch;
	readonly Vector2 location;
	readonly Texture2D texture;

	Color color = Color.White;
	Color colorHighlight = Color.White;

	/// <summary>
	/// Cell color
	/// </summary>
	public Color Color
	{
		get { return color; }
		set {
			color = value;
			colorHighlight = color.Brighten(64);
		}
	}

	/// <summary>
	/// Cell has highlight indication
	/// </summary>
	public bool IsHighlighed;

    public Cell(SpriteBatch spriteBatch, Texture2D texture, Vector2 location)
	{
		this.spriteBatch = spriteBatch;
		this.texture = texture;
		this.location = location;
	}

	public void Draw()
	{
		spriteBatch.Draw(texture, location, IsHighlighed ? colorHighlight : color);
	}
}
