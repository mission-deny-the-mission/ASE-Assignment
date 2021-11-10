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
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Width is negative")]
        public void NegativeWidthTest()
        {
            DebugDrawingClass drawingClass = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(drawingClass);

            parser.executeLine("pen width -1");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "invalid coordinate entered")]
        public void InvalidCoordInDrawTo()
        {
            DebugDrawingClass drawingClass = new DebugDrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(drawingClass);

            parser.executeLine("drawto 50,x");
        }
    }
}
