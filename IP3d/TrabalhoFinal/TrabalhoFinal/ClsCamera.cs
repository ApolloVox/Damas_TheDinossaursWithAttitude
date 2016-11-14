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
    enum CameraSelect
    {
        FreeCam,
        Surface,
        Follow
    };

    class ClsCamera
    {
        private Matrix viewMatrix, projectionMatrix;
        private BasicEffect effect;
        Vector3 position, dir;
        float yaw, pitch;
        float width, height;
        float scale = MathHelper.ToRadians(10) /500;
        MouseState mouse;
        Mapa map;
        CameraSelect camState;

        public ClsCamera(GraphicsDevice device, Vector3 startPos,Mapa map)
        {
            camState = CameraSelect.Surface;

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
                aspectRatio, 0.5f, 100.0f);

            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;

            this.map = map;
        }


        public void Update(GameTime gametime)
        {
            Vector3 oldPos = position;
            float speed = 0.5f;

            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.F1))
            {
                Console.WriteLine("FreeCam");
                camState = CameraSelect.FreeCam;
            }
            else if (keys.IsKeyDown(Keys.F2))
            {
                Console.WriteLine("Surface");
                camState = CameraSelect.Surface;
            }
            else if (keys.IsKeyDown(Keys.F3))
            {
                Console.WriteLine("Follow");
                camState = CameraSelect.Follow;
            }
            else if( keys.IsKeyDown(Keys.F4))
            {
                Console.WriteLine("FogOn!");
                map.FogOn();
            }
            else if(keys.IsKeyDown(Keys.F5))
            {
                Console.WriteLine("FogOFF!");
                map.FogOff();
            }

            switch (camState)
            {
                case CameraSelect.FreeCam:

                    yawPitchCalc();

                    MoveFps(keys,speed);

                    //verificação da camera se passa limites do campo e caso passe atriu a posição antiga
                    if ((position.X < 0 || position.Z < 0))
                        position = oldPos;
                    if ((position.X > 127 || position.Z > 127))
                        position = oldPos;

                    //calculo do vetor direção
                    dir.X = (float)Math.Cos(yaw) * (float)Math.Cos(pitch) + position.X;
                    dir.Z = -(float)Math.Sin(yaw) * (float)Math.Cos(pitch) + position.Z;
                    dir.Y = (float)Math.Sin(pitch) + position.Y;
                    break;

                case CameraSelect.Surface:

                    yawPitchCalc();

                    HeightY();

                    MoveSurface(keys,speed);

                    //verificação da camera se passa limites do campo e caso passe atriu a posição antiga
                    if ((position.X < 0 || position.Z < 0))
                        position = oldPos;
                    if ((position.X > 127 || position.Z > 127))
                        position = oldPos;

                    //calculo do vetor direção
                    dir.X = (float)Math.Cos(yaw) * (float)Math.Cos(pitch) + position.X;
                    dir.Z = -(float)Math.Sin(yaw) * (float)Math.Cos(pitch) + position.Z;
                    dir.Y = (float)Math.Sin(pitch) + position.Y;

                    break;

                case CameraSelect.Follow:
                    MoveFollow();

                    break;
            }
           
            //actualizar viewMatriz da camera
            viewMatrix = Matrix.CreateLookAt(position, dir, Vector3.Up);
            Mouse.SetPosition((int)(width / 2), (int)(height / 2));
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

        //Cálculo do yaw e pitch atráves da deslocação do rato do meio do ecra
        // ate a posição final
        private void yawPitchCalc()
        {
            mouse = Mouse.GetState();
            Vector2 mousePos;

            mousePos.X = mouse.X;
            mousePos.Y = mouse.Y;

            mousePos.X -= width / 2;
            mousePos.Y -= height / 2;

            yaw -= mousePos.X * scale;

            pitch = MathHelper.Clamp(pitch + mousePos.Y * scale, -1.5f, 1.5f);
        }

        //função com o movimento Surface
        private void MoveSurface(KeyboardState keys, float speed)
        {

            if (keys.IsKeyDown(Keys.NumPad5))
            {
                position.X -= (dir.X - position.X) * speed;
                position.Z -= (dir.Z - position.Z) * speed;

            }
            if (keys.IsKeyDown(Keys.NumPad8))
            {
                position.X += (dir.X - position.X) * speed;
                position.Z += (dir.Z - position.Z) * speed;
            }

            if (keys.IsKeyDown(Keys.NumPad4))
            {
                position -= speed * Vector3.Cross(dir - position, Vector3.Up);
            }
            if (keys.IsKeyDown(Keys.NumPad6))
            {
                position += speed * Vector3.Cross(dir - position, Vector3.Up);
            }
        }

        //função com o movimento Free
        private void MoveFps(KeyboardState keys, float speed)
        {

            if (keys.IsKeyDown(Keys.NumPad5))
            {
                position.X -= (dir.X - position.X) * speed;
                position.Z -= (dir.Z - position.Z) * speed;
                position.Y -= (dir.Y - position.Y) * speed;

            }
            if (keys.IsKeyDown(Keys.NumPad8))
            {
                position.X += (dir.X - position.X) * speed;
                position.Z += (dir.Z - position.Z) * speed;
                position.Y += (dir.Y - position.Y) * speed;
            }

            if (keys.IsKeyDown(Keys.NumPad4))
            {
                position -= speed * Vector3.Cross(dir - position, Vector3.Up);
            }
            if (keys.IsKeyDown(Keys.NumPad6))
            {
                position += speed * Vector3.Cross(dir - position, Vector3.Up);
            }
        }

        private void MoveFollow()
        {
            
        }

        //Função que cálcula a altura do mapa e assim atribui essa altura a posição da camera
        private void HeightY()
        {
            position.Y = map.GetHeight(position).Y + 3f;
        }
    }
}
