using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoFinal
{
    enum PSType
    {
        wheel,
        explosion
    };

    class ParticleSystem
    {
        BasicEffect effect;
        GraphicsDevice device;
        int numberParticles;
        List<Dust> poeira;
        List<Explosion> explosion;
        ClsCamera camera;
        Random rnd;
        bool isMoving = false;
        Tank tank;
        PSType active;
        VertexPositionColor[] verticesPoeira;

        public ParticleSystem(GraphicsDevice device, ClsCamera camera, Tank tank)
        {
            poeira = new List<Dust>();
            explosion = new List<Explosion>();

            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;

            //fogOn
            effect.FogEnabled = true;
            effect.FogColor = Color.Black.ToVector3(); // For best results, ake this color whatever your background is.
            effect.FogStart = 20f;
            effect.FogEnd = 100f;

            numberParticles = 200;

            this.camera = camera;
            this.device = device;
            this.tank = tank;

            rnd = new Random();

        }

        public void Update(GameTime gameTime, Vector3 Pos, Mapa map,PSType _PsType)
        {
            active = _PsType;
            switch (_PsType)
            {
                case PSType.wheel:
                    if (isMoving)
                    {
                        int total = 10;
                        for (int i = 0; i < total; i++)
                        {
                            if (poeira.Count < numberParticles)
                            {
                                poeira.Add(new Dust(map.GetHeight(Pos), tank.TankDirection, 1f, rnd));
                            }
                            else
                                break;
                        }
                        for (int i = 0; i < poeira.Count; i++)
                        {
                            //if(i == 5)
                            // Console.WriteLine(poeira[i].Position);
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
                    break;

                case PSType.explosion:

                    break;
            }
        }

        public void Draw()
        {
            effect.World = Matrix.Identity;
            effect.View = camera.ViewMatrixCamera;
            effect.Projection = camera.ProjectionMatrixCamera;

            effect.CurrentTechnique.Passes[0].Apply();

            switch (active)
            {
                case PSType.wheel:
                    device.DrawUserPrimitives(PrimitiveType.LineList, verticesPoeira, 0, poeira.Count);
                    break;
                case PSType.explosion:
                    break;
            }
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
