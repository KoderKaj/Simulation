namespace Simulation
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
            this.components = new System.ComponentModel.Container();
            this.moveTimer = new System.Windows.Forms.Timer(this.components);
            this.spawnTimer = new System.Windows.Forms.Timer(this.components);
            this.txtScore = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // moveTimer
            // 
            this.moveTimer.Enabled = true;
            this.moveTimer.Interval = 20;
            this.moveTimer.Tick += new System.EventHandler(this.moveTimer_Tick);
            // 
            // spawnTimer
            // 
            this.spawnTimer.Enabled = true;
            this.spawnTimer.Interval = 2000;
            this.spawnTimer.Tick += new System.EventHandler(this.spawn);
            // 
            // txtScore
            // 
            this.txtScore.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtScore.Enabled = false;
            this.txtScore.Location = new System.Drawing.Point(13, 13);
            this.txtScore.Name = "txtScore";
            this.txtScore.Size = new System.Drawing.Size(100, 22);
            this.txtScore.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtScore);
            this.Name = "Form1";
            this.Text = "Simulation";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintEvent);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer moveTimer;
        private System.Windows.Forms.Timer spawnTimer;
        private System.Windows.Forms.TextBox txtScore;
    }
}

