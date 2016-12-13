using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoFinal
{
    class Explosion
    {
        Vector3 pos, dir;
        float timer = 0,speed = 0.05f;

        public Explosion(Vector3 position,Random rnd)
        {
            pos = position;

            dir = new Vector3((float)rnd.NextDouble() * 0.4f,
                (float)rnd.NextDouble() * 0.8f,
                (float)rnd.NextDouble() * 0.4f);
        }

        public void Update(GameTime gametime, Mapa map)
        {
            timer += (float)gametime.ElapsedGameTime.TotalSeconds;
            pos += dir * new Vector3(timer)*speed;
        }

        public Vector3 Position
        {
            get
            {
                return pos;
            }
        }

        public float LifeTimer
        {
            get
            {
                return timer;
            }
        }
    }
}
