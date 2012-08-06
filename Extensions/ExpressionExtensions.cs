#region License
/****************************************************************************************
 * Licensed under the MIT License
 *
 * Copyright (c)2012
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *****************************************************************************************/
 #endregion // License
using System;
using System.Net;
using System.Linq.Expressions;

namespace PinTools.Extensions
{
    static public class ExpressionExtensions
    {
        static public string GetMemberName(this Expression<Func<object>> linqExpression)
        {
            // Make sure we got an expression
            if (linqExpression == null) throw new ArgumentNullException("linqExpression");

            // Test to see if the body is a UnaryExpression
            UnaryExpression unaryExpression = linqExpression.Body as UnaryExpression;

            // Placeholder
            MemberExpression memberExpression = null;

            // If the expression is an unary expression we have to get the member expression from the operand.
            // Otherwise, the body is the member expression itself.
            if (unaryExpression != null)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = linqExpression.Body as MemberExpression;
            }

            // Ensure we have the member expression
            if (memberExpression == null) { throw new ArgumentException("'linqExpression' should be a member expression or a unary expression with a member expression as the operand."); }

            // Extract the name of the property to raise a change on  
            return memberExpression.Member.Name;
        }
    }
}
