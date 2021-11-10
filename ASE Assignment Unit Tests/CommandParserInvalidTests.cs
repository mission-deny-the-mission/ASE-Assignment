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
    }
}
