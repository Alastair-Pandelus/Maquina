using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Expression;
using Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Operator;
using System.Text.RegularExpressions;

namespace Maquina.BusinessDomain.RulesEngine.DomainSpecificLanguage.Grammar
{
    public class Syntax
    {
        public static Regex Rule { get; }
        public static Regex Trigger { get; }
        public static Regex Nary { get; }
        public static Regex And { get; }
        public static Regex Or { get; }
        public static Regex Condition { get; }
        // Allow an empty conditions, to allow an actions only rule
        public static Regex EmptyCondition { get; }
        // Allow an empty actions, to allow for a conditions only rule
        public static Regex EmptyAction { get; }
        // The intended and full rule functionality has a set of actions
        public static Regex Actions { get; }
        public static Regex Action { get; }
        public static Regex Unary { get; }
        public static Regex MethodName { get; }
        public static string NestedBrackets { get; }

        // Changed from '(' and ')' to allow nesting of bracket characters in string const, e.g. "Select Reason (W/F-specific)
        // The alternatives are rarely used Unicode chars - see https://www.symbolsofit.com/en/bracket/
        internal const char OpeningBracket = '(';
        public const char AltOpeningBracket = '〔';

        internal const char ClosingBracket = ')';
        public const char AltClosingBracket = '〕';

        internal const char ParameterSeparator = ',';
        internal const char ParameterQualifier = ':';
        internal const char ActionSeparator = ';';
        internal const string Implies = "=>";
        internal const char StringQuotation = '"';
        internal const char ClassMethodSeparator = '.';
        internal const char EnumerationSeparator = '.';

        static Syntax()
        {
            // Congnisant of dependencies, bottom up instantiation

            // see - https://stackoverflow.com/questions/546433/regular-expression-to-match-balanced-parentheses
            NestedBrackets = $"(\\{AltOpeningBracket}(?>\\{AltOpeningBracket}(?<c>)|[^{AltOpeningBracket}{AltClosingBracket}]+|\\{AltClosingBracket}(?<-c>))*(?(c)(?!))\\{AltClosingBracket})";
            Unary = LexicalItem($"{UnaryOperator.Not}{NestedBrackets}");
            MethodName = LexicalItem($"([\\w][\\w]*)\\{ClassMethodSeparator}([\\w][\\w]*)");
            Action = LexicalItem($"({MethodName})(\\{AltOpeningBracket})([^{AltClosingBracket}]*)(\\{AltClosingBracket})");
            EmptyCondition = LexicalItem($"_");
            EmptyAction = LexicalItem($"\\{AltOpeningBracket}\\{AltClosingBracket}");
            Actions = LexicalItem($"({EmptyAction}|({Action})({ActionSeparator}{Action})*)");
            Condition = LexicalItem($"({MethodName})(\\{AltOpeningBracket})([^{AltClosingBracket}]*)(\\{AltClosingBracket})");
            Nary = LexicalItem($"({BinaryOperator.And}|{BinaryOperator.Or}){NestedBrackets}");
            And = LexicalItem($"{BinaryOperator.And}{NestedBrackets}");
            Or = LexicalItem($"{BinaryOperator.Or}{NestedBrackets}");
            Trigger = LexicalItem($"({EmptyCondition}|{Condition}|{Unary}|{Nary})");
            Rule = LexicalItem($"{Trigger}{Implies}{Actions}");
        }

        static private Regex LexicalItem(string expression)
        {
            return new Regex(expression, RegexOptions.Compiled);
        }
    }
}
