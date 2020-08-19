using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Operator;
using Maquina.BusinessDomain.RulesEngine.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression
{
    public class NaryExpression : TriggerExpression
    {
        public BinaryOperator Operator { get; set; }

        public List<TriggerExpression> SubClauses { get; set; }

        public NaryExpression(BinaryOperator @operator, List<TriggerExpression> subClauses)
        {
            Operator = @operator;
            SubClauses = subClauses;
        }

        public new static NaryExpression Parse(Metamodel metaModel, string expressionText)
        {
            BinaryOperator @operator;
            Match match;

            if((match = FullMatch(Syntax.And, expressionText)) != null)
            {
                @operator = BinaryOperator.And;
            }
            else
            if((match = FullMatch(Syntax.Or, expressionText)) != null)
            {
                @operator = BinaryOperator.Or;
            }
            else
            {
                throw new Exception($"Unmatched binary operator expression {expressionText}");
            }

            var matchGroup = match.Groups[1];

            // Split the sub-clauses, mindful of nested sub-expression
            List<TriggerExpression> subClauses = new List<TriggerExpression>();

            string parametersText = matchGroup.Value[1..^1];
            SplitNested(parametersText, Syntax.ParameterSeparator).ForEach(naryExpression =>
            {
                subClauses.Add(TriggerExpression.Parse(metaModel, naryExpression));
            });

            return new NaryExpression(@operator, subClauses);
        }

        // Split expression taking account of nested brackets (sub expressions)
        // Possible to do this with a recursive regular expression, but it gets quite messy, so go with simple parsing 
        private static List<string> SplitNested(string expression, char separator)
        {
            List<string> segments = new List<string>();

            int segmentStart = 0;
            int depth = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case Syntax.AltOpeningBracket:
                        depth++;
                        break;

                    case Syntax.AltClosingBracket:
                        depth--;
                        break;

                    default:
                        if (depth == 0 && expression[i] == separator)
                        {
                            segments.Add(expression[segmentStart..i]);
                            segmentStart = i + 1;
                        }
                        break;
                }
            }

            segments.Add(expression[segmentStart..]);

            return segments;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Operator.ToString() + Syntax.OpeningBracket);
            for(int i=0; i < SubClauses.Count-1; i++)
            {
                sb.Append($"{SubClauses[i]}{Syntax.ParameterSeparator}");
            }
            sb.Append($"{SubClauses[^1]}");
            sb.Append(Syntax.ClosingBracket);

            return sb.ToString();
        }

        // Could add selectivity or method profiling to make it more efficient
        // Use short-circuited logic in the first instance
        public override async Task<bool> Evaluate(RulesEngineService rulesEngine)
        {
            switch(Operator)
            {
                // Short-circuit the logic 
                case BinaryOperator.And:
                    foreach(var subClause in SubClauses)
                    {
                        bool status = await subClause.Evaluate(rulesEngine);

                        if (!status)
                        {
                            return false;
                        }
                    }

                    return true;

                // proof by disproof, short-circuit the logic
                case BinaryOperator.Or:
                    // proof by disproof, short-circuit the logic
                    foreach(var subClause in SubClauses)
                    {
                        if (await subClause.Evaluate(rulesEngine))
                        {
                            return true;
                        }
                    }

                    return false;

                default:
                    throw new Exception("Unexpected Binary operator");
            }
        }
    }
}
