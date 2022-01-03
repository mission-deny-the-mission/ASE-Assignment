using System.Drawing;
using System.Threading;

namespace ASE_Assignment
{
    /// <summary>
    /// Abstract shape class for different shapes
    /// </summary>
    public abstract class Shape
    {
        protected Pen pen;
        protected SolidBrush brush;
        protected bool fillState;

        protected Color color, secondColor;
        protected bool flashing, cycle;

        /// <summary>
        /// Constructor for a generic shape class.
        /// This is meant to be extended by other classes that implement differnt shapes with the extra
        /// infomation they need to be supplied as arguments
        /// </summary>
        /// <param name="colour">The colour the shape should have</param>
        /// <param name="penWidth">The width of the outline of the shape.
        /// This is not normally used during drawing if the shape is filled</param>
        /// <param name="fillState">true to fill the shape and false for an outline</param>
        protected Shape(Color colour, float penWidth, bool fillState)
        {
            color = colour;
            flashing = false;
            this.fillState = fillState;
            brush = new SolidBrush(colour);
            pen = new Pen(colour, penWidth);
            cycle = true;
        }

        ~Shape()
        {
            pen.Dispose();
            brush.Dispose();
        }

        /// <summary>
        /// Function used to actually draw the shape onto a Graphics object
        /// </summary>
        /// <param name="graphics">Graphics object to draw onto</param>
        public abstract void Paint(System.Drawing.Graphics graphics);

        public void setFlash(Color secondColor)
        {
            this.secondColor = secondColor;
            flashing = true;
        }

        public void flashRunner()
        {
            if (flashing)
            {
                if (cycle)
                {
                    pen.Color = secondColor;
                    brush.Dispose();
                    brush = new SolidBrush(secondColor);
                    cycle = false;
                }
                else
                {
                    pen.Color = color;
                    brush.Dispose();
                    brush = new SolidBrush(color);
                    cycle = true;
                }
            }
        }

        public Color GetColor()
        {
            return pen.Color;
        }

        public float GetPenWidth()
        {
            return pen.Width;
        }
    }
}