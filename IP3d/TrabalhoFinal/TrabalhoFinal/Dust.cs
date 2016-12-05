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
        float larrgura, gravity = 0.1f, height,timer = 0f;

        public Dust(Vector3 tankPos,Vector3 tankDIr,float largura,Random rnd)
        {
            this.larrgura = largura;

            pos = tankPos;
            dir = tankDIr;
        }

        public void Update(GameTime gametime,Mapa map,Boolean isMoving)
        {
            timer += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (isMoving)
            {
                pos += dir * new Vector3(timer);
                pos = map.GetHeight(pos);
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
