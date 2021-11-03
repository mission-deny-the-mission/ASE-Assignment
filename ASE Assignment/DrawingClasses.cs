using System.Collections.Generic;
using System.Drawing;

namespace ASE_Assignment
{
    interface Drawer
    {
        public void setPenColour((byte, byte, byte, byte) colour);
        public (byte, byte, byte, byte) getPenColour();
        public void setPenWidth(float width);
        public float getPenWidth();
        public void drawCircle(int x, int y, int radius);
        public void clear();
    }
    class Drawing : Drawer
    {
        (byte, byte, byte, byte) penColour;
        private Pen pen;
        protected List<Shape> shapes;
        public Drawing()
        {
            pen = new Pen(Color.Black, 2);
            shapes = new List<Shape>();
        }
        ~Drawing()
        {
            pen.Dispose();
        }
        public void Graphics_Paint(object sender, System.Windows.Forms.PaintEventArgs pe)
        {
            System.Drawing.Graphics g = pe.Graphics;
            foreach (Shape shape in shapes)
            {
                shape.Paint(g);
            }
        }
        public void setPenColour((byte, byte, byte, byte) colour)
        {
            pen.Color = Color.FromArgb(colour.Item4, colour.Item1, colour.Item2, colour.Item3);
        }
        public (byte, byte, byte, byte) getPenColour()
        {
            Color penColour = pen.Color;
            return (penColour.R, penColour.G, penColour.B, penColour.A);
        }
        public void setPenWidth(float width)
        {
            pen.Width = width;
        }
        public float getPenWidth()
        {
            return pen.Width;
        }
        public void drawCircle(int x, int y, int radius)
        {
            Circle circle = new Circle(pen.Color, x, y, pen.Width, radius);
            shapes.Add(circle);
        }
        public void clear()
        {
            shapes.Clear();
        }
    }

}