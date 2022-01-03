using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASE_Assignment;
using System.Drawing;

namespace ASE_Assignment_Unit_Tests
{
    class DebugDrawingClass : Drawer
    {
        protected List<Shape> shapes;
        protected Graphics graphics;

        public DebugDrawingClass(Graphics graphics)
        {
            this.graphics = graphics;
            shapes = new List<Shape>();
        }

        public void clear()
        {
            shapes.Clear();
        }


        public void update()
        {
            foreach (Shape shape in shapes)
            {
                shape.Paint(graphics);
            }
        }

        public void addShape(Shape shape)
        {
            shapes.Add(shape);
        }

        public void flash()
        {
            throw new NotImplementedException();
        }
    }
}
