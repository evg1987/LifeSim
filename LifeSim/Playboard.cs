﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeSim.LifeSimulation;

namespace LifeSim;

/// <summary>
/// Playboard draws cells and allows user to move and scale
/// </summary>
public class Playboard
{
	/// <summary>
	/// Cell size in pixels
	/// </summary>
	const float CELL_SIZE = 16.0f;

	GraphicsDeviceManager graphics;
    readonly SpriteBatch spriteBatch;

	bool isDragging;
	Point draggingStartPos;
	int mouseWheelPrev;
	Matrix matrixBeforeDragging;
	bool spaceKeyPressedPrev;
	bool restartKeyPressedPrev;

	TeamVisualSettings[] teamVisualSettings;

	/// <summary>
	/// Number of cells (horizontal)
	/// </summary>
	public readonly int Width;

    /// <summary>
    /// Number of cells (vertical)
    /// </summary>
    public readonly int Height;

	/// <summary>
	/// Cells with payload (size of array is Width*Height)
	/// </summary>
    public Cell[,] Cells { get; private set; }

	/// <summary>
	/// Transform matrix
	/// </summary>
	public Matrix Matrix = Matrix.Identity;

	/// <summary>
	/// Simulation on pause
	/// </summary>
	public bool Pause { get; set; } = true;

	/// <summary>
	/// Cell under mouse cursor
	/// </summary>
	public Cell CellUnderCursor { get; private set; }

	/// <summary>
	/// Initialize playboard
	/// </summary>
	/// <param name="width">Number of cells (horizontal)</param>
	/// <param name="height">Number of cells (vertical)</param>
	/// <param name="spriteBatch">Sprite batch that is responsible for drawing sprites</param>
	/// <param name="graphics">Graphics device manager</param>
	/// <exception cref="ArgumentOutOfRangeException"/>
	/// <exception cref="ArgumentNullException"/>
	public Playboard(int width, int height, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Texture2D texture)
	{
		if (width < 1 || height < 1)
		{
			throw new ArgumentOutOfRangeException("Invalid width or height");
		}

		if (spriteBatch == null)
		{
			throw new ArgumentNullException(nameof(spriteBatch));
		}

		if (graphics == null)
		{
			throw new ArgumentNullException(nameof(graphics));
		}

		if (texture == null)
		{
			throw new ArgumentNullException(nameof(texture));
		}

		Width = width;
		Height = height;
		this.spriteBatch = spriteBatch;
		this.graphics = graphics;

		// initialize cells array
		Cells = new Cell[Width, Height];
		for (int w = 0; w < Width; w++)
		{
			for (int h = 0; h < Height; h++)
			{
				Cells[w, h] = new Cell(spriteBatch, texture, new Vector2(w * CELL_SIZE, h * CELL_SIZE));
			}
		}

		// team visual settings for displaying agents
		teamVisualSettings = new TeamVisualSettings[4];
		teamVisualSettings[0] = new TeamVisualSettings
		{
			Color1 = new Color(64, 0, 0),
			Color2 = new Color(255, 0, 0)
        };
        teamVisualSettings[1] = new TeamVisualSettings
        {
            Color1 = new Color(0, 64, 0),
            Color2 = new Color(0, 255, 0)
        };
        teamVisualSettings[2] = new TeamVisualSettings
        {
            Color1 = new Color(0, 64, 64),
            Color2 = new Color(0, 255, 255)
        };
        teamVisualSettings[3] = new TeamVisualSettings
        {
            Color1 = new Color(64, 64, 0),
            Color2 = new Color(255, 255, 0)
        };

		Restart();
    }

