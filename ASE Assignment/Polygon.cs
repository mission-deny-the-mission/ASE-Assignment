using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace ASE_Assignment
{
    public class Polygon : Shape
    {
        protected (int, int)[] points;
        public Polygon(Color colour, (int, int)[] points, float penWidth, bool fillState)
            : base(colour, penWidth, fillState)
        {
            this.points = points;
        }

        public override void Paint(Graphics graphics)
        {
            Point[] points = (this.points.Select(pointTuple => new Point(pointTuple.Item1, pointTuple.Item2))).ToArray<Point>();
            if (fillState)
                graphics.FillPolygon(brush, points);
            else
                graphics.DrawPolygon(pen, points);
        }

        public (int, int)[] GetPoints()
        {
            return points;
        }
    }
}
