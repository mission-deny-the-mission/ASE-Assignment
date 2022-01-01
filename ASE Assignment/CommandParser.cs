using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASE_Assignment
{
    public class CommandParser
    {
        Drawer drawingClass;
        Dictionary<string, (byte, byte, byte, byte)> colours = new Dictionary<string, (byte, byte, byte, byte)>();
        Context context;
        ExpressionHandler expressionHandler;
        ShapeFactory shapeFactory;

        bool processLine;

        int methodStartLine;
        string methodName;
        string[] methodParameters;
        bool methodInProgress = false;

        int lineNumber;

        public CommandParser(Drawer drawingClass)
        {
            this.drawingClass = drawingClass;
            colours.Add("red", (255, 0, 0, 255));
            colours.Add("green", (0, 255, 0, 255));
            colours.Add("blue", (0, 0, 255, 255));
            colours.Add("black", (0, 0, 0, 255));
            colours.Add("white", (255, 255, 255, 255));
            context = new Context();
            expressionHandler = new ExpressionHandler(context);
            processLine = true;
            shapeFactory = new ShapeFactory(expressionHandler);
        }

        // function to decode a colour entered by the user into a series of bytes representing the different colour channels
        private (byte, byte, byte, byte) decodeColour(string colour)
        {
            string lowerColour = colour.ToLower();
            if (colours.ContainsKey(lowerColour))
            {
                return colours[lowerColour];
            }
            else
            {
                throw new Exception();
            }
        }

        // helper function to parse a coordinate entered by the user
        public (int, int) parsePoint(string point)
        {
            string[] points = point.Split(',');
            if (points.Length != 2)
            {
                throw new Exception("Invalid coordinate entered");
            }
            if (expressionHandler.TryEvalValue(points[0], out int x) && expressionHandler.TryEvalValue(points[1], out int y))
            {
                return (x, y);
            }
            else
            {
                throw new Exception("Invalid coordinate entered");
            }
        }

        protected string[] parseParametersAndName(string stringToParse, out string name)
        {
            name = "";
            int currentPosition;
            for (currentPosition = 0; stringToParse[currentPosition] != '('; currentPosition++)
            {
                name = name + stringToParse[currentPosition];
            }
            string parameterString = "";
            for (currentPosition++; stringToParse[currentPosition] != ')'; currentPosition++)
            {
                parameterString = parameterString + stringToParse[currentPosition];
            }
            if (parameterString.Length > 0)
            {
                Stack<string> parameters = new Stack<string>();
                string paramname = "";
                int i;
                bool spacePermitted = true;
                for (i = 0; i < parameterString.Length; i++)
                {
                    char currentchar = parameterString[i];
                    if (currentchar == ',')
                    {
                        if (paramname.Length == 0)
                            throw new Exception("Cannot have an empty paramater name");
                        parameters.Push(paramname);
                        spacePermitted = true;
                    }
                    else if (currentchar == ' ' && spacePermitted) { }
                    else if (currentchar == ' ' && !spacePermitted)
                    {
                        throw new Exception("Canot have a space inside a paramter name");
                    }
                    else
                    {
                        if (spacePermitted)
                            spacePermitted = false;
                        paramname = paramname + currentchar;
                    }
                }
                parameters.Push(paramname);
                if (i != parameterString.Length)
                    throw new Exception("List of parameters is invalid");
                return parameters.ToArray();
            }
            else
            {
                return new string[] { };
            }
        }

        public void executeLine(string line)
        {
            executeLine(line, 0);
        }

        // function to execute a command enterd by the user contained within a string
        public void executeLine(string line, int lineno)
        {
            // remove the new line character at the end of the string if it's present
            line = line.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            // if the line is empty return and do nothing
            if (line == "")
            {
                return;
            }
            // split the command into words based on spaces
            string[] words = line.Split(' ');
            // switch statement to deal with the first command word
            if (processLine)
            {
                switch (words[0].ToLower())
                {
                    case "triangle":
                    case "rectangle":
                    case "circle":
                    case "drawto":
                        drawingClass.addShape(shapeFactory.parseShape(line));
                        break;
                    case "while":
                        if (words.Length > 1)
                        {
                            string expression = String.Join(" ", words, 1, words.Length - 1);
                            context.AddWhile(lineno, expression);
                            if (!context.lastWhile.evaluate())
                            {
                                processLine = false;
                            }
                        }
                        break;
                    case "endwhile":
                        lineNumber = context.lastWhile.headLineNo - 1;
                        break;
                    case "if":
                        if (words.Length > 1)
                        {
                            string expression = String.Join(" ", words, 1, words.Length - 1);
                            context.AddIf(lineno, expression);
                            if (!context.lastIf.evaluate())
                            {
                                processLine = false;
                            }
                        }
                        break;
                    case "endif":
                        if (words.Length > 1)
                            throw new Exception("endif does not need any operands.");
                        break;
                    case "call":
                        if (line.Length < 8)
                        {
                            throw new Exception("Invalid method call");
                        }
                        else
                        {
                            string nameToCall;
                            string[] parameters = parseParametersAndName(line.Substring(5), out nameToCall);
                            int[] values = new int[parameters.Length];
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                values[i] = expressionHandler.EvaluateValue(parameters[i]);
                            }
                            lineNumber = context.InstantiateMethod(nameToCall, lineno, values);
                        }
                        break;
                    case "method":
                        string name;
                        string[] parameters2 = parseParametersAndName(line.Substring(7), out name);
                        methodStartLine = lineno;
                        methodName = name;
                        methodParameters = parameters2;
                        methodInProgress = true;
                        processLine = false;
                        break;
                    case "endmethod":
                        lineNumber = context.ExitMethod();
                        break;
                    case "pen":
                        // if the command is to change the pen width and has the correct number of operands
                        if (words.Length == 3 && words[1] == "width")
                        {
                            // try and parse the width as a float and execute the command
                            // otherwise trigger an exception detailing the syntax error in the command
                            if (float.TryParse(words[2], out float width))
                            {
                                shapeFactory.accessPenWidth = width;
                            }
                            else
                            {
                                throw new Exception("Operand for pen width is not a valid number");
                            }
                        }
                        // otherwise the command might be to change the colour
                        // for two words this means an already defined colour
                        else if (words.Length == 2)
                        {
                            (byte, byte, byte, byte) colour = decodeColour(words[1]);
                            shapeFactory.byteColour = colour;
                        }
                        // for 3 words this means a new colour without an alpha channel
                        // here we decode each value using expressionHandler.TryByte
                        else if (words.Length == 4)
                        {
                            byte r = expressionHandler.EvaluateByte(words[1]);
                            byte g = expressionHandler.EvaluateByte(words[2]);
                            byte b = expressionHandler.EvaluateByte(words[3]);
                            shapeFactory.byteColour = (r, g, b, 255);
                        }
                        // for 4 words this means a new colour with an alpha channel
                        // once again we use TryParse but for 4 value instead of three
                        else if (words.Length == 5)
                        {
                            byte r = expressionHandler.EvaluateByte(words[1]);
                            byte g = expressionHandler.EvaluateByte(words[2]);
                            byte b = expressionHandler.EvaluateByte(words[3]);
                            byte a = expressionHandler.EvaluateByte(words[4]);
                            shapeFactory.byteColour = (r, g, b, a);
                        }
                        else
                        {
                            throw new Exception("Invalid number of operands for command pen");
                        }
                        break;
                    // this is for the position pen command
                    // it has three words one of which is an argument
                    case "position":
                        // if the number of words is correct and the second one is the word pen
                        // parse the coordinates using the helper function and set the position
                        // contained in the drawing class
                        if (words.Length == 3 && words[1] == "pen")
                        {
                            var (x, y) = parsePoint(words[2]);
                            shapeFactory.x = x;
                            shapeFactory.y = y;
                        }
                        else
                        {
                            throw new Exception("Invalid command");
                        }
                        break;
                    // this function takes one argument
                    case "fill":
                        if (words.Length == 2)
                        {
                            // parse the argument and set the state appropriatley
                            if (words[1].ToLower() == "on")
                            {
                                shapeFactory.fillState = true;
                            }
                            else if (words[1].ToLower() == "off")
                            {
                                shapeFactory.fillState = false;
                            }
                            // if the fill state is not valid throw an exception
                            else
                            {
                                throw new Exception("Invalid operand for fill state command");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid number of operands");
                        }
                        break;
                    // new colour command currently used to define a new colour
                    case "new":
                        // takes either 4 or 5 arguments
                        // the first argument is the name of the new colour and the rest are the colour values
                        // with or without an alpha channel value
                        if (words.Length == 6)
                        {
                            // if the second word is indeed colour
                            if (words[1] == "colour" || words[1] == "color")
                            {
                                if (expressionHandler.TryByte(words[3], out byte r) && expressionHandler.TryByte(words[4], out byte g) &&
                                    expressionHandler.TryByte(words[5], out byte b))
                                {
                                    colours.Add(words[2], (r, g, b, 255));
                                }
                                else
                                {
                                    throw new Exception("Colour value is not a number");
                                }
                            }
                        }
                        else if (words.Length == 7)
                        {
                            // if the second word is indeed colour
                            if (words[1] == "colour" || words[1] == "color")
                            {
                                if (expressionHandler.TryByte(words[3], out byte r) && expressionHandler.TryByte(words[4], out byte g) &&
                                    expressionHandler.TryByte(words[5], out byte b) && expressionHandler.TryByte(words[6], out byte a))
                                {
                                    colours.Add(words[2], (r, g, b, a));
                                }
                                else
                                {
                                    throw new Exception("Colour value is not a number");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Incorrect number of operands");
                        }
                        break;
                    // moveto command that takes an argument for the position
                    case "moveto":
                        // make sure they have actually supplied the required argument or throw an error
                        if (words.Length == 2)
                        {
                            // parse the point using the helper function and set the position
                            var (x, y) = parsePoint(words[1]);
                            shapeFactory.x = x;
                            shapeFactory.y = y;
                        }
                        else
                        {
                            throw new Exception("Invalid number of operands");
                        }
                        break;
                    // clear command that shouldn't take any arguments
                    case "clear":
                        if (words.Length == 1)
                        {
                            shapeFactory.Clear();
                            drawingClass.clear();
                            colours.Clear();
                            colours.Add("red", (255, 0, 0, 255));
                            colours.Add("green", (0, 255, 0, 255));
                            colours.Add("blue", (0, 0, 255, 255));
                            colours.Add("black", (0, 0, 0, 255));
                            colours.Add("white", (255, 255, 255, 255));
                        }
                        else
                        {
                            throw new Exception("Invalid number of operands");
                        }
                        break;
                    default:
                        if (words.Length > 2 && words[1] == "=")
                        {
                            if (words.Length < 3)
                            {
                                throw new Exception();
                            }
                            string expression = String.Join(" ", words, 2, words.Length - 2);
                            int value = expressionHandler.Evaluate(expression);
                            expressionHandler.AddUpdateVariable(words[0], value);
                        }
                        else
                        {
                            throw new Exception("Invalid operation");
                        }
                        break;
                }
            }
            else
            {
                if (!methodInProgress)
                {
                    switch (words[0].ToLower())
                    {
                        case "endwhile":
                            context.removeWhile();
                            processLine = true;
                            break;
                        case "endif":
                            context.removeIf();
                            processLine = true;
                            break;
                        case "endmethod":
                            throw new Exception();
                            break;
                        default:
                            break;
                    }
                }
                else if (words[0].ToLower() == "endmethod")
                {
                    methodInProgress = false;
                    context.AddMethod(methodName, methodStartLine, lineno, methodParameters);
                    processLine = true;
                }
                
            }
        }

        // function to execute a line
        // calls executeLine
        // If there is an exception triggerd it takes the error message from that and displays
        // it to the user in a MessageBox
        // called by the handler for the execute line button and the enter key being pressed in the command area
        public void executeLineHandler(string line, string script)
        {
            if (line == "run")
            {
                executeScript(script);
            }
            else
            {
                try
                {
                    executeLine(line);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            drawingClass.update();
        }

        // Function to execute a whole script
        // calls the executeLine method repeatedly
        // if an error occurs decoding one of the commands it stops execution
        // and displays an error box with the line number to the user
        // called by the executeScript button handler
        public void executeScript(string script)
        {
            string[] commandArray = script.Split('\n');
            for (lineNumber = 0; lineNumber < commandArray.Length; lineNumber++)
            {
                try
                {
                    executeLine(commandArray[lineNumber], lineNumber);
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format("On line {0}: {1}", lineNumber, e.Message));
                    break;
                }
            }
            drawingClass.update();
        }

    }
}
