using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ASE_Assignment
{
    class CommandParser
    {
        public CommandParser()
        {

        }
        public void executeLine(string line)
        {

        }

        public void executeScript(string script)
        {

        }

        public void Graphics_Paint(object sender, System.Windows.Forms.PaintEventArgs pe)
        {
            System.Drawing.Graphics g = pe.Graphics;
            Pen myPen = new Pen(Color.Black, 2);
            g.DrawLine(myPen, 0, 0, 100, 100);
            myPen.Dispose();
        }
    }
}
