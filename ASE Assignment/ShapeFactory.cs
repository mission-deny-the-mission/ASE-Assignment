using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ASE_Assignment
{
    /// <summary>
    /// This is a class that can create Shape objects called a Factory.
    /// It retains certain state such as the current x and y position
    /// the current colour, and the width of the pen.
    /// It can also deal with some select flashing colours.
    /// It's responsible for parsing certain commands as well now.
    /// </summary>
    class ShapeFactory
    {
        public int x, y;
        protected Color color;
        protected float penWidth;
        public bool fillState;
        protected bool flashingColor;
        protected Color secondColor;

        // this  is important for processing expressions such as variables and equations
        protected ExpressionHandler expressionHandler;

        // a public method of getting the pen width
        public float accessPenWidth
        {
            get { return penWidth; }
            set
            {
                if (value > 0)
                    penWidth = value;
                else
                    throw new ArgumentOutOfRangeException("Width is negative");
            }
        }

        /// <summary>
        /// Constructor of the shape factory
        /// </summary>
        /// <param name="expressionHandler">the expression handler to use that is also shared with the CommandParser</param>
        public ShapeFactory(ExpressionHandler expressionHandler)
        {
            this.expressionHandler = expressionHandler;

            Clear();
        }

        /// <summary>
        /// Method to reset the factories states to the default. This is called by the constructor.
        /// </summary>
        public void Clear()
        {
            fillState = false;
            x = 0;
            y = 0;
            color = Color.Black;
            penWidth = 2;
        }

        /// <summary>
        /// Method to parse a set of coordinates.
        /// </summary>
        /// <param name="point">The points to parse seperated by a comma</param>
        /// <returns>The point after it has been parsed</returns>
        /// <exception cref="Exception">Triggered if the point is invalid and can't be parsed</exception>
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

        /// <summary>
        /// Public methods to get and set the colour as a byte.
        /// The methods here convert back and forth from a tuple of four bytes to a Color object
        /// </summary>
        public (byte, byte, byte, byte) byteColour
        {
            get
            {
                return (color.R, color.G, color.B, color.A);
            }
            set
            {
                flashingColor = false;
                color = Color.FromArgb(value.Item4, value.Item1, value.Item2, value.Item3);
            }
        }

        /// <summary>
        /// This is an internal method to help with parsing a circle and creating an object from it
        /// </summary>
        /// <param name="command"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected Circle parseCircle(string command, string[] words)
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
                    Circle circle =  new Circle(color, point, penWidth, fillState, radius);
                    if(flashingColor)
                        circle.setFlash(secondColor);
                    return circle;
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
                    Circle circle = new Circle(color, x, y, penWidth, fillState, radius);
                    if (flashingColor)
                        circle.setFlash(secondColor);
                    return circle;
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

        /// <summary>
        /// Method for parsing and creating a Triangle
        /// </summary>
        /// <param name="command">command to parse</param>
        /// <param name="words">the same command broken into words</param>
        /// <returns>the triangle that has been created</returns>
        /// <exception cref="Exception">Thrown if the triangle command in question is invalid</exception>
        protected Triangle parseTriangle(string command, string[] words)
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
                Triangle tri =  new Triangle(color, points, penWidth, fillState);
                if (flashingColor)
                    tri.setFlash(secondColor);
                return tri;
            }
            // if three arguments are not thrown produce an exception
            else
            {
                throw new Exception("Invalid number of operands for operation triangle");
            }
        }

        /// <summary>
        /// Method to create
        /// </summary>
        /// <param name="command"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected Shape parseRectangle(string command, string[] words)
        {
            // for three arguments we use the existing position
            if (words.Length == 3)
            {
                // parse the width and height and throw an exception if they are invalid
                if (expressionHandler.TryEvalValue(words[1], out int width) && expressionHandler.TryEvalValue(words[2], out int height))
                {
                    Rectangle rect = new Rectangle(color, x, y, width, height, penWidth, fillState);
                    if (flashingColor)
                        rect.setFlash(secondColor);
                    return rect;
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
                // we parse the coordinates using a method from the ExpressionHandler that can deal with variables as well as values
                if (expressionHandler.TryEvalValue(words[2], out int width) && expressionHandler.TryEvalValue(words[3], out int height))
                {
                    Rectangle rect = new Rectangle(color, x1, y1, width, height, penWidth, fillState);
                    if (flashingColor)
                        rect.setFlash(secondColor);
                    return rect;
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

        /// <summary>
        /// Method that parses the DrawTo line used to draw a line on the canvas
        /// </summary>
        /// <param name="command">parameter that contains the whole command line to be parsed</param>
        /// <param name="words">an array with the command broken down into seperate words</param>
        /// <returns>contains the line that has been created</returns>
        /// <exception cref="Exception">thrown if the command is invalid and cannot be parsed</exception>
        protected Line parseDrawLine(string command, string[] words)
        {
            if (words.Length == 2)
            {
                var (x1, y1) = parsePoint(words[1]);
                Line line = new Line(color, x, y, penWidth, fillState, x1, y1);
                (x, y) = (x1, y1);
                if (flashingColor)
                    line.setFlash(secondColor);
                return line;
            }
            else
            {
                throw new Exception("Invalid number of operands for command drawto");
            }
        }

        /// <summary>
        /// Method to set the colours to flashing.
        /// </summary>
        /// <param name="firstColor">Colour for the first half of the flash</param>
        /// <param name="secondColor">Colour for the second half of the flash</param>
        public void setFlash(Color firstColor, Color secondColor)
        {
            flashingColor = true;
            color = firstColor;
            this.secondColor = secondColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public Shape parseShape(string command, string[] words)
        {
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

        public void setProperty(string command, string[] words)
        {
            switch (words[0].ToLower())
            {
                case "redgreen":
                    setFlash(Color.Red, Color.Green);
                    break;
                case "blueyellow":
                    setFlash(Color.Blue, Color.Yellow);
                    break;
                case "blackwhite":
                    setFlash(Color.Black, Color.White);
                    break;
            }
        }
    }
}
