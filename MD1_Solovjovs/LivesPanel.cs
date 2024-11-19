using SharpGL;
using SharpGL.SceneGraph.Assets;


namespace MD1_Solovjovs
{
    public class LivesPanel
    {
        // Made this static so I only load the texture 1 (makes the app less buggy)
        private static bool textureIsLoaded = false;
        private static readonly Texture texture = new Texture();
        private static readonly string pathToTexture = "heart.png";
        
        private readonly float POS_X_START = -8.5f;
        private readonly float POS_Y_START = 5.5f;
        private readonly float size = 0.6f;
        private readonly float half_size;
        private readonly float space_between = 0.2f;
        OpenGL gl;

        public LivesPanel(OpenGL gl)
        {
            if (!textureIsLoaded)
            {
                texture.Create(gl, pathToTexture);
                textureIsLoaded = true;
            }
            this.gl = gl;
            half_size = size / 2;
        }

        public void Render(int livesLeft)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            texture.Bind(gl);

            gl.PushMatrix();

            for (int i = 0; i < livesLeft; i++)
            {
                gl.PushMatrix();
                gl.Translate(POS_X_START + i * size + space_between * i, POS_Y_START, 0);
                gl.Begin(OpenGL.GL_QUADS);
                // BL
                gl.TexCoord(1, 1);
                gl.Vertex(-half_size, -half_size, 0);
                gl.TexCoord(1, 0);
                gl.Vertex(-half_size, half_size, 0);
                gl.TexCoord(0, 0);
                gl.Vertex(half_size, half_size, 0);
                gl.TexCoord(0, 1);
                gl.Vertex(half_size, -half_size, 0);
                gl.End();
                gl.PopMatrix();
            }
           
            gl.PopMatrix();

            gl.Disable(OpenGL.GL_BLEND);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }
    }
}
