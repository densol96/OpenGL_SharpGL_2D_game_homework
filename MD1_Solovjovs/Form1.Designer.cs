using System.Drawing;

namespace MD1_Solovjovs
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openGLControl1 = new SharpGL.OpenGLControl();
            this.levelLabel = new System.Windows.Forms.Label();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.yellowBtn = new System.Windows.Forms.Button();
            this.redBtn = new System.Windows.Forms.Button();
            this.blueBtn = new System.Windows.Forms.Button();
            this.purpleBtn = new System.Windows.Forms.Button();
            this.gameOverLabel = new System.Windows.Forms.Label();
            this.pressSpaceLabel = new System.Windows.Forms.Label();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.pressRestartLabel = new System.Windows.Forms.Label();
            this.bossLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // openGLControl1
            // 
            this.openGLControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.openGLControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGLControl1.DrawFPS = false;
            this.openGLControl1.Location = new System.Drawing.Point(0, 0);
            this.openGLControl1.Name = "openGLControl1";
            this.openGLControl1.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl1.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl1.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl1.Size = new System.Drawing.Size(1232, 825);
            this.openGLControl1.TabIndex = 0;
            this.openGLControl1.OpenGLInitialized += new System.EventHandler(this.openGLControl1_OpenGLInitialized);
            this.openGLControl1.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl1_OpenGLDraw);
            this.openGLControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openGLControl1_KeyDown);
            // 
            // levelLabel
            // 
            this.levelLabel.AutoSize = true;
            this.levelLabel.Font = new System.Drawing.Font("Press Start 2P", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelLabel.Location = new System.Drawing.Point(756, 28);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(233, 37);
            this.levelLabel.TabIndex = 1;
            this.levelLabel.Text = "LEVEL: 1";
            this.levelLabel.Visible = false;
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Font = new System.Drawing.Font("Press Start 2P", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.Location = new System.Drawing.Point(474, 28);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(233, 37);
            this.scoreLabel.TabIndex = 1;
            this.scoreLabel.Text = "SCORE: 0";
            this.scoreLabel.Visible = false;
            // 
            // yellowBtn
            // 
            this.yellowBtn.BackColor = System.Drawing.Color.Yellow;
            this.yellowBtn.Location = new System.Drawing.Point(376, 735);
            this.yellowBtn.Name = "yellowBtn";
            this.yellowBtn.Size = new System.Drawing.Size(75, 59);
            this.yellowBtn.TabIndex = 6;
            this.yellowBtn.UseVisualStyleBackColor = false;
            this.yellowBtn.Visible = false;
            this.yellowBtn.Click += new System.EventHandler(this.yellowBtn_Click);
            // 
            // redBtn
            // 
            this.redBtn.BackColor = System.Drawing.Color.Red;
            this.redBtn.Location = new System.Drawing.Point(494, 735);
            this.redBtn.Name = "redBtn";
            this.redBtn.Size = new System.Drawing.Size(75, 59);
            this.redBtn.TabIndex = 6;
            this.redBtn.UseVisualStyleBackColor = false;
            this.redBtn.Visible = false;
            this.redBtn.Click += new System.EventHandler(this.redBtn_Click);
            // 
            // blueBtn
            // 
            this.blueBtn.BackColor = System.Drawing.Color.Blue;
            this.blueBtn.Location = new System.Drawing.Point(617, 735);
            this.blueBtn.Name = "blueBtn";
            this.blueBtn.Size = new System.Drawing.Size(75, 59);
            this.blueBtn.TabIndex = 6;
            this.blueBtn.UseVisualStyleBackColor = false;
            this.blueBtn.Visible = false;
            this.blueBtn.Click += new System.EventHandler(this.blueBtn_Click);
            // 
            // purpleBtn
            // 
            this.purpleBtn.BackColor = System.Drawing.Color.Fuchsia;
            this.purpleBtn.Location = new System.Drawing.Point(742, 735);
            this.purpleBtn.Name = "purpleBtn";
            this.purpleBtn.Size = new System.Drawing.Size(75, 59);
            this.purpleBtn.TabIndex = 6;
            this.purpleBtn.UseVisualStyleBackColor = false;
            this.purpleBtn.Visible = false;
            this.purpleBtn.Click += new System.EventHandler(this.purpleBtn_Click);
            // 
            // gameOverLabel
            // 
            this.gameOverLabel.AutoSize = true;
            this.gameOverLabel.Font = new System.Drawing.Font("Press Start 2P", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameOverLabel.Location = new System.Drawing.Point(363, 309);
            this.gameOverLabel.Name = "gameOverLabel";
            this.gameOverLabel.Size = new System.Drawing.Size(518, 74);
            this.gameOverLabel.TabIndex = 7;
            this.gameOverLabel.Text = "GAME OVER";
            this.gameOverLabel.Visible = false;
            // 
            // pressSpaceLabel
            // 
            this.pressSpaceLabel.AutoSize = true;
            this.pressSpaceLabel.Font = new System.Drawing.Font("Press Start 2P", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pressSpaceLabel.Location = new System.Drawing.Point(306, 437);
            this.pressSpaceLabel.Name = "pressSpaceLabel";
            this.pressSpaceLabel.Size = new System.Drawing.Size(638, 37);
            this.pressSpaceLabel.TabIndex = 8;
            this.pressSpaceLabel.Text = "PRESS SPACE TO CONTINUE";
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Font = new System.Drawing.Font("Press Start 2P", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.welcomeLabel.Location = new System.Drawing.Point(30, 309);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(1166, 74);
            this.welcomeLabel.TabIndex = 7;
            this.welcomeLabel.Text = "WELCOME TO KNIGHTS 2D";
            // 
            // pressRestartLabel
            // 
            this.pressRestartLabel.AutoSize = true;
            this.pressRestartLabel.Font = new System.Drawing.Font("Press Start 2P", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pressRestartLabel.Location = new System.Drawing.Point(270, 437);
            this.pressRestartLabel.Name = "pressRestartLabel";
            this.pressRestartLabel.Size = new System.Drawing.Size(719, 37);
            this.pressRestartLabel.TabIndex = 8;
            this.pressRestartLabel.Text = "PRESS SPACE TO START AGAIN";
            this.pressRestartLabel.Visible = false;
            // 
            // bossLabel
            // 
            this.bossLabel.AutoSize = true;
            this.bossLabel.Font = new System.Drawing.Font("Press Start 2P", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bossLabel.Location = new System.Drawing.Point(363, 140);
            this.bossLabel.Name = "bossLabel";
            this.bossLabel.Size = new System.Drawing.Size(518, 74);
            this.bossLabel.TabIndex = 7;
            this.bossLabel.Text = "BOSS TIME";
            this.bossLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 825);
            this.Controls.Add(this.pressRestartLabel);
            this.Controls.Add(this.pressSpaceLabel);
            this.Controls.Add(this.bossLabel);
            this.Controls.Add(this.welcomeLabel);
            this.Controls.Add(this.gameOverLabel);
            this.Controls.Add(this.purpleBtn);
            this.Controls.Add(this.blueBtn);
            this.Controls.Add(this.redBtn);
            this.Controls.Add(this.yellowBtn);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.openGLControl1);
            this.Name = "Form1";
            this.Text = "Knights 2D";
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl openGLControl1;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Button yellowBtn;
        private System.Windows.Forms.Button redBtn;
        private System.Windows.Forms.Button blueBtn;
        private System.Windows.Forms.Button purpleBtn;
        private System.Windows.Forms.Label gameOverLabel;
        private System.Windows.Forms.Label pressSpaceLabel;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Label pressRestartLabel;
        private System.Windows.Forms.Label bossLabel;
    }
}

