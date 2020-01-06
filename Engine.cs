using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SandBox
{
    public class Engine : Game
    {
        GraphicsDeviceManager graphics;
        Automata Automata;

        public Engine(int width, int height, int scale)
        {
            graphics = new GraphicsDeviceManager(this);
            Automata = new CellEngines.SandBox(width, height, scale);
            graphics.PreferredBackBufferWidth = width * scale;
            graphics.PreferredBackBufferHeight = height * scale;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Render.Initialize(graphics.GraphicsDevice);
            base.Initialize();
            Automata.Initialize();
        }        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Automata.Update();
            Automata.SimulationStep();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Render.Begin();
            Automata.Draw();
            Render.End();
            base.Draw(gameTime);
        }        
    }
}