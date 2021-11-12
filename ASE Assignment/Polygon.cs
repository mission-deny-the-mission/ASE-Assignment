using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace ASE_Assignment
{
    /// <summary>
    /// Class to store information and draw shapes such as Triangles
    /// </summary>
    public class Polygon : Shape
    {
        protected (int, int)[] points;
        public Polygon(Color colour, (int, int)[] points, float penWidth, bool fillState)
            : base(colour, penWidth, fillState)
        {
            this.points = points;
        }

        /// <summary>
        /// Function to paint the polygon onto a Graphics class
        /// </summary>
        /// <param name="graphics">graphics class to paint onto</param>
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
