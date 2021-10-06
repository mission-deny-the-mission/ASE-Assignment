
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
            this.commandArea = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.drawingArea)).BeginInit();
            this.SuspendLayout();
            // 
            // drawingArea
            // 
            this.drawingArea.Location = new System.Drawing.Point(12, 12);
            this.drawingArea.Name = "drawingArea";
            this.drawingArea.Size = new System.Drawing.Size(420, 224);
            this.drawingArea.TabIndex = 0;
            this.drawingArea.TabStop = false;
            // 
            // scriptArea
            // 
            this.scriptArea.Location = new System.Drawing.Point(12, 255);
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
            // commandArea
            // 
            this.commandArea.Cursor = System.Windows.Forms.Cursors.Default;
            this.commandArea.Location = new System.Drawing.Point(13, 414);
            this.commandArea.Name = "commandArea";
            this.commandArea.Size = new System.Drawing.Size(281, 23);
            this.commandArea.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 450);
            this.Controls.Add(this.commandArea);
            this.Controls.Add(this.executeButton);
            this.Controls.Add(this.executeScriptButton);
            this.Controls.Add(this.scriptArea);
            this.Controls.Add(this.drawingArea);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.drawingArea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox drawingArea;
        private System.Windows.Forms.TextBox scriptArea;
        private System.Windows.Forms.Button executeScriptButton;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.TextBox commandArea;
    }
}

