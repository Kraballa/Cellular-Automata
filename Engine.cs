using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SandBox
{
    public class Engine : Game
    {
        GraphicsDeviceManager graphics;
        public Automata Automata;
        public Rectangle Screen;
        UI UI;

        public static Engine Instance;
        public static int Width => Instance.Screen.Width;
        public static int Height => Instance.Screen.Height;

        private bool Paused = false;

        private float fade = 0.0f;
        public Engine(int width, int height, int scale)
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Automata = new CellEngines.GameOfLife(width, height, scale);
            graphics.PreferredBackBufferWidth = width * scale;
            graphics.PreferredBackBufferHeight = height * scale;
            Screen = new Rectangle(0,0,width * scale, height * scale);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Render.Initialize(graphics.GraphicsDevice);
            MouseInput.Initialize();
            KeyboardInput.Initialize();
            base.Initialize();
            Automata.Initialize();
            UI = new UI(Automata.ColorMapping);
            Console.WriteLine("endinitialize");
        }        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            MouseInput.Update();
            KeyboardInput.Update();
            UI.Update();
            Automata.Update();
            if(!Paused)
                Automata.SimulationStep();

            if (KeyboardInput.CheckPressed(Keys.Space))
                Paused = !Paused;
        }

        protected override void Draw(GameTime gameTime)
        {
            
            if(fade == 0)
            {
                GraphicsDevice.Clear(Color.Black);
            }

            Render.Begin();

            if (fade != 0)
            {
                Color fadedBlack = new Color(Color.Black, fade);
                Render.Rect(Screen, fadedBlack);
            }

            Automata.Draw();
            UI.Draw();
            Render.End();
            base.Draw(gameTime);
        }        
    }
}