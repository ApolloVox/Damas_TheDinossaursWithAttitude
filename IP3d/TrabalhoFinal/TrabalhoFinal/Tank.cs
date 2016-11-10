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
        Matrix cannonTransform,turretTransform,r;
        Vector3 tankPos,dir;
        float yaw, pitch,speed,cannonYaw,cannonPitch;
        Matrix[] boneTransforms;
        Model tankModel;
        ModelBone turretBone, cannonBone;
        BasicEffect effect;
        Mapa map;
        BasicEffect basicEffect;

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

            tankPos = new Vector3(20f, 15f, 20f);
            dir = new Vector3(0,0,0);

            yaw = 0;
            pitch = 0;
            speed = 0.5f;
            cannonYaw = 0f;
            cannonPitch = 0f;

        }

        public void Update()
        {
            Vector3 oldPos = tankPos;

            KeyboardState keys = Keyboard.GetState();

            Move(keys);

            tankPos.Y = map.GetHeight(tankPos).Y-0.3f;
            
            //calculo do vetor direção
            dir.X = (float)Math.Cos(yaw);
            dir.Z = -(float)Math.Sin(yaw);
            dir.Y =  0;

            //Console.WriteLine(dir);
            if ((tankPos.X < 0 || tankPos.Z < 0))
                tankPos = oldPos;
            if ((tankPos.X > 127 || tankPos.Z > 127))
                tankPos = oldPos;

        }

        public void Move(KeyboardState keys)
        {
            //TankMove
            if (keys.IsKeyDown(Keys.S))
            {
                tankPos.X += dir.X * speed;
                tankPos.Z += dir.Z * speed;
            }
            if (keys.IsKeyDown(Keys.W))
            {
                tankPos.X -= dir.X * speed;
                tankPos.Z -= dir.Z * speed;
            }
            if (keys.IsKeyDown(Keys.A))
            {
                yaw += 0.05f;
            }
            if (keys.IsKeyDown(Keys.D))
            {
                yaw -= 0.05f;
            }

            //Cannon Move
            if(keys.IsKeyDown(Keys.Up))
            {
                if(cannonPitch >= -MathHelper.PiOver2)
                cannonPitch -= 0.05f;
            }
            if(keys.IsKeyDown(Keys.Down))
            {
                if (cannonPitch < 0.6f)
                    cannonPitch += 0.05f;
            }
            if(keys.IsKeyDown(Keys.Left))
            {
                cannonYaw += 0.1f;
            }
            if (keys.IsKeyDown(Keys.Right))
            {
                cannonYaw -= 0.1f;
            }
        }

        public void Draw(GraphicsDevice device,ClsCamera camera)
        {
            Vector3 N = map.InterpolyNormals(tankPos);
            Vector3 right = Vector3.Cross(new Vector3(dir.X,0,dir.Z), N);

            Vector3 d = Vector3.Cross(N, right);
            r = Matrix.Identity;
            r.Forward = d;
            r.Up = N;
            r.Right = right;
         
            //r = Matrix.CreateFromYawPitchRoll(0f, yaw, MathHelper.ToRadians(90)) * r;

            tankModel.Root.Transform = Matrix.CreateScale(0.01f) * r * Matrix.CreateTranslation(tankPos);// Matrix.CreateRotationY(yaw + MathHelper.ToRadians(90));
            turretBone.Transform = Matrix.CreateRotationY(cannonYaw) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(cannonPitch) * cannonTransform;

            tankModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in tankModel.Meshes)
            {
                foreach(BasicEffect effect in  mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = camera.ViewMatrixCamera;
                    effect.Projection = camera.ProjectionMatrixCamera;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        private void DrawVectors(GraphicsDevice device, Vector3 startPoint, Vector3 endPoint, Color color,ClsCamera camera)
        {
            basicEffect = new BasicEffect(device);
            basicEffect.Projection = camera.ProjectionMatrixCamera;
            basicEffect.View = camera.ViewMatrixCamera;
            basicEffect.World = worldMatrix;
            basicEffect.VertexColorEnabled = true;
            basicEffect.CurrentTechnique.Passes[0].Apply();
            startPoint.Y += 4;
            endPoint.Y += 4;
            var vertices = new[] { new VertexPositionColor(startPoint, color), new VertexPositionColor(endPoint, color) };
            device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
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

        public Vector3 TankPosition
        {
            get
            {
                return tankPos;
            }
        }

        public Vector3 TankDirection
        {
            get
            {
                return dir;
            }
        }
    }
}