	/// <summary>
	/// Draw the playboard
	/// </summary>
	public void Draw()
	{
		spriteBatch.Begin(transformMatrix: Matrix);
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
				Cells[w, h].Draw();
            }
        }
		spriteBatch.End();
    }

	/// <summary>
	/// Restart simulation
	/// </summary>
	public void Restart()
	{
        new World(Width, Height, 0);
    }

    /// <summary>
    /// Update (every frame)
    /// </summary>
    /// <param name="delta">Delta seconds from prev update</param>
    public void Update(float delta)
	{
		UpdateTransform();
		UpdateCells();
		UpdateCellUnderCursor();
		UpdatePauseState();
		UpdateRestartState();

		if (!Pause)
		{
			World.Instance.Update();
		}
    }

	private void UpdateTransform()
	{
        MouseState state = Mouse.GetState();

        // dragging/not dragging
        if (state.RightButton == ButtonState.Pressed && !isDragging)
        {
            // drag begin
            isDragging = true;
            draggingStartPos = state.Position;
            matrixBeforeDragging = Matrix;
        }
        else if (state.RightButton != ButtonState.Pressed && isDragging)
        {
            // drag end
            isDragging = false;
        }

        // scaling
        if (state.ScrollWheelValue != mouseWheelPrev && !isDragging)
        {
            float scaleDelta = state.ScrollWheelValue < mouseWheelPrev ? 0.8f : 1.2f;

            Vector2 s = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Vector2 delta = s - s * scaleDelta;

            Vector2 cursorPos = new Vector2(
                (float)state.Position.X / (float)graphics.PreferredBackBufferWidth,
                (float)state.Position.Y / (float)graphics.PreferredBackBufferHeight
            );

            Matrix matrixS = Matrix.CreateScale(scaleDelta);
            Matrix matrixT = Matrix.CreateTranslation(delta.X * cursorPos.X, delta.Y * cursorPos.Y, 0.0f);

            Matrix *= matrixS * matrixT;
            mouseWheelPrev = state.ScrollWheelValue;
        }

        // dragging state
        if (isDragging)
        {
            Vector2 delta = (state.Position - draggingStartPos).ToVector2();
            Matrix = matrixBeforeDragging * Matrix.CreateTranslation(delta.X, delta.Y, 0.0f);
        }
    }

	private void UpdateCells()
	{
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                Update(World.Instance.Sites[w, h], Cells[w, h]);
            }
        }
    }

	private void Update(Site site, Cell cell)
	{
		if (site.Agent != null)
		{
			TeamVisualSettings settings = teamVisualSettings[site.Agent.Team];
			float f = Math.Clamp(site.Agent.Energy / 200.0f, 0.0f, 1.0f);
			cell.Color = Color.Lerp(settings.Color1, settings.Color2, f);
		}
		else
		{
			Color color1 = new Color(32, 32, 32);
            Color color2 = new Color(128, 128, 128);

			float r = Math.Clamp(site.Energy / 30.0f, 0.0f, 1.0f);
			cell.Color = Color.Lerp(color1, color2, r);
        }
    }

	private void UpdateCellUnderCursor()
	{
		MouseState state = Mouse.GetState();

		Vector3 cursorPos = new Vector3(state.Position.X, state.Position.Y, 0.0f);
		cursorPos = Vector3.Transform(cursorPos, Matrix.Invert(Matrix));

		// cell coordinates under cursor
		int cx = (int)(cursorPos.X / CELL_SIZE);
		int cy = (int)(cursorPos.Y / CELL_SIZE);

		// reset prev cell
		if (CellUnderCursor != null)
		{
			CellUnderCursor.IsHighlighed = false;
		}

		if (cx >= 0 && cx < Width && cy >= 0 && cy < Height)
		{
			CellUnderCursor = Cells[cx, cy];
			CellUnderCursor.IsHighlighed = true;
		}
		else
		{
			CellUnderCursor = null;
		}
    }

	private void UpdatePauseState()
	{
		KeyboardState state = Keyboard.GetState();
		if (state.IsKeyDown(Keys.Space) && !spaceKeyPressedPrev)
		{
			Pause = !Pause;
		}

		spaceKeyPressedPrev = state.IsKeyDown(Keys.Space);
    }

	private void UpdateRestartState()
	{
        KeyboardState state = Keyboard.GetState();
		if (state.IsKeyDown(Keys.R) && !restartKeyPressedPrev)
		{
			Restart();
		}

		restartKeyPressedPrev = state.IsKeyDown(Keys.R);
    }
}