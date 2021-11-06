using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ASE_Assignment
{
    class Line : Shape
    {
        protected int endX, endY;
        public Line(Color colour, int x, int y, float penWidth, bool fillState, int endX, int endY)
            : base (colour, x, y, penWidth, fillState)
        {
            this.endX = endX;
            this.endY = endY;
        }

        public override void Paint(Graphics graphics)
        {
            graphics.DrawLine(pen, x, y, endX, endY);
        }
    }
}
