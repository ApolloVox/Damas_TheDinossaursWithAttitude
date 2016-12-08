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
        Vector3 pos, dir,randomSpawn;
        float larrgura, gravity = 0.1f, height,timer = 0f;

        public Dust(Vector3 tankPos,Vector3 tankDIr,float largura,Random rnd)
        {
            this.larrgura = largura;

            randomSpawn = PerpendicularVector(dir,rnd);

            pos = new Vector3(tankPos.X+randomSpawn.X,
                tankPos.Y, 
                tankPos.Z+randomSpawn.Z);
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

        private Vector3 PerpendicularVector(Vector3 direction,Random rnd)
        {
            if(rnd.NextDouble() % 2 == 0)
                return new Vector3(direction.Z + (float)rnd.NextDouble() * 0.1f, direction.Y, -direction.X + (float)rnd.NextDouble() * 0.1f);
            else
                return new Vector3(-direction.Z + (float)rnd.NextDouble() * 0.1f, direction.Y, direction.X + (float)rnd.NextDouble() * 0.1f);
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
