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
    //Para saber qual tank estamos a tratar
    enum TankNumber
    {
        Tank1,
        Tank2
    }
    class Tank
    {
        Matrix worldMatrix, viewMatrix, projectionMatrix;
        Matrix cannonTransform, turretTransform, r, rfWheelTransform, rbWheelTransform, lfWheelTransform, lbWheelTransform, leftSteerTransform, rightSteerTransform;
        Vector3 tankPos, dir, bulletPos;
        float yaw, pitch, speed, cannonYaw, cannonPitch, wheelRotation, steerRotation;
        Matrix[] boneTransforms;
        Model tankModel;
        ModelBone turretBone, cannonBone, rfWheelBone, lfWheelBone, rbWheelBone, lbWheelBone, leftSteer, rightSteer;
        BasicEffect effect;
        Mapa map;
        BasicEffect basicEffect;
        float scale = 0.001f;
        List<Bullet> ammoList;
        ContentManager content;
        GraphicsDevice device;
        TankNumber player;

        public Tank(GraphicsDevice device, ContentManager content, Mapa mapa, int number)
        {
            effect = new BasicEffect(device);

            if (number == 1)
                player = TankNumber.Tank1;
            else
                player = TankNumber.Tank2;

            worldMatrix = Matrix.Identity;
            viewMatrix = Matrix.Identity;
            projectionMatrix = Matrix.Identity;

            this.content = content;
            this.device = device;

            tankModel = content.Load<Model>("tank");
            turretBone = tankModel.Bones["turret_geo"];
            cannonBone = tankModel.Bones["canon_geo"];
            rfWheelBone = tankModel.Bones["r_front_wheel_geo"];
            rbWheelBone = tankModel.Bones["r_back_wheel_geo"];
            lfWheelBone = tankModel.Bones["l_front_wheel_geo"];
            lbWheelBone = tankModel.Bones["l_back_wheel_geo"];
            leftSteer = tankModel.Bones["l_steer_geo"];
            rightSteer = tankModel.Bones["r_steer_geo"];

            turretTransform = turretBone.Transform;
            cannonTransform = cannonBone.Transform;
            rfWheelTransform = rfWheelBone.Transform;
            rbWheelTransform = rbWheelBone.Transform;
            lfWheelTransform = lfWheelBone.Transform;
            lbWheelTransform = lbWheelBone.Transform;
            leftSteerTransform = leftSteer.Transform;
            rightSteerTransform = rightSteer.Transform;

            boneTransforms = new Matrix[tankModel.Bones.Count];

            map = mapa;

            if (player == TankNumber.Tank1)
                tankPos = new Vector3(20f, 15f, 20f);
            else
                tankPos = new Vector3(70f, 15f, 70f);

            dir = new Vector3(0, 0, 0);

            yaw = 0;
            pitch = 0;
            speed = 0.1f;
            cannonYaw = 0f;
            cannonPitch = 0f;
            wheelRotation = 0;
            steerRotation = 0;

            ammoList = new List<Bullet>();
        }

        public void Update()
        {
            Vector3 oldPos = tankPos;

            KeyboardState keys = Keyboard.GetState();

            Move(keys);

            tankPos.Y = map.GetHeight(tankPos).Y;

            //Lista que obtem informação sobre as balas de canhao
            if (keys.IsKeyDown(Keys.Space))
            {
                ammoList.Add(new Bullet(device, content, tankPos));
            }

            //calculo do vetor direção
            dir.X = (float)Math.Cos(yaw);
            dir.Z = (float)Math.Sin(yaw);
            dir.Y = 0;

            if ((tankPos.X < 0 || tankPos.Z < 0))
                tankPos = oldPos;
            if ((tankPos.X > 127 || tankPos.Z > 127))
                tankPos = oldPos;

        }

        public void Move(KeyboardState keys)
        {
            //Ve qual tank é que se mexe
            switch (player)
            {
                case TankNumber.Tank1:
                    if (keys.IsKeyDown(Keys.S))
                    {
                        tankPos.X += dir.X * speed;
                        tankPos.Z += dir.Z * speed;
                        wheelRotation -= 0.05f;
                        yaw += MathHelper.ToRadians(steerRotation);
                    }
                    if (keys.IsKeyDown(Keys.W))
                    {
                        tankPos.X -= dir.X * speed;
                        tankPos.Z -= dir.Z * speed;
                        wheelRotation += 0.05f;
                        yaw -= MathHelper.ToRadians(steerRotation);
                    }
                    if (keys.IsKeyDown(Keys.A))
                    {
                        if (steerRotation <= 0.85f)
                            steerRotation += 0.03f;
                    }
                    if (keys.IsKeyDown(Keys.D))
                    {
                        if (steerRotation >= -0.85f)
                            steerRotation -= 0.03f;
                    }

                    //Cannon Move
                    if (keys.IsKeyDown(Keys.Up))
                    {
                        if (cannonPitch >= -MathHelper.PiOver2)
                            cannonPitch -= 0.05f;
                    }
                    if (keys.IsKeyDown(Keys.Down))
                    {
                        if (cannonPitch < 0.6f)
                            cannonPitch += 0.05f;
                    }
                    if (keys.IsKeyDown(Keys.Left))
                    {
                        cannonYaw += 0.1f;
                    }
                    if (keys.IsKeyDown(Keys.Right))
                    {
                        cannonYaw -= 0.1f;
                    }
                    break;

                case TankNumber.Tank2:
                    if (keys.IsKeyDown(Keys.K))
                    {
                        tankPos.X += dir.X * speed;
                        tankPos.Z += dir.Z * speed;
                        wheelRotation -= 0.05f;
                        yaw += MathHelper.ToRadians(steerRotation);
                    }
                    if (keys.IsKeyDown(Keys.I))
                    {
                        tankPos.X -= dir.X * speed;
                        tankPos.Z -= dir.Z * speed;
                        wheelRotation += 0.05f;
                        yaw -= MathHelper.ToRadians(steerRotation);
                    }
                    if (keys.IsKeyDown(Keys.J))
                    {
                        if (steerRotation <= 0.85f)
                            steerRotation += 0.03f;
                    }
                    if (keys.IsKeyDown(Keys.L))
                    {
                        if (steerRotation >= -0.85f)
                            steerRotation -= 0.03f;
                    }

                    //Cannon Move
                    if (keys.IsKeyDown(Keys.Up))
                    {
                        if (cannonPitch >= -MathHelper.PiOver2)
                            cannonPitch -= 0.05f;
                    }
                    if (keys.IsKeyDown(Keys.Down))
                    {
                        if (cannonPitch < 0.6f)
                            cannonPitch += 0.05f;
                    }
                    if (keys.IsKeyDown(Keys.Left))
                    {
                        cannonYaw += 0.1f;
                    }
                    if (keys.IsKeyDown(Keys.Right))
                    {
                        cannonYaw -= 0.1f;
                    }
                    break;
            }

        }

        public void Draw(GraphicsDevice device, ClsCamera camera)
        {
            Vector3 N = map.InterpolyNormals(tankPos);
            Vector3 right = Vector3.Cross(new Vector3(dir.X, 0, dir.Z), N);
            Vector3 d = Vector3.Cross(N, right);

            r = Matrix.Identity;
            r.Forward = d;
            r.Up = N;
            r.Right = right;

            tankModel.Root.Transform = Matrix.CreateScale(scale) * r * Matrix.CreateTranslation(tankPos);
            turretBone.Transform = Matrix.CreateRotationY(cannonYaw) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(cannonPitch) * cannonTransform;
            rfWheelBone.Transform = Matrix.CreateRotationX(wheelRotation) * rfWheelTransform;
            rbWheelBone.Transform = Matrix.CreateRotationX(wheelRotation) * rbWheelTransform;
            lfWheelBone.Transform = Matrix.CreateRotationX(wheelRotation) * lfWheelTransform;
            lbWheelBone.Transform = Matrix.CreateRotationX(wheelRotation) * lbWheelTransform;
            rightSteer.Transform = Matrix.CreateRotationY(steerRotation * 0.5f) * rightSteerTransform;
            leftSteer.Transform = Matrix.CreateRotationY(steerRotation * 0.5f) * leftSteerTransform;

            tankModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            switch(player)
            {
                case TankNumber.Tank1:
                    foreach (ModelMesh mesh in tankModel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.World = boneTransforms[mesh.ParentBone.Index];
                            effect.View = camera.ViewMatrixCamera;
                            effect.Projection = camera.ProjectionMatrixCamera;
                            effect.AmbientLightColor = new Vector3(0.6f, 0.1f, 0.5f);

                            effect.EnableDefaultLighting();
                        }
                        mesh.Draw();
                    }
                    break;
                case TankNumber.Tank2:
                    foreach (ModelMesh mesh in tankModel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.World = boneTransforms[mesh.ParentBone.Index];
                            effect.View = camera.ViewMatrixCamera;
                            effect.Projection = camera.ProjectionMatrixCamera;
                            effect.AmbientLightColor = new Vector3(0.1f, 0.9f, 1f);

                        }
                        mesh.Draw();
                    }
                    break;
            }       
        }

        /*private void DrawVectors(GraphicsDevice device, Vector3 startPoint, Vector3 endPoint, Color color, ClsCamera camera)
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
        }*/

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
