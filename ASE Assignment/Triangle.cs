using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace ASE_Assignment
{
    public class Triangle : Polygon
    {
        /// <summary>
        /// Constructor for a Triangle with three different arguments for the three seperate points
        /// </summary>
        /// <param name="colour">Colour the triangle should be</param>
        /// <param name="point1">pair of coordinates for the first point</param>
        /// <param name="point2"></param>
        /// <param name="point3">pair of coordinates for the last point</param>
        /// <param name="penWidth">how thick to draw the shape if it's an outline</param>
        /// <param name="fillState">true for a filled triangle and false for an outline of one</param>
        public Triangle(Color colour, (int, int) point1, (int, int) point2, (int, int) point3, float penWidth, bool fillState)
            : base(colour, new (int, int)[] { point1, point2, point3 }, penWidth, fillState)
        {}

        /// <summary>
        /// Constructor for a triangle with an array of three points
        /// </summary>
        /// <param name="colour">Colour the triangle should be</param>
        /// <param name="points">Array of points to draw the triangle. Must contain three and only three valid points</param>
        /// <param name="penWidth">how thick to draw the shape if it's an outline</param>
        /// <param name="fillState">true for a filled triangle and false for an outline of one</param>
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
