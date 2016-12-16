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
        int numberParticlesPoeira, numberParticlesExplosion;
        List<Dust> poeira;
        List<Explosion> explosion;
        ClsCamera camera;
        Random rnd;
        bool isMoving = false;
        Tank tank;
        VertexPositionColor[] verticesPoeira,verticesExplosion;
        Vector3 explosionLocation;
        Mapa map;

        public ParticleSystem(GraphicsDevice device, ClsCamera camera, Tank tank,Mapa map)
        {
            poeira = new List<Dust>();
            explosion = new List<Explosion>();

            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;

            this.map = map;

            numberParticlesPoeira = 10000;
            numberParticlesExplosion = 200000;

            this.camera = camera;
            this.device = device;
            this.tank = tank;

            rnd = new Random();

        }

        public void UpdatePoeira(GameTime gameTime, Vector3 Pos)
        {
            int total;

            if (isMoving)
            {
                total = 100;
                for (int i = 0; i < total; i++)
                {
                    if (poeira.Count < numberParticlesPoeira)
                    {
                        poeira.Add(new Dust(map.GetHeight(Pos), rnd));
                    }
                    else
                        break;
                }
                for (int i = 0; i < poeira.Count; i++)
                {
                    if (poeira[i].LifeTimer > 0.4f)
                    {
                        poeira.RemoveAt(i);
                        i--;
                    }
                    else
                        poeira[i].Update(gameTime, map, isMoving);
                }
            }

            verticesPoeira = new VertexPositionColor[poeira.Count * 2];

            for (int i = 0; i < poeira.Count; i++)
            {
                verticesPoeira[i * 2] = new VertexPositionColor(poeira[i].Position, Color.GreenYellow);
                verticesPoeira[i * 2 + 1] = new VertexPositionColor(poeira[i].Position + new Vector3(0.01f, 0.0f, 0.01f), Color.Green);
            }
        }


        public void UpdateExplosion(GameTime gameTime)
        {
            for (int i = 0; i < explosion.Count; i++)
            {
                if (explosion[i].LifeTimer > 1.2f || explosion[i].Position.X <=0 
                    || explosion[i].Position.Z <= 0 
                    || explosion[i].Position.X >=127
                    || explosion[i].Position.Z >= 127)
                {
                    explosion.RemoveAt(i);
                    i--;
                }
                else
                    explosion[i].Update(gameTime, map);
            }

            verticesExplosion = new VertexPositionColor[explosion.Count * 2];

            for(int i = 0;i< explosion.Count;i++)
            {
                verticesExplosion[i * 2] = new VertexPositionColor(explosion[i].Position, Color.Red);
                verticesExplosion[i * 2 + 1] = new VertexPositionColor(explosion[i].Position + new Vector3(0.01f, 0.0f, 0.01f), Color.Yellow);
            }
        }

        public void AddParticlesExplosion(Vector3 pos)
        {
            Vector3 normalDir = map.InterpolyNormals(pos);
            explosionLocation = pos;
            int total = 750;
            for (int i = 0; i < total; i++)
            {
                if (explosion.Count < numberParticlesExplosion)
                {
                    explosion.Add(new Explosion(explosionLocation,normalDir, rnd,1));
                }
                else
                    break;
            }
        }

        public void AddParticlesExplosionCannon(Vector3 pos,Vector3 dir)
        {
            int total = 100;

            for(int i = 0;i< total;i++)
            {
                if (explosion.Count < numberParticlesExplosion)
                {
                    explosion.Add(new Explosion(pos - dir/10f, -dir, rnd,2));
                }
                else
                    break;
            }
        }

        public void Draw()
        {
            effect.World = Matrix.Identity;
            effect.View = camera.ViewMatrixCamera;
            effect.Projection = camera.ProjectionMatrixCamera;

            effect.CurrentTechnique.Passes[0].Apply();

            if (poeira.Count != 0)
                device.DrawUserPrimitives(PrimitiveType.LineList, verticesPoeira, 0, poeira.Count);
        }

        public void DrawExplosion()
        {
            effect.World = Matrix.Identity;
            effect.View = camera.ViewMatrixCamera;
            effect.Projection = camera.ProjectionMatrixCamera;

            effect.CurrentTechnique.Passes[0].Apply();

            if(explosion.Count != 0)
            device.DrawUserPrimitives(PrimitiveType.LineList, verticesExplosion, 0, explosion.Count);
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
