using System;
using System.Collections.Generic;
using System.Text;

namespace ASE_Assignment
{
    class Scope
    {
        public Dictionary<string, int> variables;
        public Scope()
        {
            variables = new Dictionary<string, int>();
        }
        public void AddUpdateVariable(string name, int value)
        {
            variables[name] = value;
        }
        public bool HasVariable(string name)
        {
            if (variables.ContainsKey(name))
                return true;
            else
                return false;
        }
    }

    class WhileLoop
    {
        Scope scope;
        int head;
        string condition;
        Context context;
        public WhileLoop(Scope scope, int head, string condition, Context context)
        {
            this.scope = scope;
            this.head = head;
            this.condition = condition;
            this.context = context;
        }
        public bool evaluate()
        {
            ExpressionHandler expressionHandler = new ExpressionHandler(context);
            return expressionHandler.EvaluateCondition(condition);
        }
        public int headLineNo
        {
            get => head;
        }
    }

    class IfStatement
    {
        Scope scope;
        int lineno;
        string condition;
        Context context;
        public IfStatement(Scope scope, int lineno, string condition, Context context)
        {
            this.scope = scope;
            this.lineno = lineno;
            this.condition = condition;
            this.context = context;
        }
        public bool evaluate()
        {
            ExpressionHandler expressionHandler = new ExpressionHandler(context);
            return expressionHandler.EvaluateCondition(condition);
        }
        public int getlineno
        {
            get => lineno;
        }
    }

    class Method
    {
        int beginningLineNo, endLineNo;
        string[] parameters;
        public Method(int start, int end, string[] parameters)
        {
            beginningLineNo = start;
            endLineNo = end;
            this.parameters = parameters;
        }
        public Scope CreateScope(int[] values)
        {
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
    }

    class Context
    {
        Stack<Scope> innerScopes;
        Stack<WhileLoop> whileLoops;
        Stack<IfStatement> ifStatements;
        Dictionary<string, Method> methods;
        public Context()
        {
            innerScopes = new Stack<Scope>();
            whileLoops = new Stack<WhileLoop>();
            ifStatements = new Stack<IfStatement>();
            methods = new Dictionary<string, Method>();
            innerScopes.Push(new Scope());
        }
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
        public void AddWhile(int line, string condition)
        {
            foreach (WhileLoop whileLoop in whileLoops)
            {
                if (whileLoop.headLineNo == line)
                    return;
            }
            Scope scope = new Scope();
            innerScopes.Push(scope);
            WhileLoop wl = new WhileLoop(scope, line, condition, this);
            whileLoops.Push(wl);
        }
        public void AddIf(int lineno, string condition)
        {
            foreach (IfStatement ifStatement in ifStatements)
            {
                if (ifStatement.getlineno == lineno)
                    return;
            }
            Scope scope = new Scope();
            innerScopes.Push(scope);
            IfStatement newIfStatement = new IfStatement(scope, lineno, condition, this);
            ifStatements.Push(newIfStatement);
        }
        public void removeWhile()
        {
            innerScopes.Pop();
            whileLoops.Pop();
        }
        public void removeIf()
        {
            innerScopes.Pop();
            ifStatements.Pop();
        }
        public WhileLoop lastWhile
        {
            get { return whileLoops.Peek(); }
        }
        public IfStatement lastIf
        {
            get { return ifStatements.Peek(); }
        }
        public void AddMethod(string name, int start, int end, string[] parameters)
        {
            if (methods.ContainsKey(name))
            {
                throw new Exception("A method with that name already exists.");
            }
            Method method = new Method(start, end, parameters);
            methods.Add(name, method);
        }
        public int InstantiateMethod(string name, int[] values)
        {
            if (!methods.ContainsKey(name))
                throw new Exception("A method with that name could not be found");
            Method method = methods[name];
            innerScopes.Push(method.CreateScope(values));
            return method.startLine;
        }
        public void removeLastScope()
        {
            innerScopes.Pop();
        }
    }
}
