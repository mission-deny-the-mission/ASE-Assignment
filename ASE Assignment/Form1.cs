using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASE_Assignment
{
    public partial class Form1 : Form
    {
        CommandParser parser;
        DrawingClass drawer;
        public Form1()
        {
            InitializeComponent();
            drawer = new DrawingClass(drawingArea);
            parser = new CommandParser(drawer);
            drawingArea.BackColor = Color.White;
            drawingArea.Paint += new System.Windows.Forms.PaintEventHandler(drawer.Graphics_Paint);
        }
        private void execute_script(object sender, EventArgs e)
        {
            parser.executeScript(scriptArea.Text);
        }

        private void execute(object sender, EventArgs e)
        {
            parser.executeLineHandler(commandArea.Text, scriptArea.Text);
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                parser.executeLineHandler(commandArea.Text, scriptArea.Text);
            }
        }

        private void openScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // openFileDialog.InitialDirectory = "";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();

                        scriptArea.Text = fileContent;
                    }

                }
            }
        }

        private void saveScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    using (StreamWriter writer = new StreamWriter(myStream))
                        writer.Write(scriptArea.Text);
                    myStream.Close();
                }
            }
        }
    }
}
