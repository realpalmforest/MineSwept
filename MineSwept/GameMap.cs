using System;
using System.Collections.Generic;
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

    public GameMap(int width, int height, Vector2 offset, int tileSize)
    {
        Tiles = new Tile[width, height];
        this.offset = offset;
        this.tileSize = tileSize;
    }

    public void LoadContent(ContentManager content)
    {

    }

    public void Update()
    {

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

    public void DrawTile(int x, int y, SpriteBatch spriteBatch)
    {
        Tile tile = Tiles[x, y];
    }
}
