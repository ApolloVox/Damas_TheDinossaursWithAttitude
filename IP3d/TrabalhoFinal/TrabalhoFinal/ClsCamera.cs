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

        public ClsCamera(GraphicsDevice device, Vector3 startPos)
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
                aspectRatio, 1f, 10.0f);

            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            oldState = Mouse.GetState();
        }


        public void Update(GameTime gametime)
        {
            KeyboardState keys = Keyboard.GetState();
            mouse = Mouse.GetState();
            Vector2 mousePos;

            if (keys.IsKeyDown(Keys.S))
            {
                position.X -= 0.2f;
            }
            if (keys.IsKeyDown(Keys.W))
            {
                position.X += 0.2f;
            }
            if (keys.IsKeyDown(Keys.A))
            {
                position.Z -= 0.2f;
            }
            if (keys.IsKeyDown(Keys.D))
            {
                position.Z += 0.2f;
            }
           if (oldState.ScrollWheelValue > mouse.ScrollWheelValue)
                position.Y -= 0.1f;
            if (oldState.ScrollWheelValue < mouse.ScrollWheelValue)
                position.Y += 0.1f;

            mousePos.X = mouse.X;
            mousePos.Y = mouse.Y;

            mousePos.X -= width / 2;
            mousePos.Y -= height / 2;

            /*yaw += mousePos.X * scale;
            angleX = (float)Math.Cos(yaw);
            angleZ = -(float)Math.Sin(yaw);
            dir.X = (angleX) + position.X;
            dir.Z = (angleZ) + position.Z;

             pitch += mousePos.Y * scale;
             dir.Y = (float)Math.Sin(pitch) + position.Y;*/

            yaw -= mousePos.X * scale;

            pitch = MathHelper.Clamp(pitch + mousePos.Y * scale, -1.5f, 1.5f);

            dir.X = (float)Math.Cos(yaw) * (float)Math.Cos(pitch) + position.X ;
            dir.Z = -(float)Math.Sin(yaw) * (float)Math.Cos(pitch) + position.Z;
            //dir.Y = MathHelper.Clamp((float)Math.Sin(pitch) + position.Y,position.Y-0.95f,position.Y+0.95f);
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
    }
}
