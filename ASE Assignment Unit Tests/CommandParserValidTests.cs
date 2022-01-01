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
        public static bool CompareBitmapsFast(System.Drawing.Bitmap bmp1, System.Drawing.Bitmap bmp2)
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
            drawingClass.update();

            Assert.IsTrue(CompareBitmapsFast(bitmap1, bitmap2));
        }

        [TestMethod]
        public void ParserClassTest1()
        {
            testHelper(
                parser => { parser.executeLine("DrawTo 100,100"); },
                (graphics, pen) => {
                    graphics.DrawLine(pen, 0, 0, 100, 100);
                });
        }

        /*
        [TestMethod]
        public void ParserClassTest2()
        {
            DebugDrawingClass class1 = new DebugDrawingClass(new PictureBox());
            DebugDrawingClass class2 = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(class1);

            string script = "pen red\n" +
                "fill off\n" +
                "Position pen 100,100\n" +
                "DrawTo 200,100\n" +
                "fill on\n" +
                "Position pen 200,200\n" +
                "new colour teal 0 128 128\n" +
                "pen teal\n" +
                "circle 80\n" +
                "Position pen 200,290\n" +
                "new colour halfred 255 0 0 128\n" +
                "pen halfred\n" +
                "circle 80\n";
            parser.executeScript(script);

            class2.setPenColour((255, 0, 0, 255));
            class2.setFillState(false);
            class2.setPosition(100, 100);
            class2.drawTo(200, 100);
            class2.setFillState(true);
            class2.setPosition(200, 200);
            class2.setPenColour((0, 128, 128, 255));
            class2.drawCircle(80);
            class2.setPosition(200, 290);
            class2.setPenColour((255, 0, 0, 128));
            class2.drawCircle(80);

            CompareListOfShapes(class1.GetShapes(), class2.GetShapes(), 3);
        }

        [TestMethod]
        public void TriangleTest1()
        {
            DebugDrawingClass class1 = new DebugDrawingClass(new PictureBox());
            DebugDrawingClass class2 = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(class1);

            parser.executeLine("Triangle 100,100 100,200 200,100");
            class2.drawTriangle((100, 100), (100, 200), (200, 100));

            CompareListOfShapes(class1.GetShapes(), class2.GetShapes(), 1);
        }

        [TestMethod]
        public void RectangleTest1()
        {
            DebugDrawingClass class1 = new DebugDrawingClass(new PictureBox());
            DebugDrawingClass class2 = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(class1);

            string script = "position pen 50,50\n"
                + "rectangle 50 50\n"
                + "fill on\n"
                + "pen blue\n"
                + "rectangle 200,200 100 50";
            parser.executeScript(script);

            class2.setPosition(50, 50);
            class2.drawRectangle(50, 50);
            class2.setFillState(true);
            class2.setPenColour((0, 0, 255, 255));
            class2.drawRectangle(200, 200, 100, 50);

            CompareListOfShapes(class1.GetShapes(), class2.GetShapes(), 2);
        }

        [TestMethod]
        public void WidthTest()
        {
            DebugDrawingClass class1 = new DebugDrawingClass(new PictureBox());
            DebugDrawingClass class2 = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(class1);

            parser.executeLine("pen width 6.1");
            parser.executeLine("drawto 200,200");
            parser.executeLine("pen width 1");
            parser.executeLine("drawto 300,200");

            class2.setPenWidth(6.1f);
            class2.drawTo(200, 200);
            class2.setPenWidth(1f);
            class2.drawTo(300, 200);

            CompareListOfShapes(class1.GetShapes(), class2.GetShapes(), 2);
        }

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
