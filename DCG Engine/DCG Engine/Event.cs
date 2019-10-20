using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DCG_Engine.DCG.Resource;

namespace DCG_Engine.DCG
{
    public class Event
    {
        public interface IGameEvent
        {
            void Invoke(object[] parameters);
        }


        public class OnDraw : IGameEvent
        {
            public OnDraw(Action callback)
            {
                Callback = callback;
            }

            public Action Callback { get; }

            public void Invoke(object[] parameters)
            {
                Callback?.Invoke();
            }
        }
        public class OnStart : IGameEvent
        {
            public OnStart(Action callback)
            {
                Callback = callback;
            }

            public Action Callback { get; }

            public void Invoke(object[] parameters)
            {
                Callback?.Invoke();
            }
        }
        public class OnStep : IGameEvent
        {
            public OnStep(Action callback)
            {
                Callback = callback;
            }

            public Action Callback { get; }

            public void Invoke(object[] parameters)
            {
                Callback?.Invoke();
            }
        }
        public class OnStop : IGameEvent
        {
            public OnStop(Action<int> callback)
            {
                Callback = callback;
            }

            public Action<int> Callback { get; }

            public void Invoke(object[] parameters)
            {
                Callback?.Invoke((int)parameters[0]);
            }
        }

        public class OnMouseClick : IGameEvent
        {
            public OnMouseClick(Action<System.Windows.Forms.MouseButtons, DCG.Resource.Position> callback)
            {
                Callback = callback;
            }

            public Action<System.Windows.Forms.MouseButtons, DCG.Resource.Position> Callback { get; }

            public void Invoke(object[] parameters)
            {
                Callback?.Invoke(
                    (System.Windows.Forms.MouseButtons)parameters[0],
                    (DCG.Resource.Position)parameters[1]
                );
            }
        }
    }
}
