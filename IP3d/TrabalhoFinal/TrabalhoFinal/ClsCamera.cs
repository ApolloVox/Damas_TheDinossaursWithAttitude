using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoFinal
{
    class ClsCamera
    {
        private Matrix viewMatrix, projectionMatrix;
        private BasicEffect effect;
        Vector3 position, dir;
        float yaw, pitch;
        float width, height;
        float scale = MathHelper.ToRadians(10) /500;
        MouseState oldState,mouse;
        Mapa map;

        public ClsCamera(GraphicsDevice device, Vector3 startPos,Mapa map)
        {
            width = device.Viewport.Width;
            height = device.Viewport.Height;
            yaw = 0;
            pitch = 0;
            position = new Vector3(8.0f,5.0f, 8.0f);
            dir = Vector3.Zero - position;
            dir.Normalize();
            effect = new BasicEffect(device);
            float aspectRatio = (float)(width /
                height);
            viewMatrix = Matrix.CreateLookAt(
                position,
                dir,
                Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                aspectRatio, 0.5f, 50.0f);

            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            oldState = Mouse.GetState();

            this.map = map;
        }


        public void Update(GameTime gametime)
        {
            KeyboardState keys = Keyboard.GetState();
            mouse = Mouse.GetState();
            Vector2 mousePos;

            if (keys.IsKeyDown(Keys.NumPad5))
            {
                if (position.X - (float)gametime.ElapsedGameTime.TotalSeconds * 2 > 0)
                    position.X = MathHelper.Clamp(position.X - (float)gametime.ElapsedGameTime.TotalSeconds * 2f, 0, map.MapBoundariesWidth-1);
                else
                    position.X = 0;
            }
            if (keys.IsKeyDown(Keys.NumPad8))
            {
                if (position.X + (float)gametime.ElapsedGameTime.TotalSeconds * 2 < 128)
                    position.X = MathHelper.Clamp(position.X + (float)gametime.ElapsedGameTime.TotalSeconds * 2f, 0, map.MapBoundariesWidth-1);
                else
                    position.X = 127;
            }
            if (keys.IsKeyDown(Keys.NumPad4))
            {
                if (position.Z - (float)gametime.ElapsedGameTime.TotalSeconds * 2 > 0)
                    position.Z = MathHelper.Clamp(position.Z - (float)gametime.ElapsedGameTime.TotalSeconds * 2f, 0, map.MapBoundariesHeight-1);
                else
                    position.Z = 0;
            }
            if (keys.IsKeyDown(Keys.NumPad6))
            {
                  position.Z = MathHelper.Clamp(position.Z + (float)gametime.ElapsedGameTime.TotalSeconds * 2f, 0, map.MapBoundariesHeight-1);
            }

           if (oldState.ScrollWheelValue > mouse.ScrollWheelValue)
                position.Y -= 0.1f;
            if (oldState.ScrollWheelValue < mouse.ScrollWheelValue)
                position.Y += 0.1f;

            mousePos.X = mouse.X;
            mousePos.Y = mouse.Y;

            mousePos.X -= width / 2;
            mousePos.Y -= height / 2;

            yaw -= mousePos.X * scale;

            pitch = MathHelper.Clamp(pitch + mousePos.Y * scale, -1.5f, 1.5f);

            //getMapY();
            HeightY();

            dir.X = (float)Math.Cos(yaw) * (float)Math.Cos(pitch) + position.X ;
            dir.Z = -(float)Math.Sin(yaw) * (float)Math.Cos(pitch) + position.Z;
            dir.Y = (float)Math.Sin(pitch) + position.Y;

            viewMatrix = Matrix.CreateLookAt(position, dir, Vector3.Up);
            Mouse.SetPosition((int)(width / 2), (int)(height / 2));
            oldState = mouse;
        }

        public Matrix ViewMatrixCamera
        {
            get
            {
                return viewMatrix;
            }
        }

        public Matrix ProjectionMatrixCamera
        {
            get
            {
                return projectionMatrix;
            }
        }

        public void getMapY()
        {
            /*Vector3 verticeA, verticeB, verticeC, verticeD;

            /*verticeA = map.mapVertices[(int)(position.X) + (int)position.Z * (int)map.MapBoundariesHeight].Position;
            verticeB = map.mapVertices[(int)(position.X*+1) + (int)position.Z * (int)map.MapBoundariesHeight-1].Position;
            verticeC = map.mapVertices[(int)(position.X) + (int)(position.Z) * (int)map.MapBoundariesHeight-1].Position;
            verticeD = map.mapVertices[(int)(position.X) + (int)(position.Z) * (int)map.MapBoundariesHeight+1-1].Position;
            verticeA = map.mapVertices[(int)position.X * (int)position.Z].Position;
            verticeB = map.mapVertices[(int)(position.X) * (int)position.Z+1].Position;
            verticeC = map.mapVertices[(int)position.X * (int)(position.Z)+128].Position;
            verticeD = map.mapVertices[(int)position.X * (int)position.Z + 129].Position;
            Console.WriteLine("A:" + verticeA);
            /*Console.WriteLine("B:" + verticeB);
            Console.WriteLine("C:" + verticeC);
            Console.WriteLine("D:" + verticeD);
            Console.WriteLine(position.X);
            Console.WriteLine(position.Z);
            //Console.WriteLine(position);
            /*Console.WriteLine("A:" + verticeA);
            Console.WriteLine("B:" + verticeB);
            Console.WriteLine("C:" + verticeC);
            Console.WriteLine("D:" + verticeD);
            /*Console.WriteLine("X:"+position.X/10f);
            Console.WriteLine("Z:" + position.Z/10f);

            float da = position.X - verticeA.X;
            float db = 1f - da;

            float dc = position.X - verticeC.X;
            float dd = 1f - dc;

            float yab = db * verticeA.Y + da * verticeB.Y;
            float ycd = dd * verticeC.Y + dc * verticeD.Y;


            /*Console.WriteLine("A:" + verticeA);
            Console.WriteLine("B:" + verticeB);
            Console.WriteLine("C:" + verticeC);
            Console.WriteLine("D:" + verticeD);
            Console.WriteLine("POS:" + position);
            Console.WriteLine(yab);
            Console.WriteLine(ycd);
            position.Y = yab + ycd;
            Console.WriteLine(yab);
            Console.WriteLine(ycd);
            //Console.WriteLine(position);
            /*Console.WriteLine(yab);
            Console.WriteLine(ycd);
            Console.WriteLine(yac);
            Console.WriteLine(ybd);*/

            /*position.Y = yab *ycd;
            Console.WriteLine(position.Y);*/
            //Console.WriteLine(position.Y);
            //Console.WriteLine(position.Y);*/
        }

        public void HeightY()
        {
            Vector3 verticeA,verticeB,verticeC,verticeD;

            if ((int)(position.X) + (int)(position.Z + 1) * (int)map.MapBoundariesHeight < map.MapBoundariesHeight * map.MapBoundariesWidth)
            {
                verticeA = map.mapVertices[(int)(position.X) + (int)position.Z * (int)map.MapBoundariesHeight].Position;
                verticeB = map.mapVertices[(int)(position.X + 1) + (int)position.Z * (int)map.MapBoundariesHeight].Position;
                verticeC = map.mapVertices[(int)(position.X) + (int)(position.Z + 1) * (int)map.MapBoundariesHeight].Position;
                verticeD = map.mapVertices[(int)(position.X) + (int)(position.Z + 1) * (int)map.MapBoundariesHeight].Position;
            }
            else
            {
                verticeA = map.mapVertices[(int)map.MapBoundariesWidth * (int)map.MapBoundariesHeight-1].Position;
                verticeB = map.mapVertices[(int)map.MapBoundariesWidth * (int)map.MapBoundariesHeight - 1].Position;
                verticeC = map.mapVertices[(int)map.MapBoundariesWidth * (int)map.MapBoundariesHeight - 1].Position;
                verticeD = map.mapVertices[(int)map.MapBoundariesWidth * (int)map.MapBoundariesHeight - 1].Position;
            }

            float Ya, Yb, Yc, Yd;
            Ya = verticeA.Y;
            Yb = verticeB.Y;
            Yc = verticeC.Y;
            Yd = verticeD.Y;

            /*float yac = dcaZ * verticeA.Y + dacz * verticeC.Y;
            float ybd = ddbx * verticeB.Y + dbdx * verticeD.Y;*/
            /*Console.WriteLine(yab);
            Console.WriteLine(ycd);
            Console.WriteLine(verticeA.Y);*/
            /* Console.WriteLine(yac);
             Console.WriteLine(ybd);
             Console.WriteLine("T:"+(yab+ycd+yac+ybd)/4f);*/

            float Yab = (1 - (position.X - verticeA.X)) * Ya + (position.X - verticeA.X) * Yb;
            float Ycd = (1 - (position.X - verticeC.X)) * Yc + (position.X - verticeC.X) * Yd;
            float Y = (1 - (position.Z - verticeA.Z)) * Yab + (position.Z - verticeA.Z) * Ycd;

            position.Y = Y + 2;
            Console.WriteLine(position.Y);

            /*float dx = 1 - (position.X - (int)position.X);
            float dz = 1 - (position.Z - (int)position.Z);

            position.Y = verticeA.Y * dx * dz + verticeB.Y * dz * (1 - dx) + verticeC.Y * dx * (1 - dz) + verticeD.Y * (1 - dx) * (1 - dz);*/
        }
    }
}

    /*public float retCameraHeight(Vector3 P)
        {
            float UpLeft = HeightData[(int)P.X, (int)P.Z];
            float UpRight = HeightData[(int)P.X + 1, (int)P.Z];
            float BotLeft = HeightData[(int)P.X, (int)P.Z + 1];
            float BotRight = HeightData[(int)P.X + 1, (int)P.Z + 1];

            float dX = 1 - (P.X - (int)P.X);
            float dY = 1 - (P.Z - (int)P.Z);

            return UpLeft * dX * dY + UpRight * dY * (1 - dX) + BotLeft * dX * (1 - dY) + BotRight * (1 - dX) * (1 - dY);
        }*/
