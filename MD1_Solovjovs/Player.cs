using SharpGL;
using SharpGL.SceneGraph.Assets;
using System;

namespace MD1_Solovjovs
{
    public class Player
    {
        private static bool textureIsLoaded = false;
        private static readonly Texture texture = new Texture();
        private static readonly string pathToTexture = "player.png";

        private readonly int POS_X = 0;
        private readonly int POS_Y = 0;
        private readonly int POS_Z = 0;

        private int lives = 5;

        // Keep a reference
        private OpenGL gl;

        public Player(OpenGL gl) {
            this.gl = gl;
            if(!textureIsLoaded)
            {
                texture.Create(gl, pathToTexture);
                textureIsLoaded = true;
            }
        }

        public int getLives() { return lives; }

        public void Render()
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
            gl.Vertex(-1f, -1f, 0);
            gl.TexCoord(1, 0);
            gl.Vertex(-1f, 1f, 0);
            gl.TexCoord(0, 0);
            gl.Vertex(1f, 1f, 0);
            gl.TexCoord(0, 1);
            gl.Vertex(1f, -1f, 0);
            gl.End();
            gl.PopMatrix();

            gl.Disable(OpenGL.GL_BLEND);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

    }
}
