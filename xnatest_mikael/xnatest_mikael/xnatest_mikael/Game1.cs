//
// programeringstest 
// Mikael Bendiksen
//
/*
    Identity Matrix
    Scale Matrix
    Rotation Matrix
    Orbit Matrix
    Translate Matrix
    
    effect.World = I*S*R*O*T;
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace xnatest_mikael
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private GraphicsDevice device;      // Representerer tegneflata.

        private BasicEffect effect;

        // Liste med vertekser
        private VertexPositionNormalTexture[] cubeVertices;
        private VertexBuffer vertexBuffer;

        // WVP-matrisene
        private Matrix world;
        private Matrix projection;
        private Matrix view;

        private Vector3 cameraPosition = new Vector3(7.0f, 12.0f, 15.0f);
        private Vector3 cameraTarget = new Vector3(3, 0, 0);
        private Vector3 cameraUpVector = new Vector3(0.0f, 1.0f, 0.0f);

        private Texture2D vann;
        private Texture2D cube;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private void CreateFloor()
        {
            float w = 50;
            float h = 50;

            Vector3[] positions =
            {
                new Vector3(-w, -1, -h),
                new Vector3( w, -1, -h),
                new Vector3(-w, -1,  h),
                new Vector3( w, -1,  h)
            };

            Vector2[] texCoords =
            {
                new Vector2(0.0f, 0.0f),
                new Vector2(1, 0.0f),
                new Vector2(0.0f, 1),
                new Vector2(1, 1)
            };

            VertexPositionNormalTexture[] vertices =
            {
                new VertexPositionNormalTexture(
                    positions[0], Vector3.Up, texCoords[0]),
                new VertexPositionNormalTexture(
                    positions[1], Vector3.Up, texCoords[1]),
                new VertexPositionNormalTexture(
                    positions[2], Vector3.Up, texCoords[2]),
                new VertexPositionNormalTexture(
                    positions[3], Vector3.Up, texCoords[3]),
            };

            vertexBuffer = new VertexBuffer(GraphicsDevice,
                typeof(VertexPositionNormalTexture), vertices.Length,
                BufferUsage.WriteOnly);

            vertexBuffer.SetData(vertices);
        }

        private void DrawFloor()
        {
            CreateFloor();
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = false;

            effect.TextureEnabled = true;
            effect.Texture = vann;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

        }

        private void InitVerticesCube3()
        {
            cubeVertices = new VertexPositionNormalTexture[36];

            Vector3 topLeftFront = new Vector3(4.0f, 1.0f, 1.0f);
            Vector3 bottomLeftFront = new Vector3(4.0f, -1.0f, 1.0f);
            Vector3 topRightFront = new Vector3(6.0f, 1.0f, 1.0f);
            Vector3 bottomRightFront = new Vector3(6.0f, -1.0f, 1.0f);
            Vector3 topLeftBack = new Vector3(4.0f, 1.0f, -1.0f);
            Vector3 topRightBack = new Vector3(6.0f, 1.0f, -1.0f);
            Vector3 bottomLeftBack = new Vector3(4.0f, -1.0f, -1.0f);
            Vector3 bottomRightBack = new Vector3(6.0f, -1.0f, -1.0f);

            // skalering
            /*
            Vector3 fScale = new Vector3(2,2,2);
            topLeftFront = topLeftFront * fScale;
            bottomLeftFront = bottomLeftFront * fScale;
            topRightFront = topRightFront * fScale;
            bottomRightFront = bottomRightFront * fScale;
            topLeftBack = topLeftBack * fScale;
            topRightBack = topRightBack * fScale;
            bottomLeftBack = bottomLeftBack * fScale;
            bottomRightBack = bottomRightBack * fScale;
            */

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 backNormal = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

            Color frontColor = Color.Red;
            Color backColor = Color.Tomato;
            Color topColor = Color.Yellow;
            Color bottomColor = Color.SandyBrown;
            Color leftColor = Color.Pink;
            Color rightColor = Color.Gray;

            //Disse må endres for å teksturere forsiden:
            Vector2 texture00 = new Vector2(0, 0);
            Vector2 texture01 = new Vector2(0, 1);
            Vector2 texture10 = new Vector2(1, 0);
            Vector2 texture11 = new Vector2(1, 1);

            // Front face.
            cubeVertices[0] =
                new VertexPositionNormalTexture(
                    topLeftFront, frontNormal, texture00);

            cubeVertices[1] =
                new VertexPositionNormalTexture(
                    bottomLeftFront, frontNormal, texture01);

            cubeVertices[2] =
                new VertexPositionNormalTexture(
                    topRightFront, frontNormal, texture10);

            cubeVertices[3] =
                new VertexPositionNormalTexture(
                    bottomLeftFront, frontNormal, texture01);

            cubeVertices[4] =
                new VertexPositionNormalTexture(
                    bottomRightFront, frontNormal, texture11);

            cubeVertices[5] =
                new VertexPositionNormalTexture(
                    topRightFront, frontNormal, texture10);

            //"Nullstiller" slik at de andre sidene ikke blir teksturert:
            texture00 = new Vector2();
            texture01 = new Vector2();
            texture10 = new Vector2();
            texture11 = new Vector2();

            // Back face.
            cubeVertices[6] =
                new VertexPositionNormalTexture(
                    topLeftBack, backNormal, texture10);
            cubeVertices[7] =
                new VertexPositionNormalTexture(
                topRightBack, backNormal, texture00);
            cubeVertices[8] =
                new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, texture11);
            cubeVertices[9] =
                new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, texture11);
            cubeVertices[10] =
                new VertexPositionNormalTexture(
                topRightBack, backNormal, texture00);
            cubeVertices[11] =
                new VertexPositionNormalTexture(
                bottomRightBack, backNormal, texture01);

            // Top face.
            cubeVertices[12] =
                new VertexPositionNormalTexture(
                topLeftFront, topNormal, texture00);
            cubeVertices[13] =
                new VertexPositionNormalTexture(
                topRightBack, topNormal, texture01);
            cubeVertices[14] =
                new VertexPositionNormalTexture(
                topLeftBack, topNormal, texture10);
            cubeVertices[15] =
                new VertexPositionNormalTexture(
                topLeftFront, topNormal, texture01);
            cubeVertices[16] =
                new VertexPositionNormalTexture(
                topRightFront, topNormal, texture11);
            cubeVertices[17] =
                new VertexPositionNormalTexture(
                topRightBack, topNormal, texture10);

            // Bottom face. 
            cubeVertices[18] =
                new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, texture00);
            cubeVertices[19] =
                new VertexPositionNormalTexture(
                bottomLeftBack, bottomNormal, texture01);
            cubeVertices[20] =
                new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, texture10);
            cubeVertices[21] =
                new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, texture01);
            cubeVertices[22] =
                new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, texture11);
            cubeVertices[23] =
                new VertexPositionNormalTexture(
                bottomRightFront, bottomNormal, texture10);

            // Left face.
            cubeVertices[24] =
                new VertexPositionNormalTexture(
                topLeftFront, leftNormal, texture00);
            cubeVertices[25] =
                new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, texture01);
            cubeVertices[26] =
                new VertexPositionNormalTexture(
                bottomLeftFront, leftNormal, texture10);
            cubeVertices[27] =
                new VertexPositionNormalTexture(
                topLeftBack, leftNormal, texture01);
            cubeVertices[28] =
                new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, texture11);
            cubeVertices[29] =
                new VertexPositionNormalTexture(
                topLeftFront, leftNormal, texture10);

            // Right face. 
            cubeVertices[30] =
                new VertexPositionNormalTexture(
                topRightFront, rightNormal, texture00);
            cubeVertices[31] =
                new VertexPositionNormalTexture(
                bottomRightFront, rightNormal, texture01);
            cubeVertices[32] =
                new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, texture10);
            cubeVertices[33] =
                new VertexPositionNormalTexture(
                topRightBack, rightNormal, texture01);
            cubeVertices[34] =
                new VertexPositionNormalTexture(
                topRightFront, rightNormal, texture11);
            cubeVertices[35] =
                new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, texture10);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = cube;

            // Starter tegning - må bruke effect-objektet
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserPrimitives(PrimitiveType.TriangleList, cubeVertices, 0, 12, VertexPositionNormalTexture.VertexDeclaration);
            }

        }

        private void InitVerticesCube2()
        {
            cubeVertices = new VertexPositionNormalTexture[36];

            Vector3 topLeftFront = new Vector3(-1.0f, 3.0f, 1.0f);
            Vector3 bottomLeftFront = new Vector3(-1.0f, 1.0f, 1.0f);
            Vector3 topRightFront = new Vector3(1.0f, 3.0f, 1.0f);
            Vector3 bottomRightFront = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 topLeftBack = new Vector3(-1.0f, 3.0f, -1.0f);
            Vector3 topRightBack = new Vector3(1.0f, 3.0f, -1.0f);
            Vector3 bottomLeftBack = new Vector3(-1.0f, 1.0f, -1.0f);
            Vector3 bottomRightBack = new Vector3(1.0f, 1.0f, -1.0f);

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 backNormal = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

            Color frontColor = Color.Red;
            Color backColor = Color.Tomato;
            Color topColor = Color.Yellow;
            Color bottomColor = Color.SandyBrown;
            Color leftColor = Color.Pink;
            Color rightColor = Color.Gray;

            //Disse må endres for å teksturere forsiden:
            Vector2 texture00 = new Vector2(0, 0);
            Vector2 texture01 = new Vector2(0, 1);
            Vector2 texture10 = new Vector2(1, 0);
            Vector2 texture11 = new Vector2(1, 1);

            // Front face.
            cubeVertices[0] =
                new VertexPositionNormalTexture(
                    topLeftFront, frontNormal, texture00);

            cubeVertices[1] =
                new VertexPositionNormalTexture(
                    bottomLeftFront, frontNormal, texture01);

            cubeVertices[2] =
                new VertexPositionNormalTexture(
                    topRightFront, frontNormal, texture10);

            cubeVertices[3] =
                new VertexPositionNormalTexture(
                    bottomLeftFront, frontNormal, texture01);

            cubeVertices[4] =
                new VertexPositionNormalTexture(
                    bottomRightFront, frontNormal, texture11);

            cubeVertices[5] =
                new VertexPositionNormalTexture(
                    topRightFront, frontNormal, texture10);

            //"Nullstiller" slik at de andre sidene ikke blir teksturert:
            texture00 = new Vector2();
            texture01 = new Vector2();
            texture10 = new Vector2();
            texture11 = new Vector2();

            // Back face.
            cubeVertices[6] =
                new VertexPositionNormalTexture(
                    topLeftBack, backNormal, texture10);
            cubeVertices[7] =
                new VertexPositionNormalTexture(
                topRightBack, backNormal, texture00);
            cubeVertices[8] =
                new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, texture11);
            cubeVertices[9] =
                new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, texture11);
            cubeVertices[10] =
                new VertexPositionNormalTexture(
                topRightBack, backNormal, texture00);
            cubeVertices[11] =
                new VertexPositionNormalTexture(
                bottomRightBack, backNormal, texture01);

            // Top face.
            cubeVertices[12] =
                new VertexPositionNormalTexture(
                topLeftFront, topNormal, texture00);
            cubeVertices[13] =
                new VertexPositionNormalTexture(
                topRightBack, topNormal, texture01);
            cubeVertices[14] =
                new VertexPositionNormalTexture(
                topLeftBack, topNormal, texture10);
            cubeVertices[15] =
                new VertexPositionNormalTexture(
                topLeftFront, topNormal, texture01);
            cubeVertices[16] =
                new VertexPositionNormalTexture(
                topRightFront, topNormal, texture11);
            cubeVertices[17] =
                new VertexPositionNormalTexture(
                topRightBack, topNormal, texture10);

            // Bottom face. 
            cubeVertices[18] =
                new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, texture00);
            cubeVertices[19] =
                new VertexPositionNormalTexture(
                bottomLeftBack, bottomNormal, texture01);
            cubeVertices[20] =
                new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, texture10);
            cubeVertices[21] =
                new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, texture01);
            cubeVertices[22] =
                new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, texture11);
            cubeVertices[23] =
                new VertexPositionNormalTexture(
                bottomRightFront, bottomNormal, texture10);

            // Left face.
            cubeVertices[24] =
                new VertexPositionNormalTexture(
                topLeftFront, leftNormal, texture00);
            cubeVertices[25] =
                new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, texture01);
            cubeVertices[26] =
                new VertexPositionNormalTexture(
                bottomLeftFront, leftNormal, texture10);
            cubeVertices[27] =
                new VertexPositionNormalTexture(
                topLeftBack, leftNormal, texture01);
            cubeVertices[28] =
                new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, texture11);
            cubeVertices[29] =
                new VertexPositionNormalTexture(
                topLeftFront, leftNormal, texture10);

            // Right face. 
            cubeVertices[30] =
                new VertexPositionNormalTexture(
                topRightFront, rightNormal, texture00);
            cubeVertices[31] =
                new VertexPositionNormalTexture(
                bottomRightFront, rightNormal, texture01);
            cubeVertices[32] =
                new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, texture10);
            cubeVertices[33] =
                new VertexPositionNormalTexture(
                topRightBack, rightNormal, texture01);
            cubeVertices[34] =
                new VertexPositionNormalTexture(
                topRightFront, rightNormal, texture11);
            cubeVertices[35] =
                new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, texture10);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = cube;

            // Starter tegning - må bruke effect-objektet
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserPrimitives(PrimitiveType.TriangleList, cubeVertices, 0, 12, VertexPositionNormalTexture.VertexDeclaration);
            }

        }

        private void InitVerticesCube()
        {
            cubeVertices = new VertexPositionNormalTexture[36];

            Vector3 topLeftFront = new Vector3(-1.0f, 1.0f, 1.0f);
            Vector3 bottomLeftFront = new Vector3(-1.0f, -1.0f, 1.0f);
            Vector3 topRightFront = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 bottomRightFront = new Vector3(1.0f, -1.0f, 1.0f);
            Vector3 topLeftBack = new Vector3(-1.0f, 1.0f, -1.0f);
            Vector3 topRightBack = new Vector3(1.0f, 1.0f, -1.0f);
            Vector3 bottomLeftBack = new Vector3(-1.0f, -1.0f, -1.0f);
            Vector3 bottomRightBack = new Vector3(1.0f, -1.0f, -1.0f);

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 backNormal = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);

            Color frontColor = Color.Red;
            Color backColor = Color.Tomato;
            Color topColor = Color.Yellow;
            Color bottomColor = Color.SandyBrown;
            Color leftColor = Color.Pink;
            Color rightColor = Color.Gray;

            //Disse må endres for å teksturere forsiden:
            Vector2 texture00 = new Vector2(0,0);
            Vector2 texture01 = new Vector2(0,1);
            Vector2 texture10 = new Vector2(1,0);
            Vector2 texture11 = new Vector2(1,1);

            // Front face.
            cubeVertices[0] =
                new VertexPositionNormalTexture(
                    topLeftFront, frontNormal, texture00);

            cubeVertices[1] =
                new VertexPositionNormalTexture(
                    bottomLeftFront, frontNormal, texture01);

            cubeVertices[2] =
                new VertexPositionNormalTexture(
                    topRightFront, frontNormal, texture10);

            cubeVertices[3] =
                new VertexPositionNormalTexture(
                    bottomLeftFront, frontNormal, texture01);

            cubeVertices[4] =
                new VertexPositionNormalTexture(
                    bottomRightFront, frontNormal, texture11);

            cubeVertices[5] =
                new VertexPositionNormalTexture(
                    topRightFront, frontNormal, texture10);

            //"Nullstiller" slik at de andre sidene ikke blir teksturert:
            texture00 = new Vector2();
            texture01 = new Vector2();
            texture10 = new Vector2();
            texture11 = new Vector2();

            // Back face.
            cubeVertices[6] =
                new VertexPositionNormalTexture(
                    topLeftBack, backNormal, texture10);
            cubeVertices[7] =
                new VertexPositionNormalTexture(
                topRightBack, backNormal, texture00);
            cubeVertices[8] =
                new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, texture11);
            cubeVertices[9] =
                new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, texture11);
            cubeVertices[10] =
                new VertexPositionNormalTexture(
                topRightBack, backNormal, texture00);
            cubeVertices[11] =
                new VertexPositionNormalTexture(
                bottomRightBack, backNormal, texture01);

            // Top face.
            cubeVertices[12] =
                new VertexPositionNormalTexture(
                topLeftFront, topNormal, texture00);
            cubeVertices[13] =
                new VertexPositionNormalTexture(
                topRightBack, topNormal, texture01);
            cubeVertices[14] =
                new VertexPositionNormalTexture(
                topLeftBack, topNormal, texture10);
            cubeVertices[15] =
                new VertexPositionNormalTexture(
                topLeftFront, topNormal, texture01);
            cubeVertices[16] =
                new VertexPositionNormalTexture(
                topRightFront, topNormal, texture11);
            cubeVertices[17] =
                new VertexPositionNormalTexture(
                topRightBack, topNormal, texture10);

            // Bottom face. 
            cubeVertices[18] =
                new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, texture00);
            cubeVertices[19] =
                new VertexPositionNormalTexture(
                bottomLeftBack, bottomNormal, texture01);
            cubeVertices[20] =
                new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, texture10);
            cubeVertices[21] =
                new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, texture01);
            cubeVertices[22] =
                new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, texture11);
            cubeVertices[23] =
                new VertexPositionNormalTexture(
                bottomRightFront, bottomNormal, texture10);

            // Left face.
            cubeVertices[24] =
                new VertexPositionNormalTexture(
                topLeftFront, leftNormal, texture00);
            cubeVertices[25] =
                new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, texture01);
            cubeVertices[26] =
                new VertexPositionNormalTexture(
                bottomLeftFront, leftNormal, texture10);
            cubeVertices[27] =
                new VertexPositionNormalTexture(
                topLeftBack, leftNormal, texture01);
            cubeVertices[28] =
                new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, texture11);
            cubeVertices[29] =
                new VertexPositionNormalTexture(
                topLeftFront, leftNormal, texture10);

            // Right face. 
            cubeVertices[30] =
                new VertexPositionNormalTexture(
                topRightFront, rightNormal, texture00);
            cubeVertices[31] =
                new VertexPositionNormalTexture(
                bottomRightFront, rightNormal, texture01);
            cubeVertices[32] =
                new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, texture10);
            cubeVertices[33] =
                new VertexPositionNormalTexture(
                topRightBack, rightNormal, texture01);
            cubeVertices[34] =
                new VertexPositionNormalTexture(
                topRightFront, rightNormal, texture11);
            cubeVertices[35] =
                new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, texture10);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = cube;

            // Starter tegning - må bruke effect-objektet
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserPrimitives(PrimitiveType.TriangleList, cubeVertices, 0, 12, VertexPositionNormalTexture.VertexDeclaration);
            }

        }

        private void InitDevice()
        {
            device = graphics.GraphicsDevice;

            // Setter størrelse på framebuffer:
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 900;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Window.Title = "Kuben - Mikael";

            // Initialiserer Effect-objektet:
            effect = new BasicEffect(graphics.GraphicsDevice);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
        }

        private void InitCamera()
        {
            

            // Projeksjon
            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;

            //Oppretter view-matrisa
            Matrix.CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out view);

            // Oppretter projeksjonsmatrisa
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.01f, 1000.0f, out projection);

            // Gir matrisene til shader
            effect.Projection = projection;
            effect.View = view;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            LoadContent();
            InitDevice();
            InitCamera();
            
        }

       
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            vann = Content.Load<Texture2D>(@"tex_water");
            cube = Content.Load<Texture2D>(@"boximg");

            

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            RasterizerState rasterizerState1 = new RasterizerState();
            rasterizerState1.CullMode = CullMode.None;
            //rasterizerState1.FillMode = FillMode.WireFrame; // for å se kun streker av trekanten
            rasterizerState1.FillMode = FillMode.Solid;
            device.RasterizerState = rasterizerState1;

            //device.Clear(Color.Black);
            device.BlendState = BlendState.AlphaBlend; 

            // Setter world
            world = Matrix.Identity;
            // Setter world-matrisa på effect-objektet (verteks-shaderen)
            effect.World = world;

            //effectTexture.SetValue(vann); 

            DrawFloor();
            InitVerticesCube();
            InitVerticesCube2();
            InitVerticesCube3();

            base.Draw(gameTime);
        }
    }
}
