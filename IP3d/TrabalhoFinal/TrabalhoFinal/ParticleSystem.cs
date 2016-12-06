using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoFinal
{
    class ParticleSystem
    {
        BasicEffect effect;
        GraphicsDevice device;
        Vector3 poeiraPos, explosionPos;
        int numberParticles;
        float timer;
        List<Dust> poeira;
        List<Explosion> explosion;
        ClsCamera camera;
        Random rnd;
        bool isMoving = false;
        Tank tank;

        public ParticleSystem(GraphicsDevice device,ClsCamera camera,Tank tank)
        {
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;

            numberParticles = 100;

            this.camera = camera;
            this.device = device;
            this.tank = tank;

            poeira = new List<Dust>();
            explosion = new List<Explosion>();

            rnd = new Random();
        }

        public void Update(GameTime gameTime,Vector3 Pos,Mapa map)
        {
            if (isMoving)
            {
                int total = 10;
                for (int i = 0; i < total; i++)
                {
                    if (poeira.Count < numberParticles)
                    {
                        //Console.WriteLine(Pos);
                        //Console.WriteLine(poeira.Count);
                        poeira.Add(new Dust(Pos, tank.TankDirection, 1f, rnd));
                    }
                    else
                        break;
                }
                for (int i = 0; i < poeira.Count; i++)
                {
                    poeira[i].Update(gameTime, map, isMoving);
                    //if(i == 5)
                       // Console.WriteLine(poeira[i].Position);
                    if (poeira[i].LifeTimer > 0.5f)
                    {
                        poeira.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public void Draw()
        {
            effect.World = Matrix.Identity;
            effect.View = camera.ViewMatrixCamera;
            effect.Projection = camera.ProjectionMatrixCamera;

            effect.CurrentTechnique.Passes[0].Apply();

            VertexPositionColor[] vertices = new VertexPositionColor[poeira.Count * 2];

            for(int i = 0;i< poeira.Count;i++)
            {
                vertices[i * 2] = new VertexPositionColor(poeira[i].Position, Color.Black);
                vertices[i * 2 + 1] = new VertexPositionColor(poeira[i].Position + new Vector3(0.05f,0.05f,0.05f), Color.Black);
            }

            device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, poeira.Count);
        }

        public Boolean Moving
        {
            set
            {
                isMoving = value;
            }
            get
            {
                return isMoving;
            }
        }
    }
}
