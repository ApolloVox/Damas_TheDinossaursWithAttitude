using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoFinal
{
    class Dust
    {            
        Vector3 pos, dir;
        float timer = 0f;

        public Dust(Vector3 tankPos,Random rnd)
        {
            pos = new Vector3(tankPos.X,
                tankPos.Y, 
                tankPos.Z);
            dir = new Vector3(dir.X + (float)rnd.NextDouble()*0.05f,
                dir.Y+(float)rnd.NextDouble()*0.1f,
                dir.Z + (float)rnd.NextDouble()*0.05f);
        }

        public void Update(GameTime gametime,Mapa map,Boolean isMoving)
        {
            timer += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (isMoving)
            {
                pos += dir * new Vector3(timer);
            }
        }

        public Vector3 Position
        {
            get
            {
                return pos;
            }
        }

        public Vector3 Direction
        {
            get
            {
                return dir;
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
