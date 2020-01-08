using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SandBox
{
    public class UI
    {
        private List<Mapping> Mappings;

        private Color color;
        private Rectangle rect;
        private float fade = 10;

        public UI(Dictionary<int,Color> mappings)
        {
            Mappings = new List<Mapping>();
            int count = 0;
            foreach (KeyValuePair<int, Color> kv in mappings)
            {
                Mappings.Add(new Mapping(kv.Key, kv.Value, Keys.D1 + count));
                count++;
            }
            rect = new Rectangle(Engine.Width - 96, Engine.Height - 96, 94, 94);
        }

        public void Initialize()
        {
            
        }

        public void Update()
        {
            foreach(Mapping mapping in Mappings)
            {
                if (KeyboardInput.CheckPressed(mapping.Key))
                {
                    Engine.Instance.Automata.LeftPlace = mapping.Value;
                    color = mapping.Color;
                    fade = 10;
                    break;
                }
            }
        }
        public void Draw()
        {
            if(fade < 60)
            {
                Render.Rect(rect, new Color(color, 30 / fade));
                Render.HollowRect(rect, new Color(Color.Black, 30 / fade));
                fade++;
            }
        }

        public class Mapping
        {
            public int Value { get; private set; }
            public Color Color { get; private set; }
            public Keys Key { get; private set; }
            public Mapping(int value, Color color, Keys key)
            {
                Value = value;
                Color = color;
                Key = key;
            }
        }
    }
}
