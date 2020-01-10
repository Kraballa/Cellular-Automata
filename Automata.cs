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
        public int Refreshrate = 0;
        public string Name { get; protected set; }

        protected Random Random;
        protected int Width;
        protected int Height;
        protected int Scale;
        

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
            Console.WriteLine("automata initialized");
        }

        /// <summary>
        /// Used for adding new Cells into the Automata
        /// </summary>
        public virtual void Update()
        {
            Point mousePos = new Point(MouseInput.X / Scale, MouseInput.Y / Scale);
            if (Engine.Instance.Screen.Contains(mousePos))
            {
                if(MouseInput.LeftClick())
                {
                    Write(mousePos.X, mousePos.Y, LeftPlace);
                }else if (MouseInput.RightClick())
                {
                    Write(mousePos.X, mousePos.Y, RightPlace);
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

        protected int Read(Point p)
        {
            return Read(p.X, p.Y);
        }

        protected void Write(int x, int y, int value)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                buffer[x, y] = value;
        }

        protected void Write(Point p, int value)
        {
            Write(p.X, p.Y, value);
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
            return 8-GetNumNeightbors(x, y, NULL);
        }

        protected int GetNumNeightbors(int x, int y, int cellType)
        {
            int numNeighbors = 0;

            for (int xx = x - 1; xx <= x + 1; xx++)
            {
                for (int yy = y - 1; yy <= y + 1; yy++)
                {
                    if ((xx != x || yy != y) && Read(xx, yy) == cellType)
                    {
                        numNeighbors++;
                    }
                }
            }

            return numNeighbors;
        }

        protected Point[] GetAllNeighbors(int x, int y)
        {
            Point[] points = new Point[8];
            int index = 0;
            for (int xx = x - 1; xx <= x + 1; xx++)
            {
                for (int yy = y - 1; yy <= y + 1; yy++)
                {
                    if (xx != x || yy != y)
                    {
                        points[index] = new Point(xx, yy);
                        index++;
                    }
                }
            }

            return points;
        }
    }
}
