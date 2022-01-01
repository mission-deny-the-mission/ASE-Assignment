using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ASE_Assignment
{
    class ShapeFactory
    {
        public int x, y;
        protected Color color;
        public float penWidth;
        public bool fillState;

        ExpressionHandler expressionHandler;
        Context context;

        public ShapeFactory(Context context)
        {
            this.context = context;
            expressionHandler = new ExpressionHandler(context);

            Clear();
        }

        public void Clear()
        {
            x = 0;
            y = 0;
            color = Color.Black;
            penWidth = 2;
        }

        public (int, int) parsePoint(string point)
        {
            string[] points = point.Split(',');
            if (points.Length != 2)
            {
                throw new Exception("Invalid coordinate entered");
            }
            if (expressionHandler.TryEvalValue(points[0], out int x) && expressionHandler.TryEvalValue(points[1], out int y))
            {
                return (x, y);
            }
            else
            {
                throw new Exception("Invalid coordinate entered");
            }
        }

        public (byte, byte, byte, byte) byteColour
        {
            get
            {
                return (color.R, color.G, color.B, color.A);
            }
            set
            {
                color = Color.FromArgb(value.Item4, value.Item1, value.Item2, value.Item3);
            }
        }

        protected Shape parseCircle(string command, string[] words)
        {
            // circle command. should take either 3 or 2 arguments
            // three arguments includes the postition to draw the circle in and the radius
            if (words.Length == 3)
            {
                // parse the coordinates for the position using a helper function
                // that is defined and implemented earlier
                (int, int) point = parsePoint(words[1]);
                if (expressionHandler.TryEvalValue(words[2], out int radius))
                {
                    
                    return new Circle(color, point, penWidth, fillState, radius);
                }
                // if the radius couldn't be parsed throw an error
                else
                {
                    throw new Exception("Invalid command");
                }
            }
            // if there are only two arguments the position is not included
            // and the current position should be used instead
            else if (words.Length == 2)
            {
                if (expressionHandler.TryEvalValue(words[1], out int radius))
                {
                    return new Circle(color, x, y, penWidth, fillState, radius);
                }
                else
                {
                    throw new Exception("Invalid operand for command circle");
                }
            }
            else
            {
                throw new Exception("Invalid command");
            }
        }

        protected Shape parseTriangle(string command, string[] words)
        {
            // triangles have three points so needs three arguments
            if (words.Length == 4)
            {
                // parse the three points using the helper function above
                // then call drawTriangle
                (int, int) point1 = parsePoint(words[1]);
                (int, int) point2 = parsePoint(words[2]);
                (int, int) point3 = parsePoint(words[3]);
                (int, int)[] points = { point1, point2, point3 };
                return new Triangle(color, points, penWidth, fillState);
            }
            // if three arguments are not thrown produce an exception
            else
            {
                throw new Exception("Invalid number of operands for operation triangle");
            }
        }

        protected Shape parseRectangle(string command, string[] words)
        {
            // for three arguments we use the existing position
            if (words.Length == 3)
            {
                // parse the width and height and throw an exception if they are invalid
                if (expressionHandler.TryEvalValue(words[1], out int width) && expressionHandler.TryEvalValue(words[2], out int height))
                {

                    return new Rectangle(color, x, y, width, height, penWidth, fillState);
                }
                else
                {
                    throw new Exception("Incorrectly formatted operands for command rectangle");
                }
            }
            // for 4 arguments we parse the coordinates and then do the same thing
            else if (words.Length == 4)
            {
                var (x1, y1) = parsePoint(words[1]);
                if (expressionHandler.TryEvalValue(words[2], out int width) && expressionHandler.TryEvalValue(words[3], out int height))
                {
                    return new Rectangle(color, x1, y1, width, height, penWidth, fillState);
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
        }

        protected Line parseDrawLine(string command, string[] words)
        {
            if (words.Length == 2)
            {
                var (x1, y1) = parsePoint(words[1]);
                Line line = new Line(color, x, y, penWidth, fillState, x1, y1);
                (x, y) = (x1, y1);
                return line;
            }
            else
            {
                throw new Exception("Invalid number of operands for command drawto");
            }
        }

        public Shape parseShape(string command)
        {
            string[] words = command.Split(" ");
            switch (words[0].ToLower())
            {
                case "circle":
                    return parseCircle(command, words);
                    break;
                case "triangle":
                    return parseTriangle(command, words);
                    break;
                case "rectangle":
                    return parseRectangle(command, words);
                    break;
                case "drawto":
                    return parseDrawLine(command, words);
                    break;
                default:
                    return null;
                    break;
            }
        }
    }
}
