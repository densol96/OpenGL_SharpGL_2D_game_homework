using SharpGL.SceneGraph.Assets;
using SharpGL;
using System;
using System.Collections.Generic;

namespace MD1_Solovjovs
{
    public enum Side
    {
        TOP, BOTTOM, LEFT, RIGHT
    }

    public enum Defence
    {
        YELLOW, RED, BLUE, PURPLE
    }

    public class Enemy
    {
        private static Random random = new Random();
        private static bool textureIsLoaded = false;
        private static readonly Texture texture = new Texture();
        private static readonly string pathToTexture = "enemy.png";
        // Once again, could be adjustable but egh :/
        private static readonly float HALF_SIZE = 0.75f;
        public static readonly float MAX_X = 8f;
        public static readonly float MAX_Y = 4.5f;

        public float POS_X { get; set; }
        public float POS_Y { get; set; }
        private int POS_Z = 0;

        private float speed = 0.01f;
        private int difficultyLevel = 1;
        
        private List<Defence> defences = new List<Defence>();

        private OpenGL gl;

        public bool IsBoss { get; } = false;
        public bool IsReadyToJoin { get; set; } = false;

        public Enemy(OpenGL gl, float? x, float? y, int difficultyLevel, bool readyToJoingFight = false, bool isBoss = false, float speed = 0.004f)
        {
            this.gl = gl;
            this.difficultyLevel = difficultyLevel;
            this.speed = MakeSpeedMoreRandom(( speed <= 0 || speed >= 0.05f) ? 0.004f : speed);
            IsBoss = isBoss;
            IsReadyToJoin = readyToJoingFight;
            GenerateDefenceCombos();
            if (!textureIsLoaded)
            {
                texture.Create(gl, pathToTexture);
                textureIsLoaded = true;
            }
            if(x == null || y == null)
            {
                RandomlySelectPosition();
            } else
            {
                // Initially, when the game starts, I want them to start coming from all 4 sides
                // Therefore, initial 4 enemies, I will hard code their initial coords
                // After that, they will appear randomly
                this.POS_X = (float)x;
                this.POS_Y = (float)y;
            }
        }

        private float MakeSpeedMoreRandom(float speed)
        {
            // Want to make speed slightly more random with difficulty level + to make them come at different speed
            float minSpeed = speed;
            float maxSpeed = minSpeed + difficultyLevel * 0.015f;
            float randomSpeed = (float)(random.NextDouble() * (maxSpeed - minSpeed) + minSpeed);
            return randomSpeed;
        }

        private void RandomlySelectPosition()
        {
            Array allSideOptions = Enum.GetValues(typeof(Side));
            Side randomLevel = (Side)allSideOptions.GetValue(random.Next(allSideOptions.Length));
            float randomX;
            float randomY;
            switch (randomLevel)
            {
                case Side.TOP:
                    randomY = MAX_Y;
                    randomX = (float)(random.NextDouble() * (2 * MAX_X) - MAX_X);
                    break;
                case Side.BOTTOM:
                    randomY = -MAX_Y;
                    randomX = (float)(random.NextDouble() * (2 * MAX_X) - MAX_X);
                    break;
                case Side.LEFT:
                    randomX = -MAX_X;
                    randomY = (float)(random.NextDouble() * (2 * MAX_Y) - MAX_Y);
                    break;
                case Side.RIGHT:
                    randomX = MAX_X;
                    randomY = (float)(random.NextDouble() * (2 * MAX_Y) - MAX_Y);
                    break;
                default:
                    randomX = 8;
                    randomY = 0;
                    break;
            }
            this.POS_X = randomX;
            this.POS_Y = randomY;
        }

        private void GenerateDefenceCombos()
        {
            for (int i = 0; i < difficultyLevel; i++)
            {
                Array allColorOptions = Enum.GetValues(typeof(Defence));
                Defence randomDefence = (Defence)allColorOptions.GetValue(random.Next(allColorOptions.Length));
                defences.Add(randomDefence);
            }
        }
        public void Render(int difficultyLevel = 1)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            texture.Bind(gl);

            gl.PushMatrix();
            gl.Translate(POS_X, POS_Y, POS_Z);
            gl.Begin(OpenGL.GL_QUADS);
            // BL
            gl.TexCoord(1, 1);
            gl.Vertex(-HALF_SIZE, -HALF_SIZE, 0);
            gl.TexCoord(1, 0);
            gl.Vertex(-HALF_SIZE, HALF_SIZE, 0);
            gl.TexCoord(0, 0);
            gl.Vertex(HALF_SIZE, HALF_SIZE, 0);
            gl.TexCoord(0, 1);
            gl.Vertex(HALF_SIZE, -HALF_SIZE, 0);
            gl.End();
            gl.PopMatrix();
            gl.Disable(OpenGL.GL_BLEND);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            RenderCombinations();
            MoveForfard();
        }

        private void MoveForfard()
        {
            // Looked this formula up on the Web
            float deltaX = 0 - POS_X;
            float deltaY = 0 - POS_Y;
            float length = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            float normalizedX = deltaX / length;
            float normalizedY = deltaY / length;

            float moveX = normalizedX * speed;
            float moveY = normalizedY * speed;
            POS_X += moveX;
            POS_Y += moveY;
        }

        private void RenderCombinations()
        {
            const float COMBO_HALF_SIZE = 0.1f;
            const float LITTLE_SPACE = 0.2f;
            
            for(int i = 0; i < defences.Count; i++)
            {
                gl.PushMatrix();
                gl.Translate(POS_X - HALF_SIZE / 2 + COMBO_HALF_SIZE * i + LITTLE_SPACE * i, POS_Y + HALF_SIZE, POS_Z);
                gl.Begin(OpenGL.GL_QUADS);

                Defence def = defences[i];
                if (def == Defence.YELLOW)
                {
                    gl.Color(1.0f, 1.0f, 0.0f);
                }
                else if (def == Defence.RED)
                {
                    gl.Color(1.0f, 0.0f, 0.0f);
                }
                else if(def == Defence.BLUE)
                {
                    gl.Color(0.0f, 0.0f, 1.0f);
                }
                else
                {
                    gl.Color(0.5f, 0.0f, 0.5f);
                }

                // BL
                gl.TexCoord(1, 1);
                gl.Vertex(-COMBO_HALF_SIZE, -COMBO_HALF_SIZE, 0);
                gl.TexCoord(1, 0);
                gl.Vertex(-COMBO_HALF_SIZE, COMBO_HALF_SIZE, 0);
                gl.TexCoord(0, 0);
                gl.Vertex(COMBO_HALF_SIZE, COMBO_HALF_SIZE, 0);
                gl.TexCoord(0, 1);
                gl.Vertex(COMBO_HALF_SIZE, -COMBO_HALF_SIZE, 0);
                gl.End();
                gl.PopMatrix();
                gl.Color(1.0f, 1.0f, 1.0f);
            }
        }
    }
}
