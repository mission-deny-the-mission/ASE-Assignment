using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASE_Assignment;
using System.Drawing;

namespace ASE_Assignment_Unit_Tests
{
    class DebugDrawingClass : DrawingClass
    {
        public DebugDrawingClass(System.Windows.Forms.PictureBox pb) : base(pb)
        {}

        public List<Shape> GetShapes()
        {
            return shapes;
        }
    }
}
