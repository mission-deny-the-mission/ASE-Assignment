using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace ASE_Assignment
{
    public class Triangle : Shape
    {
        (int, int)[] points = new (int, int)[3];
        public Triangle(Color colour, (int, int) point1, (int, int) point2, (int, int) point3, float penWidth, bool fillState)
            : base(colour, penWidth, fillState)
        {
            points[0] = point1;
            points[1] = point2;
            points[2] = point3;
        }
        
        public Triangle(Color colour, (int, int)[] points, float penWidth, bool fillState)
            : base(colour, penWidth, fillState)
        {
            if (points.Length == 3)
            {
                this.points = points;
            }
            else
            {
                throw new Exception("Incorrect number of points for a triangle");
            }
        }

        public override void Paint(Graphics graphics)
        {
            Point[] points = (this.points.Select(pointTuple => new Point(pointTuple.Item1, pointTuple.Item2))).ToArray<Point>();
            if (fillState)
                graphics.FillPolygon(brush, points);
            else
                graphics.DrawPolygon(pen, points);
        }
    }
}
