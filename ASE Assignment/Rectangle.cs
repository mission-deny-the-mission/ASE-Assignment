using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ASE_Assignment
{
    public class Rectangle : Shape
    {
        (int, int) position;
        int width, height;

        /// <summary>
        /// Constructor for a new rectangle with x and y coordinates as seperate arguments
        /// </summary>
        /// <param name="colour">Colour to make the rectangle</param>
        /// <param name="x">x coordinate to place the top left corner of the rectangle at</param>
        /// <param name="y">y coordinate to place the top left corner of the rectangle at</param>
        /// <param name="width">width of the rectangle</param>
        /// <param name="height">height of the rectangle</param>
        /// <param name="penWidth">How thick to draw the rectangle. Ignored if the shape if filled</param>
        /// <param name="fillState">true for filled, false for an outline</param>
        public Rectangle(Color colour, int x, int y, int width, int height, float penWidth, bool fillState)
            : base(colour, penWidth, fillState)
        {
            position = (x, y);
            this.width = width;
            this.height = height;
        }
        
        /// <summary>
        /// Constructor for a new rectangle with x and y coordinates as a tuple
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="penWidth"></param>
        /// <param name="fillState"></param>
        public Rectangle(Color colour, (int, int) position, int width, int height, float penWidth, bool fillState)
            : base(colour, penWidth, fillState)
        {
            this.position = position;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Gets the current position of the rectangle
        /// </summary>
        /// <returns></returns>
        public (int, int) GetPosition()
        {
            return position;
        }

        public void SetPosition(int x, int y)
        {
            position = (x, y);
        }

        public void SetPosition((int, int) position)
        {
            this.position = position;
        }

        public int GetWidth()
        {
            return width;
        }

        public void SetWidth(int width)
        {
            this.width = width;
        }

        public int GetHeight()
        {
            return height;
        }

        public void SetHeight(int height)
        {
            this.height = height;
        }

        /// <summary>
        /// Function to paint the rectangle onto a Graphics class
        /// </summary>
        /// <param name="graphics">graphics class to paint onto</param>
        public override void Paint(Graphics graphics)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(position.Item1, position.Item2, width, height);
            if (fillState)
                graphics.FillRectangle(brush, rect);
            else
                graphics.DrawRectangle(pen, rect);
        }
    }
}
