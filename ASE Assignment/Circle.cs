using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASE_Assignment
{
    public class Circle : Shape
    {
        protected int radius;
        protected int x, y;

        /// <summary>
        /// Constructor for making a new circle with seperate arguments for the x and y coordinate
        /// </summary>
        /// <param name="colour">what colour the circle should be</param>
        /// <param name="x">the x coordinate the center of the circle should be on</param>
        /// <param name="y">the y coordinate the center of the circle should be on</param>
        /// <param name="penWidth">The width of the circle if it's not filled. Is not used if it is filled</param>
        /// <param name="fillState">True for a filled shape and false for an outline</param>
        /// <param name="radius">radius of the circle (length from center to outer edge)</param>
        public Circle(Color colour, int x, int y, float penWidth, bool fillState, int radius)
            : base (colour, penWidth, fillState)
        {
            this.radius = radius;
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor for creating a new circle using one argument that is a tuple for the position/coordinates
        /// </summary>
        /// <param name="colour">what colour the circle should be</param>
        /// <param name="position">coordinates/position where the center of the circle should be</param>
        /// <param name="penWidth">The width of the circle if it's not filled. Is not used if it is filled</param>
        /// <param name="fillState">True for a filled shape and false for an outline</param>
        /// <param name="radius">radius of the circle (length from center to outer edge)</param>
        public Circle(Color colour, (int, int) position, float penWidth, bool fillState, int radius)
            : base(colour, penWidth, fillState)
        {
            this.radius = radius;
            (x, y) = position;
        }

        public (int, int) GetPosition()
        {
            return (x, y);
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void SetPosition((int, int) position)
        {
            (x, y) = position;
        }

        public int GetRadius()
        {
            return radius;
        }

        public void SetRadius(int radius)
        {
            this.radius = radius;
        }

        /// <summary>
        /// Function to paint the circle onto a Graphics class
        /// </summary>
        /// <param name="graphics">graphics class to paint onto</param>
        public override void Paint(Graphics graphics)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x - radius, y - radius, radius * 2, radius * 2);
            if (fillState)
                graphics.FillEllipse(brush, rect);
            else
                graphics.DrawEllipse(pen, rect);
        }
    }
}
