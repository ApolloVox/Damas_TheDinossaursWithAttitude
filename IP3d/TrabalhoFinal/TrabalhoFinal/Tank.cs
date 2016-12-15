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
        float yaw, speed = 0.05f, cannonYaw, cannonPitch, wheelRotation, steerRotation;
        Matrix[] boneTransforms;
        Model tankModel;
        ModelBone turretBone, cannonBone, rfWheelBone, lfWheelBone, rbWheelBone, lbWheelBone, leftSteer, rightSteer;
        BasicEffect effect;
        Mapa map;
        float scale = 0.001f, fireRate = 2f, fireTimer = 0f;
        List<Bullet> ammoList;
        ContentManager content;
        GraphicsDevice device;
        TankNumber player;

        ParticleSystem particleSystem;

        public Tank(GraphicsDevice device, ContentManager content, Mapa mapa, ClsCamera camera, int number)
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
                //tankPos = new Vector3(70f, 15f, 70f);
                tankPos = new Vector3(30f, 15f, 40f);

            dir = new Vector3(0, 0, 0);

            yaw = 0;
            cannonYaw = 0f;
            cannonPitch = 0f;
            wheelRotation = 0;
            steerRotation = 0;

            ammoList = new List<Bullet>();

            particleSystem = new ParticleSystem(device, camera, this,mapa);
        }

        public void Update(GameTime gameTime, Vector3 enemyPos, Model enemyModel, Matrix[] enemyWordlMatrix)
        {
            Vector3 oldPos = tankPos;
            float oldyaw = yaw;
            KeyboardState keys = Keyboard.GetState();

            if (player == TankNumber.Tank1)
                Move(keys);
            else if (player == TankNumber.Tank2)
                FollowPlayer(enemyPos, keys);

            tankPos.Y = map.GetHeight(tankPos).Y;

            //Lista que obtem informação sobre as balas de canhao
            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (player == TankNumber.Tank1)
                if (keys.IsKeyDown(Keys.Space) && fireTimer > fireRate)
                {
                    //creação de uma bala mas ainda sem movimento
                    ammoList.Add(new Bullet(device, content, boneTransforms[tankModel.Meshes["canon_geo"].ParentBone.Index].Translation, cannonYaw, cannonPitch, yaw));
                    fireTimer = 0;
                }

            //Impedir que saia do terreno de jogo
            if (player == TankNumber.Tank1)
            {
                if ((tankPos.X < 0 || tankPos.Z < 0) || CollisionBetweenTanks(enemyModel, enemyWordlMatrix))
                    tankPos = oldPos;
                if ((tankPos.X > 127 || tankPos.Z > 127) || CollisionBetweenTanks(enemyModel, enemyWordlMatrix))
                    tankPos = oldPos;
            }

            if (player == TankNumber.Tank2)
            {
                if ((tankPos.X < 0 || tankPos.Z < 0) || CollisionBetweenTanks(enemyModel, enemyWordlMatrix))
                    tankPos = oldPos;
                if ((tankPos.X > 127 || tankPos.Z > 127) || CollisionBetweenTanks(enemyModel, enemyWordlMatrix))
                    tankPos = oldPos;
                if ((tankPos - enemyPos).Length() < 5f)
                {
                    if (!AimCannon(enemyPos))
                        FollowPlayer(enemyPos, keys);
                    else
                    {
                        tankPos = oldPos;
                        yaw = oldyaw;
                    }
                }
            }

            if (tankPos != oldPos)
                particleSystem.Moving = true;
            else
                particleSystem.Moving = false;

            //calculo do vetor direção
            dir.X = (float)Math.Cos(yaw);
            dir.Z = (float)Math.Sin(yaw);
            dir.Y = 0;

            if (player == TankNumber.Tank1)
                foreach (Bullet bullet in ammoList)
                {
                    bullet.Update(map, enemyPos, enemyModel, enemyWordlMatrix);
                    if (bullet.CollisionHit)
                    {
                        particleSystem.AddParticlesExplosion(enemyWordlMatrix[enemyModel.Meshes[bullet.ModelHit].ParentBone.Index].Translation);
                    }
                    else if (bullet.Ground)
                    {
                        particleSystem.AddParticlesExplosion(map.GetHeight(bullet.BulletPos));
                    }
                }

            particleSystem.UpdateExplosion(gameTime);
            particleSystem.UpdatePoeira(gameTime, boneTransforms[tankModel.Meshes["l_back_wheel_geo"].ParentBone.Index].Translation);
            particleSystem.UpdatePoeira(gameTime, boneTransforms[tankModel.Meshes["r_back_wheel_geo"].ParentBone.Index].Translation);

        }

        private bool CollisionBetweenTanks(Model enemyModel, Matrix[] enemyMatrix)
        {
            for (int i = 0; i < tankModel.Meshes.Count; i++)
            {
                BoundingSphere sphere = tankModel.Meshes[i].BoundingSphere;
                Matrix spherePos = boneTransforms[tankModel.Meshes[i].ParentBone.Index];
                spherePos.Translation = tankPos;
                sphere = sphere.Transform(spherePos);

                for (int j = 0; j < enemyModel.Meshes.Count; j++)
                {
                    BoundingSphere eSphere = enemyModel.Meshes[i].BoundingSphere;
                    eSphere = sphere.Transform(enemyMatrix[i]);

                    if ((sphere.Center - eSphere.Center).Length() < sphere.Radius + sphere.Radius)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        public void Move(KeyboardState keys)
        {
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
                if (cannonYaw <= MathHelper.Pi / 3f + MathHelper.PiOver2)
                    cannonYaw += 0.01f;
            }
            if (keys.IsKeyDown(Keys.Right))
            {
                if (cannonYaw >= -MathHelper.Pi / 3f - MathHelper.PiOver2)
                    cannonYaw -= 0.01f;
            }

        }

        private void FollowPlayer(Vector3 tankTarget, KeyboardState keys)
        {
            Vector3 direction = tankPos - tankTarget;
            direction.Normalize();

            float angle = (float)Math.Acos(Vector2.Dot(new Vector2(dir.X, dir.Z), new Vector2(direction.X, direction.Z)));

            if ((dir.X - 0.05f < direction.X) && (dir.X + 0.05f > direction.X) && (dir.Z - 0.05f < direction.Z) && (dir.Z + 0.05f > direction.Z))
            {
                if (steerRotation > 0.1f)
                {
                    steerRotation -= 0.1f;
                    //yaw += MathHelper.ToRadians(steerRotation);
                }
                else if (steerRotation < -0.1f)
                {
                    steerRotation += 0.1f;
                }
                else
                {
                    yaw += MathHelper.ToRadians(steerRotation);
                }

                tankPos.X -= dir.X * speed;
                tankPos.Z -= dir.Z * speed;
            }
            else
                switch (CalculateSteerMoviment(angle, direction))
                {
                    case 1:
                        if (steerRotation <= 0.85f)
                            steerRotation += 0.03f;

                        tankPos.X -= dir.X * speed;
                        tankPos.Z -= dir.Z * speed;
                        yaw -= MathHelper.ToRadians(steerRotation);
                        break;

                    case -1:
                        if (steerRotation >= -0.85f)
                            steerRotation -= 0.03f;

                        tankPos.X -= dir.X * speed;
                        tankPos.Z -= dir.Z * speed;
                        yaw -= MathHelper.ToRadians(steerRotation);
                        break;

                    case 0:
                        tankPos.X -= dir.X * speed;
                        tankPos.Z -= dir.Z * speed;
                        yaw -= MathHelper.ToRadians(steerRotation);
                        break;
                }

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
        }

        private Boolean AimCannon(Vector3 enemyPosition)
        {
            Vector3 direction = tankPos - enemyPosition;
            direction.Y = 0;
            direction.Normalize();

            float angle = (float)Math.Acos(Vector2.Dot(new Vector2(dir.X, dir.Z), new Vector2(direction.X, direction.Z)));

            float yaw1 = cannonYaw + 0.01f;
            float yaw2 = cannonYaw - 0.01f;

            Vector3 test = new Vector3((float)Math.Cos(yaw - yaw1), 0, (float)Math.Sin(yaw - yaw1));
            Vector3 test2 = new Vector3((float)Math.Cos(yaw - yaw2), 0, (float)Math.Sin(yaw - yaw2));
            //Cosine_similarity
            // Se -1 Vector está oposto, 1 está em "frente" e 0 está perpendicular
            float cosino = Vector3.Dot(direction, test) / (direction.Length() * test.Length());
            float cosino2 = Vector3.Dot(direction, test2) / (direction.Length() * test2.Length());

            if (cosino > cosino2)
            {
                if (cannonYaw <= MathHelper.Pi / 3f + MathHelper.PiOver2)
                    cannonYaw += 0.01f;
                else
                    return false;
            }
            else if (cosino2 > cosino)
            {
                if (cannonYaw >= -MathHelper.Pi / 3f - MathHelper.PiOver2)
                    cannonYaw -= 0.01f;
                else
                    return false;
            }
            return true;
        }

        public int CalculateSteerMoviment(float angle, Vector3 direction)
        {
            float leftSteer = 0.06f + steerRotation, rightSteer = -0.06f + steerRotation;

            float leftYaw = yaw + MathHelper.ToRadians(leftSteer);
            float rightYaw = yaw + MathHelper.ToRadians(rightSteer);

            Vector2 testDir1 = new Vector2((float)Math.Cos(leftYaw), (float)Math.Sin(leftYaw));
            Vector2 testDir2 = new Vector2((float)Math.Cos(rightYaw), (float)Math.Sin(rightYaw));

            float testAngle1 = (float)Math.Acos(Vector2.Dot(new Vector2(testDir1.X, testDir1.Y), new Vector2(direction.X, direction.Z)));
            float testAngle2 = (float)Math.Acos(Vector2.Dot(new Vector2(testDir2.X, testDir2.Y), new Vector2(direction.X, direction.Z)));

            if (testAngle1 > testAngle2)
            {
                return 1;
            }
            if (testAngle2 > testAngle1)
            {
                return -1;
            }
            return 0;

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

            //desenho dos tanks
            //AmbienteLightColors diferentes só para mostrar qual o tank 1 e 2
            switch (player)
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

            foreach (Bullet bullet in ammoList)
            {
                bullet.Draw(device, camera, map);
            }

            for (int i = 0; i < ammoList.Count; i++)
            {
                if (!ammoList[i].Life)
                {
                    ammoList.RemoveAt(i);
                    i--;
                }
            }

            if (particleSystem.Moving)
            {
                particleSystem.Draw();
            }
            particleSystem.DrawExplosion();
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
