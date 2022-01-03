using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ASE_Assignment
{
    /// <summary>
    /// This is the class that forms the main GUI of the program.
    /// </summary>
    public partial class MainWindow : Form
    {
        CommandParser parser;
        DrawingClass drawer;
        Thread changeColourThread;
        Boolean flag = true;
        public MainWindow()
        {
            InitializeComponent();
            drawer = new DrawingClass(drawingArea);
            parser = new CommandParser(drawer);
            drawingArea.BackColor = Color.White;
            drawingArea.Paint += new System.Windows.Forms.PaintEventHandler(drawer.Graphics_Paint);
            changeColourThread = new Thread(this.flashThread);
            changeColourThread.Start();
        }

        override protected void OnFormClosing(System.Windows.Forms.FormClosingEventArgs e)
        {
            flag = false;
        }

        
        public void flashThread()
        {
            while(flag)
            {
                Thread.Sleep(500);
                try
                {
                    UpdateSafe();
                }
                catch (Exception ex) { }
            }
        }

        public void UpdateSafe()
        {
            if (drawingArea.InvokeRequired)
            {
                Action safeUpdate = delegate { UpdateSafe(); };
                drawingArea.Invoke(safeUpdate);
            }
            else
            {
                drawer.flash();
            }
        }
        
        private void execute_script(object sender, EventArgs e)
        {
            parser.executeScript(scriptArea.Text);
        }

        private void execute(object sender, EventArgs e)
        {
            if (commandArea.Text.ToLower() == "reset")
            {
                drawer.clear();
                parser = new CommandParser(drawer);
                drawer.update();
            }
            else
            {
                parser.executeLineHandler(commandArea.Text, scriptArea.Text);
            }
        }

        /*
        private void ScriptArea_KeyPress(object sender, KeyEventArgs e, SendKeys sendKeys)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                sendKeys(^{TAB});
            }
        }
        */

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (commandArea.Text.ToLower() == "reset")
                {
                    drawer.clear();
                    parser = new CommandParser(drawer);
                    drawer.update();
                }
                else
                {
                    parser.executeLineHandler(commandArea.Text, scriptArea.Text);
                }
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

        private void exportBitmapImage(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp|All files(*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = drawer.generateBitmap(drawingArea.Size.Width, drawingArea.Size.Height);
                string path = Path.GetFullPath(saveFileDialog.FileName);
                bmp.Save(path);
            }
        }
    }
}
