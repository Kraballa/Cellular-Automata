using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox.CellEngines
{
    public class SandBox: Automata
    {
        const int SAND = 1;
        const int SOLID = 2;
        const int WATER = 3;
        const int ICE = 4;
        const int SMOKE = 5;

        public SandBox(int width, int height, int scale) : base(width, height, scale)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            //define colours
            ColorMapping.Add(SAND, Color.Yellow);
            ColorMapping.Add(SOLID, Color.Gray);
            ColorMapping.Add(WATER, Color.Blue);
            ColorMapping.Add(ICE, Color.LightSkyBlue);
            ColorMapping.Add(SMOKE, Color.LightGray);
            //define callbacks
            ActionMapping.Add(SAND, (x, y) => {
                if (Read(x, y + 1) != NULL)
                {
                    if(Read(x,y+1)== WATER)
                    {
                        Write(x, y + 1, SAND);
                        Write(x, y, WATER);
                        return;
                    }
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
            });
            ActionMapping.Add(ICE, (x, y) => {
                if (Read(x, y + 1) != NULL)
                {
                    if (Read(x, y + 1) == WATER)
                    {
                        Write(x, y + 1, ICE);
                        Write(x, y, ICE);
                        return;
                    }
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
            });
            ActionMapping.Add(WATER, (x, y) =>
            {
                if (Read(x, y + 1) != NULL)
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
                    else
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
                }
                else
                {
                    //move downwards
                    Write(x, y + 1, Read(x, y));
                    Write(x, y, NULL);
                }
            });
            ActionMapping.Add(SMOKE, (x, y) =>
            {
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
            });

            FillBorder(SOLID);
            for (int i = Width / 4; i < Width * 3 / 4; i++)
            {
                Write(i, (int)(Math.Sin(i * 0.08) * 10 + Width / 2), SOLID);
            }
        }

        public override void Update()
        {
            base.Update();
            WriteRect(Random.Next(Width - 5) + 1, 1, 2, 3, WATER);
            WriteRect(Random.Next(Width - 5) + 1, 1, 1, 4, ICE);
        }
    }
}
