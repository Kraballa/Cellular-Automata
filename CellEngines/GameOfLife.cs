using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox.CellEngines
{
    public class GameOfLife : Automata
    {
        public const int ALIVE = 1;

        public GameOfLife(int width, int height, int scale) : base(width, height, scale)
        {
            Console.WriteLine("helloworld");
            ColorMapping.Add(NULL, Color.Black);
            ColorMapping.Add(ALIVE, Color.White);

            ActionMapping.Add(ALIVE, (x, y) => {
                int numNeighbors = GetNumNeightbors(x, y);
                if(numNeighbors < 2 || numNeighbors > 3)
                {
                    Write(x, y, NULL);
                }
            });
            ActionMapping.Add(NULL, (x, y) => { if (GetNumNeightbors(x, y) == 3) { Write(x, y, ALIVE); } });

            LeftPlace = ALIVE;
            Console.WriteLine("end");
        }
    }
}
