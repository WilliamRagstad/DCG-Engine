using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace DCG_Engine.DCG
{
    public static class Actions
    {
        public static bool KeyDown(Key key) => Keyboard.IsKeyDown(key);
        public static bool KeyUp(Key key) => Keyboard.IsKeyUp(key);
        public static bool KeyToggled(Key key) => Keyboard.IsKeyToggled(key);
        public static Resource.Position PositionOnScreen(Resource.Position position)
        {
            return new Resource.Position(
                PositionOnScreenX(position.X),
                PositionOnScreenX(position.Y)
                );
        }
        public static int PositionOnScreenX(int x)
        {
            return Window.Position.X + ExtendedConsole.GetConsoleFont().SizeX * x;
        }
        public static int PositionOnScreenY(int y)
        {
            return Window.Position.Y + ExtendedConsole.GetConsoleFont().SizeY * y;
        }
        public static Resource.Position PositionOnConsole(Resource.Position position)
        {
            return new Resource.Position(
                PositionOnConsoleX(position.X),
                PositionOnConsoleY(position.Y)
                );
        }
        public static int PositionOnConsoleX(int x)
        {
            return (Window.Position.X + Window.Size.Width - x) / ExtendedConsole.GetConsoleFont().SizeX;
        }
        public static int PositionOnConsoleY(int y)
        {
            return (Window.Position.Y - y) / ExtendedConsole.GetConsoleFont().SizeY;
        }


        public struct Window
        {
            public static Resource.Position Position
            {
                get
                {
                    return ExtendedConsole.GetConsoleWindowArea().Position;
                }
            }
            public static Resource.Size Size
            {
                get
                {
                    return ExtendedConsole.GetConsoleWindowArea().Size;
                }
            }

            public static int Columns
            {
                get
                {
                    return Console.WindowWidth;
                }
            }
            public static int Rows
            {
                get
                {
                    return Console.WindowHeight;
                }
            }
        }

        public struct ExtendedConsole
        {

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct ConsoleFont
            {
                public uint Index;
                public short SizeX, SizeY;
            }
            
            [DllImport("kernel32")]
            public static extern bool SetConsoleIcon(IntPtr hIcon);

            public static bool SetConsoleIcon(System.Drawing.Icon icon)
            {
                return SetConsoleIcon(icon.Handle);
            }

            [DllImport("kernel32")]
            private extern static bool SetConsoleFont(IntPtr hOutput, uint index);

            private enum StdHandle
            {
                OutputHandle = -11
            }

            [DllImport("kernel32")]
            private static extern IntPtr GetStdHandle(StdHandle index);

            public static bool SetConsoleFont(uint index)
            {
                return SetConsoleFont(GetStdHandle(StdHandle.OutputHandle), index);
            }

            [DllImport("kernel32")]
            private static extern bool GetConsoleFontInfo(IntPtr hOutput, [MarshalAs(UnmanagedType.Bool)]bool bMaximize,
                uint count, [MarshalAs(UnmanagedType.LPArray), Out] ConsoleFont[] fonts);



            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public class CONSOLE_FONT_INFO_EX
            {
                private int cbSize;
                public CONSOLE_FONT_INFO_EX()
                {

                }
                public int FontIndex;
                public short FontWidth;
                public short FontHeight;
                public int FontFamily;
                public int FontWeight;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                public string FaceName;
            }
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            extern static bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, [In, Out] CONSOLE_FONT_INFO_EX lpConsoleCurrentFont);
            [DllImport("kernel32.dll")]
            static extern bool GetCurrentConsoleFont(
                    IntPtr hConsoleOutput,
                    bool bMaximumWindow,
                    out ConsoleFont lpConsoleCurrentFont);

            [DllImport("kernel32")]
            private static extern uint GetNumberOfConsoleFonts();

            public static uint ConsoleFontsCount
            {
                get
                {
                    return GetNumberOfConsoleFonts();
                }
            }

            public static ConsoleFont[] ConsoleFonts
            {
                get
                {
                    ConsoleFont[] fonts = new ConsoleFont[GetNumberOfConsoleFonts()];
                    if (fonts.Length > 0)
                        GetConsoleFontInfo(GetStdHandle(StdHandle.OutputHandle), false, (uint)fonts.Length, fonts);
                    return fonts;
                }
            }
            const int SWP_NOSIZE = 0x0001;

            [DllImport("kernel32.dll", ExactSpelling = true)]
            private static extern IntPtr GetConsoleWindow();

            private static IntPtr currentConsoleHandle = GetConsoleWindow();
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int Left;        // x position of upper-left  corner
                public int Top;         // y position of upper-left  corner
                public int Right;       // x position of lower-right corner
                public int Bottom;      // y position of lower-right corner
            }
            public static ConsoleFont GetConsoleFont()
            {
                ConsoleFont font = new ConsoleFont();
                GetCurrentConsoleFont(GetStdHandle(StdHandle.OutputHandle), false, out font);
                return font;
            }

            public static Resource.Area GetConsoleWindowArea()
            {
                RECT area;
                GetWindowRect(currentConsoleHandle, out area);
                return new Resource.Area(
                    new Resource.Position(area.Left, area.Top),
                    new Resource.Size(area.Right - area.Left, area.Bottom - area.Top)
                );
            }

            public static void SetChar(int column, int row, char c)
            {
                if (column < 0 || row < 0 || column > Console.BufferWidth || row > Console.BufferHeight) return;
                Console.SetCursorPosition(row, column);
                Console.Write(c);
            }
        }

        public static class Draw
        {
            public static void Line(Resource.Position p1, Resource.Position p2)
            {
                if (p1.X == p2.X) return;
                double k = (p2.Y - p1.Y) / (p2.X - p1.X);
                int y(int x) => (int)( k * (x - p1.X) + p1.Y );
                if (p1.X < p2.X)
                {
                    for (int x = p1.X; x < p2.X; x++)
                    {
                        ExtendedConsole.SetChar(x, y(x), '#');
                    }
                }
                else
                {
                    for (int x = p2.X; x < p1.X; x++)
                    {
                        ExtendedConsole.SetChar(x, y(x), '#');
                    }
                }
            }
        }
    }
}
