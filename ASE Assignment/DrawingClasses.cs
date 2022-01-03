using System.Collections.Generic;
using System.Drawing;
using System;

namespace ASE_Assignment
{
    /// <summary>
    /// interface for a class that can store and draw shapes
    /// </summary>
    public interface Drawer
    {
        public void clear();
        public void addShape(Shape shape);
        public void update();
        public void flash();
    }
    /// <summary>
    /// An implementation of that interface that works with the PictureBox class.
    /// It has a function that acts as a paint handler.
    /// It can also force the PictureBox to update causing that paint handler to be called.
    /// Internally it has a list of shapes to be drawn
    /// </summary>
    public class DrawingClass : Drawer
    {
        protected List<Shape> shapes;
        protected System.Windows.Forms.PictureBox drawingArea;

        public DrawingClass(System.Windows.Forms.PictureBox drawingArea)
        {
            this.drawingArea = drawingArea;
            shapes = new List<Shape>();
        }

        /// <summary>
        /// This is the paint event handler that grabs a graphics object from the paint even arguments and draws on them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pe"></param>
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

        /// <summary>
        /// Method used for exporting the drawing to an image
        /// </summary>
        /// <param name="x">Width of the bitmap</param>
        /// <param name="y">Height of the bitmap</param>
        /// <returns>A bitmap image of the drawing</returns>
        public Bitmap generateBitmap(int x, int y)
        {
            Bitmap bitmap = new Bitmap(x, y);
            Graphics graphics = Graphics.FromImage(bitmap);
            foreach (Shape shape in shapes)
            {
                shape.Paint(graphics);
            }
            return bitmap;
        }

        /// <summary>
        /// This method is for making the shapes flash
        /// </summary>
        public void flash()
        {
            // call the method on each shape that makes them flash
            foreach(Shape shape in shapes)
            {
                shape.flashRunner();
            }
            // update and redraw the drawing area by calling the update function.
            update();
        }
    }

}