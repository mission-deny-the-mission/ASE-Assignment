using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASE_Assignment;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System;
using System.IO;

namespace ASE_Assignment_Unit_Tests
{

    [TestClass]
    public class CommandParserInvalidTests
    {
        public void TestCommand(string command)
        {
            DrawingClass drawingClass = new DrawingClass(new PictureBox());
            CommandParser parser = new CommandParser(drawingClass);

            parser.executeLine(command);
        }

        protected void TestScript(string script)
        {
            NoDraw noDraw = new NoDraw();
            CommandParser parser = new CommandParser(noDraw);
            Assert.IsFalse(parser.executeScript(script, false));
        }

        public void TestScriptFile(string filename)
        {
            filename = "..\\..\\..\\ScriptsForInvalidTests\\" + filename;
            using (StreamReader scriptFile = File.OpenText(filename))
            {
                string script = scriptFile.ReadToEnd();
                scriptFile.Close();
                TestScript(script);
            }
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
            TestCommand("MoveTo jkhkj jhkjhkh");
        }

        [TestMethod]
        public void AccessUndefinedVariable()
        {
            TestScript("drawto var1,var2");
        }

        [TestMethod]
        public void CallMethodWithWrongNumberOfParams()
        {
            TestScriptFile("wrong params 1.txt");
            TestScriptFile("wrong params 2.txt");
        }

        [TestMethod]
        public void MethodWithoutBrackets()
        {
            TestScriptFile("method without brackets.txt");
        }

        [TestMethod]
        public void AccessingVariablesOutOfScope()
        {
            TestScriptFile("access variable out of scope.txt");
            TestScriptFile("access variable out of scope 2.txt");
        }
    }
}
