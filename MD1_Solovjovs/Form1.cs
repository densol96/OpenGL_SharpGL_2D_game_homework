using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MD1_Solovjovs
{
    public partial class Form1 : Form
    {

        private OpenGL gl;
        private readonly int DISTANCE_FROM_CAMERA = -10;
        private Timer enemySpawnTimer;

        private Player player;
        private List<Enemy> enemies = new List<Enemy>();
        private LivesPanel livesPanel;


        private int level = 3;
        private int score = 0;

        public Form1()
        {
            InitializeComponent();
            player = new Player(gl);
            livesPanel = new LivesPanel(gl);
            LabelSetUp();
            EnemiesSetUp();
            TimerSetUp();
        }

        private void LabelSetUp()
        {
            levelLabel.Parent = openGLControl1;
            levelLabel.BackColor = Color.Transparent;
            levelLabel.Text = $"LEVEL: {level}";

            scoreLabel.Parent = openGLControl1;
            scoreLabel.BackColor = Color.Transparent;
            scoreLabel.Text = $"SCORE: {score}";
        }

        private void EnemiesSetUp()
        {
            Enemy top = new Enemy(gl, 0, Enemy.MAX_Y, level);
            Enemy bottom = new Enemy(gl, 0, -Enemy.MAX_Y, level);
            Enemy left = new Enemy(gl, -Enemy.MAX_X, 0, level);
            Enemy right = new Enemy(gl, Enemy.MAX_X, 0, level);
            enemies.Add(top);
            enemies.Add(bottom);
            enemies.Add(left);
            enemies.Add(right);
        }

        private void TimerSetUp()
        {
            enemySpawnTimer = new Timer();
            enemySpawnTimer.Interval = 1000; // 1000ms = 1s
            enemySpawnTimer.Tick += ((object sender, EventArgs e) => enemies.Add(new Enemy(gl, null, null, level)));
            enemySpawnTimer.Start();
        }

        private void openGLControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.PushMatrix();
            //gl.Translate(0, 0, -10);
            //// Move camera back a bit to be able see what is rendered at z=0 and properly posiition it
            gl.LookAt(0.0f, 0.0f, 15.0f, // Camera's position
              0.0f, 0.0f, 0.0f,   // Camera pointed at
              0.0f, 1.0f, 0.0f);  // Axis vector
            player.Render();
            RenderEnemies();
            livesPanel.Render(player.getLives());
            gl.PopMatrix();   
            gl.Flush();
        }

        private void RenderEnemies()
        {
            foreach(Enemy enemy in enemies) {
                enemy.Render();
            }
        }

        private void CheckDistanceBetweenPlayerAndEnemy()
        {

            foreach (Enemy enemy in enemies) {
                float dx = enemy. - playerX;
                float dy = enemy.Y - playerY;
            }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            gl = this.openGLControl1.OpenGL;
            //gl.ClearColor(0.5f, 0.8f, 1.0f, 1.0f); // blue
            gl.ClearColor(0.6f, 0.9f, 0.6f, 1.0f);
            
        }

        private void openGLControl1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
