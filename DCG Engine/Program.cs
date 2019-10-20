using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DCG_Engine.DCG;

namespace DCG_Engine
{
    class Program
    {

        static Engine GameEngine;
        // My Game Objects!
        public static DCG.Object ball;
        static void Main(string[] args)
        {

            DCG.Sprite spr = new DCG.Sprite(new[] {
                " .---.",
                "| o o |",
                "| ._. |",
                " '---'"
            });

            ball = new DCG.Object("Ball", spr, 2, 3);

            List<DCG.Object> sceneObjects = new List<DCG.Object>();
            sceneObjects.Add(ball);

            List<DCG.Scene> scenes = new List<DCG.Scene>();
            scenes.Add(new DCG.Scene(sceneObjects, "Main Scene"));

            GameEngine = new DCG.Engine(scenes, 10);
            GameEngine.Events.Add(new DCG.Event.OnStep(Step));
            GameEngine.Events.Add(new DCG.Event.OnMouseClick(Shoot));
            // engine.OnStep = ref Step;

            GameEngine.Start();
        }

        public static void Step()
        {
            if (DCG.Actions.KeyDown(System.Windows.Input.Key.Left)) ball.X  -= 1;
            if (DCG.Actions.KeyDown(System.Windows.Input.Key.Right)) ball.X += 1;
            if (DCG.Actions.KeyDown(System.Windows.Input.Key.Down)) ball.Y  += 1;
            if (DCG.Actions.KeyDown(System.Windows.Input.Key.Up)) ball.Y    -= 1;

            for(int i = 0; i < GameEngine.CurrentScene.Objects.Count; i++)
            {
                DCG.Object obj = GameEngine.CurrentScene.Objects[i];
                if (obj is Projectile)
                {
                    Projectile pr = (Projectile)obj;
                    if (pr.Remove())
                    {
                        GameEngine.CurrentScene.Objects.Remove(obj);
                    }
                    else
                    {
                        pr.Update();
                    }
                }
            }
        }

        public static void Shoot(System.Windows.Forms.MouseButtons mb, DCG.Resource.Position mp)
        {
            if (mb == System.Windows.Forms.MouseButtons.Left)
            {
                Actions.Draw.Line(new DCG.Resource.Position(ball.X, ball.Y), DCG.Actions.PositionOnConsole(mp));
                int x = Actions.PositionOnScreenX(ball.X);
                int y = Actions.PositionOnScreenY(ball.Y);
                double dy = mp.Y - y;
                double dx = mp.X - x;
                double angle = Math.Atan( dy / dx );
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"{dy}/{dx}");
                Console.WriteLine( (int)(angle * 180 / Math.PI) );
                GameEngine.CurrentScene.Objects.Add(
                    new Projectile(
                        (2 * ball.X + ball.Sprite.Width) / 2,
                        (2 * ball.Y + ball.Sprite.Height) / 2,
                        angle
                    )
                );
            }
        }


        class Projectile : DCG.Object
        {
            static DCG.Sprite sprite = new DCG.Sprite(new[] {
                "+"
            });
            static string name = "projectile";
            static double speed = 20 / GameEngine.FPS;
            public Projectile(int x, int y, double dir) : base(name, sprite, x, y)
            {
                Direction = dir;
                SimulateX = x;
                SimulateY = y;
            }

            public double Direction { get; }
            public double SimulateX { get; set; }
            public double SimulateY { get; set; }

            public void Update()
            {
                SimulateX += Math.Cos(Direction) * speed;
                SimulateY += Math.Sin(Direction) * speed;
                X = (int)SimulateX;
                Y = (int)SimulateY;
            }

            public bool Remove()
            {
                return X < 0 || Y < 0 || X >= Actions.Window.Columns - 1 || Y >= Actions.Window.Rows - 1;
            }
        }
    }
}
