using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace ASE_Assignment
{
    public class Triangle : Polygon
    {
        public Triangle(Color colour, (int, int) point1, (int, int) point2, (int, int) point3, float penWidth, bool fillState)
            : base(colour, new (int, int)[] { point1, point2, point3 }, penWidth, fillState)
        {}
        
        public Triangle(Color colour, (int, int)[] points, float penWidth, bool fillState)
            : base(colour, points, penWidth, fillState)
        {
            if (points.Length != 3)
            {
                throw new Exception("Wrong number of points for a triangle.");
            }
        }
    }
}
