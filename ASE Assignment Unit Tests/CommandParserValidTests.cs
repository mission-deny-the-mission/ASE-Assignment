using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASE_Assignment;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System;
using System.Drawing.Imaging;
using System.Drawing;

namespace ASE_Assignment_Unit_Tests
{
    [TestClass]
    public class CommandParserValidTests
    {
        public static bool CompareBitmapsFast(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            int bytes = bmp1.Width * bmp1.Height * (System.Drawing.Image.GetPixelFormatSize(bmp1.PixelFormat) / 8);

            bool result = true;
            byte[] b1bytes = new byte[bytes];
            byte[] b2bytes = new byte[bytes];

            System.Drawing.Imaging.BitmapData bitmapData1 = bmp1.LockBits(new System.Drawing.Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, bmp1.PixelFormat);
            System.Drawing.Imaging.BitmapData bitmapData2 = bmp2.LockBits(new System.Drawing.Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            System.Runtime.InteropServices.Marshal.Copy(bitmapData1.Scan0, b1bytes, 0, bytes);
            System.Runtime.InteropServices.Marshal.Copy(bitmapData2.Scan0, b2bytes, 0, bytes);

            for (int n = 0; n <= bytes - 1; n++)
            {
                if (b1bytes[n] != b2bytes[n])
                {
                    result = false;
                    break;
                }
            }

            bmp1.UnlockBits(bitmapData1);
            bmp2.UnlockBits(bitmapData2);

            return result;
        }

        protected void testHelper(Action<CommandParser> test, Action<System.Drawing.Graphics, System.Drawing.Pen> control)
        {
            Bitmap bitmap1 = new Bitmap(640, 480);
            Bitmap bitmap2 = new Bitmap(640, 480);

            Graphics graphics1 = Graphics.FromImage(bitmap1);
            Graphics graphics2 = Graphics.FromImage(bitmap2);

            DebugDrawingClass drawingClass = new DebugDrawingClass(graphics1);
            CommandParser commandParser = new CommandParser(drawingClass);

            Pen pen = new Pen(Color.Black, 2);

            test(commandParser);
            control(graphics2, pen);

            bitmap1.Save("C:\\Users\\Harry Hall\\test.bmp");
            bitmap2.Save("C:\\Users\\Harry Hall\\control.bmp");

            Assert.IsTrue(CompareBitmapsFast(bitmap1, bitmap2));
        }

        [TestMethod]
        public void ParserClassTest1()
        {
            testHelper(
                parser => { parser.executeLineHandler("DrawTo 100,100", ""); },
                (graphics, pen) => {
                    graphics.DrawLine(pen, 0, 0, 100, 100);
                });
        }

        [TestMethod]
        public void ParserClassTest2()
        {
            void test(CommandParser parser)
            {
                string script = "pen red\n" +
                    "fill off\n" +
                    "Position pen 100,100\n" +
                    "DrawTo 200,100\n" +
                    "fill on\n" +
                    "Position pen 200,200\n" +
                    "new colour teal 0 255 255\n" +
                    "pen teal\n" +
                    "circle 80\n" +
                    "Position pen 200,290\n" +
                    "new colour halfred 255 0 0 128\n" +
                    "pen halfred\n" +
                    "circle 80\n";
                parser.executeScript(script);
            }
            void control(Graphics graphics, Pen pen)
            {
                pen.Color = Color.FromArgb(255, 255, 0, 0);
                graphics.DrawLine(pen, 100, 100, 200, 100);
                Brush brush = new SolidBrush(Color.FromArgb(255, 0, 255, 255));
                int x = 200, y = 200, radius = 80;
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x - radius, y - radius, radius * 2, radius * 2);
                graphics.FillEllipse(brush, rect);
                brush = new SolidBrush(Color.FromArgb(128, 255, 0, 0));
                y = 290;
                rect = new System.Drawing.Rectangle(x - radius, y - radius, radius * 2, radius * 2);
                graphics.FillEllipse(brush, rect);
            }
            testHelper(test, control);
        }


        [TestMethod]
        public void TriangleTest1()
        {
            testHelper(
                (parser) => { parser.executeLineHandler("Triangle 100,100 100,200 200,100", ""); },
                (graphics, pen) => {
                    Point[] points =
                        new Point[] { new Point(100, 100), new Point(100, 200), new Point(200, 100) };
                    graphics.DrawPolygon(pen, points);
                });
        }

        [TestMethod]
        public void RectangleTest1()
        {
            void test(CommandParser parser)
            {
                string script = "position pen 50,50\n"
                    + "rectangle 50 50\n"
                    + "fill on\n"
                    + "pen blue\n"
                    + "rectangle 200,200 100 50";
                parser.executeScript(script);
            }
            void control(Graphics graphics, Pen pen)
            {
                graphics.DrawRectangle(pen, 50, 50, 50, 50);
                Brush brush = new SolidBrush(Color.Blue);
                graphics.FillRectangle(brush, 200, 200, 100, 50);
            }
            testHelper(test, control);
        }

        [TestMethod]
        public void WidthTest()
        {
            void test(CommandParser parser)
            {
                parser.executeLine("pen width 6.1");
                parser.executeLine("drawto 200,200");
                parser.executeLine("pen width 1");
                parser.executeLineHandler("drawto 300,200", "");
            }
            void control(Graphics graphics, Pen pen)
            {
                pen.Width = 6.1f;
                graphics.DrawLine(pen, 0, 0, 200, 200);
                pen.Width = 1f;
                graphics.DrawLine(pen, 200, 200, 300, 200);
            }
            testHelper(test, control);
        }

        /*
        [TestMethod]
        public void MoveToTest()
        {
            DebugDrawingClass class1 = new DebugDrawingClass(new PictureBox());
            DebugDrawingClass class2 = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(class1);

            parser.executeLine("moveto 100,100");
            parser.executeLine("circle 80");

            class2.setPosition(100, 100);
            class2.drawCircle(80);

            CompareListOfShapes(class1.GetShapes(), class2.GetShapes(), 1);
        }

        [TestMethod]
        public void ClearTest()
        {
            DebugDrawingClass class1 = new DebugDrawingClass(new PictureBox());
            DebugDrawingClass class2 = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(class1);

            parser.executeLine("moveto 200,200");
            parser.executeLine("pen 200 200 200 200");
            parser.executeLine("fill on");
            parser.executeLine("circle 40");
            parser.executeLine("clear");

            parser.executeLine("rectangle 100 100");

            class2.drawRectangle(100, 100);

            CompareListOfShapes(class1.GetShapes(), class2.GetShapes(), 1);
        }

        [TestMethod]
        public void PenColourTest()
        {
            DebugDrawingClass class1 = new DebugDrawingClass(new PictureBox());
            DebugDrawingClass class2 = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(class1);

            parser.executeLine("pen 128 128 0 255");
            parser.executeLine("circle 50");
            parser.executeLine("pen 0 0 0");
            parser.executeLine("rectangle 100,100 80 80");
            parser.executeLine("pen red");
            parser.executeLine("triangle 200,200 300,200 200,300");

            class2.setPenColour((128, 128, 0, 255));
            class2.drawCircle(50);
            class2.setPenColour((0, 0, 0, 255));
            class2.drawRectangle(100, 100, 80, 80);
            class2.setPenColour((255, 0, 0, 255));
            class2.drawTriangle((200, 200), (300, 200), (200, 300));

            CompareListOfShapes(class1.GetShapes(), class2.GetShapes(), 3);
        }
        */
    }
}
