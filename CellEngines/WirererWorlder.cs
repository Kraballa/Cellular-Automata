using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox.CellEngines
{
    /// <summary>
    /// Extension of the Wire World Cellular Automata.
    /// </summary>
    class WirererWorlder : Automata
    {
        public const int WIRE = 1;
        public const int POSITIVE = 2;
        public const int NEGATIVE = 3;
        //additional controls
        public const int DEADWIRE = 4;
        public const int WIRECUTTER = 5;
        public const int BRIDGE = 6;
        public const int PULSAR = 7;

        public WirererWorlder(int width, int height, int scale) : base(width, height, scale)
        {
            Name = "Wire World";

            ColorMapping.Add(WIRE, Color.Yellow);
            ColorMapping.Add(POSITIVE, Color.Red);
            ColorMapping.Add(NEGATIVE, Color.Blue);
            ColorMapping.Add(DEADWIRE, Color.DarkOrange);
            ColorMapping.Add(WIRECUTTER, Color.Green);
            ColorMapping.Add(BRIDGE, Color.Gray);
            ColorMapping.Add(PULSAR, Color.LightCoral);

            ActionMapping.Add(POSITIVE, (x, y) => {
                Write(x, y, NEGATIVE);
            });
            ActionMapping.Add(NEGATIVE, (x, y) => { Write(x, y, WIRE); });
            ActionMapping.Add(WIRE, (x, y) =>
            {
                int numPositive = GetNumNeightbors(x, y, POSITIVE);
                if (numPositive == 1 || numPositive == 2)
                {
                    Write(x, y, POSITIVE);
                }
            });
            ActionMapping.Add(BRIDGE, (x, y) =>
            {
                List<Tuple<Point, Point>> tuples = new List<Tuple<Point, Point>>();
                Point top = new Point(x, y - 1);
                Point bottom = new Point(x, y + 1);
                Point left = new Point(x - 1, y);
                Point right = new Point(x + 1, y);
                tuples.Add(new Tuple<Point, Point>(top, bottom));
                tuples.Add(new Tuple<Point, Point>(bottom, top));
                tuples.Add(new Tuple<Point, Point>(left,right));
                tuples.Add(new Tuple<Point, Point>(right,left));

                foreach(Tuple<Point,Point> tuple in tuples)
                {
                    if(Read(tuple.Item1) == POSITIVE)
                    {
                        Point delta = tuple.Item2 - tuple.Item1;
                        delta.X /= 2;
                        delta.Y /= 2;
                        Point newPoint = tuple.Item1 + delta;

                        while (Read(newPoint) == BRIDGE) { newPoint += delta; }
                        if(Read(newPoint) == WIRE)
                        {
                            Write(newPoint, POSITIVE);
                        }
                    }
                }
            });
            ActionMapping.Add(WIRECUTTER, (x, y) =>
            {
                List<Tuple<Point, Point>> tuples = new List<Tuple<Point, Point>>();
                Point top = new Point(x, y - 1);
                Point bottom = new Point(x, y + 1);
                Point left = new Point(x - 1, y);
                Point right = new Point(x + 1, y);
                tuples.Add(new Tuple<Point, Point>(top, bottom));
                tuples.Add(new Tuple<Point, Point>(bottom, top));
                tuples.Add(new Tuple<Point, Point>(left, right));
                tuples.Add(new Tuple<Point, Point>(right, left));

                foreach (Tuple<Point, Point> tuple in tuples)
                {
                    if (Read(tuple.Item1) == POSITIVE)
                    {
                        if(Read(tuple.Item2) == WIRE)
                        {
                            Write(tuple.Item2, DEADWIRE);
                        }
                        else if (Read(tuple.Item2) == DEADWIRE)
                        {
                            Write(tuple.Item2, WIRE);
                        }
                    }
                }
            });
            ActionMapping.Add(PULSAR, (x, y) => {
                if(Read(x+1,y) == WIRE)
                {
                    Write(x + 1, y, POSITIVE);
                }
                if (Read(x - 1, y) == WIRE)
                {
                    Write(x - 1, y, POSITIVE);
                }
                if (Read(x, y + 1) == WIRE)
                {
                    Write(x, y + 1, POSITIVE);
                }
                if (Read(x, y - 1) == WIRE)
                {
                    Write(x, y - 1, POSITIVE);
                }
            });

            LeftPlace = WIRE;
            Refreshrate = 6;
        }
    }
}
