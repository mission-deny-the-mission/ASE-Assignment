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

        public Circle(Color colour, int x, int y, float penWidth, bool fillState, int radius)
            : base (colour, penWidth, fillState)
        {
            this.radius = radius;
            this.x = x;
            this.y = y;
        }

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
