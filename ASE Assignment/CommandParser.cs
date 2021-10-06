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
        enum command
        {
            While, Var, Draw, Method, If
        }

        IDictionary<string, int> Variables = new Dictionary<string, int>();
        public CommandParser()
        {

        }
        public void executeLine(string line)
        {
            
        }

        public void executeScript(string script)
        {

        }

        private command commandDecode(string command)
        {
            switch (command)
            {
                case "While":
                    return CommandParser.command.While;
                    break;
                case "Method":
                    return CommandParser.command.Method;
                    break;
                case "Var":
                    return CommandParser.command.Var;
                    break;
                case "If":
                    return CommandParser.command.If;
                default:
                    throw new Exception("There is something wrong with the command");
            }
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
