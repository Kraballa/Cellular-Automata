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
        public int Place = NULL;

        protected Random Random;
        protected int Width;
        protected int Height;
        protected int Scale;

        private int[,] screen;
        private int[,] buffer;

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

        }

        /// <summary>
        /// Used for simulating every cell in the Automata
        /// </summary>
        public void SimulationStep()
        {
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

            for (int x = 0; x < Width; x++)
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    screen[x, y] = buffer[x, y];
                }
            }
        }

        public void Draw()
        {
            
            for(int x = 0; x < Width; x++)
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
            return screen[x, y];
        }

        protected void Write(int x, int y, int value)
        {
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
                for(int yy = y-1; yy < yy+1; yy++)
                {
                    if (Read(x, y) != NULL)
                    {
                        numNeighbors++;
                    }
                }
            }

            return numNeighbors;
        }
    }
}
