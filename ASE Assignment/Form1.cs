using System;
using System.Drawing;
using System.IO;
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
                    // get the file path and open it
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();

                    // Open a file reader
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        // get the contents of the file and put it in the script area
                        scriptArea.Text = reader.ReadToEnd();
                        reader.Close();
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
                    // write the contents of the textArea to a file using a StreamWriter
                    using (StreamWriter writer = new StreamWriter(myStream))
                    {
                        writer.Write(scriptArea.Text);
                        writer.Close();
                    }
                    myStream.Close();
                }
            }
        }
    }
}
