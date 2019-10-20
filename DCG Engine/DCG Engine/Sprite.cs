using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCG_Engine.DCG
{
    public class Sprite
    {
        public Sprite(string[] structure) : this(structure, ' ') { }
        public Sprite(string[] structure, char transparentKey)
        {
            Structure = structure;
            _transparentKey = transparentKey;

            int maxWidth = -1;
            for(int h = 0; h < structure.Length; h++) if (structure[h].Length > maxWidth) maxWidth = structure[h].Length;
            Width = maxWidth;
            Height = structure.Length;
            Scale = 1;
            Rotation = 0;
        }

        public string[] Structure { get; set; }
        private char _transparentKey;
        private int _rotation;

        public int Width { get; }
        public int Height { get; }

        public float Scale { get; set; }
        public int Rotation {
            get { return _rotation; }
            set {
                _rotation = value % 360;
            }
        }
        public void Draw(int x, int y)
        {
            for (int dy = 0; dy < Height; dy++)
            {
                for (int dx = 0; dx < Width; dx++)
                {
                    int px = dx + x;
                    int py = dy + y;
                    if (dx > Structure[dy].Length - 1) break;

                    if (px < 0 || py < 0) continue;
                    if (px >= Console.BufferWidth || py >= Console.BufferHeight) break;

                    char c = Structure[dy][dx];
                    if (c != _transparentKey)
                    {
                        Console.SetCursorPosition(px, py);
                        Console.Write(c);
                    }
                }
            }
        }
    }
}