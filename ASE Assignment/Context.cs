using System;
using System.Collections.Generic;
using System.Text;

// This file has classes that deal with keeping track of context like scopes, while loops, methods, if statements, etc.

namespace ASE_Assignment
{
    /// <summary>
    /// Scopes hold the variables in the program.
    /// Since this project uses scoped variables you can't access variables defined in something like method outside of that method.
    /// The same also applies to while loops. This mechanic is created by having several instances of this class held in a stack.
    /// When something like a method is created an extra instance is created and pushed onto the stack.
    /// At the end of the method the instance is popped off the stack
    /// </summary>
    class Scope
    {
        // variables consist of a dictionary that has the name of the variable as they key and the value as the value
        // only integer variables are supported at this point
        // this might change in the future by making the value some form of object or by using multiple dictionaries
        // or possibly some more complex data structure I haven't decided upon yet.
        public Dictionary<string, int> variables;
        public Scope()
        {
            variables = new Dictionary<string, int>();
        }

        /// <summary>
        /// this method adds a new variable or updates a previous one
        /// </summary>
        /// <param name="name">name of the variable</param>
        /// <param name="value">value to assign the variable</param>
        public void AddUpdateVariable(string name, int value)
        {
            variables[name] = value;
        }

        /// <summary>
        /// Determines if a variable exists in the scope
        /// </summary>
        /// <param name="name">name of the variable</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool HasVariable(string name)
        {
            if (variables.ContainsKey(name))
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// WhileLoop class deals with while loops.
    /// Each while loop in the program source will cause on of these to be instantiated.
    /// WhileLoop contains a scope with it's own set of variables that are not accessible out of the while loop.
    /// WhileLoops are also stored in a stack
    /// </summary>
    class WhileLoop
    {
        Scope scope;
        int head;
        string condition;
        ExpressionHandler expressionHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scope">the new scope is passed to the WhileLoop instead of the WhileLoop creating it internally.
        /// This is done because the Context class needs a copy of the scope</param>
        /// <param name="head">the number of the first line of the while loop containingthe condition</param>
        /// <param name="condition">a copy of the while loops condition</param>
        /// <param name="context">ExpressionHandler instance used for evaluating the condition.
        /// Needs to be from the same scope as the while loop.</param>
        public WhileLoop(Scope scope, int head, string condition, ExpressionHandler expressionHandler)
        {
            this.scope = scope;
            this.head = head;
            this.condition = condition;
            this.expressionHandler = expressionHandler;
        }

        /// <summary>
        /// Evaluated the condition in the while loop using an expression handler
        /// </summary>
        /// <returns></returns>
        public bool evaluate()
        {
            return expressionHandler.EvaluateCondition(condition);
        }
        public int headLineNo
        {
            get => head;
        }
    }

    /// <summary>
    /// This class deals with keeping track of if statements.
    /// </summary>
    class IfStatement
    {
        int lineno;
        string condition;
        Context context;
        ExpressionHandler expressionHandler;

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="lineno">the line number of the first line of the if statement containg the condition</param>
        /// <param name="condition">the condition of the if statement coming from the first line</param>
        /// <param name="expressionHandler">An ExpressionHandler instance used to evaluate the condition
        /// needs to be from the same scope as the if statement</param>
        public IfStatement(int lineno, string condition, ExpressionHandler expressionHandler)
        {
            this.lineno = lineno;
            this.condition = condition;
            this.expressionHandler= expressionHandler;
        }
        public bool evaluate()
        {
            return expressionHandler.EvaluateCondition(condition);
        }
        public int getlineno
        {
            get => lineno;
        }
    }

    class Method
    {
        // the endlineno currently isn't used but might be useful for the future.
        int beginningLineNo, endLineNo;
        string[] parameters;
        public Method(int start, int end, string[] parameters)
        {
            beginningLineNo = start;
            endLineNo = end;
            this.parameters = parameters;
        }
        /// <summary>
        /// This method is used to create the scope for a method call
        /// It takes the parameter values and instantiates them as variables in the new scope
        /// </summary>
        /// <param name="values">values to pass to the method</param>
        /// <returns>The scope that has been created with the parameters and their values</returns>
        /// <exception cref="ArgumentException">thrown if the wrong number of parameters are passed to the method</exception>
        public Scope CreateScope(int[] values)
        {
            // check if the number of parameters are correct
            if (values.Length != parameters.Length)
                throw new ArgumentException("Incorrect number of parameters given to the method");
            
            Scope scope = new Scope();
            for (int i = 0; i < parameters.Length; i++)
            {
                scope.AddUpdateVariable(parameters[i], values[i]);
            }
            return scope;
        }
        public int startLine
        {
            get => beginningLineNo;
        }
        public int endLine
        {
            get => endLineNo;
        }
    }

    /// <summary>
    /// This is the main context class that this file is named after
    /// </summary>
    class Context
    {
        // This stores all the scopes which in turn have all of the variables
        // When a new context is entered a new scope is created and pushed onto the stack.
        // When it's destroyed the scope gets popped of and destroyed
        Stack<Scope> innerScopes;
        // This hold all of the while loops and the informatino related to them
        Stack<WhileLoop> whileLoops;
        // This one holds the if statements
        Stack<IfStatement> ifStatements;
        // and the methods
        Dictionary<string, Method> methods;
        // These all work the same way as the scopes above.
        // Each while loop and if statement hold a reference to the scope they are responsible for.

        // I got the idea of using a stack from the way C and C like programming languages are implemented on
        // most computers including x86 and ARM where they have seperate stack and heap memory and return
        // addresses for function calls are held on a stack along with local variables.

        Stack<int> returnPoints;

        ExpressionHandler expressionHandler;

        /// <summary>
        /// constructor function
        /// </summary>
        public Context()
        {
            // initialises all of the required data stuctures
            innerScopes = new Stack<Scope>();
            whileLoops = new Stack<WhileLoop>();
            ifStatements = new Stack<IfStatement>();
            methods = new Dictionary<string, Method>();
            returnPoints = new Stack<int>();

            // creates the first scope that acts like a global scope
            // if this wasn't here you would only be able to create variables inside while loop and method bodies
            innerScopes.Push(new Scope());

            expressionHandler = new ExpressionHandler(this);
        }

        /// <summary>
        /// Used to access the scopes from another class
        /// I made this protected and only defined a get property so that it couldn't be changed from another class
        /// </summary>
        public Stack<Scope> scopes
        {
            get => innerScopes;
        }
        public Scope lastScope
        {
            get { return innerScopes.Peek(); }
        }
        public void AddScope(Scope scope)
        {
            innerScopes.Push(scope);
        }

        /// <summary>
        /// Method to add a new while loop to the context
        /// </summary>
        /// <param name="line">the line number of the line with the while loop</param>
        /// <param name="condition">The condition that determines if the while loop body executes again</param>
        public void AddWhile(int line, string condition)
        {
            // makes sure the same while loop is not added twice
            foreach (WhileLoop whileLoop in whileLoops)
            {
                if (whileLoop.headLineNo == line)
                    return;
            }
            // create a new scope for the while loop and push it onto the stack of scopes
            Scope scope = new Scope();
            innerScopes.Push(scope);
            // create the while loop and push it onto the stack
            WhileLoop wl = new WhileLoop(scope, line, condition, expressionHandler);
            whileLoops.Push(wl);
        }
        public void AddIf(int lineno, string condition)
        {
            // makes sure the same if statement hasn't been added twice
            foreach (IfStatement ifStatement in ifStatements)
            {
                if (ifStatement.getlineno == lineno)
                    return;
            }
            IfStatement newIfStatement = new IfStatement(lineno, condition, expressionHandler);
            ifStatements.Push(newIfStatement);
        }

        /// <summary>
        /// Method that remove the last while loop added
        /// </summary>
        public void removeWhile()
        {
            innerScopes.Pop();
            whileLoops.Pop();
        }
        /// <summary>
        /// Method that removes the last if statement added
        /// </summary>
        public void removeIf()
        {
            ifStatements.Pop();
        }
        /// <summary>
        /// property that returns the last while loop added
        /// </summary>
        public WhileLoop lastWhile
        {
            get { return whileLoops.Peek(); }
        }
        /// <summary>
        /// Method that return the last if statement added
        /// </summary>
        public IfStatement lastIf
        {
            get { return ifStatements.Peek(); }
        }
        /// <summary>
        /// method that is used to add a new method to the context
        /// </summary>
        /// <param name="name">the name of the method to add to the context</param>
        /// <param name="start">the line number of the start of the method declaration/definition</param>
        /// <param name="end">the line number for the last line of the method declaration/definition</param>
        /// <param name="parameters">the names of the parameters the method takes</param>
        /// <exception cref="Exception">thrown if a method with the same identifier already exists</exception>
        public void AddMethod(string name, int start, int end, string[] parameters)
        {
            // check to make sure a method of the same name does not already exist
            // this throws an exception if a method with the same name already exists
            if (methods.ContainsKey(name))
            {
                throw new Exception("A method with that name already exists.");
            }
            Method method = new Method(start, end, parameters);
            methods.Add(name, method);
        }
        /// <summary>
        /// method used when a method is called.
        /// It creates a scope with the parameters set as variables with the values passed to the function
        /// </summary>
        /// <param name="name"></param>
        /// <param name="returnLine"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int InstantiateMethod(string name, int returnLine, int[] values)
        {
            // make sure the method we are trying to instantiate actually exists
            if (!methods.ContainsKey(name))
                throw new Exception("A method with that name could not be found");
            // gets the method from the stack
            Method method = methods[name];
            // uses the CreateScope method to setup the scope with the parameters passed as variables
            innerScopes.Push(method.CreateScope(values));
            returnPoints.Push(returnLine);
            return method.startLine;
        }
        public int ExitMethod()
        {
            if (returnPoints.Count == 0)
                throw new Exception("Not in a method");
            innerScopes.Pop();
            return returnPoints.Pop();
        }
    }
}
