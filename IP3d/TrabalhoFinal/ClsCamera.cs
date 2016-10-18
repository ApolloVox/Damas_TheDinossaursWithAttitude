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
        float scale = MathHelper.ToRadians(10) / 50;
        MouseState oldState,mouse;

        public ClsCamera(GraphicsDevice device, Vector3 startPos)
        {
            width = device.Viewport.Width;
            height = device.Viewport.Height;
            yaw = 0;
            pitch = 0;
            position = new Vector3(8.0f,1.0f, 8.0f);
            dir = Vector3.Zero - position;
            effect = new BasicEffect(device);
            float aspectRatio = (float)(width /
                height);
            viewMatrix = Matrix.CreateLookAt(
                position,
                dir,
                Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians((float)Math.PI),
                aspectRatio, 0.5f, 100.0f);

            effect.World = Matrix.Identity;
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            oldState = Mouse.GetState();
        }


        public void Update(GameTime gametime)
        {
            KeyboardState keys = Keyboard.GetState();
            mouse = Mouse.GetState();
            Vector2 mousePos;
            float oldyaw = yaw;

            if (keys.IsKeyDown(Keys.A))
            {
                position.X += 0.2f;
            }
            if (keys.IsKeyDown(Keys.D))
            {
                position.X -= 0.2f;
            }
            if (keys.IsKeyDown(Keys.W))
            {
                position.Z += 0.2f;
            }
            if (keys.IsKeyDown(Keys.S))
            {
                position.Z -= 0.2f;
            }
           if (oldState.ScrollWheelValue > mouse.ScrollWheelValue)
                position.Y += 0.1f;
            if (oldState.ScrollWheelValue < mouse.ScrollWheelValue)
                position.Y -= 0.1f;

            mousePos.X = mouse.X;
            mousePos.Y = mouse.Y;

            mousePos.X -= width / 2;
            mousePos.Y -= height / 2;

            if (mousePos.X != 0)
            {
                yaw += mousePos.X * scale;
                oldyaw += yaw;
                dir.X += (float)Math.Cos(yaw);
                dir.Z += (float)Math.Sin(yaw);
                /*Console.WriteLine(Math.Cos(yaw));
                Console.WriteLine(Math.Sin(yaw));*/
                Console.WriteLine(yaw);
                Console.WriteLine(oldyaw);
                Console.WriteLine(mousePos.X);
                Console.WriteLine(mousePos.Y);
            }

            if (mousePos.Y != 0)
            {
                pitch = mousePos.Y * scale;
                dir.Y += pitch;
            }


            //dir.Normalize();
            //position *= dir;
            viewMatrix = Matrix.CreateLookAt(position, dir, Vector3.Up);
            Mouse.SetPosition((int)(width / 2), (int)(height / 2));
            oldState = mouse;
            //Console.WriteLine(dir);
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
    }
}
