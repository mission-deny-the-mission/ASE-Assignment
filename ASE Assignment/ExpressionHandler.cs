using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ASE_Assignment
{
    /// <summary>
    /// This is a class that handles things like parsing numbers and variables
    /// </summary>
    class ExpressionHandler
    {
        Context context;
        public ExpressionHandler(Context context)
        {
            this.context = context;
        }

        /// <summary>
        /// This checks if a variable exists in all scopes when replacing identifiers with values
        /// </summary>
        /// <param name="equation">equation to have variables replaced on</param>
        /// <returns>the equation with the variable identifiers replaced with values</returns>
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

        /// <summary>
        /// A function which evaluates an equation including replacing variables with values
        /// </summary>
        /// <param name="equation">the equation</param>
        /// <returns>the value the equation evaluates to</returns>
        public int Evaluate(string equation)
        {
            DataTable dt = new DataTable();
            equation = ReplaceVariables(equation);
            double rawValue = Convert.ToDouble(dt.Compute(equation, ""));
            return (int) Math.Floor(rawValue);
        }

        /// <summary>
        /// method that evaluates a boolean condition like those used in while loops and if statements
        /// </summary>
        /// <param name="condition">equation to evaluate</param>
        /// <returns></returns>
        /// <exception cref="Exception">is thrown if equation does not contains a boolean operation</exception>
        public bool EvaluateCondition(string condition)
        {
            DataTable dt = new DataTable();
            bool containsOperation = false;
            foreach (string operation in new string[] { "<", ">", "=", "!=", "<=", ">=" })
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

        /// <summary>
        /// a method to evaluate a value such as a number or a variable
        /// this is used for passing valus to commands or methods
        /// </summary>
        /// <param name="value">literal or identifier to parse</param>
        /// <returns>the value of the literal or identifier</returns>
        /// <exception cref="Exception">thrown if it's an identifier that cannot be found</exception>
        public int EvaluateValue(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            foreach(Scope scope in context.scopes)
            {
                if (scope.variables.ContainsKey(value))
                {
                    return scope.variables[value];
                }
            }
            throw new Exception("Could not find variable " + value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        /// <returns></returns>
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
