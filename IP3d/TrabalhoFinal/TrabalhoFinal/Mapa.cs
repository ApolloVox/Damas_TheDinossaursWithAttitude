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
        GraphicsDevice device;

        VertexPositionNormalTexture[] vertices;
        short[] verIndex;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        float maxHeight, maxWidht;

        public Mapa(GraphicsDevice device, ContentManager Content)
        {
            this.device = device;
            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;
            mapaImagem = Content.Load<Texture2D>("lh3d1");
            texture = Content.Load<Texture2D>("ground1");

            ReadPixeis();

            effect.VertexColorEnabled = false;

            effect.TextureEnabled = true;
            effect.Texture = texture;

            //LIGHTS
            //effect.EnableDefaultLighting();
            effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3(0.78f,0.43f, 0f);
            effect.DirectionalLight0.Direction = new Vector3(1f, -1f, 0);
            effect.DirectionalLight0.SpecularColor = new Vector3(0, 0.025f, 0);
            effect.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);
            effect.EmissiveColor = new Vector3(0f, 0f, 0f);

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

        public void FogOn()
        {
            //FOG
            effect.FogEnabled = true;
            effect.FogColor = new Color(180f, 180f, 180f).ToVector3(); // For best results, ake this color whatever your background is.
            effect.FogStart = 10f;
            effect.FogEnd = 60f;
        }

        public void FogOff()
        {
            effect.FogEnabled = false;
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
        private void GetNormals()
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
                        normais[contador] = Vector3.Subtract( vertices[x - 1 + (z - 1) * (int)maxHeight].Position,vertices[x + z * (int)maxHeight].Position);
                        contador++;
                    }
                    //Center Up
                    if (z - 1 >= 0)
                    {
                        normais[contador] = Vector3.Subtract( vertices[x + (z - 1) * (int)maxHeight].Position , vertices[x + z * (int)maxHeight].Position);
                        contador++;
                    }
                    //Right Up
                    if (x + 1 < maxHeight && z - 1 >= 0)
                    {
                        normais[contador] = Vector3.Subtract( vertices[x + 1 + (z - 1) * (int)maxHeight].Position , vertices[x + z * (int)maxHeight].Position);
                        contador++;
                    }
                    //Right
                    if (x + 1 < maxHeight)
                    {
                        normais[contador] = Vector3.Subtract(vertices[x + 1 + z * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                        contador++;
                    }
                    //Down Right
                    if (x + 1 < mapaImagem.Width && z + 1 < maxHeight)
                    {
                        normais[contador] = Vector3.Subtract(vertices[x + 1 + (z + 1) * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                        contador++;
                    }
                    //Down Center
                    if (z + 1 < maxHeight)
                    {
                        normais[contador] = Vector3.Subtract(vertices[x + (z + 1) * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                        contador++;
                    }
                    //Down Left
                    if (x - 1 >= 0 && z + 1 < maxHeight)
                    {
                        normais[contador] = Vector3.Subtract(vertices[x - 1 + (z + 1) * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                        contador++;
                    }
                    //Left
                    if (x - 1 >= 0)
                    {
                        normais[contador] = Vector3.Subtract(vertices[x - 1 + z * (int)maxHeight].Position , vertices[x + z * (int)maxHeight].Position);
                        contador++;
                    }

                    //Cálculo final das normais
                    for(int i = contador-1;i>=1;i--)
                    {
                        normais[i] = -Vector3.Cross(normais[i-1], normais[i]);

                    }

                    //Obtenção da media das normais e atribuindo ao vertice central
                    // a normal correspondente
                    Vector3 media = Vector3.Zero;
                    for(int i = 0;i<contador;i++)
                    {
                        media += normais[i];
                    }

                    media /= (float)contador;
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

        //Interpolação das normais do terreno
        public Vector3 InterpolyNormals(Vector3 position)
        {
            Vector3 verticeA, verticeB, verticeC, verticeD;
            Vector3 verAN, verBN, verCN, verDN;

            //Obtem os vertices adjacentes a posiçaõ tal como as normais
            if ((int)(position.X) + (int)(position.Z + 1) * (int)MapBoundariesHeight < MapBoundariesHeight * MapBoundariesWidth && (int)(position.X) + (int)(position.Z + 1) * (int)MapBoundariesHeight > 0)
            {
                verticeA = mapVertices[(int)(position.X) + (int)position.Z * (int)MapBoundariesHeight].Position;
                verticeB = mapVertices[(int)(position.X + 1) + (int)position.Z * (int)MapBoundariesHeight].Position;
                verticeC = mapVertices[(int)(position.X) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Position;
                verticeD = mapVertices[(int)(position.X + 1) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Position;

                verAN = mapVertices[(int)(position.X) + (int)position.Z * (int)MapBoundariesHeight].Normal;
                verBN = mapVertices[(int)(position.X + 1) + (int)position.Z * (int)MapBoundariesHeight].Normal;
                verCN = mapVertices[(int)(position.X) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Normal;
                verDN = mapVertices[(int)(position.X + 1) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Normal;
            }
            else
            {
                verticeA = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Position;
                verticeB = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Position;
                verticeC = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Position;
                verticeD = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Position;

                verAN = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Normal;
                verBN = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Normal;
                verCN = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Normal;
                verDN = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Normal;
            }

            //calculo do peso da posição com os vertices adjacentes
            //e depois calculo final da normal
            Vector3 Dab = (1 - (position.X-verticeA.X)) * verAN + (position.X-verticeA.X) * verBN;
            Vector3 Dcd = (1 - (position.X - verticeC.X)) * verCN + (position.X - verticeC.X) * verDN;

            Vector3 N = (1 - (position.Z - verticeA.Z)) * Dab + (position.Z - verticeA.Z) * Dcd;
  
            return N;
        }

        public Vector3 GetHeight(Vector3 position)
        {
            Vector3 verticeA, verticeB, verticeC, verticeD;

            //Obtem os vertices adjacentes a camera
            if ((int)(position.X) + (int)(position.Z + 1) * (int)MapBoundariesHeight < MapBoundariesHeight * MapBoundariesWidth && (int)(position.X) + (int)(position.Z + 1) * (int)MapBoundariesHeight > 0)
            {
                verticeA = mapVertices[(int)(position.X) + (int)position.Z * (int)MapBoundariesHeight].Position;
                verticeB = mapVertices[(int)(position.X + 1) + (int)position.Z * (int)MapBoundariesHeight].Position;
                verticeC = mapVertices[(int)(position.X) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Position;
                verticeD = mapVertices[(int)(position.X + 1) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Position;
            }
            else
            {
                verticeA = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Position;
                verticeB = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Position;
                verticeC = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Position;
                verticeD = mapVertices[(int)MapBoundariesWidth * (int)MapBoundariesHeight - 1].Position;
            }

            //interpolação das alturas para à medida que se anda com a camera o movimento ser fluido
            //interpolação feita com o peso que cada vertice da à camera
            float Ya, Yb, Yc, Yd;
            Ya = verticeA.Y;
            Yb = verticeB.Y;
            Yc = verticeC.Y;
            Yd = verticeD.Y;

            float Yab = (1 - (position.X - verticeA.X)) * Ya + (position.X - verticeA.X) * Yb;
            float Ycd = (1 - (position.X - verticeC.X)) * Yc + (position.X - verticeC.X) * Yd;
            float Y = (1 - (position.Z - verticeA.Z)) * Yab + (position.Z - verticeA.Z) * Ycd;

            position.Y = Y;

            return position;
        }
        
        //Após contacto com a bala, descer a altura do terreno mas
        //só nos vertices afectados
        public void RecalculateHeight(Vector3 position)
        {
            Vector3[] verticesNewNormals = new Vector3[16];
            if(mapVertices[(int)(position.X) + (int)position.Z * (int)MapBoundariesHeight].Position.Y - 0.5f > 0)
                mapVertices[(int)(position.X) + (int)position.Z * (int)MapBoundariesHeight].Position.Y -= 0.5f;
            if (mapVertices[(int)(position.X + 1) + (int)position.Z * (int)MapBoundariesHeight].Position.Y - 0.5f > 0)
                mapVertices[(int)(position.X + 1) + (int)position.Z * (int)MapBoundariesHeight].Position.Y -= 0.5f;
            if (mapVertices[(int)(position.X) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Position.Y - 0.5f > 0)
                mapVertices[(int)(position.X) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Position.Y -= 0.5f;
            if (mapVertices[(int)(position.X + 1) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Position.Y - 0.5f > 0)
                mapVertices[(int)(position.X + 1) + (int)(position.Z + 1) * (int)MapBoundariesHeight].Position.Y -= 0.5f;

            verticesNewNormals[0] = mapVertices[(int)(position.X-1) + (int)(position.Z-1) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[1] = mapVertices[(int)(position.X) + (int)(position.Z - 1) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[2] = mapVertices[(int)(position.X+1) + (int)(position.Z - 1) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[3] = mapVertices[(int)(position.X+2) + (int)(position.Z - 1) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[4] = mapVertices[(int)(position.X - 1) + (int)(position.Z) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[5] = mapVertices[(int)(position.X) + (int)(position.Z) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[6] = mapVertices[(int)(position.X + 1) + (int)(position.Z) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[7] = mapVertices[(int)(position.X + 2) + (int)(position.Z) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[8] = mapVertices[(int)(position.X - 1) + (int)(position.Z+1) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[9] = mapVertices[(int)(position.X) + (int)(position.Z+1) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[10] = mapVertices[(int)(position.X + 1) + (int)(position.Z+1) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[11] = mapVertices[(int)(position.X + 2) + (int)(position.Z+1) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[12] = mapVertices[(int)(position.X - 1) + (int)(position.Z + 2) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[13] = mapVertices[(int)(position.X) + (int)(position.Z + 2) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[14] = mapVertices[(int)(position.X + 1) + (int)(position.Z + 2) * (int)MapBoundariesHeight].Position;
            verticesNewNormals[15] = mapVertices[(int)(position.X + 2) + (int)(position.Z + 2) * (int)MapBoundariesHeight].Position;


            RecalculateNormals(verticesNewNormals);

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

        //Recalcular as normais dos vertices onde a altura foi afectada
        private void RecalculateNormals(Vector3[] vectorsNormals)
        {
            for(int j = 0;j<vectorsNormals.Length;j++)
            {
                //Produto externo (V0 - p) * (V1-p)
                Vector3[] normais = new Vector3[8];
                int contador = 0;
                int x = (int)vectorsNormals[j].X;
                int z = (int)vectorsNormals[j].Z;

                //Left Up
                if (x - 1 >= 0 && z - 1 >= 0)
                {
                    normais[contador] = Vector3.Subtract(vertices[x - 1 + (z - 1) * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                    contador++;
                }
                //Center Up
                if (z - 1 >= 0)
                {
                    normais[contador] = Vector3.Subtract(vertices[x + (z - 1) * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                    contador++;
                }
                //Right Up
                if (x + 1 < maxHeight && z - 1 >= 0)
                {
                    normais[contador] = Vector3.Subtract(vertices[x + 1 + (z - 1) * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                    contador++;
                }
                //Right
                if (x + 1 < maxHeight)
                {
                    normais[contador] = Vector3.Subtract(vertices[x + 1 + z * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                    contador++;
                }
                //Down Right
                if (x + 1 < mapaImagem.Width && z + 1 < maxHeight)
                {
                    normais[contador] = Vector3.Subtract(vertices[x + 1 + (z + 1) * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                    contador++;
                }
                //Down Center
                if (z + 1 < maxHeight)
                {
                    normais[contador] = Vector3.Subtract(vertices[x + (z + 1) * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                    contador++;
                }
                //Down Left
                if (x - 1 >= 0 && z + 1 < maxHeight)
                {
                    normais[contador] = Vector3.Subtract(vertices[x - 1 + (z + 1) * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                    contador++;
                }
                //Left
                if (x - 1 >= 0)
                {
                    normais[contador] = Vector3.Subtract(vertices[x - 1 + z * (int)maxHeight].Position, vertices[x + z * (int)maxHeight].Position);
                    contador++;
                }

                //Cálculo final das normais
                for (int i = contador - 1; i >= 1; i--)
                {
                    normais[i] = -Vector3.Cross(normais[i - 1], normais[i]);

                }

                //Obtenção da media das normais e atribuindo ao vertice central
                // a normal correspondente
                Vector3 media = Vector3.Zero;
                for (int i = 0; i < contador; i++)
                {
                    media += normais[i];
                }

                media /= (float)contador;
                media.Normalize();
                vertices[x + z * (int)maxHeight].Normal = media;
            }
        }
    }
}
