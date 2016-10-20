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
        float maxHeight, maxWidht;

        public Mapa(GraphicsDevice device, ContentManager Content)
        {
            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;
            mapaImagem = Content.Load<Texture2D>("lh3d1");
            texture = Content.Load<Texture2D>("ground1");

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

            maxHeight = mapaImagem.Height;
            maxWidht = mapaImagem.Width;

            for(int z = 0;z<mapaImagem.Width;z++)
            {
                for(int x = 0;x<mapaImagem.Height;x++)
                {
                    vertices[x+z*mapaImagem.Width] = new VertexPositionColorTexture(new Vector3((float)x,(float)pixeis[x+z*mapaImagem.Width].R/255*10f, (float)z), pixeis[x + z * mapaImagem.Width], new Vector2(x%2,z%2));
                }
            }

            int contador = 0;
            for(int y = 0;y<mapaImagem.Height-1;y++)
            {
                for(int x = 0;x<mapaImagem.Width-1;x++)
                {
                    verIndex[contador] = (short)(x + y * mapaImagem.Width);
                    verIndex[contador + 1] = (short)(x + y * mapaImagem.Width + 1);
                    verIndex[contador+2] = (short)(x + (y + 1) * mapaImagem.Width);
                    verIndex[contador + 3] = (short)(x + y * mapaImagem.Width + 1);
                    verIndex[contador + 4] = (short)(x + (y + 1) * mapaImagem.Width + 1);
                    verIndex[contador + 5] = (short)(x + (y + 1) * mapaImagem.Width);
                    contador += 6;
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

            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, verIndex.Length / 3);
        }

        public float MapBoundariesHeight
        {
            get
            {
                return maxHeight;
            }
        }

        public float MapBoundariesWidth
        {
            get
            {
                return maxWidht;
            }
        }

        public VertexPositionColorTexture[] mapVertices
        {
            get
            {
                return vertices;
            }
        }
    }
}
