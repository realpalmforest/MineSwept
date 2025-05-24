using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineSwept.Core;
using MineSwept.Utility;

namespace MineSwept
{
    public class Game1 : Game
    {
        public static Game1 Instance { get; private set; }

        public Random Random { get; } = new Random();
        public int WindowWidth
        {
            get => graphics.PreferredBackBufferWidth;
            set
            {
                graphics.PreferredBackBufferWidth = value;
                graphics.ApplyChanges();
            }
        }
        public int WindowHeight
        {
            get => graphics.PreferredBackBufferHeight;
            set
            {
                graphics.PreferredBackBufferHeight = value;
                graphics.ApplyChanges();
            }
        }

        public Texture2D WhiteTexture { get; private set; }

        private GameMap map;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Instance = this;
        }

        protected override void Initialize()
        {
            map = new GameMap(12, 12, new Vector2(40, 40), 35);
            map.PlaceMines(15);

            DevTools.CurrentMap = map;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            WhiteTexture.SetData([ Color.White ]);

            map.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update();
            DevTools.UpdateInputs();

            map.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            map.Draw(spriteBatch);

            DevTools.DrawDebug(spriteBatch);

            spriteBatch.End();
        }
    }
}
