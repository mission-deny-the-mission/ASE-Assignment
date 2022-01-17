using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASE_Assignment;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace ASE_Assignment_Unit_Tests
{
    [TestClass]
    public class CommandParserValidTests
    {
        /// <summary>
        /// Function to compare two bitmaps. This was found at: http://csharpexamples.com/c-fast-bitmap-compare/
        /// </summary>
        /// <param name="bmp1">The first bitmap</param>
        /// <param name="bmp2">The second bitmap</param>
        /// <returns>This is true if the bitmaps are the same</returns>
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

            BitmapData bitmapData1 = bmp1.LockBits(new System.Drawing.Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, bmp1.PixelFormat);
            BitmapData bitmapData2 = bmp2.LockBits(new System.Drawing.Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

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

        /// <summary>
        /// This is used to create the valid unit tests. It works by taking two functions, one that executed arguments aginst a Command Parser class.
        /// The other executes directly against a graphics area. The two resultant bitmaps are then compared.
        /// This is used to test the CommandParser class the shape classes. It has to use a custom implementation of the Drawer interface
        /// that is defined in another file.
        /// </summary>
        /// <param name="test">Takes a function with the command parser class as an argument</param>
        /// <param name="control">Takes a function that is passed a pen and a graphics class</param>
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
            
            // this is used for debugging purposes to look at the two bitmaps.
            // bitmap1.Save("C:\\Users\\Harry Hall\\Image1 - test.bmp");
            // bitmap2.Save("C:\\Users\\Harry Hall\\Image2 - control.bmp");

            Assert.IsTrue(CompareBitmapsFast(bitmap1, bitmap2));
        }

        // methods to make drawing a circle against a graphics class easier.
        protected void drawCircle(Graphics graphics, Pen pen, int x, int y, int radius)
        {
            graphics.DrawEllipse(pen, x - radius, y - radius, radius * 2, radius * 2);
        }
        protected void drawCircle(Graphics graphics, Pen pen, int radius)
        {
            int x = 0, y = 0;
            graphics.DrawEllipse(pen, x - radius, y - radius, radius * 2, radius * 2);
        }
        protected void fillCircle(Graphics graphics, Brush brush, int x, int y, int radius)
        {
            graphics.FillEllipse(brush, x - radius, y - radius, radius * 2, radius * 2);
        }
        protected void fillCircle(Graphics graphics, Brush brush, int radius)
        {
            int x = 0, y = 0;
            graphics.FillEllipse(brush, x - radius, y - radius, radius * 2, radius * 2);
        }
        
        /// <summary>
        /// This tests the DrawTo command of the CommandParser
        /// </summary>
        [TestMethod]
        public void ParserClassTest1()
        {
            // Here a lambda is used because the method bodies are only one line and it seemed excessive to put a nested function here.
            // Another name for a lambda is an anonymous function, they are functions without a name that can be passed as an argument.
            testHelper(
                parser => { parser.executeLineHandler("DrawTo 100,100", ""); },
                (graphics, pen) => {
                    graphics.DrawLine(pen, 0, 0, 100, 100);
                });
        }

        /// <summary>
        /// This test a variety of commands including setting custom colours and transparency.
        /// It also tests the ability to run scripts.
        /// </summary>
        [TestMethod]
        public void ParserClassTest2()
        {
            // unlike methods functions embedded in a method don't require an access modifier like public or private
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
                fillCircle(graphics, brush, x, y, radius);
                brush = new SolidBrush(Color.FromArgb(128, 255, 0, 0));
                y = 290;
                fillCircle(graphics, brush, x, y, radius);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// Tests the triangle command
        /// </summary>
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

        /// <summary>
        /// Tests the position pen and fill commands with a triangle
        /// </summary>
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

        /// <summary>
        /// tests the drawto command and pen width
        /// </summary>
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

        /// <summary>
        /// Tests moveto by drawing a circle
        /// </summary>
        [TestMethod]
        public void MoveToTest()
        {
            void test(CommandParser parser)
            {
                parser.executeLine("moveto 100,100");
                parser.executeLineHandler("circle 80", "");
            }
            void control(Graphics graphics, Pen pen)
            {
                drawCircle(graphics, pen, 100, 100, 80);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// this is here to test the clear command
        /// It's not really possible to test the reset command as it's part of the Form1 class and not CommandParser
        /// </summary>
        [TestMethod]
        public void ClearTest()
        {
            void test(CommandParser parser)
            {
                parser.executeLine("moveto 200,200");
                parser.executeLine("pen 200 200 200 200");
                parser.executeLine("fill on");
                parser.executeLine("circle 40");
                parser.executeLine("clear");

                parser.executeLineHandler("rectangle 100 100", "");
            }
            void control(Graphics graphics, Pen pen)
            {
                Brush brush = new SolidBrush(Color.FromArgb(200, 200, 200, 200));
                graphics.FillRectangle(brush, 200, 200, 100, 100);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// This tests setting different colours without defining them using the pen command
        /// </summary>
        [TestMethod]
        public void PenColourTest()
        {
            void test(CommandParser parser)
            {
                parser.executeLine("pen 128 128 0 255");
                parser.executeLine("circle 50");
                parser.executeLine("pen 0 0 0");
                parser.executeLine("rectangle 100,100 80 80");
                parser.executeLine("pen red");
                parser.executeLineHandler("triangle 200,200 300,200 200,300", "");
            }
            void control(Graphics graphics, Pen pen)
            {
                pen.Color = Color.FromArgb(255, 128, 128, 0);
                graphics.DrawEllipse(pen, -50, -50, 100, 100);
                pen.Color = Color.Black;
                graphics.DrawRectangle(pen, 100, 100, 80, 80);
                pen.Color = Color.Red;
                Point[] points =
                    new Point[] { new Point(200, 200), new Point(300, 200), new Point(200, 300) };
                graphics.DrawPolygon(pen, points);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// This tests defining a method and calling it.
        /// </summary>
        [TestMethod]
        public void MethodTest()
        {
            void test(CommandParser parser)
            {
                // For the newer tests I have choosen to change my approach and put the scripts in a seperate class
                // that is loaded as part of the test method.
                using (StreamReader methodFile = File.OpenText("..\\..\\..\\ScriptsForTests\\method.txt"))
                {
                    string script = methodFile.ReadToEnd();
                    parser.executeScript(script);
                }
            }
            void control(Graphics graphics, Pen pen)
            {
                Pen pen2 = new Pen(Color.Red, 2);
                drawCircle(graphics, pen2, 100);
                graphics.DrawRectangle(pen, 200, 200, 50, 50);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// This tests using a recursive method as I wanted recursion to work in my program
        /// </summary>
        [TestMethod]
        public void RecursiveMethodTest()
        {
            void test(CommandParser parser)
            {
                using (StreamReader methodFile = File.OpenText("..\\..\\..\\ScriptsForTests\\recursion.txt"))
                {
                    string script = methodFile.ReadToEnd();
                    parser.executeScript(script);
                }
            }
            void control(Graphics graphics, Pen pen)
            {
                pen.Color = Color.Red;
                for (int radius = 100; radius > 0; radius -= 20)
                {
                    drawCircle(graphics, pen, 100, 100, radius);
                }
                pen.Color = Color.Black;
                graphics.DrawRectangle(pen, 200, 200, 50, 50);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// Unit test for a method without any parameters
        /// </summary>
        [TestMethod]
        public void MethodWithNoParameterTest()
        {
            void test(CommandParser parser)
            {
                using (StreamReader scriptFile = File.OpenText("..\\..\\..\\ScriptsForTests\\method without parameters.txt"))
                {
                    string script = scriptFile.ReadToEnd();
                    parser.executeScript(script);
                }
            }
            void control(Graphics graphics, Pen pen)
            {
                drawCircle(graphics, pen, 50);
                graphics.DrawRectangle(pen, 100, 100, 60, 60);
            }
            testHelper(test, control);
        }

        [TestMethod]
        public void MethodWithManyParametersTest()
        {
            void test(CommandParser parser)
            {
                using (StreamReader scriptFile = File.OpenText("..\\..\\..\\ScriptsForTests\\method with many parameters.txt"))
                {
                    string script = scriptFile.ReadToEnd();
                    parser.executeScript(script);
                }
            }
            void control(Graphics graphics, Pen pen)
            {
                graphics.DrawRectangle(pen, 0, 0, 100, 200);
                pen.Color = Color.FromArgb(255, 100, 200, 0);
                drawCircle(graphics, pen, 100, 200, 50);
                drawCircle(graphics, pen, 100, 200, 250);
                pen.Color = Color.FromArgb(255, 100, 200, 250);
                graphics.DrawLine(pen, 0, 0, 100, 100);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// This method tests using a while loop.
        /// </summary>
        [TestMethod]
        public void WhileLoopTest()
        {
            void test(CommandParser parser)
            {
                using (StreamReader scriptFile = File.OpenText("..\\..\\..\\ScriptsForTests\\while loop.txt"))
                {
                    string script = scriptFile.ReadToEnd();
                    parser.executeScript(script);
                }
            }
            void control(Graphics graphics, Pen pen)
            {
                drawCircle(graphics, pen, 50);
                drawCircle(graphics, pen, 100);
                drawCircle(graphics, pen, 150);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// Method to test nested while loops as I also wanted that feature to work in my program.
        /// </summary>
        [TestMethod]
        public void NestedWhileLoopTest()
        {
            void test(CommandParser parser)
            {
                using (StreamReader scriptFile = File.OpenText("..\\..\\..\\ScriptsForTests\\nested while loop.txt"))
                {
                    string script = scriptFile.ReadToEnd();
                    parser.executeScript(script);
                }
            }
            void control(Graphics graphics, Pen pen)
            {
                drawCircle(graphics, pen, 40);
                drawCircle(graphics, pen, 80);
                drawCircle(graphics, pen, 40);
                drawCircle(graphics, pen, 160);
                drawCircle(graphics, pen, 120);
                drawCircle(graphics, pen, 240);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// Unit test for if statements
        /// </summary>
        [TestMethod]
        public void IfStatementTest()
        {
            void test(CommandParser parser)
            {
                using (StreamReader scriptFile = File.OpenText("..\\..\\..\\ScriptsForTests\\if statement.txt"))
                {
                    string script = scriptFile.ReadToEnd();
                    parser.executeScript(script);
                }
            }
            void control(Graphics graphics, Pen pen)
            {
                drawCircle(graphics, pen, 100);
                pen.Color = Color.Red;
                drawCircle(graphics, pen, 200);
            }
            testHelper(test, control);
        }

        /// <summary>
        /// Unit test for nested if statements
        /// </summary>
        [TestMethod]
        public void NestedIfStatementTest()
        {
            void test(CommandParser parser)
            {
                using (StreamReader scriptFile = File.OpenText("..\\..\\..\\ScriptsForTests\\nested if statements.txt"))
                {
                    string script = scriptFile.ReadToEnd();
                    parser.executeScript(script);
                }
            }
            void control(Graphics graphics, Pen pen)
            {
                drawCircle(graphics, pen, 100);
            }
            testHelper(test, control);
        }

        [TestMethod]
        public void VariableTest1()
        {
            void test(CommandParser parser)
            {
                using (StreamReader scriptFile = File.OpenText("..\\..\\..\\ScriptsForTests\\variable test 1.txt"))
                {
                    string script = scriptFile.ReadToEnd();
                    parser.executeScript(script);
                }
            }
            void control(Graphics graphics, Pen pen)
            {
                graphics.DrawRectangle(pen, 100, 150, 200, 100);
                drawCircle(graphics, pen, 200, 100, 150);
                graphics.DrawLine(pen, 0, 0, 100, 100);
                drawCircle(graphics, pen, 100, 100, 150);
                pen.Color = Color.FromArgb(255, 100, 150, 200);
                graphics.DrawLine(pen, 100, 100, 200, 200);
                pen.Color = Color.FromArgb(100, 100, 150, 200);
                graphics.DrawLine(pen, 200, 200, 100, 100);
                graphics.DrawLine(pen, 100, 100, 300, 300);
                pen.Color = Color.Black;
                graphics.DrawLine(pen, 300, 300, 100, 50);
            }
            testHelper(test, control);
        }
    }
}
