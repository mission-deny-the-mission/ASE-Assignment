using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASE_Assignment;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace ASE_Assignment_Unit_Tests
{
    [TestClass]
    public class UnitTest1
    {
        public void CompareTwoShapes(Shape shape1, Shape shape2)
        {
            Assert.AreEqual(shape1.GetColor(), shape2.GetColor());
            Assert.AreEqual(shape1.GetPenWidth(), shape2.GetPenWidth());
            Assert.AreEqual(shape1.GetType(), shape1.GetType());

            if (shape1.GetType() == typeof(Line))
            {
                Line line1 = (Line)shape1;
                Line line2 = (Line)shape2;
                Assert.AreEqual(line1.GetPoints(), line2.GetPoints());
            }
            else if (shape1.GetType() == typeof(Circle))
            {
                Circle circle1 = (Circle)shape1;
                Circle circle2 = (Circle)shape2;
                Assert.AreEqual(circle1.GetPosition(), circle2.GetPosition());
                Assert.AreEqual(circle1.GetRadius(), circle2.GetRadius());
            }
            else if (shape1.GetType().IsSubclassOf(typeof(Polygon)))
            {
                Polygon polygon1 = (Triangle)shape1;
                Polygon polygon2 = (Triangle)shape2;
                Assert.IsTrue(Enumerable.SequenceEqual(polygon1.GetPoints(), polygon2.GetPoints()));
            }
            else if (shape1.GetType() == typeof(Rectangle))
            {
                Rectangle rect1 = (Rectangle)shape1;
                Rectangle rect2 = (Rectangle)shape2;
                Assert.AreEqual(rect1.GetPosition(), rect2.GetPosition());
                Assert.AreEqual(rect1.GetWidth(), rect2.GetWidth());
                Assert.AreEqual(rect1.GetHeight(), rect2.GetHeight());
            }
        }

        public void CompareListOfShapes(List<Shape> shapes1, List<Shape> shapes2)
        {
            Assert.AreEqual(shapes1.Count, shapes2.Count);
            for (int i = 0; i < shapes1.Count; i++)
            {
                CompareTwoShapes(shapes1[i], shapes2[i]);
            }
        }

        public void CompareListOfShapes(List<Shape> shapes1, List<Shape> shapes2, int length)
        {
            Assert.AreEqual(shapes1.Count, length);
            CompareListOfShapes(shapes1, shapes2);
        }

        [TestMethod]
        public void ParserClassTest1()
        {
            DebugDrawingClass class1 = new DebugDrawingClass(new PictureBox());
            DebugDrawingClass class2 = new DebugDrawingClass(new PictureBox());

            CommandParser commandParser = new CommandParser(class1);

            commandParser.executeLine("DrawTo 100,100");
            class2.drawTo(100, 100);

            List<Shape> shapes1 = class1.GetShapes();
            List<Shape> shapes2 = class2.GetShapes();

            CompareListOfShapes(shapes1, shapes2, 1);
        }

        [TestMethod]
        public void ParsetClasttTest2()
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
    }
}
