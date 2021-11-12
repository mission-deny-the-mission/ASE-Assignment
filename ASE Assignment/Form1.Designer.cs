
namespace ASE_Assignment
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.drawingArea = new System.Windows.Forms.PictureBox();
            this.scriptArea = new System.Windows.Forms.TextBox();
            this.executeScriptButton = new System.Windows.Forms.Button();
            this.executeButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.commandArea = new System.Windows.Forms.TextBox();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.drawingArea)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // drawingArea
            // 
            this.drawingArea.Location = new System.Drawing.Point(12, 27);
            this.drawingArea.Name = "drawingArea";
            this.drawingArea.Size = new System.Drawing.Size(420, 224);
            this.drawingArea.TabIndex = 0;
            this.drawingArea.TabStop = false;
            // 
            // scriptArea
            // 
            this.scriptArea.Location = new System.Drawing.Point(12, 266);
            this.scriptArea.Multiline = true;
            this.scriptArea.Name = "scriptArea";
            this.scriptArea.Size = new System.Drawing.Size(282, 131);
            this.scriptArea.TabIndex = 1;
            // 
            // executeScriptButton
            // 
            this.executeScriptButton.Location = new System.Drawing.Point(357, 296);
            this.executeScriptButton.Name = "executeScriptButton";
            this.executeScriptButton.Size = new System.Drawing.Size(130, 23);
            this.executeScriptButton.TabIndex = 2;
            this.executeScriptButton.Text = "Execute Script";
            this.executeScriptButton.UseVisualStyleBackColor = true;
            this.executeScriptButton.Click += new System.EventHandler(this.execute_script);
            // 
            // executeButton
            // 
            this.executeButton.Location = new System.Drawing.Point(357, 414);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(75, 23);
            this.executeButton.TabIndex = 3;
            this.executeButton.Text = "Execute";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.execute);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.menuStrip1.Location = new System.Drawing.Point(0, 24);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(550, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // commandArea
            // 
            this.commandArea.Cursor = System.Windows.Forms.Cursors.Default;
            this.commandArea.Location = new System.Drawing.Point(13, 414);
            this.commandArea.Name = "commandArea";
            this.commandArea.Size = new System.Drawing.Size(281, 23);
            this.commandArea.TabIndex = 4;
            this.commandArea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_KeyDown);
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.menuStrip2.Size = new System.Drawing.Size(550, 24);
            this.menuStrip2.TabIndex = 6;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openScriptToolStripMenuItem,
            this.saveScriptToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openScriptToolStripMenuItem
            // 
            this.openScriptToolStripMenuItem.Name = "openScriptToolStripMenuItem";
            this.openScriptToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.openScriptToolStripMenuItem.Text = "Open script";
            this.openScriptToolStripMenuItem.Click += new System.EventHandler(this.openScriptToolStripMenuItem_Click);
            // 
            // saveScriptToolStripMenuItem
            // 
            this.saveScriptToolStripMenuItem.Name = "saveScriptToolStripMenuItem";
            this.saveScriptToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.saveScriptToolStripMenuItem.Text = "Save script";
            this.saveScriptToolStripMenuItem.Click += new System.EventHandler(this.saveScriptToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 448);
            this.Controls.Add(this.commandArea);
            this.Controls.Add(this.executeButton);
            this.Controls.Add(this.executeScriptButton);
            this.Controls.Add(this.scriptArea);
            this.Controls.Add(this.drawingArea);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.menuStrip2);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.drawingArea)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox drawingArea;
        private System.Windows.Forms.TextBox scriptArea;
        private System.Windows.Forms.Button executeScriptButton;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TextBox commandArea;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveScriptToolStripMenuItem;
    }
}

