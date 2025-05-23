using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace MineSwept;

public static class DevTools
{
    public static GameMap CurrentMap;

    private static bool showMines = false;

    [Conditional("DEBUG")]
    public static void UpdateInputs()
    {
        if (InputManager.GetKeyDown(Keys.M))
            showMines = !showMines;
    }

    [Conditional("DEBUG")]
    public static void DrawDebug(SpriteBatch spriteBatch)
    {
        if (showMines)
        {
            for (int x = 0; x < CurrentMap.Width; x++)
            {
                for (int y = 0; y < CurrentMap.Height; y++)
                {
                    if (CurrentMap.Tiles[x, y].Mine)
                    {
                        DrawDebugRect(spriteBatch, CurrentMap.GetTileBounds(x, y), new Color(Color.Red, 0.3f));
                    }
                }
            }
        }
    }

    public static void DrawDebugRect(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        spriteBatch.Draw(Game1.Instance.WhiteTexture, rectangle, new Color(color, 0.5f));
    }
}