using System.Drawing;

namespace ASE_Assignment
{
    abstract class Shape
    {
        protected int x, y;
        protected Pen pen;
        protected Shape(Color colour, int x, int y, float penWidth)
        {
            this.x = x;
            this.y = y;
            pen = new Pen(colour, penWidth);
        }
        ~Shape()
        {
            pen.Dispose();
        }
        public abstract void Paint(System.Drawing.Graphics graphics);
        public (int, int) GetPosition()
        {
            return (x, y);
        }
        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}