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
        Vector3 tankPos, dir;
        float yaw, speed = 0.1f, cannonYaw, cannonPitch, wheelRotation, steerRotation;
        Matrix[] boneTransforms;
        Model tankModel;
        ModelBone turretBone, cannonBone, rfWheelBone, lfWheelBone, rbWheelBone, lbWheelBone, leftSteer, rightSteer;
        BasicEffect effect;
        Mapa map;
        float scale = 0.001f,fireRate = 2f,fireTimer = 0f;
        List<Bullet> ammoList;
        ContentManager content;
        GraphicsDevice device;
        TankNumber player;

        ParticleSystem particleSystem;

        public Tank(GraphicsDevice device, ContentManager content, Mapa mapa,ClsCamera camera, int number)
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
            cannonYaw = 0f;
            cannonPitch = 0f;
            wheelRotation = 0;
            steerRotation = 0;

            ammoList = new List<Bullet>();

            particleSystem = new ParticleSystem(device, camera, this);
        }

        public void Update(GameTime gameTime,Vector3 enemyPos,Model enemyModel,Matrix[] enemyWordlMatrix)
        {
            Vector3 oldPos = tankPos;

            KeyboardState keys = Keyboard.GetState();

            Move(keys);

            tankPos.Y = map.GetHeight(tankPos).Y;

            //Lista que obtem informação sobre as balas de canhao
            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (player == TankNumber.Tank1)
                if (keys.IsKeyDown(Keys.Space) && fireTimer > fireRate)
                {
                    //creação de uma bala mas ainda sem movimento
                    ammoList.Add(new Bullet(device, content, tankPos, cannonYaw, cannonPitch,yaw));
                    fireTimer = 0;
                }

            //calculo do vetor direção
            dir.X = (float)Math.Cos(yaw);
            dir.Z = (float)Math.Sin(yaw);
            dir.Y = 0;

            //Impedir que saia do terreno de jogo
            if ((tankPos.X < 0 || tankPos.Z < 0))
                tankPos = oldPos;
            if ((tankPos.X > 127 || tankPos.Z > 127))
                tankPos = oldPos;

            if (tankPos != oldPos)
                particleSystem.Moving = true;
            else
                particleSystem.Moving = false;

            if(player == TankNumber.Tank1)
            foreach(Bullet bullet in ammoList)
            {
                    bullet.Update(map,enemyPos,enemyModel,enemyWordlMatrix);
            }

            particleSystem.Update(gameTime, boneTransforms[tankModel.Meshes["l_back_wheel_geo"].ParentBone.Index].Translation ,map);
            //WriteLine(Vector3.Transform(Vector3.Zero, Matrix.Invert(rbWheelTransform*turretTransform)));
        }

        public void Move(KeyboardState keys)
        {
            //Ve qual tank é que se mexe
            switch (player)
            {
                case TankNumber.Tank1:
                    //Movimento do tank de acorod com o yaw do steerRotation
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
                        if (cannonYaw <= MathHelper.Pi/3f+MathHelper.PiOver2)
                            cannonYaw += 0.01f;
                    }
                    if (keys.IsKeyDown(Keys.Right))
                    {
                        if (cannonYaw >= -MathHelper.Pi/3f-MathHelper.PiOver2)
                            cannonYaw -= 0.01f;
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
                    break;
            }

        }

        public void Draw(GraphicsDevice device, ClsCamera camera)
        {
            //Calculo da matriz rotação com a normal do terreno
            Vector3 N = map.InterpolyNormals(tankPos);
            Vector3 right = Vector3.Cross(new Vector3(dir.X, 0, dir.Z), N);
            Vector3 d = Vector3.Cross(N, right);

            r = Matrix.Identity;
            r.Forward = d;
            r.Up = N;
            r.Right = right;

            //Deslocaçoes dos varios bones do tank
            tankModel.Root.Transform = Matrix.CreateScale(scale) *r * Matrix.CreateTranslation(tankPos);
            turretBone.Transform = Matrix.CreateRotationY(cannonYaw) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(cannonPitch) * cannonTransform;
            rfWheelBone.Transform = Matrix.CreateRotationX(wheelRotation) * rfWheelTransform;
            rbWheelBone.Transform = Matrix.CreateRotationX(wheelRotation) * rbWheelTransform;
            lfWheelBone.Transform = Matrix.CreateRotationX(wheelRotation) * lfWheelTransform;
            lbWheelBone.Transform = Matrix.CreateRotationX(wheelRotation) * lbWheelTransform;
            rightSteer.Transform = Matrix.CreateRotationY(steerRotation * 0.5f) * rightSteerTransform;
            leftSteer.Transform = Matrix.CreateRotationY(steerRotation * 0.5f) * leftSteerTransform;

            tankModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            //desenho dos tanks
            //AmbienteLightColors diferentes só para mostrar qual o tank 1 e 2
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

            foreach(Bullet bullet in ammoList)
            {
                bullet.Draw(device,camera,cannonYaw,map);
            }


            if(particleSystem.Moving)
            {
                particleSystem.Draw();
            }
        }

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

        public Model TankModel()
        {
            return tankModel;
        }

        public Matrix[] WorldMatrix()
        {
            return boneTransforms;
        }
    }
}
