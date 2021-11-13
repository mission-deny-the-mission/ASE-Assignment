using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASE_Assignment;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System;

namespace ASE_Assignment_Unit_Tests
{

    [TestClass]
    public class CommandParserInvalidTests
    {
        public void TestCommand(string command)
        {
            DebugDrawingClass drawingClass = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(drawingClass);

            parser.executeLine(command);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Width is negative")]
        public void NegativeWidthTest()
        {
            TestCommand("pen width -1");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "invalid coordinate entered")]
        public void InvalidCoordInDrawTo()
        {
            TestCommand("drawto 50,x");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid command")]
        public void InvalidCommandTest()
        {
            TestCommand("asdfsdf");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Colour value is not a number")]
        public void InvalidColour()
        {
            TestCommand("new colour name a 123 123 123");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid operand for fill state command")]
        public void InvalidFillState()
        {
            TestCommand("Fill asdf");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Operand for pen width is not a valid number")]
        public void InvalidPenWidth()
        {
            TestCommand("pen width 5.x");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid number of operands")]
        public void InvalidMoveTo()
        {
            TestCommand("MoveTo");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "invalid coordinate entered")]
        public void InvalidCoordInMoveTo()
        {
            TestCommand("MoveTo 50,x");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid number of operands for command drawto")]
        public void InvalidMoveToNumberOfArguments()
        {
            TestCommand("MoveTo");
        }
    }
}
