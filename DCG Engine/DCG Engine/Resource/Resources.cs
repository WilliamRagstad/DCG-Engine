using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCG_Engine.DCG.Resource
{
    public class Size
    {
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }
    public class Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static explicit operator Position(System.Drawing.Point a)
        {
            return new Position(a.X, a.Y);
        }
    }

    public class Area
    {
        public Position Position;
        public Size Size;

        public Area(Position position, Size size)
        {
            Position = position;
            Size = size;
        }
    }
}
