using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ASE_Assignment
{
    class Scope
    {
        public Dictionary<string, int> variables;
        public Scope()
        {
            variables = new Dictionary<string, int>();
        }
        public void AddVariable(string name, int value)
        {
            variables[name] = value;
        }

    }

    class ExpressionHandler
    {
        Stack<Scope> scopes;
        public ExpressionHandler(Stack<Scope> scopes)
        {
            this.scopes = scopes;
        }
        public int Evaluate(string equation)
        {
            DataTable dt = new DataTable();
            foreach (Scope scope in scopes)
            {
                foreach (KeyValuePair<string, int> variable in scope.variables)
                {
                    if (equation.Contains(variable.Key))
                    {
                        equation = equation.Replace(variable.Key, variable.Value.ToString());
                    }
                }
            }
            double rawValue = Convert.ToDouble(dt.Compute(equation, ""));
            return (int) Math.Floor(rawValue);
        }
        public int EvaluateValue(string name)
        {
            if (int.TryParse(name, out int result))
            {
                return result;
            }
            foreach(Scope scope in scopes)
            {
                if (scope.variables.ContainsKey(name))
                {
                    return scope.variables[name];
                }
            }
            throw new Exception("Could not find variable " + name);
        }
        public bool TryEvalValue(string input, out int result)
        {
            try
            {
                result = EvaluateValue(input);
                return true;
            }
            catch (Exception ex)
            {
                result = 0;
                return false;
            }
        }
        public byte EvaluateByte(string toEval)
        {
            if (byte.TryParse(toEval, out byte result))
            {
                return result;
            }
            foreach (Scope scope in scopes)
            {
                var variables = scope.variables;
                if (variables.ContainsKey(toEval))
                {
                    int value = variables[toEval];
                    if (byte.MinValue <= value && value <= byte.MaxValue)
                    {
                        return (byte)value;
                    }
                    else
                    {
                        throw new Exception("Variable value out of range.");
                    }
                }
            }
            throw new Exception("Invalid expression: " + toEval);
        }
        public bool TryByte(string toEval, out byte result)
        {
            try
            {
                result = EvaluateByte(toEval);
                return true;
            }
            catch (Exception e)
            {
                result = 0;
                return false;
            }
        }
    }
}
