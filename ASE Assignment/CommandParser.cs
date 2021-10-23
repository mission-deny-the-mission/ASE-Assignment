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

        IDictionary<string, int> Variables = new Dictionary<string, int>();
        public CommandParser(string script)
        {
            foreach (string line in script.Split('\n'))
            {
                executeLine(line);
            }
        }
        struct valueSymbol
        {
            public bool isVariable;
            public string? name;
            public int value;
        }
        private valueSymbol decodeValueSymbol(string symbol)
        {
            valueSymbol result = new valueSymbol();
            if (Char.IsLetter(symbol[0]))
            {
                if (Variables.ContainsKey(symbol))
                {
                    result.name = symbol;
                    result.isVariable = true;
                    result.value = Variables[symbol];
                    return result;
                } else
                {
                    throw new Exception("Variable is not valid");
                }
            } else
            {
                if (int.TryParse(symbol, out int value))
                {
                    result.value = value;
                    result.isVariable = false;
                    return result;
                } else
                {
                    throw new Exception("Symbol is not a valid number or variable");
                }
            }
        }
        public void executeLine(string line)
        {
            string[] words = new string[];
            foreach (string word in line.Split(' ')
            {
                words.Append(word);
            }

            string command = words[0];
            switch (command)
            {
                case "While":
                    if (words.Length == 4)
                    {

                    } else
                    {
                        throw new Exception("While does not have the correct number of arguments supplied.");
                    }
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

        public void executeScript(string script)
        {
            foreach (string line in script.Split('\n'))
            {
                executeLine(line);
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
