using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ASE_Assignment
{
    class ExpressionHandler
    {
        Context context;
        public ExpressionHandler(Context context)
        {
            this.context = context;
        }
        protected string ReplaceVariables(string equation)
        {
            foreach (Scope scope in context.scopes)
            {
                foreach (KeyValuePair<string, int> variable in scope.variables)
                {
                    if (equation.Contains(variable.Key))
                    {
                        equation = equation.Replace(variable.Key, variable.Value.ToString());
                    }
                }
            }
            return equation;
        }
        public int Evaluate(string equation)
        {
            DataTable dt = new DataTable();
            equation = ReplaceVariables(equation);
            double rawValue = Convert.ToDouble(dt.Compute(equation, ""));
            return (int) Math.Floor(rawValue);
        }
        public bool EvaluateCondition(string condition)
        {
            DataTable dt = new DataTable();
            bool containsOperation = false;
            foreach (string operation in new string[] { "<", ">", "==", "!=", "<=", ">=" })
            {
                if (condition.Contains(operation))
                {
                    containsOperation = true;
                    break;
                }
            }
            if (containsOperation)
            {
                condition = ReplaceVariables(condition);
                bool result = (bool)dt.Compute(condition, "");
                return result;
            }
            else
            {
                throw new Exception("Operation string does not contain a boolean operation.");
            }
        }
        public int EvaluateValue(string name)
        {
            if (int.TryParse(name, out int result))
            {
                return result;
            }
            foreach(Scope scope in context.scopes)
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
            foreach (Scope scope in context.scopes)
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
        public void AddUpdateVariable(string name, int value)
        {
            foreach (Scope scope in context.scopes)
            {
                if (scope.HasVariable(name))
                {
                    scope.AddUpdateVariable(name, value);
                    return;
                }
            }
            context.lastScope.AddUpdateVariable(name, value);
        }
    }
}
