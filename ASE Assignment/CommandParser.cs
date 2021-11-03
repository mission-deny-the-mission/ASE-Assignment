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
        protected System.Windows.Forms.PictureBox pictureBox;
        Dictionary<string, (byte, byte, byte, byte)> colours = new Dictionary<string, (byte, byte, byte, byte)>();
        public CommandParser(Drawer drawingClass, System.Windows.Forms.PictureBox pictureBox)
        {
            this.drawingClass = drawingClass;
            this.pictureBox = pictureBox;
            colours.Add("red", (255, 0, 0, 255));
            colours.Add("green", (0, 255, 0, 255));
            colours.Add("blue", (0, 0, 255, 255));
            colours.Add("black", (0, 0, 0, 255));
            colours.Add("white", (255, 255, 255, 255));
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
            line = line.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            string[] words = line.Split(' ');
            switch (words[0])
            {
                case "pen":
                    (byte, byte, byte, byte) colour = decodeColour(words[1]);
                    drawingClass.setPenColour(colour);
                    break;
                case "circle":
                    if (Int32.TryParse(words[1], out int x) && Int32.TryParse(words[2], out int y)
                        && Int32.TryParse(words[3], out int radius))
                    {
                        drawingClass.drawCircle(x, y, radius);
                    }
                    break;
                case "clear":
                    if (words.Length == 1)
                    {
                        drawingClass.clear();
                    }
                    break;
                default:
                    throw new Exception("Invalid command");
            }
        }

        public void executeLineHandler(string line)
        {
            executeLine(line);
            pictureBox.Refresh();
        }

        public void executeScript(string script)
        {
            foreach (string line in script.Split('\n'))
            {
                executeLine(line);
            }
            pictureBox.Refresh();
        }

    }
}
