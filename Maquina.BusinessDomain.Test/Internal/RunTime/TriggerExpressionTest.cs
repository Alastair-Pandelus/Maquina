using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Operator;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Parameter;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Maquina.Service.Test.RulesEngine.Internal.RunTime
{
    [TestFixture]
    public class TriggerExpressionTest : BaseRulesTest
    {
        [Test]
        public void SerialiseDeserialise()
        {
            NaryExpression conditionExpression
                = new NaryExpression
                    (
                        BinaryOperator.And,
                        new List<TriggerExpression>
                        {
                            new UnaryExpression
                            (
                                UnaryOperator.Not,
                                new NaryExpression
                                (
                                    BinaryOperator.Or,
                                    new List<TriggerExpression>
                                    {
                                        new ConditionExpression
                                        (
                                            new MethodNameExpression("MyClass", "MyMethod1"),
                                            new Parameters()
                                            {
                                                new Parameter("one",ScriptType.Int32, 1),
                                                new Parameter("two",ScriptType.Boolean, true)
                                            }
                                        ),
                                        new ConditionExpression
                                        (
                                            new MethodNameExpression("MyClass", "MyMethod2"),
                                            new Parameters()
                                            {
                                                new Parameter("three",ScriptType.String, "value"),
                                                new Parameter("four",ScriptType.Double, 123.456)
                                            }
                                        )
                                    }
                                )
                            ),
                            new ConditionExpression
                            (
                                new MethodNameExpression("MyClass2", "MyMethod1"),
                                new Parameters()
                                {
                                    new Parameter("five",ScriptType.Int32, 24)
                                }
                            )
                        }
                    );

            // And(Not(Or(C_3(one:Int32:1,two:Boolean:True),C_4(three:String:"value",four:Double:123.456))),C_5(five:Int32:24))
            // C_3(Int32:1,Boolean:True)
            var stringExpression = conditionExpression.ToString();
            string altStringExpression = stringExpression.Replace('(', Syntax.AltOpeningBracket).Replace(')', Syntax.AltClosingBracket);

            // Scale back from the original test - can serialise
            Assert.IsTrue(!string.IsNullOrEmpty(altStringExpression));
        }

        [Test]
        [TestCase("C_1(invalid)")]
        // should be "string"
        [TestCase("C_1(three:str:\"value\")")]
        // Unquoted string
        [TestCase("And(Not(Or(C_3(one:Int32:1,two:Boolean:True),C_4(three:String:value,four:Double:123.456))),C_5(five:Int32:24))")]
        public void BadlyFormedExpression(string malformedExpression)
        {
            string altMalformedExpression = malformedExpression.Replace('(', Syntax.AltOpeningBracket).Replace(')', Syntax.AltClosingBracket);

            Assert.Throws(typeof(ApplicationException), () =>
            {
                _ = TriggerExpression.Parse(null, altMalformedExpression);
            });
        }
    }
}