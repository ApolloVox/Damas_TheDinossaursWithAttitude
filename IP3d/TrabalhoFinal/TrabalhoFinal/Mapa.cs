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
        Matrix worldMatrix;
        Color[] pixeis;

        VertexPositionNormalTexture[] vertices;
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
            effect.EnableDefaultLighting();
            /*effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f); // a red light
            //effect.DirectionalLight0.Direction = new Vector3(0.5f, 0.5f, 0.5f);  // coming along the x-axis
            effect.DirectionalLight0.SpecularColor = new Vector3(0.5f, 0.5f,0.5f); // with green highlights
            effect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f); // a red light
            //effect.DirectionalLight1.Direction = new Vector3(0.5f, 0.5f, 0.5f);  // coming along the x-axis
            effect.DirectionalLight1.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f); // with green highlights
            effect.DirectionalLight2.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f); // a red light
           // effect.DirectionalLight2.Direction = new Vector3(0.5f, 0.5f, 0.5f);  // coming along the x-axis
            effect.DirectionalLight2.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f); // with green highlights
            effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
            effect.EmissiveColor = new Vector3(1, 0.2f, 0.2f);*/

            CreateMap();

            vertexBuffer = new VertexBuffer(device,
                typeof(VertexPositionNormalTexture),
                vertices.Length,
                BufferUsage.None);

            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);

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

        //Cria o mapa atráves de indices e de triangle list
        private void CreateMap()
        {
            verIndex = new short[6 * (mapaImagem.Width - 1) * (mapaImagem.Height - 1)];
            vertices = new VertexPositionNormalTexture[mapaImagem.Width * mapaImagem.Height];

            maxHeight = mapaImagem.Height;
            maxWidht = mapaImagem.Width;

            for(int z = 0;z<mapaImagem.Width;z++)
            {
                for(int x = 0;x<mapaImagem.Height;x++)
                {
                    vertices[x+z*mapaImagem.Width] = new VertexPositionNormalTexture(new Vector3((float)x,(float)pixeis[x+z*mapaImagem.Width].R/255*10f, (float)z), Vector3.Up, new Vector2(x%2,z%2));
                }
            }

            GetNormals();

            int contador = 0;

            //Indices calculados 6 a 6 de modo que cada ciclo seja um "quadrado" da textura
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

        //Método que contém as normais
        public void GetNormals()
        {
            for(int z= 0;z < mapaImagem.Width;z++)
            {
                for(int x = 0;x<MapBoundariesHeight;x++)
                {
                    //Produto externo (V0 - p) * (V1-p)
                    Vector3[] normais = new Vector3[8];
                    int contador = 0;
                    
                    //Left Up
                    if (x - 1 >= 0 && z - 1 >= 0)
                    {
                        normais[contador] = vertices[x - 1 + (z - 1) * (int)maxHeight].Position;
                        contador++;
                    }
                    //Center Up
                    if (z - 1 >= 0)
                    {
                        normais[contador] = vertices[x + (z - 1) * (int)maxHeight].Position;
                        contador++;
                    }
                    //Right Up
                    if (x + 1 < maxHeight && z - 1 >= 0)
                    {
                        normais[contador] = vertices[x + 1 + (z - 1) * (int)maxHeight].Position;
                        contador++;
                    }
                    //Left
                    if (x - 1 >= 0)
                    {
                        normais[contador] = vertices[x - 1 + z * (int)maxHeight].Position;
                        contador++;
                    }
                    //Right
                    if (x + 1 < maxHeight)
                    {
                        normais[contador] = vertices[x + 1 + z*(int)maxHeight].Position;
                        contador++;
                    }
                    //Down Left
                    if (x - 1 >= 0 && z + 1 < maxHeight)
                    {
                        normais[contador] = vertices[x - 1 + (z + 1) * (int)maxHeight].Position;
                        contador++;
                    }
                    //Down Center
                    if (z + 1 < maxHeight)
                    {
                        normais[contador] = vertices[x + (z + 1) * (int)maxHeight].Position;
                        contador++;
                    }
                    //Down Right
                    if (x + 1 < mapaImagem.Width && z + 1 < maxHeight)
                    {
                        normais[contador] = vertices[x + 1 + (z + 1) * (int)maxHeight].Position;
                        contador++;
                    }

                    //Cálculo Da Distãncia do vários pontos ao vertice central
                    for (int i = contador-1; i >= 0; i--)
                    {
                        normais[i] = normais[i] - vertices[x + z * (int)maxHeight].Position;
                    }

                    //Cálculo final das normais
                    for(int i = contador-1;i>= 1;i--)
                    {
                        normais[i] = Vector3.Cross(normais[i], normais[i-1]);
                    }

                    //Obtenção da media das normais e atribuindo ao vertice central
                    // a normal correspondente
                    Vector3 media = Vector3.Zero;
                    for(int i = 0;i<contador;i++)
                    {
                        media += normais[i];
                    }

                    media /= contador;
                    media.Normalize();
                    vertices[x + z * (int)maxHeight].Normal = media;

                }
            }
        }

        //Metodo draw com triangle list
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

        public VertexPositionNormalTexture[] mapVertices
        {
            get
            {
                return vertices;
            }
        }
    }
}
