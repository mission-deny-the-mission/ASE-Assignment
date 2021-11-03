using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASE_Assignment
{
    public partial class Form1 : Form
    {
        CommandParser parser;
        Drawing drawer;
        public Form1()
        {
            drawer = new Drawing();
            InitializeComponent();
            parser = new CommandParser(drawer, drawingArea);
            drawingArea.BackColor = Color.White;
            drawingArea.Paint += new System.Windows.Forms.PaintEventHandler(drawer.Graphics_Paint);
        }
        private void execute_script(object sender, EventArgs e)
        {
            parser.executeScript(scriptArea.Text);
        }

        private void execute(object sender, EventArgs e)
        {
            parser.executeLineHandler(commandArea.Text);
        }

    }
}
