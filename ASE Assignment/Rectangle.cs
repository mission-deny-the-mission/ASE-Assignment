using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ASE_Assignment
{
    public class Rectangle : Shape
    {
        (int, int) position;
        int width, height;

        public Rectangle(Color colour, int x, int y, int width, int height, float penWidth, bool fillState)
            : base(colour, penWidth, fillState)
        {
            position = (x, y);
            this.width = width;
            this.height = height;
        }
        
        public Rectangle(Color colour, (int, int) position, int width, int height, float penWidth, bool fillState)
            : base(colour, penWidth, fillState)
        {
            this.position = position;
            this.width = width;
            this.height = height;
        }

        public (int, int) GetPosition()
        {
            return position;
        }

        public void SetPosition(int x, int y)
        {
            position = (x, y);
        }

        public void SetPosition((int, int) position)
        {
            this.position = position;
        }

        public int GetWidth()
        {
            return width;
        }

        public void SetWidth(int width)
        {
            this.width = width;
        }

        public int GetHeight()
        {
            return height;
        }

        public void SetHeight(int height)
        {
            this.height = height;
        }

        public override void Paint(Graphics graphics)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(position.Item1, position.Item2, width, height);
            if (fillState)
                graphics.FillRectangle(brush, rect);
            else
                graphics.DrawRectangle(pen, rect);
        }
    }
}
