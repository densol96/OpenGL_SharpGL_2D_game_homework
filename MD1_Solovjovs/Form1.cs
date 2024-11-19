using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MD1_Solovjovs
{

    enum GameResult
    {
        WON, LOST
    }

    public partial class Form1 : Form
    {
        // CONSTANTS
        private readonly int DISTANCE_FROM_CAMERA = -10;
        private readonly int?[] enemiesPerLevel = { null, 5, 10, 15 };
        private readonly int FINAL_LEVEL = 3;
        private readonly float PLAYER_REACH_THRESHOLD = 0.5f;

        // FOR INIT IN APP
        private OpenGL gl;
        private Timer enemySpawnTimer;

        private Player player;
        private List<Enemy> enemies;
        private LivesPanel livesPanel;

        // STATS
        private int level = 1;
        private int score = 0;
        private int killedEnemiesTotal = 0;

        // GAME STATES
        private bool gameOver = false;
        private bool gameFirstOpen = true;

        public Form1()
        {
           InitializeComponent();
           AfterFormInit();
           MusicController.Loop("loop.mp3");
        }

        private void AfterFormInit()
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

            bossLabel.Parent = openGLControl1;
            bossLabel.BackColor = Color.Transparent;

            totalKilledLabel.Parent = openGLControl1;
            totalKilledLabel.BackColor = Color.Transparent;

            resultLabel.Parent = openGLControl1;
            resultLabel.BackColor = Color.Transparent;

            recordLabel.Parent = openGLControl1;
            recordLabel.BackColor = Color.Transparent;

            newRecordBeatLbl.Parent = openGLControl1;
            newRecordBeatLbl.BackColor = Color.Transparent;
        }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            gl = this.openGLControl1.OpenGL;
            gl.ClearColor(0.6f, 0.9f, 0.6f, 1.0f);
        }

        private void openGLControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            if (!gameFirstOpen && !gameOver)
            {
                GameOn();
            }
        }

        private void openGLControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((gameOver | gameFirstOpen) && e.KeyCode == Keys.Space)
            {
                SetGame();
            }
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
            killedEnemiesTotal = 0;
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
            totalKilledLabel.Visible = true;
            redBtn.Visible = true;
            yellowBtn.Visible = true;   
            purpleBtn.Visible = true; 
            blueBtn.Visible = true;
            pressRestartLabel.Visible = false;
            resultLabel.Visible = false;
            scoreLabel.Text = $"SCORE: {score}";
            levelLabel.Text = $"LEVEL: {level}";
            totalKilledLabel.Text = $"TOTAL KILLED: {killedEnemiesTotal}";
            recordLabel.Text = $"RECORD: {DataManager.GetRecord()}";
            recordLabel.Visible = true;
            newRecordBeatLbl.Visible = false;
        }

        private void EnemiesSetUp()
        {
            // First 4 enemies with hard-coded coords to come from all 4 sides on a new level
            // I have logic inside Enemy class to increase speed depending on the level with a bit of randomness as well
            Enemy top = new Enemy(gl, 0, Enemy.MAX_Y, level, true);
            Enemy bottom = new Enemy(gl, 0, -Enemy.MAX_Y, level, true);
            Enemy left = new Enemy(gl, -Enemy.MAX_X, 0, level, true);
            Enemy right = new Enemy(gl, Enemy.MAX_X, 0, level, true);
            enemies.Add(top);
            enemies.Add(bottom);
            enemies.Add(left);
            enemies.Add(right);

            // And additional enemies that will be appearing ar random coords every second
            // Count dependent on the current level. The higher the level, the more enemies are spawned)
            for (int i = 0; i < CalcEnemyCountPerLevel(); i++)
            {
                // I use in-class logic to work with speed depending on the level and assign random coords
                enemies.Add(new Enemy(gl, null, null, level, false));
            }
            // Add a BOSS at the very end
            enemies.Add(new Enemy(gl, null, null, level, false, true));
        }

        private int CalcEnemyCountPerLevel()
        {
            // For now I will go with 3 levels alltogether cause I will also add BOSSES
            // Also it will less take time for a Lecturer to test the game
            return enemiesPerLevel[level] != null ? (int)enemiesPerLevel[level]: 5;
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
            foreach(Enemy enemy in enemies)
            {
                if (!enemy.IsBoss && !enemy.IsReadyToJoin)
                {
                    enemy.IsReadyToJoin = true;
                    break;
                }
                if (enemies.Count == 1 && enemy.IsBoss && !enemy.IsReadyToJoin)
                {
                    MusicController.PlaySound("boss.wav");
                    bossLabel.Visible = true;
                    Application.DoEvents(); // Makes sure that label update is rendered before I pause the main thread
                    System.Threading.Thread.Sleep(3000); // A little bit of time to indicate that boss is coming
                    bossLabel.Visible = false;
                    enemy.IsReadyToJoin = true;
                }
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
                if(!enemy.IsReadyToJoin)
                {
                    break;
                }
                enemy.Render();
            }
        }

        private void CheckDistanceBetweenPlayerAndEnemy()
        {
            foreach (Enemy enemy in enemies)
            {
                // IsReadyToJoin -> practically means that enemy has already joined the fight (it is being rendered on the screen)
                if (enemy.IsReadyToJoin)
                {
                    float dx = enemy.POS_X - player.POS_X;
                    float dy = enemy.POS_Y - player.POS_Y;
                    // Math.Sqrt(x^2) is more expensive to compute than x*x
                    if (dx * dx + dy * dy <= PLAYER_REACH_THRESHOLD * PLAYER_REACH_THRESHOLD)
                    {
                        /* It doesn't make sense that simply colliding with the boss causes him to die and you to win.
                         * Similarly, with defense regenerating, the boss should additionally respawn once upon colliding 
                         * with the player, each time deducting 2 lives. This means the only way to win against the final 
                         * level 3 boss by colliding with him twice is to keep all 5 lives throughout the rest of the game.
                         */
                        MusicController.PlaySound("collided.wav");
                        if (enemy.IsBoss)
                        {
                            player.Lives -= 2;
                        } 
                        else {
                            player.Lives -= 1;
                        }
                        
                        // If 1st time, boss respawns
                        bool result = enemy.DieOnCollide(enemies); // Player gets no points for this
                        if(result)
                        {
                            killedEnemiesTotal += 1;
                            totalKilledLabel.Text = $"TOTAL KILLED: {killedEnemiesTotal}";
                        }

                        if (player.Lives <= 0)
                        {
                            EndGame(GameResult.LOST);
                        }
                        else if (enemies.Count == 0 && level < FINAL_LEVEL)
                        {
                            LevelUp();
                        }
                        else if (enemies.Count == 0 && level == FINAL_LEVEL)
                        {
                            EndGame(GameResult.WON);
                        }
                        break;
                    }
                }
            }
        }

        private void EndGame(GameResult result)
        {
            gameOver = true;
            enemySpawnTimer.Stop();
            redBtn.Visible = false;
            blueBtn.Visible = false;
            yellowBtn.Visible = false;
            purpleBtn.Visible = false;
            gameOverLabel.Visible = true;
            pressRestartLabel.Visible = true;
            resultLabel.Visible = true;
            if (result == GameResult.WON)
            {
               
                resultLabel.Text = "YOU WON"; 
                MusicController.PlaySound("won.wav");
            }
            else
            {
                resultLabel.Text = "YOU LOST";
                MusicController.PlaySound("lost.wav");
            }
            int record = DataManager.GetRecord();
            if (record < score)
            {
                newRecordBeatLbl.Visible = true;
            }
            DataManager.SaveScore(score);
        }

        private void yellowBtn_Click(object sender, EventArgs e)
        {
            HitEnemy(Defence.YELLOW);
        }

        private void redBtn_Click(object sender, EventArgs e)
        {
            HitEnemy(Defence.RED);
        }

        private void blueBtn_Click(object sender, EventArgs e)
        {
            HitEnemy(Defence.BLUE);
        }

        private void purpleBtn_Click(object sender, EventArgs e)
        {
            HitEnemy(Defence.PURPLE);
        }

        private void HitEnemy(Defence color)
        {
            MusicController.PlaySound("btn.wav");

            List<Enemy> killedEnemies = new List<Enemy>();

            foreach(Enemy enemy in enemies)
            {
                if(enemy.IsReadyToJoin)
                {
                    int pointsForKill = enemy.GetHit(color);
                    if (pointsForKill > 0) // Died
                    {
                        killedEnemies.Add(enemy);
                        score += pointsForKill;
                        scoreLabel.Text = $"SCORE: {score}";
                        MusicController.PlaySound("kill.wav");
                    }
                    else if(pointsForKill == -1) // -1 if Boss regenerated the 1st line of defence
                    {
                        score += 10; // at least some points for the 1st line of defence
                        scoreLabel.Text = $"SCORE: {score}";
                    }
                    // 0 points means no effect
                }
            }

            killedEnemiesTotal += killedEnemies.Count;
            totalKilledLabel.Text = $"TOTAL KILLED: {killedEnemiesTotal}";

            foreach (Enemy killedEnemy in killedEnemies)
            {
                enemies.Remove(killedEnemy);
            }

            if (enemies.Count == 0 && level < FINAL_LEVEL)
            {
                LevelUp();
            }
            else if (enemies.Count == 0 && level == FINAL_LEVEL)
            {
                EndGame(GameResult.WON);
            }
        }

        private void LevelUp()
        {
            level += 1;
            levelLabel.Text = $"LEVEL: {level}";
            EnemiesSetUp();
            MusicController.PlaySound("lvl.wav");
        }
    }
}
