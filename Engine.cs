using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SandBox
{
    public class Engine : Game
    {
        GraphicsDeviceManager graphics;

        int[,] screen;
        int[,] buffer;
        int scale;
        int width;
        int height;
        int count = 0;

        Random rand;

        const int NULL = 0;
        const int SAND = 1;
        const int SOLID = 2;
        const int WATER = 3;
        const int SNOW = 4;
        const int SMOKE = 5;

        public Engine(int width, int height, int scale)
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = width * scale;
            graphics.PreferredBackBufferHeight = height * scale;

            screen = new int[width,height];
            buffer = new int[width, height];
            this.scale = scale;
            this.width = width;
            this.height = height;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            rand = new Random();
            Render.Initialize(graphics.GraphicsDevice);
            FillBorder();
            for(int i = width/4; i < width*3/4; i++)
            {
                screen[i, (int)(Math.Sin(i * 0.08) * 10 + width/2)] = SOLID;
            }
            base.Initialize();
        }

        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            count++;
            if(count < 0) { }
            else if(count < 180)
            {
                screen[rand.Next(width-2)+1, 1] = SAND;
                screen[rand.Next(width - 2) + 1, 1] = SAND;
                screen[rand.Next(width - 2) + 1, 1] = SAND;
                screen[rand.Next(width - 2) + 1, 1] = SAND;

            }
            else if(count < 480)
            {
                screen[rand.Next(width/3)+width/3, 1] = WATER;
                screen[rand.Next(width / 5) + width*2 / 5, 1] = WATER;
            }
            else if (count < 600 || true)
            {
                screen[rand.Next(width - 2) + 1, height-10] = SMOKE;
            }

            UpdateScreen();            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Render.Begin();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (screen[x,y] == SAND)
                    {
                        Render.Rect(x * scale, y * scale, scale, scale, Color.Yellow);
                    }
                    else if(screen[x,y] == SOLID)
                    {
                        Render.Rect(x * scale, y * scale, scale, scale, Color.Gray);
                    }
                    else if (screen[x, y] == WATER)
                    {
                        Render.Rect(x * scale, y * scale, scale, scale, Color.Blue);
                    }
                    else if (screen[x, y] == SNOW)
                    {
                        Render.Rect(x * scale, y * scale, scale, scale, Color.White);
                    }
                    else if (screen[x, y] == SMOKE)
                    {
                        Render.Rect(x * scale, y * scale, scale, scale, Color.LightGray);
                    }
                }
            }

            Render.End();
            base.Draw(gameTime);
        }


        private void UpdateScreen()
        {
            for (int x = 0; x < screen.GetLength(0); x++)
            {
                for (int y = height - 2; y >= 0; y--)
                {
                    switch (screen[x, y])
                    {
                        case SAND: //sand
                        case SNOW: //snow
                            if (screen[x, y + 1] != NULL)
                            {
                                if (rand.Chance(0.5f))
                                {
                                    if (x + 1 != width && screen[x + 1, y + 1] == NULL)
                                    {
                                        //prevent clipping through diagonals
                                        if (screen[x+1,y] == NULL)
                                        {
                                            //move right down
                                            screen[x + 1, y + 1] = screen[x, y];
                                            screen[x, y] = NULL;
                                        }
                                    }
                                }
                                else
                                {
                                    if (x != 0 && screen[x - 1, y + 1] == NULL)
                                    {
                                        //prevent clipping through diagonals
                                        if (screen[x - 1, y] == NULL)
                                        {
                                            //move left down
                                            screen[x - 1, y + 1] = screen[x, y];
                                            screen[x, y] = NULL;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //move downwards
                                screen[x, y + 1] = screen[x, y];
                                screen[x, y] = NULL;
                            }
                            break;
                        case WATER: //water
                            if (screen[x, y + 1] != NULL)
                            {
                                if (rand.Chance(0.5f))
                                {
                                    if (x + 1 != width && screen[x + 1, y] == NULL)
                                    {
                                        //move right down
                                        screen[x + 1, y] = screen[x, y];
                                        screen[x, y] = NULL;
                                    }
                                }
                                else
                                {
                                    if (x != 0 && screen[x - 1, y] == NULL)
                                    {
                                        //move left down
                                        screen[x - 1, y] = screen[x, y];
                                        screen[x, y] = NULL;
                                    }
                                }
                            }
                            else
                            {
                                //move downwards
                                screen[x, y + 1] = screen[x, y];
                                screen[x, y] = NULL;
                            }
                            break;
                        case SMOKE:
                            if (screen[x, y - 1] != NULL)
                            {
                                if (rand.Chance(0.5f))
                                {
                                    if (x + 1 != width && screen[x + 1, y] == NULL)
                                    {
                                        //move right down
                                        screen[x + 1, y] = screen[x, y];
                                        screen[x, y] = NULL;
                                    }
                                }
                                else
                                {
                                    if (x != 0 && screen[x - 1, y] == NULL)
                                    {
                                        //move left down
                                        screen[x - 1, y] = screen[x, y];
                                        screen[x, y] = NULL;
                                    }
                                }
                            }
                            else
                            {
                                //move downwards
                                screen[x, y - 1] = screen[x, y];
                                screen[x, y] = NULL;
                            }
                            break;
                    }
                }
            }
        }

        private void FillBorder()
        {
            for(int x = 0; x < width; x++)
            {
                screen[x, height - 1] = SOLID;
                screen[x, 0] = SOLID;
            }

            for(int y = 0; y < height; y++)
            {
                screen[0, y] = SOLID;
                screen[width - 1, y] = SOLID;
            }
        }
    }
}