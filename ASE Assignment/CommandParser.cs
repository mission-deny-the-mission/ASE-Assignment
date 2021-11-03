using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASE_Assignment
{
    class CommandParser
    {
        Drawer drawingClass;
        Dictionary<string, (byte, byte, byte, byte)> colours = new Dictionary<string, (byte, byte, byte, byte)>();
        public CommandParser(Drawer drawingClass)
        {
            this.drawingClass = drawingClass;
            colours.Add("red", (255, 0, 0, 255));
            colours.Add("green", (0, 255, 0, 255));
            colours.Add("blue", (0, 0, 255, 255));
        }
        private (byte, byte, byte, byte) decodeColour(string colour)
        {
            string lowerColour = colour.ToLower();
            if (colours.ContainsKey(lowerColour))
            {
                return colours[lowerColour];
            }
            else
            {
                throw new Exception();
            }
        }
        public void executeLine(string line)
        {
            string[] words = line.Split(' ');
            switch (words[0])
            {
                case "pen":
                    (byte, byte, byte, byte) colour = decodeColour(words[1]);
                    drawingClass.setPenColour(colour);
                    break;
            }

        }

        public void executeScript(string script)
        {
            foreach (string line in script.Split('\n'))
            {
                executeLine(line);
            }
        }

    }
}
