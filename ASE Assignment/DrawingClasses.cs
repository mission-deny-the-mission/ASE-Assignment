using System.Collections.Generic;
using System.Drawing;
using System;

namespace ASE_Assignment
{
    public interface Drawer
    {
        public void clear();
        public void addShape(Shape shape);
        public void update();
    }
    public class DrawingClass : Drawer
    {
        protected List<Shape> shapes;
        protected System.Windows.Forms.PictureBox drawingArea;

        public DrawingClass(System.Windows.Forms.PictureBox drawingArea)
        {
            this.drawingArea = drawingArea;
            shapes = new List<Shape>();
        }

        public void Graphics_Paint(object sender, System.Windows.Forms.PaintEventArgs pe)
        {
            System.Drawing.Graphics g = pe.Graphics;
            foreach (Shape shape in shapes)
            {
                shape.Paint(g);
            }
        }

        public void clear()
        {
            shapes.Clear();
        }


        public void update()
        {
            drawingArea.Refresh();
        }

        public void addShape(Shape shape)
        {
            shapes.Add(shape);
        }
    }

}