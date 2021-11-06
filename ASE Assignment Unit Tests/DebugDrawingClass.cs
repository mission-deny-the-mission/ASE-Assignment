using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASE_Assignment;

namespace ASE_Assignment_Unit_Tests
{
    class DebugDrawingClass : DrawingClass
    {
        public override DrawingClass()
        {
            pen = new Pen(Color.Black, 2);
            shapes = new List<Shape>();
            x = 0;
            y = 0;
            fillState = false;
        }

        public DebugDrawingClass()
        {
            base();
        }

        public Shapes[] GetShapes()
        {
            return shapes;
        }
    }
}
