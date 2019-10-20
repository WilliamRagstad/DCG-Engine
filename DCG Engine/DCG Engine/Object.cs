using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCG_Engine.DCG
{
    public class Object
    {
        public Object(string name, Sprite sprite) : this(name, sprite, 0, 0) { }
        public Object(string name, Sprite sprite, int x, int y)
        {
            Name = name;
            Sprite = sprite;
            X = x;
            Y = y;
            Events = new List<Event.IGameEvent>();
        }

        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; }
        public Sprite Sprite { get; }

        public List<Event.IGameEvent> Events { get; set; }

        public void Draw()
        {
            Sprite.Draw(X, Y);
        }
    }
}