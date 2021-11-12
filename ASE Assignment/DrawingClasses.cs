﻿using System.Collections.Generic;
using System.Drawing;
using System;

namespace ASE_Assignment
{
    public interface Drawer
    {
        public void setPenColour((byte, byte, byte, byte) colour);
        public (byte, byte, byte, byte) getPenColour();
        public void setPenWidth(float width);
        public float getPenWidth();
        public void drawCircle(int x, int y, int radius);
        public void drawCircle((int, int) position, int radius);
        public void drawCircle(int radius);
        public void drawTriangle((int, int) point1, (int, int) point2, (int, int) point3);
        void drawRectangle(int x, int y, int width, int height);
        void drawRectangle(int width, int height);
        void drawTo(int endX, int endY);
        public void clear();
        public (int, int) getPosition();
        public void setPosition(int x, int y);
        public void setPosition((int, int) point);
        public bool getFillState();
        public void setFillState(bool fillState);
        public void update();
    }
    public class DrawingClass : Drawer
    {
        (byte, byte, byte, byte) penColour;
        private Pen pen;
        protected List<Shape> shapes;
        protected int x, y;
        protected bool fillState;
        protected System.Windows.Forms.PictureBox drawingArea;

        public DrawingClass(System.Windows.Forms.PictureBox drawingArea)
        {
            this.drawingArea = drawingArea;
            pen = new Pen(Color.Black, 2);
            shapes = new List<Shape>();
            x = 0;
            y = 0;
            fillState = false;
        }

        ~DrawingClass()
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
            if (width < 0)
                throw new ArgumentOutOfRangeException("Width is negative");
            pen.Width = width;
        }

        public float getPenWidth()
        {
            return pen.Width;
        }

        public void drawCircle(int x, int y, int radius)
        {
            Circle circle = new Circle(pen.Color, x, y, pen.Width, fillState, radius);
            shapes.Add(circle);
        }

        public void drawCircle((int, int) position, int radius)
        {
            Circle circle = new Circle(pen.Color, position, pen.Width, fillState, radius);
            shapes.Add(circle);
        }

        public void drawCircle(int radius)
        {
            Circle circle = new Circle(pen.Color, x, y, pen.Width, fillState, radius);
            shapes.Add(circle);
        }

        public void drawTriangle((int, int) point1, (int, int) point2, (int, int) point3)
        {
            Triangle triangle = new Triangle(pen.Color, point1, point2, point3, pen.Width, fillState);
            shapes.Add(triangle);
        }

        public void drawTo(int endX, int endY)
        {
            Line line = new Line(pen.Color, x, y, pen.Width, fillState, endX, endY);
            shapes.Add(line);
            x = endX;
            y = endY;
        }

        public void clear()
        {
            shapes.Clear();
        }

        public (int, int) getPosition()
        {
            return (x, y);
        }

        public void setPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void setPosition((int, int) point)
        {
            x = point.Item1;
            y = point.Item2;
        }

        public bool getFillState()
        {
            return fillState;
        }

        public void setFillState(bool fillState)
        {
            this.fillState = fillState;
        }

        public void update()
        {
            drawingArea.Refresh();
        }

        public void drawRectangle(int x, int y, int width, int height)
        {

            Rectangle rect = new Rectangle(pen.Color, x, y, width, height, pen.Width, fillState);
            shapes.Add(rect);
        }
        
        public void drawRectangle(int width, int height)
        {
            Rectangle rect = new Rectangle(pen.Color, x, y, width, height, pen.Width, fillState);
            shapes.Add(rect);
        }
    }

}