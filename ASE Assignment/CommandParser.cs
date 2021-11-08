using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASE_Assignment
{
    public class CommandParser
    {
        Drawer drawingClass;
        Dictionary<string, (byte, byte, byte, byte)> colours = new Dictionary<string, (byte, byte, byte, byte)>();

        public CommandParser(Drawer drawingClass)
        {
            this.drawingClass = drawingClass;
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

        public (int, int) parsePoint(string point)
        {
            string[] points = point.Split(',');
            if (points.Length != 2)
            {
                throw new Exception("Invalid coordinate entered");
            }
            if (int.TryParse(points[0], out int x) && int.TryParse(points[1], out int y))
            {
                return (x, y);
            }
            else
            {
                throw new Exception("Invalid coordinate entered");
            }
        }

        public void executeLine(string line)
        {
            line = line.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            if (line == "")
            {
                return;
            }
            string[] words = line.Split(' ');
            switch (words[0].ToLower())
            {
                case "pen":
                    (byte, byte, byte, byte) colour = decodeColour(words[1]);
                    drawingClass.setPenColour(colour);
                    break;
                case "circle":
                    if (words.Length == 3)
                    {
                        (int, int) point = parsePoint(words[1]);
                        if (Int32.TryParse(words[2], out int radius))
                        {
                            drawingClass.drawCircle(point, radius);
                        }
                        else
                        {
                            throw new Exception("Invalid command");
                        }
                    }
                    else if (words.Length == 2)
                    {
                        if (Int32.TryParse(words[1], out int radius))
                        {
                            drawingClass.drawCircle(radius);
                        }
                        else
                        {
                            throw new Exception("Invalid command");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid command");
                    }
                    break;
                case "triangle":
                    if (words.Length == 4)
                    {
                        (int, int) point1 = parsePoint(words[1]);
                        (int, int) point2 = parsePoint(words[2]);
                        (int, int) point3 = parsePoint(words[3]);
                        drawingClass.drawTriangle(point1, point2, point3);
                    }
                    else
                    {
                        throw new Exception("Invalid number of operands for operation triangle");
                    }
                    break;
                case "rectangle":
                    if (words.Length == 3)
                    {
                        if (int.TryParse(words[1], out int width) && int.TryParse(words[2], out int height))
                        {
                            drawingClass.drawRectangle(width, height);
                        }
                        else
                        {
                            throw new Exception("Incorrectly formatted operands for command rectangle");
                        }
                    }
                    else if(words.Length == 4)
                    {
                        var (x, y) = parsePoint(words[1]);
                        if (int.TryParse(words[2], out int width) && int.TryParse(words[3], out int height))
                        {
                            drawingClass.drawRectangle(x, y, width, height);
                        }
                        else
                        {
                            throw new Exception("Incorrect number of operands for command rectangle");
                        }
                    }
                    else
                    {
                        throw new Exception("Incorrect number of operands for command rectangle.");
                    }
                    break;
                case "position":
                    if (words.Length == 3 && words[1] == "pen")
                    {
                        string[] coordsText = words[2].Split(',');
                        if (coordsText.Length != 2)
                        {
                            throw new Exception("Invalid coordinates");
                        }
                        if (Int32.TryParse(coordsText[0], out int x) && Int32.TryParse(coordsText[1], out int y))
                        {
                            drawingClass.setPosition(x, y);
                        }
                        else
                        {
                            throw new Exception("Position is not a number");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid command");
                    }
                    break;
                case "fill":
                    if (words.Length == 2)
                    {
                        if (words[1] == "on")
                        {
                            drawingClass.setFillState(true);
                        }
                        else if (words[1] == "off")
                        {
                            drawingClass.setFillState(false);
                        }
                        else
                        {
                            throw new Exception("Invalid operand");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid number of operands");
                    }
                    break;
                case "new":
                    if (words.Length == 6)
                    {
                        if (words[1] == "colour" || words[1] == "color")
                        {
                            if (Byte.TryParse(words[3], out byte r) && Byte.TryParse(words[4], out byte g) &&
                                Byte.TryParse(words[5], out byte b))
                            {
                                colours.Add(words[2], (r, g, b, 255));
                            }
                            else
                            {
                                throw new Exception("Colour value is not a number");
                            }
                        }
                    }
                    else if (words.Length == 7)
                    {
                        if (words[1] == "colour" || words[1] == "color")
                        {
                            if (Byte.TryParse(words[3], out byte r) && Byte.TryParse(words[4], out byte g) &&
                                Byte.TryParse(words[5], out byte b) && Byte.TryParse(words[6], out byte a))
                            {
                                colours.Add(words[2], (r, g, b, a));
                            }
                            else
                            {
                                throw new Exception("Colour value is not a number");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Incorrect number of operands");
                    }
                    break;
                case "drawto":
                    if (words.Length == 2)
                    {
                        string[] parts = words[1].Split(',');
                        if (Int32.TryParse(parts[0], out int x) && Int32.TryParse(parts[1], out int y))
                        {
                            drawingClass.drawTo(x, y);
                        }
                    }
                    break;
                case "clear":
                    if (words.Length == 1)
                    {
                        drawingClass.clear();
                    }
                    colours.Clear();
                    colours.Add("red", (255, 0, 0, 255));
                    colours.Add("green", (0, 255, 0, 255));
                    colours.Add("blue", (0, 0, 255, 255));
                    colours.Add("black", (0, 0, 0, 255));
                    colours.Add("white", (255, 255, 255, 255));
                    break;
                default:
                    throw new Exception("Invalid command");
            }
        }

        public void executeLineHandler(string line, string script)
        {
            if (line == "run")
            {
                executeScript(script);
            }
            else
            {
                try
                {
                    executeLine(line);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            drawingClass.update();
        }

        public void executeScript(string script)
        {
            int i = 0;
            foreach (string line in script.Split('\n'))
            {
                i++;
                try
                {
                    executeLine(line);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("On line {0}: {1}", i, e.Message));
                }
            }
            drawingClass.update();
        }

    }
}
