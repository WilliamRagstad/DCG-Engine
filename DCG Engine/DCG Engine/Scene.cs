using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCG_Engine.DCG
{
    public class Scene
    {
        public Scene(List<Object> objects, string name)
        {
            Objects = objects;
            Name = name;
        }

        public List<Object> Objects { get; set; }
        public string Name { get; }

        public void Draw()
        {
            for(int i = 0; i < Objects.Count; i++)
            {
                Objects[i].Draw();
            }
        }
    }
}
