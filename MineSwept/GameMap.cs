using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MineSwept;

public class GameMap
{
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    public Tile[,] Tiles { get; set; }

    private Vector2 offset;
    private int tileSize;
    private Texture2D tilesTexture;

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
                    continue;


                if (InputManager.GetMouseButtonDown(MouseButton.Left))
                {
                    RevealTile(x, y);
                    return;
                }
                else if (InputManager.GetMouseButtonDown(MouseButton.Right))
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
    }

    public void RevealTile(int x, int y)
    {
        if(Tiles[x, y].Flagged || !Tiles[x, y].Covered)
            return;

        if (Tiles[x, y].Mine)
        {
            Debug.WriteLine("Game Over");
            return;
        }

        Tiles[x, y].Covered = false;
        Tiles[x, y].Flagged = false;
    }

    public void RevealTilesRecursively(int x, int y)
    {
        // Do funny recursion
    }

    public Rectangle GetTileBounds(int x, int y) => new Rectangle(offset.ToPoint() + new Point(x * tileSize, y * tileSize), new Point(tileSize));
}
