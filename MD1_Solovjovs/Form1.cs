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
        private List<Enemy> enemies;
        private LivesPanel livesPanel;


        private int level = 1;
        private int score = 0;
        private readonly float PLAYER_REACH_THRESHOLD = 0.5f;

        private bool gameOver = false;
        private bool gameFirstOpen = true;

        public Form1()
        {
           InitializeComponent();
           AfterInit();
        }

        private void AfterInit()
        {
            welcomeLabel.Parent = openGLControl1;
            welcomeLabel.BackColor = Color.Transparent;

            pressSpaceLabel.Parent = openGLControl1;
            pressSpaceLabel.BackColor = Color.Transparent;

            gameOverLabel.Parent = openGLControl1;
            gameOverLabel.BackColor = Color.Transparent;

            levelLabel.Parent = openGLControl1;
            levelLabel.BackColor = Color.Transparent;

            scoreLabel.Parent = openGLControl1;
            scoreLabel.BackColor = Color.Transparent;

            pressRestartLabel.Parent = openGLControl1;
            pressRestartLabel.BackColor = Color.Transparent;
        }

        public void SetGame()
        {
            gameFirstOpen = false;
            gameOver = false;
            player = new Player(gl);
            livesPanel = new LivesPanel(gl);
            enemies = new List<Enemy>();
            level = 1;
            score = 0;
            LabelSetUp();
            EnemiesSetUp();
            TimerSetUp();
        }

        private void LabelSetUp()
        {
            pressSpaceLabel.Visible = false;
            gameOverLabel.Visible = false;
            welcomeLabel.Visible = false;
            scoreLabel.Visible = true;
            levelLabel.Visible = true;
            redBtn.Visible = true;
            yellowBtn.Visible = true;   
            purpleBtn.Visible = true; 
            blueBtn.Visible = true;
            pressRestartLabel.Visible = false;
        }

        private void EnemiesSetUp()
        {
            // First 4 enemies with hard-coded coords to come ffrom all 4 sides on a new level
            // I have logic inside Enemy class to increase speed depending on the level with a bit of random as well
            Enemy top = new Enemy(gl, 0, Enemy.MAX_Y, level, true);
            Enemy bottom = new Enemy(gl, 0, -Enemy.MAX_Y, level, true);
            Enemy left = new Enemy(gl, -Enemy.MAX_X, 0, level, true);
            Enemy right = new Enemy(gl, Enemy.MAX_X, 0, level, true);
            enemies.Add(top);
            enemies.Add(bottom);
            enemies.Add(left);
            enemies.Add(right);

            // And additional enemies that will be appering ar random coords every second
            // (Count dependent on the current level. The higher the level - the more enemies are spawned)
            for(int i = 0; i < CalcEnemyCountPerLevel(); i++)
            {
                // I use in-class logic to work with speed depending on the level
                enemies.Add(new Enemy(gl, null, null, level, false));
            }

            // Add a BOSS at the very end
            enemies.Add(new Enemy(gl, null, null, level, false, true));
        }

        private int CalcEnemyCountPerLevel()
        {
            // For now I will go with 3 levels alltogether cause I will also add BOSSES
            // Also it will less take time for a Lecturer to test the game
            switch (level)
            {
                case 1:
                    return 5;
                case 2:
                    return 10;
                case 3:
                    return 15;
                default:
                    return 0;
            }
        }

        private void TimerSetUp()
        {
            if(enemySpawnTimer != null)
            {
                enemySpawnTimer.Stop();
            }
            else
            {
                enemySpawnTimer = new Timer();
                enemySpawnTimer.Interval = 1000; // 1000ms = 1s
                enemySpawnTimer.Tick += TimerActionToAddEnemy;
            }
            enemySpawnTimer.Start();
        }

        private void TimerActionToAddEnemy(object sender, EventArgs e)
        {
            enemies.Add(new Enemy(gl, null, null, level));
        }

        private void openGLControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            if (!gameFirstOpen && !gameOver)
            {
                GameOn();
            }
        }

            private void GameOn()
        {
            gl.LoadIdentity();
            gl.PushMatrix();
            //gl.Translate(0, 0, -10);
            //// Move camera back a bit to be able see what is rendered at z=0 and properly posiition it
            gl.LookAt(0.0f, 0.0f, 15.0f, // Camera's position
              0.0f, 0.0f, 0.0f,   // Camera pointed at
              0.0f, 1.0f, 0.0f);  // Axis vector
            player.Render();
            RenderEnemies();
            CheckDistanceBetweenPlayerAndEnemy();
            livesPanel.Render(player.Lives);
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
            foreach (Enemy enemy in enemies)
            {
                float dx = enemy.POS_X - player.POS_X;
                float dy = enemy.POS_Y - player.POS_Y;
                // Math.Sqrt is more expensive to compute than *
                if (dx * dx + dy * dy <= PLAYER_REACH_THRESHOLD * PLAYER_REACH_THRESHOLD)
                {
                    player.Lives -= 1;
                    enemies.Remove(enemy);
                    if(player.Lives == 0)
                    {
                        EndGame();
                    }
                    break;
                }
            }
        }

        private void EndGame()
        {
            gameOver = true;
            enemySpawnTimer.Stop();
            redBtn.Visible = false;
            blueBtn.Visible = false;
            yellowBtn.Visible = false;
            purpleBtn.Visible = false;
            gameOverLabel.Visible = true;
            pressRestartLabel.Visible = true;
        }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            gl = this.openGLControl1.OpenGL;
            //gl.ClearColor(0.5f, 0.8f, 1.0f, 1.0f); // blue
            gl.ClearColor(0.6f, 0.9f, 0.6f, 1.0f);
            
        }

        private void openGLControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((gameOver | gameFirstOpen) && e.KeyCode == Keys.Space)
            {
                SetGame();
            }
        }
    }
}
