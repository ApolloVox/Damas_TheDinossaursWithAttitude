using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoFinal
{
    class Tank
    {
        Matrix worldMatrix, viewMatrix, projectionMatrix;
        Matrix cannonTransform,turretTransform;
        Vector3 tankPos,dir;
        float yaw, pitch,speed;
        Matrix[] boneTransforms;
        Model tankModel;
        ModelBone turretBone, cannonBone;
        BasicEffect effect;
        Mapa map;

        public Tank(GraphicsDevice device, ContentManager content,Mapa mapa)
        {
            effect = new BasicEffect(device);

            worldMatrix = Matrix.Identity;
            viewMatrix = Matrix.Identity;
            projectionMatrix = Matrix.Identity;

            tankModel = content.Load<Model>("tank");
            turretBone = tankModel.Bones["turret_geo"];
            cannonBone = tankModel.Bones["canon_geo"];

            turretTransform = turretBone.Transform;
            cannonTransform = cannonBone.Transform;

            boneTransforms = new Matrix[tankModel.Bones.Count];

            map = mapa;

            tankPos = new Vector3(5f, 15f, 5f);
            dir = new Vector3(1,0,1);

            yaw = 0;
            pitch = 0;
            speed = 0.5f;
        }

        public void Update()
        {
            Vector3 oldPos = tankPos;

            KeyboardState keys = Keyboard.GetState();
            Move(keys);

            tankPos.Y = map.GetHeight(tankPos).Y;

            //calculo do vetor direção
            dir.X = (float)Math.Cos(yaw) + tankPos.X;
            dir.Z = -(float)Math.Sin(yaw) + tankPos.Z;
            dir.Y =  tankPos.Y;

            if ((tankPos.X < 0 || tankPos.Z < 0))
                tankPos = oldPos;
            if ((tankPos.X > 127 || tankPos.Z > 127))
                tankPos = oldPos;
            //Console.WriteLine(tankPos);
        }

        public void Move(KeyboardState keys)
        {

            if (keys.IsKeyDown(Keys.S))
            {
                tankPos.X -= (dir.X - tankPos.X) * speed;
                tankPos.Z -= (dir.Z - tankPos.Z) * speed;
            }
            if (keys.IsKeyDown(Keys.W))
            {
                tankPos.X += (dir.X - tankPos.X) * speed;
                tankPos.Z += (dir.Z - tankPos.Z) * speed;
            }

            //Cálculo das normais para andar paralelo à direçao
            if (keys.IsKeyDown(Keys.A))
            {
                yaw += 0.1f;
            }
            if (keys.IsKeyDown(Keys.D))
            {
                yaw -= 0.1f;
            }
        }

        public void Draw(GraphicsDevice device,ClsCamera camera)
        {
            tankModel.Root.Transform = Matrix.CreateScale(0.01f) * Matrix.CreateRotationY(yaw + MathHelper.ToRadians(90f));
            turretBone.Transform = Matrix.CreateRotationY(MathHelper.ToRadians(180)) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(2f);

            tankModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in tankModel.Meshes)
            {
                foreach(BasicEffect effect in  mesh.Effects)
                {
                    effect.World = effect.World = boneTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(tankPos);
                    effect.View = camera.ViewMatrixCamera;
                    effect.Projection = camera.ProjectionMatrixCamera;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        /*
         (Vector3)Nab = (escalar)db * Na + da * Nb 
         (Vector3)Ncd = (escalar)dd * Nc + dc * Nd
         (Vector3)N = (escalar) dcd *Nab + dab* Ncd
          
         right = cross(dir-h,n)
         dir = Cross(n,right)
         Matrix r = Matrix.Identity
         r.Forward = d
         r.Up = n
         r.right = right*/
    }
}
