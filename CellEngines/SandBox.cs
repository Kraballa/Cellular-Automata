using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox.CellEngines
{
    public class SandBox: Automata
    {
        const int NULL = 0;
        const int SAND = 1;
        const int SOLID = 2;
        const int WATER = 3;
        const int SNOW = 4;
        const int SMOKE = 5;

        public SandBox(int width, int height, int scale) : base(width, height, scale)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            ColorMapping.Add(SAND, Color.Yellow);
            ColorMapping.Add(SOLID, Color.Gray);
            ColorMapping.Add(WATER, Color.Blue);
            ColorMapping.Add(SNOW, Color.White);
            ColorMapping.Add(SMOKE, Color.LightGray);

            FillBorder();
            for (int i = Width / 4; i < Width * 3 / 4; i++)
            {
                Write(i, (int)(Math.Sin(i * 0.08) * 10 + Width / 2), SOLID);
            }
        }

        public override void Update()
        {
            base.Update();
            Write(Random.Next(Width - 2) + 1, 1, SAND);
        }

        public override void SimulationStep()
        {
            base.Update();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    switch (Read(x, y))
                    {
                        case SAND: //sand
                        case SNOW: //snow
                            if (Read(x, y + 1) != NULL)
                            {
                                if (Random.Chance(0.5f))
                                {
                                    if (x + 1 < Width && Read(x + 1, y + 1) == NULL)
                                    {
                                        //prevent clipping through diagonals
                                        if (Read(x + 1, y) == NULL)
                                        {
                                            //move right down
                                            Write(x + 1, y + 1, Read(x, y));
                                            Write(x, y, NULL);
                                        }
                                    }
                                }
                                else
                                {
                                    if (x > 0 && Read(x - 1, y + 1) == NULL)
                                    {
                                        //prevent clipping through diagonals
                                        if (Read(x - 1, y) == NULL)
                                        {
                                            //move left down
                                            Write(x - 1, y + 1, Read(x, y));
                                            Write(x, y, NULL);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //move downwards
                                Write(x, y + 1, Read(x, y));
                                Write(x, y, NULL);
                            }
                            break;
                        case WATER: //water
                            if (Read(x, y + 1) != NULL)
                            {
                                if (Random.Chance(0.5f))
                                {
                                    if (x + 1 != Width && Read(x + 1, y) == NULL)
                                    {
                                        //move right down
                                        Write(x + 1, y, Read(x, y));
                                        Write(x, y, NULL);
                                    }
                                }
                                else
                                {
                                    if (x != 0 && Read(x - 1, y) == NULL)
                                    {
                                        //move left down
                                        Write(x - 1, y, Read(x, y));
                                        Write(x, y, NULL);
                                    }
                                }
                            }
                            else
                            {
                                //move downwards
                                Write(x, y + 1, Read(x, y));
                                Write(x, y, NULL);
                            }
                            break;
                        case SMOKE:
                            if (y != 0 && Read(x, y - 1) != NULL)
                            {
                                if (Random.Chance(0.5f))
                                {
                                    if (x + 1 != Width && Read(x + 1, y) == NULL)
                                    {
                                        //move right
                                        Write(x + 1, y, Read(x, y));
                                        Write(x, y, NULL);
                                    }
                                }
                                else
                                {
                                    if (x != 0 && Read(x - 1, y) == NULL)
                                    {
                                        //move left
                                        Write(x - 1, y, Read(x, y));
                                        Write(x, y, NULL);
                                    }
                                }
                            }
                            else
                            {
                                //move up
                                Write(x, y - 1, Read(x, y));
                                Write(x, y, NULL);
                            }
                            break;
                    }
                }
            }
        }

        private void FillBorder()
        {
            for (int x = 0; x < Width; x++)
            {
                Write(x, Height - 1,SOLID);
                Write(x, 0, SOLID);
            }

            for (int y = 0; y < Height; y++)
            {
                Write(0, y, SOLID);
                Write(Width - 1, y, SOLID);
            }
        }
    }
}
