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

        public Explosion(Vector3 position,Vector3 direction,Random rnd)
        {
            pos = position;

            dir = new Vector3(direction.X + (float)rnd.NextDouble() * 0.4f-0.2f,
                direction.Y + (float)rnd.NextDouble() * 0.8f-0.4f,
                direction.Z + (float)rnd.NextDouble() * 0.4f-0.2f);
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
