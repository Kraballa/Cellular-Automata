using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox.CellEngines
{
    /// <summary>
    /// Wire World Cellular Automata. See `https://en.wikipedia.org/wiki/Wireworld`
    /// </summary>
    class WireWorld : Automata
    {
        public const int WIRE = 1;
        public const int POSITIVE = 2;
        public const int NEGATIVE = 3;
        public WireWorld(int width, int height, int scale) : base(width, height, scale)
        {
            Name = "Wire World";

            ColorMapping.Add(WIRE, Color.Yellow);
            ColorMapping.Add(POSITIVE, Color.Red);
            ColorMapping.Add(NEGATIVE, Color.Blue);

            ActionMapping.Add(POSITIVE, (x, y) => {
                Write(x, y, NEGATIVE);
            });
            ActionMapping.Add(NEGATIVE, (x, y) => { Write(x, y, WIRE); });
            ActionMapping.Add(WIRE, (x, y) =>
            {
                int numPositive = 0;
                for (int xx = x - 1; xx <= x + 1; xx++)
                {
                    for (int yy = y - 1; yy <= y + 1; yy++)
                    {
                        if (Read(xx, yy) == POSITIVE)
                        {
                            numPositive++;
                        }
                    }
                }
                if (numPositive == 1 || numPositive == 2)
                {
                    Write(x, y, POSITIVE);
                }
            });

            LeftPlace = WIRE;
            RightPlace = POSITIVE;
            Refreshrate = 6;
        }
    }
}
