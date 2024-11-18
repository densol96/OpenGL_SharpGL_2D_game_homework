using SharpGL.SceneGraph.Assets;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
        private static readonly Random random = new Random();
        private static bool textureIsLoaded = false;
        private static readonly Texture texture = new Texture();
        private static readonly string pathToTexture = "enemy.png";
        
        private static float HALF_SIZE = 0.75f;
        public static readonly float MAX_X = 8f;
        public static readonly float MAX_Y = 4.5f;
        private static readonly int MAX_REGENARTIONS = 2; // As per homework

        private static int BASE_POINTS_PER_KILL = 10;

        private float _pos_x;
        private float _pos_y;

        public float POS_X { 
            get { return _pos_x; } 
            set
            {
                if (value > MAX_X)
                {
                    _pos_x = MAX_X;
                }
                else if (value < -MAX_X)
                {
                    _pos_x = -MAX_X;
                }
                else
                {
                    _pos_x = value;
                }
            } 
        }

        public float POS_Y
        {
            get { return _pos_y; }
            set
            {
                if (value > MAX_Y)
                {
                    _pos_y = MAX_Y;
                }
                else if (value < -MAX_Y)
                {
                    _pos_y = -MAX_Y;
                }
                else
                {
                    _pos_y = value;
                }
            }
        }
        private readonly int POS_Z = 0;

        private float speed;
        private int difficultyLevel = 1;

        private int bossRespawnedTimes = 1;
        private int defenceRegeneratedTimes = 0;

        private List<Defence> defences = new List<Defence>();

        private OpenGL gl;

        public bool IsBoss { get; }
        public bool IsReadyToJoin { get; set; }

        public Enemy(
            OpenGL gl, 
            float? x = null, 
            float? y = null, 
            int difficultyLevel = 1, 
            bool readyToJoingFight = false, 
            bool isBoss = false, 
            float speed = 0.004f
        )
        {
            this.gl = gl;
            this.difficultyLevel = difficultyLevel;
            this.speed = MakeSpeedMoreRandom(( speed <= 0 || speed >= 0.05f) ? 0.004f : speed);
            IsBoss = isBoss;
            IsReadyToJoin = readyToJoingFight;
           
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
            GenerateDefenceCombos();
        }

        private float MakeSpeedMoreRandom(float speed)
        {
            // Want to make speed slightly more random with difficulty level + to make them come at different speed
            float minSpeed = speed + (difficultyLevel - 1) * 0.015f;
            float maxSpeed = minSpeed + difficultyLevel * 0.015f;
            float randomSpeed = (float)(random.NextDouble() * (maxSpeed - minSpeed) + minSpeed);
            return randomSpeed;
        }

        private void RandomlySelectPosition()
        {
            Array allSideOptions = Enum.GetValues(typeof(Side));
            Side randomSide = (Side)allSideOptions.GetValue(random.Next(allSideOptions.Length));
            float randomX;
            float randomY;
            switch (randomSide)
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
            if (IsBoss){
                gl.Color(0f, 1f, 0f); // Blends with the texture and makes boss appear special
                HALF_SIZE = 1f; // Makes boss larger
            };

            // Uzbruceji no dazadam pusem tiek atbilstosi atteloti
            if (POS_X <= 0)
            {
                gl.TexCoord(1, 1);
                gl.Vertex(-HALF_SIZE, -HALF_SIZE, 0);
                gl.TexCoord(1, 0);
                gl.Vertex(-HALF_SIZE, HALF_SIZE, 0);
                gl.TexCoord(0, 0);
                gl.Vertex(HALF_SIZE, HALF_SIZE, 0);
                gl.TexCoord(0, 1);
                gl.Vertex(HALF_SIZE, -HALF_SIZE, 0);
            }
            else
            {
                gl.TexCoord(1, 1);
                gl.Vertex(HALF_SIZE, -HALF_SIZE, 0);
                gl.TexCoord(1, 0);
                gl.Vertex(HALF_SIZE, HALF_SIZE, 0);
                gl.TexCoord(0, 0);
                gl.Vertex(-HALF_SIZE, HALF_SIZE, 0);
                gl.TexCoord(0, 1);
                gl.Vertex(-HALF_SIZE, -HALF_SIZE, 0);
            }


            // Reset these back
            gl.Color(0f, 0f, 0f);
            HALF_SIZE = 0.75f;
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

        // Returns Points Per Kill
        public int GetHit(Defence color)
        {
            if (defences.Count > 0 && defences[0] == color)
            {
                defences.RemoveAt(0);
            }
            if(defences.Count == 0)
            {
                if (IsBoss && defenceRegeneratedTimes < MAX_REGENARTIONS)
                {
                    GenerateDefenceCombos();
                    defenceRegeneratedTimes += 1;
                    return -1; // special code to give SOME points for getting the 1st line of defence
                }
                return BASE_POINTS_PER_KILL * difficultyLevel * (IsBoss ? 5 : 1);
            }
            return 0;
        }

        public bool DieOnCollide(List<Enemy> enemies)
        {
            // Need to create a method inside enemy to check if boss, and if yes, respawn it once
            if(IsBoss && bossRespawnedTimes < MAX_REGENARTIONS)
            {
                bossRespawnedTimes += 1;
                RandomlySelectPosition();
                return false;
            }
            enemies.Remove(this);
            return true;
        }
    }
}
