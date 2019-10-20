using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DCG_Engine.DCG
{
    public class Engine
    {
        public Engine(List<Scene> scenes = null, int fps = 30)
        {
            _runtime = new Thread(_runtimeMethod);
            _running = false;
            FPS = fps;
            CurrentSceneIndex = 0;
            Scenes = scenes != null ? scenes : new List<Scene>();
            Events = new List<Event.IGameEvent>();
        }

        private Thread _runtime;
        private bool _running;

        public List<Scene> Scenes { get; set; }
        public int CurrentSceneIndex { get; set; }
        public int FPS { get; }
        public Scene CurrentScene {
            get
            {
                return Scenes[CurrentSceneIndex];
            }
            set
            {
                CurrentSceneIndex = _findScene(value);
            }
        }
        private int _findScene(Scene scene)
        {
            for(int i = 0; i < Scenes.Count; i++)
            {
                if (scene.GetHashCode() == Scenes[i].GetHashCode()) return i;
            }
            throw new ArgumentOutOfRangeException(nameof(scene), scene, "Scene does not exist in the current engines list.");
        }

        public List<Event.IGameEvent> Events { get; set; }

        public void Start()
        {
            _running = true;
            _runtime.ApartmentState = ApartmentState.STA;
            _runtime.Start();
        }
        public void Stop(int exitValue = 1)
        {
            _sendEventToSubscribers(typeof(Event.OnStop), new object[] { 1 });
            _running = false;
            _runtime.Join();  // Wait for thread to close 
            _runtime.Abort(); // Might be problematic
        }

        [STAThread]
        private void _runtimeMethod()
        {
            int millisecondsPerFrame = (int)(1000 / (float)FPS);
            System.Diagnostics.Stopwatch latency = new System.Diagnostics.Stopwatch();

            _sendEventToSubscribers(typeof(Event.OnStart), null);
            while (_running)
            {
                latency.Reset();
                latency.Start();
                // Clear scene
                Console.Clear(); // Can this be optimized?
                Scenes[CurrentSceneIndex].Draw(); // Draw current scene

                // Check for new events
                _checkEvents();
                
                _sendEventToSubscribers(typeof(Event.OnDraw), null);
                _sendEventToSubscribers(typeof(Event.OnStep), null);

                latency.Stop();
                int sleep = millisecondsPerFrame - latency.Elapsed.Milliseconds;
                Thread.Sleep( sleep > 0 ? sleep : 0 );
            }
            _sendEventToSubscribers(typeof(Event.OnStop), new object[] { 1 });
        }

        private void _checkEvents()
        {
            if (System.Windows.Forms.Control.MouseButtons != System.Windows.Forms.MouseButtons.None)
            {
                _sendEventToSubscribers(typeof(Event.OnMouseClick), new object[] {
                    System.Windows.Forms.Control.MouseButtons,
                    (DCG.Resource.Position)System.Windows.Forms.Control.MousePosition
                });
            }
        }

        private void _sendEventToSubscribers(Type eventType, object[] parameters)
        {
            for (int i = 0; i < Events.Count; i++)
            {
                if (Events[i].ToString() == eventType.ToString()) Events[i].Invoke(parameters);
            }
        }
    }
}
