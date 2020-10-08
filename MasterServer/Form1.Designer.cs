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
            this.SuspendLayout();
            // 
            // ConsoleOutput
            // 
            this.ConsoleOutput.Location = new System.Drawing.Point(12, 12);
            this.ConsoleOutput.Name = "ConsoleOutput";
            this.ConsoleOutput.ReadOnly = true;
            this.ConsoleOutput.Size = new System.Drawing.Size(527, 426);
            this.ConsoleOutput.TabIndex = 0;
            this.ConsoleOutput.Text = "";
            this.ConsoleOutput.TextChanged += new System.EventHandler(this.ConsoleOutput_TextChanged);
            // 
            // startServerButton
            // 
            this.startServerButton.Location = new System.Drawing.Point(556, 12);
            this.startServerButton.Name = "startServerButton";
            this.startServerButton.Size = new System.Drawing.Size(240, 40);
            this.startServerButton.TabIndex = 1;
            this.startServerButton.Text = "Start Server";
            this.startServerButton.UseVisualStyleBackColor = true;
            this.startServerButton.Click += new System.EventHandler(this.StartServerButton_Click);
            // 
            // stopServerButton
            // 
            this.stopServerButton.Location = new System.Drawing.Point(556, 59);
            this.stopServerButton.Name = "stopServerButton";
            this.stopServerButton.Size = new System.Drawing.Size(240, 40);
            this.stopServerButton.TabIndex = 2;
            this.stopServerButton.Text = "Stop Server";
            this.stopServerButton.UseVisualStyleBackColor = true;
            this.stopServerButton.Click += new System.EventHandler(this.StopServerButton_Click);
            // 
            // restartServerButton
            // 
            this.restartServerButton.Location = new System.Drawing.Point(556, 105);
            this.restartServerButton.Name = "restartServerButton";
            this.restartServerButton.Size = new System.Drawing.Size(240, 40);
            this.restartServerButton.TabIndex = 3;
            this.restartServerButton.Text = "Restart Server";
            this.restartServerButton.UseVisualStyleBackColor = true;
            this.restartServerButton.Click += new System.EventHandler(this.restartServerButton_Click);
            // 
            // DbCheckBox
            // 
            this.DbCheckBox.AutoSize = true;
            this.DbCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DbCheckBox.Enabled = false;
            this.DbCheckBox.Location = new System.Drawing.Point(556, 165);
            this.DbCheckBox.Name = "DbCheckBox";
            this.DbCheckBox.Size = new System.Drawing.Size(119, 21);
            this.DbCheckBox.TabIndex = 5;
            this.DbCheckBox.Text = "DbConnection";
            this.DbCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DbCheckBox);
            this.Controls.Add(this.restartServerButton);
            this.Controls.Add(this.stopServerButton);
            this.Controls.Add(this.startServerButton);
            this.Controls.Add(this.ConsoleOutput);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ConsoleOutput;
        private System.Windows.Forms.Button startServerButton;
        private System.Windows.Forms.Button stopServerButton;
        private System.Windows.Forms.Button restartServerButton;
        internal System.Windows.Forms.CheckBox DbCheckBox;
    }
}

