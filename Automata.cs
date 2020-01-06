using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox
{
    public class Automata
    {
        public Dictionary<int, Color> ColorMapping;

        protected Random Random;
        protected int Width;
        protected int Height;
        protected int Scale;

        private int[,] screen;
        private int[,] buffer;
        
        public Automata(int width, int height, int scale)
        {
            screen = new int[width, height];
            buffer = new int[width, height];
            Random = new Random();
            ColorMapping = new Dictionary<int, Color>();

            Width = width;
            Height = height;
            Scale = scale;
        }

        public virtual void Initialize()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void SimulationStep()
        {

        }

        public void Draw()
        {
            screen = buffer;
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
    }
}
