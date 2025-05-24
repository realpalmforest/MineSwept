using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MineSwept.Utility;

namespace MineSwept.Core;

public class GameMap
{
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    public Tile[,] Tiles { get; set; }

    private Vector2 offset;
    private int tileSize;

    private Vector2 numbersOffset = new Vector2(2, 3);
    private SpriteFont numbersFont;
    private Texture2D tilesTexture;

    private Color[] numbersColors = new Color[]
    {
        Color.Black,
        Color.Blue,
        Color.Green,
        Color.Red,
        Color.Purple,
        Color.Orange,
        Color.Cyan,
        Color.Yellow
    };

    public GameMap(int width, int height, Vector2 offset, int tileSize)
    {
        Tiles = new Tile[width, height];
        this.offset = offset;
        this.tileSize = tileSize;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Tiles[x, y] = new Tile();
            }
        }
    }

    public void LoadContent(ContentManager content)
    {
        tilesTexture = content.Load<Texture2D>("tiles");
        numbersFont = content.Load<SpriteFont>("UI/Fonts/numFont");
    }

    public void Update()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (!GetTileBounds(x, y).Contains(InputManager.MouseState.Position))
                    continue;

                if (!Tiles[x, y].Covered)
                {
                    if (InputManager.GetMouseButtonUp(MouseButton.Middle))
                    {
                        int adjacentFlagged = 0;
                        foreach ((int x, int y) tile in GetAdjacentTilePositions(x, y))
                        {
                            if (Tiles[tile.x, tile.y].Flagged)
                                adjacentFlagged++;
                        }

                        if(adjacentFlagged != Tiles[x, y].AdjacentMines)
                            continue;

                        foreach ((int x, int y) tile in GetAdjacentTilePositions(x, y))
                        {
                            RevealTile(tile.x, tile.y);
                        }
                    }

                    continue;
                }



                if (InputManager.GetMouseButtonUp(MouseButton.Left))
                {
                    RevealTile(x, y);
                    return;
                }
                else if (InputManager.GetMouseButtonUp(MouseButton.Right))
                {
                    Tiles[x, y].Flagged = !Tiles[x, y].Flagged;
                    return;
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                DrawTile(x, y, spriteBatch);
            }
        }
    }

    public void PlaceMines(int count)
    {
        Random random = new Random();
        int placedMines = 0;

        while (placedMines < count)
        {
            int x = random.Next(Width);
            int y = random.Next(Height);

            if (!Tiles[x, y].Mine)
            {
                Tiles[x, y].Mine = true;
                placedMines++;

                // Increment adjacent mine counts for surrounding tiles
                if (y > 0)
                    Tiles[x, y - 1].AdjacentMines++;
                if (y < Height - 1)
                    Tiles[x, y + 1].AdjacentMines++;
                if (x > 0)
                    Tiles[x - 1, y].AdjacentMines++;
                if (x < Width - 1)
                    Tiles[x + 1, y].AdjacentMines++;
                if (x > 0 && y > 0)
                    Tiles[x - 1, y - 1].AdjacentMines++;
                if (x < Width - 1 && y > 0)
                    Tiles[x + 1, y - 1].AdjacentMines++;
                if (x > 0 && y < Height - 1)
                    Tiles[x - 1, y + 1].AdjacentMines++;
                if (x < Width - 1 && y < Height - 1)
                    Tiles[x + 1, y + 1].AdjacentMines++;
            }
        }
    }

    private void DrawTile(int x, int y, SpriteBatch spriteBatch)
    {
        Tile tile = Tiles[x, y];
        Rectangle source;

        if (tile.Flagged)
            source = new Rectangle(32, 0, 32, 32);
        else if (tile.Covered)
            source = new Rectangle(0, 0, 32, 32);
        else source = new Rectangle(64, 0, 32, 32);

        spriteBatch.Draw(tilesTexture, GetTileBounds(x, y), source, Color.White);

        if (!tile.Covered && tile.AdjacentMines > 0)
        {
            spriteBatch.DrawString(numbersFont, tile.AdjacentMines.ToString(), numbersOffset + GetTileBounds(x, y).Center.ToVector2(), numbersColors[tile.AdjacentMines], 0f, numbersFont.MeasureString(tile.AdjacentMines.ToString()) / 2, 1f, SpriteEffects.None, 0f);
        }
    }

    public void RevealTile(int x, int y)
    {
        if (Tiles[x, y].Flagged || !Tiles[x, y].Covered)
            return;

        if (Tiles[x, y].Mine)
        {
            Debug.WriteLine("Game Over");
            return;
        }

        Tiles[x, y].Covered = false;
        Tiles[x, y].Flagged = false;

        if (Tiles[x, y].AdjacentMines == 0)
        {
            foreach ((int x, int y) tile in GetAdjacentTilePositions(x, y))
            {
                RevealTile(tile.x, tile.y);
            }
        }
    }

    private (int x, int y)[] GetAdjacentTilePositions(int x, int y)
    {
        List<(int x, int y)> points = new List<(int x, int y)>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int newX = x + i;
                int newY = y + j;

                if (newX == x && newY == y)
                    continue;

                if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
                    continue;

                points.Add((newX, newY));
            }
        }
        return points.ToArray();
    }

    public Rectangle GetTileBounds(int x, int y) => new Rectangle(offset.ToPoint() + new Point(x * tileSize, y * tileSize), new Point(tileSize));
}
