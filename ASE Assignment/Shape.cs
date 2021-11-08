using System.Drawing;

namespace ASE_Assignment
{
    public abstract class Shape
    {
        protected Pen pen;
        protected SolidBrush brush;
        protected bool fillState;

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

        public abstract void Paint(System.Drawing.Graphics graphics);

        public Color GetColor()
        {
            return pen.Color;
        }

        public float GetWidth()
        {
            return pen.Width;
        }
    }
}