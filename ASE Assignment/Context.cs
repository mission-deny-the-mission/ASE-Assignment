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
    class Context
    {
        Stack<Scope> innerScopes;
        Stack<WhileLoop> whileLoops;
        public Context()
        {
            innerScopes = new Stack<Scope>();
            whileLoops = new Stack<WhileLoop>();
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
        public void removeWhile()
        {
            innerScopes.Pop();
            whileLoops.Pop();
        }
        public WhileLoop lastWhile
        {
            get { return whileLoops.Peek(); }
        }
    }
}
