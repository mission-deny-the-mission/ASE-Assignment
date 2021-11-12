using System.Drawing;

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
            this.fillState = fillState;
            brush = new SolidBrush(colour);
            pen = new Pen(colour, penWidth);
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