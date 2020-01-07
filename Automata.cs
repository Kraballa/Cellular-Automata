using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox
{
    public class Automata
    {
        public Dictionary<int, Color> ColorMapping;
        public Dictionary<int, Action<int,int>> ActionMapping;
        public int LeftPlace = NULL;
        public int RightPlace = NULL;

        protected Random Random;
        protected int Width;
        protected int Height;
        protected int Scale;
        protected int Refreshrate = 0;

        private int[,] screen;
        private int[,] buffer;
        private int count = 0;

        protected const int NULL = 0;
        
        public Automata(int width, int height, int scale)
        {
            screen = new int[width, height];
            buffer = new int[width, height];
            Random = new Random();
            ColorMapping = new Dictionary<int, Color>();
            ActionMapping = new Dictionary<int, Action<int, int>>();
            Width = width;
            Height = height;
            Scale = scale;
        }

        public virtual void Initialize()
        {
            
        }

        /// <summary>
        /// Used for adding new Cells into the Automata
        /// </summary>
        public virtual void Update()
        {
            Point mousePos = new Point(MouseInput.X / Scale, MouseInput.Y / Scale);
            Engine.Instance.Window.Title = "[" + mousePos.X + " : " + mousePos.Y + "]";
            if (Engine.Instance.Screen.Contains(mousePos))
            {
                if(MouseInput.LeftClick())
                {
                    Write(mousePos.X, mousePos.Y, LeftPlace);
                }else if (MouseInput.RightClick())
                {
                    Write(mousePos.X, mousePos.Y, NULL);
                }
            }
        }

        /// <summary>
        /// Used for simulating every cell in the Automata
        /// </summary>
        public void SimulationStep()
        {
            if (count > Refreshrate)
            {
                count = 0;
            }
            else
            {
                count++;
                return;
            }


            for (int x = 0; x < Width; x++)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    int val = Read(x, y);
                    if (ActionMapping.ContainsKey(val))
                    {
                        ActionMapping[val](x,y);   
                    }
                }
            }
        }

        public void Draw()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    screen[x, y] = buffer[x, y];
                }
            }


            for (int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    int val = Read(x, y);
                    if (ColorMapping.ContainsKey(val))
                    {
                        Color color = ColorMapping[val];
                        Render.Rect(x*Scale, y*Scale, Scale, Scale, color);
                    }
                }
            }
        }

        protected int Read(int x, int y)
        {
            if(x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return screen[x, y];
            }
            else
            {
                return NULL;
            }            
        }

        protected void Write(int x, int y, int value)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                buffer[x, y] = value;
        }

        protected void WriteRect(int x, int y, int xscale, int yscale, int value)
        {
            for(int xx = x; xx < x + xscale; xx++)
            {
                for(int yy = y; yy < y + yscale; yy++)
                {
                    Write(xx, yy, value);
                }
            }
        }

        protected void FillBorder(int value)
        {
            for (int x = 0; x < Width; x++)
            {
                Write(x, Height - 1, value);
                Write(x, 0, value);
            }

            for (int y = 0; y < Height; y++)
            {
                Write(0, y, value);
                Write(Width - 1, y, value);
            }
        }

        protected int GetNumNeightbors(int x, int y)
        {
            int numNeighbors = -1;

            for(int xx = x-1; xx < x+1; xx++)
            {
                for(int yy = y-1; yy < y+1; yy++)
                {
                    if (Read(xx,yy) != NULL)
                    {
                        numNeighbors++;
                    }
                }
            }

            return numNeighbors;
        }
    }
}
