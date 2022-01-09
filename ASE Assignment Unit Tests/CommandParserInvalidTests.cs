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
        /// <summary>
        /// Used for executing lines and generating an exception if they are incorrect
        /// </summary>
        /// <param name="command">command to test</param>
        public void TestCommand(string command)
        {
            NoDraw noDraw = new NoDraw();
            CommandParser parser = new CommandParser(noDraw);

            parser.executeLine(command);
        }

        /// <summary>
        /// Used for running invalid scripts and making sure it registers as invalid
        /// </summary>
        /// <param name="script">script to test</param>
        protected void TestScript(string script)
        {
            NoDraw noDraw = new NoDraw();
            CommandParser parser = new CommandParser(noDraw);
            Assert.IsFalse(parser.executeScript(script, false));
        }

        /// <summary>
        /// Used for testing an invalid script stored in a file
        /// Opens and reads the script file then passes the contents to the TestScript method
        /// </summary>
        /// <param name="filename"></param>
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

        /// <summary>
        /// Test negative pen width gives an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Width is negative")]
        public void NegativeWidthTest()
        {
            TestCommand("pen width -1");
        }

        /// <summary>
        /// test using a random invalid command
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid command")]
        public void InvalidCommandTest()
        {
            TestCommand("asdfsdf");
        }

        /// <summary>
        /// test using an invalid value for settings a new colour
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Colour value is not a number")]
        public void InvalidColour()
        {
            TestCommand("new colour name a 123 123 123");
        }

        /// <summary>
        /// test the fill command with something that isn't true or false
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid operand for fill state command")]
        public void InvalidFillState()
        {
            TestCommand("Fill asdf");
        }

        /// <summary>
        /// test setting the pen width with something that isn't a number
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Operand for pen width is not a valid number")]
        public void InvalidPenWidth()
        {
            TestCommand("pen width 5.x");
        }

        /// <summary>
        /// Try the moveto command with no parameters
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid number of operands")]
        public void InvalidMoveTo()
        {
            TestCommand("MoveTo");
        }

        /// <summary>
        /// Try the moveto command with something that isn't a number or a variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "invalid coordinate entered")]
        public void InvalidCoordInMoveTo()
        {
            TestCommand("MoveTo 50,x");
        }

        /// <summary>
        /// test for invalid number of arguments on the MoveTo command
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid number of operands for command moveto")]
        public void InvalidMoveToNumberOfArguments()
        {
            TestCommand("MoveTo jkhkj jhkjhkh");
        }

        /// <summary>
        /// test for checking if undefined variables give a syntax error
        /// </summary>
        [TestMethod]
        public void AccessUndefinedVariable()
        {
            TestScript("drawto var1,var2");
            TestScript("var = x + y");
            TestScript("var2 = var1");
        }

        /// <summary>
        /// test for passing the wrong number of parameters to a method
        /// </summary>
        [TestMethod]
        public void CallMethodWithWrongNumberOfParams()
        {
            TestScriptFile("wrong params 1.txt");
            TestScriptFile("wrong params 2.txt");
        }

        /// <summary>
        /// test for creating a method without the required brackets
        /// </summary>
        [TestMethod]
        public void MethodWithoutBrackets()
        {
            TestScriptFile("method without brackets.txt");
        }

        /// <summary>
        /// test for trying to access a variable after it's scope has expired
        /// there is one script file for a while loop and one for a method
        /// </summary>
        [TestMethod]
        public void AccessingVariablesOutOfScope()
        {
            TestScriptFile("access variable out of scope.txt");
            TestScriptFile("access variable out of scope 2.txt");
        }

        /// <summary>
        /// test for a method call without the required brackets
        /// </summary>
        [TestMethod]
        public void MethodCallWithNoBrackets()
        {
            TestScriptFile("method call with no brackets.txt");
        }

        /// <summary>
        /// tests for calling a method that has not been created
        /// </summary>
        [TestMethod]
        public void CallMethodThatDoesNotExist()
        {
            TestScript("call mymethod(5)");
        }
    }
}
