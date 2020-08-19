using System;
using System.Collections.Generic;
using System.Linq;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Operator
{
    internal class LogicalOperator
    {
        internal static Dictionary<string, string> Tokens = new Dictionary<string, string>();

        static LogicalOperator()
        {
            GetTokens<UnaryOperator>(Tokens);
            GetTokens<BinaryOperator>(Tokens);
        }

        public static void GetTokens<T>(Dictionary<string, string> tokenType)
        {
            Enum.GetValues(typeof(T))
                            .Cast<T>()
                            .ToList()
                            .ForEach(op =>
                            {
                                tokenType[op.ToString()] = typeof(T).Name;
                            }
                        );
        }
    }
}
