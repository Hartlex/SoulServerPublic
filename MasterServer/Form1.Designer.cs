namespace MasterServer
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
            this.ConsoleOutput = new System.Windows.Forms.RichTextBox();
            this.startServerButton = new System.Windows.Forms.Button();
            this.stopServerButton = new System.Windows.Forms.Button();
            this.restartServerButton = new System.Windows.Forms.Button();
            this.DbCheckBox = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainServerBox = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.MainServerBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConsoleOutput
            // 
            this.ConsoleOutput.Location = new System.Drawing.Point(6, 21);
            this.ConsoleOutput.Name = "ConsoleOutput";
            this.ConsoleOutput.ReadOnly = true;
            this.ConsoleOutput.Size = new System.Drawing.Size(527, 563);
            this.ConsoleOutput.TabIndex = 0;
            this.ConsoleOutput.Text = "";
            this.ConsoleOutput.TextChanged += new System.EventHandler(this.ConsoleOutput_TextChanged);
            // 
            // startServerButton
            // 
            this.startServerButton.Location = new System.Drawing.Point(544, 21);
            this.startServerButton.Name = "startServerButton";
            this.startServerButton.Size = new System.Drawing.Size(240, 40);
            this.startServerButton.TabIndex = 1;
            this.startServerButton.Text = "Start Server";
            this.startServerButton.UseVisualStyleBackColor = true;
            this.startServerButton.Click += new System.EventHandler(this.StartServerButton_Click);
            // 
            // stopServerButton
            // 
            this.stopServerButton.Location = new System.Drawing.Point(544, 67);
            this.stopServerButton.Name = "stopServerButton";
            this.stopServerButton.Size = new System.Drawing.Size(240, 40);
            this.stopServerButton.TabIndex = 2;
            this.stopServerButton.Text = "Stop Server";
            this.stopServerButton.UseVisualStyleBackColor = true;
            this.stopServerButton.Click += new System.EventHandler(this.StopServerButton_Click);
            // 
            // restartServerButton
            // 
            this.restartServerButton.Location = new System.Drawing.Point(544, 113);
            this.restartServerButton.Name = "restartServerButton";
            this.restartServerButton.Size = new System.Drawing.Size(240, 40);
            this.restartServerButton.TabIndex = 3;
            this.restartServerButton.Text = "Restart Server";
            this.restartServerButton.UseVisualStyleBackColor = true;
            this.restartServerButton.Click += new System.EventHandler(this.restartServerButton_Click);
            // 
            // DbCheckBox
            // 
            this.DbCheckBox.AutoCheck = false;
            this.DbCheckBox.AutoSize = true;
            this.DbCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DbCheckBox.Location = new System.Drawing.Point(544, 159);
            this.DbCheckBox.Name = "DbCheckBox";
            this.DbCheckBox.Size = new System.Drawing.Size(119, 21);
            this.DbCheckBox.TabIndex = 5;
            this.DbCheckBox.Text = "DbConnection";
            this.DbCheckBox.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainServerToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1237, 28);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "Main Menu";
            // 
            // mainServerToolStripMenuItem
            // 
            this.mainServerToolStripMenuItem.Name = "mainServerToolStripMenuItem";
            this.mainServerToolStripMenuItem.Size = new System.Drawing.Size(99, 24);
            this.mainServerToolStripMenuItem.Text = "Main Server";
            this.mainServerToolStripMenuItem.Click += new System.EventHandler(this.mainServerToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.MainServerBox.Controls.Add(this.ConsoleOutput);
            this.MainServerBox.Controls.Add(this.DbCheckBox);
            this.MainServerBox.Controls.Add(this.startServerButton);
            this.MainServerBox.Controls.Add(this.restartServerButton);
            this.MainServerBox.Controls.Add(this.stopServerButton);
            this.MainServerBox.Location = new System.Drawing.Point(12, 31);
            this.MainServerBox.Name = "MainServerBox";
            this.MainServerBox.Size = new System.Drawing.Size(1225, 592);
            this.MainServerBox.TabIndex = 7;
            this.MainServerBox.TabStop = false;
            this.MainServerBox.Text = "Main Server";
            this.MainServerBox.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1237, 623);
            this.Controls.Add(this.MainServerBox);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.MainServerBox.ResumeLayout(false);
            this.MainServerBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ConsoleOutput;
        private System.Windows.Forms.Button startServerButton;
        private System.Windows.Forms.Button stopServerButton;
        private System.Windows.Forms.Button restartServerButton;
        internal System.Windows.Forms.CheckBox DbCheckBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mainServerToolStripMenuItem;
        private System.Windows.Forms.GroupBox MainServerBox;
    }
}

