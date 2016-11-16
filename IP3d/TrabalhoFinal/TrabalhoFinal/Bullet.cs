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
        Vector3 position;
        Model bullet;

        public Bullet(GraphicsDevice device,ContentManager content, Vector3 startPos)
        {
            bullet = content.Load<Model>("projektil FBX");
            position = startPos;
            
            Console.WriteLine(position);
        }

        public void Draw(ClsCamera camera)
        {
            foreach (ModelMesh mesh in bullet.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(0.03f) * Matrix.CreateTranslation(position) * Matrix.Identity;
                    effect.View = camera.ViewMatrixCamera;
                    effect.Projection = camera.ProjectionMatrixCamera;
                }
                mesh.Draw();
            }
        }
    }
}
