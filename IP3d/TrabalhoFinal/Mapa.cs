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
    class Mapa
    {
        BasicEffect effect;
        Texture2D mapaImagem,texture;
        Matrix worldMatrix,viewMatrix;
        Color[] pixeis;

        VertexPositionColorTexture[] vertices;
        short[] verIndex;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        public Mapa(GraphicsDevice device, ContentManager Content)
        {
            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;
            mapaImagem = Content.Load<Texture2D>("lh3d1");
            texture = Content.Load<Texture2D>("sandTexture");

            ReadPixeis();

            float aspectRatio = (float)(device.Viewport.Width /
                device.Viewport.Height);

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                aspectRatio, 1.0f, 10.0f);

            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;

            effect.TextureEnabled = true;
            effect.Texture = texture;

            CreateMap();

            vertexBuffer = new VertexBuffer(device,
                typeof(VertexPositionColorTexture),
                vertices.Length,
                BufferUsage.None);

            vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

            indexBuffer = new IndexBuffer(device,
                typeof(short),
                verIndex.Length,
                BufferUsage.None);

            indexBuffer.SetData(verIndex);
        }

        private void ReadPixeis()
        {
            pixeis = new Color[mapaImagem.Width * mapaImagem.Height];
            mapaImagem.GetData<Color>(pixeis);
        }

        private void CreateMap()
        {
            verIndex = new short[6 * (mapaImagem.Width - 1) * (mapaImagem.Height - 1)];
            vertices = new VertexPositionColorTexture[mapaImagem.Width * mapaImagem.Height];

            for(int x = 0;x<mapaImagem.Width;x++)
            {
                for(int z = 0;z<mapaImagem.Height;z++)
                {
                    if(x %2 == 0 && z % 2 == 0)
                        vertices[x+z*mapaImagem.Width] = new VertexPositionColorTexture(new Vector3((float)x/(float)10,(float)pixeis[x+z*mapaImagem.Width].R/255, (float)z / (float)10), pixeis[x + z * mapaImagem.Width], new Vector2(0,0));
                    else if(x%2 != 0 && z % 2 == 0)
                        vertices[x + z * mapaImagem.Width] = new VertexPositionColorTexture(new Vector3((float)x / (float)10, (float)pixeis[x + z * mapaImagem.Width].R / 255, (float)z / (float)10), pixeis[x + z * mapaImagem.Width], new Vector2(1, 0));
                    else if(x % 2 == 0 && z % 2 !=0)
                        vertices[x + z * mapaImagem.Width] = new VertexPositionColorTexture(new Vector3((float)x / (float)10, (float)pixeis[x + z * mapaImagem.Width].R / 255, (float)z / (float)10), pixeis[x + z * mapaImagem.Width], new Vector2(0,1));
                    else if(x %2 != 0 && z % 2 !=0)
                        vertices[x + z * mapaImagem.Width] = new VertexPositionColorTexture(new Vector3((float)x / (float)10, (float)pixeis[x + z * mapaImagem.Width].R / 255, (float)z / (float)10), pixeis[x + z * mapaImagem.Width], new Vector2(1 ,1));
                }
            }

            int number = 0;
            for(int y = 0;y<mapaImagem.Height-1;y++)
            {
                for(int x = 0;x<mapaImagem.Width-1;x++)
                {
                    verIndex[number] = (short)(x + y * mapaImagem.Width);
                    verIndex[number + 1] = (short)(x + y * mapaImagem.Width + 1);
                    verIndex[number+2] = (short)(x + (y + 1) * mapaImagem.Width);
                    verIndex[number + 3] = (short)(x + y * mapaImagem.Width + 1);
                    verIndex[number + 4] = (short)(x + (y + 1) * mapaImagem.Width + 1);
                    verIndex[number + 5] = (short)(x + (y + 1) * mapaImagem.Width);
                    number += 6;
                }
            }
        }

        public void Draw(GraphicsDevice device,ClsCamera camera)
        {
            effect.World = worldMatrix;
            effect.View = camera.ViewMatrixCamera;
            effect.Projection = camera.ProjectionMatrixCamera;

            effect.CurrentTechnique.Passes[0].Apply();

            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;

            device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length, verIndex, 0, verIndex.Length/3);
        }
    }
}
