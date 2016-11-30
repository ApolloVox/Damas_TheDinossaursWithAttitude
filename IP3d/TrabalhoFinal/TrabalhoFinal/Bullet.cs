using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoFinal
{
    class Bullet
    {
        Vector3 position,start,direction,oldPos;
        BoundingSphere sphere;
        Model bullet;
        float velocity,gravity,angle,time,scale,yaw,pitch;
        Matrix r,world;
        bool isALive = false;

        public Bullet(GraphicsDevice device,ContentManager content, Vector3 startPos,float cannonYaw,float cannonPitch,float _yaw)
        {
            bullet = content.Load<Model>("projektil FBX");
            position = startPos;
            start = startPos;
            position.Y = position.Y + 0.3f;

            velocity = 0.4f;
            gravity = 9.8f;
            time = 0;
            scale = 0.03f;
            yaw = cannonYaw;
            pitch = cannonPitch;
            angle = -cannonPitch;
            isALive = true;

            direction = new Vector3((float)Math.Cos(_yaw - yaw), 0, (float)Math.Sin(_yaw - yaw));

            direction.Normalize();
        }

        public void Update(Mapa map,Vector3 enemyPos,Model enemyModel,Matrix[] enemyWorldMatrix)
        {
            //P = P+v*t
            //V = V+A*t
            oldPos = position;
            if (map.GetHeight(position).Y >= position.Y)
                isALive = false;
            if (isALive)
            {
                float velocityY = velocity * (float)Math.Sin(angle) - (gravity * (time * time) / 2f);
                position.X += -direction.X * velocity;
                position.Z += -direction.Z * velocity;
                position.Y += velocityY;

                time += 0.01f;
                if(CollisionDectection(enemyPos,enemyModel,enemyWorldMatrix))
                isALive = false;

                sphere.Transform(Matrix.CreateTranslation(position));
            }
        }

        public bool CollisionDectection(Vector3 enemyPos,Model enemyModel,Matrix[] enemyWorldMatrix)
        {
            //(P1-P2).Length < r1+r2
            BoundingSphere bulletSphere = bullet.Meshes[0].BoundingSphere;
            bulletSphere = bulletSphere.Transform(world);

            for (int i = 0; i < enemyModel.Meshes.Count; i++)
            {
                BoundingSphere sphere = enemyModel.Meshes[i].BoundingSphere;
                sphere = sphere.Transform(enemyWorldMatrix[i]);

                //HERON'S FORMULA
                float a = (sphere.Center - position).Length();
                float b = (sphere.Center - oldPos).Length();
                float c = (oldPos - position).Length();
                float sp = (a + b + c) / 2f;
                float area = (float)Math.Sqrt(sp * (sp - a) * (sp - b) * (sp - c));
                float d = 2 * area / c;

                if (d < sphere.Radius)
                {
                    Console.WriteLine("HIT!!!!");
                    return true;
                }
                /*if ((position - enemyPos).Length() < sphere.Radius + bulletSphere.Radius)
                {
                    Console.WriteLine("HIT!!");
                    return true;
                }*/
            }
            return false;
        }

        public void Draw(GraphicsDevice device,ClsCamera camera,float yaw, Mapa map)
        {
            if (isALive)
            {
                Vector3 N = map.InterpolyNormals(position);
                Vector3 right = Vector3.Cross(direction, N);
                Vector3 d = Vector3.Cross(N, right);

                r = Matrix.Identity;
                r.Forward = d;
                r.Up = N;
                r.Right = right;

                foreach (ModelMesh mesh in bullet.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        world = Matrix.CreateScale(scale) * r * Matrix.CreateRotationY(-MathHelper.PiOver2) * Matrix.CreateTranslation(position);
                        effect.World = world;
                        effect.View = camera.ViewMatrixCamera;
                        effect.Projection = camera.ProjectionMatrixCamera;
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
