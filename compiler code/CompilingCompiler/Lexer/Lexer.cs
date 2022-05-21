using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CompilingCompiler.Controllers;
public enum Token_Class
{
    // DatatTypes
    Integer, SInteger, Character, String, Float, SFloat,

    // reserved
    Void, Loop, Return, Break, Struct, Condition, Endline, End, Inclusion,


    // Oprators
    ArithmeticOperator, // (+, -, *, /,)
    LogicOperators, // (&&, ||, ~)
    relationalOperators, // (==, <, >, !=, <=, >=)
    AssignmentOperator, // =
    AccessOperator, // ->
                    //

    Semicolon, Comma, Braces,



    Identifier, Constant, Comment, QuotationMark
}
namespace Myfirstcompilerproject
{
    public class Token
    {
        public int tokenLine;
        public string lex;
        public Token_Class token_type;
    }
    public class Errors
    {
        public static List<string> Error_List = new List<string>();
    }
    public class Lexer
    {
        public List<Token> Tokens = new List<Token>();
        public int lineCounter = 1;
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();
        List<string> sperators = new List<string>();
        public Lexer()
        {
            ReservedWords.Add("If", Token_Class.Condition);
            ReservedWords.Add("Else", Token_Class.Condition);
            ReservedWords.Add("ElseIf", Token_Class.Condition);
            ReservedWords.Add("Iow", Token_Class.Integer);
            ReservedWords.Add("SIow", Token_Class.SInteger);
            ReservedWords.Add("Iowf", Token_Class.Float);
            ReservedWords.Add("SIowf", Token_Class.SFloat);
            ReservedWords.Add("Chlo", Token_Class.Character);
            ReservedWords.Add("Chain", Token_Class.String);
            ReservedWords.Add("Turnback", Token_Class.Return);
            ReservedWords.Add("Worthless", Token_Class.Void);
            ReservedWords.Add("Loopwhen", Token_Class.Loop);
            ReservedWords.Add("Iteratewhen", Token_Class.Loop);
            ReservedWords.Add("Loli", Token_Class.Struct);
            ReservedWords.Add("Stop", Token_Class.Break);
            ReservedWords.Add("Include", Token_Class.Inclusion);




            Operators.Add("=", Token_Class.AssignmentOperator);
            Operators.Add("<", Token_Class.relationalOperators);
            Operators.Add(">", Token_Class.relationalOperators);
            Operators.Add("!=", Token_Class.relationalOperators);
            Operators.Add("==", Token_Class.relationalOperators);
            Operators.Add(">=", Token_Class.relationalOperators);
            Operators.Add("<=", Token_Class.relationalOperators);
            Operators.Add("+", Token_Class.ArithmeticOperator);
            Operators.Add("-", Token_Class.ArithmeticOperator);
            Operators.Add("*", Token_Class.ArithmeticOperator);
            Operators.Add("/", Token_Class.ArithmeticOperator);
            Operators.Add("&&", Token_Class.LogicOperators);
            Operators.Add("||", Token_Class.LogicOperators);
            Operators.Add("~", Token_Class.LogicOperators);
            Operators.Add("->", Token_Class.AccessOperator);
            Operators.Add(")", Token_Class.Braces);
            Operators.Add("(", Token_Class.Braces);
            Operators.Add("}", Token_Class.Braces);
            Operators.Add("{", Token_Class.Braces);
            //Operators.Add(",", Token_Class.Comma);
            Operators.Add(",", Token_Class.QuotationMark);
            Operators.Add("'", Token_Class.QuotationMark);
            //Operators.Add(";", Token_Class.Semicolon);
        }
        public void StartScanning(string SourceCode)
        {
            Tokens.Clear();
            this.lineCounter = 1;
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar+"";

                if (isItEmpty(CurrentChar))
                    continue;

                if (isLineDelimter(CurrentChar))
                {
                    while (j + 1 < SourceCode.Length && isSaparator(SourceCode[j + 1]))
                    {

                        if (isLineDelimter(SourceCode[j + 1]))
                        {
                            j++;
                            break;
                        }
                        j++;
                    }
                    i = j;

                    this.lineCounter += 1;
                    continue;
                }
                // if it starts with a char it can be a reserevedword or an identifier (allow  only digits and letters to the lexeme) 
                if (isLetter(CurrentChar)) //if you read a character
                {

                    while (j + 1 < SourceCode.Length && !isSaparator(SourceCode[j + 1]))
                    {
                        j++;
                        CurrentLexeme += SourceCode[j]+"";
                    }

                    i = j;

                }

                // if it starts with a digit it can only be a number
                else if (isDigit(CurrentChar))
                {

                    while (j + 1 < SourceCode.Length && !isSaparator(SourceCode[j + 1]))
                    {
                        j++;
                        CurrentLexeme += SourceCode[j]+"";
                    }
                    i = j;
                }

                // if it starts with a " then every thing is allowed till typing " again

                else if (CurrentChar == '"')
                {
                    // if it's not the last char
                    while (j + 1 < SourceCode.Length && SourceCode[j + 1] != '"')
                    {
                        j++;
                        CurrentLexeme += SourceCode[j]+"";
                    }
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                    }
                    i = j;

                }

                // if it starts with ! if the next is = then it's a notEqualOp 
                else if (CurrentChar == '!')
                {
                    if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '=')
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                        i = j;
                    }
                }
                else if (CurrentChar == '>')
                {
                    if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '=')
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                        i = j;
                    }
                }
                else if (CurrentChar == '<')
                {
                    if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '=')
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                        i = j;
                    }
                }
                else if (CurrentChar == '-')
                {
                    if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '>')
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                        i = j;
                    }
                }
                // if it starts with | check if it's followed by another | to get OrOp
                else if (CurrentChar == '|')
                {
                    if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '|')
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                        i = j;
                    }
                }
                // if it starts with & check if it's followed by another & to get AndOp
                else if (CurrentChar == '&')
                {
                    if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '&')
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                        i = j;
                    }
                }
                else if (CurrentChar == '=')
                {
                    if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '=')
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                        i = j;
                    }
                }
                // if it starts with / followed by * then allow every thing except */
                else if (j + 1 < SourceCode.Length && CurrentChar == '/' && SourceCode[j + 1] == '$')
                {

                    ++j;
                    bool isAboutToFinish = false;
                    bool isFinished = false;
                    CurrentLexeme += SourceCode[j];
                    j++;
                    while (j < SourceCode.Length && !isFinished)
                    {

                        if (isAboutToFinish && SourceCode[j] == '/')
                        {
                            CurrentLexeme += SourceCode[j];
                            isFinished = true;
                        }
                        else
                        {
                            CurrentLexeme += SourceCode[j];
                            isAboutToFinish = (SourceCode[j] == '$');
                        }
                        if (isLineDelimter(SourceCode[j]))
                        {
                            this.lineCounter++;
                        }
                        j++;
                    }
                    j--;

                    i = j;


                }
                else if (j + 2 < SourceCode.Length && CurrentChar == '$' && SourceCode[j + 1] == '$' && SourceCode[j + 2] == '$')
                {
                    CurrentLexeme += SourceCode[j];
                    while (j + 1 < SourceCode.Length)
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];

                        if (isLineDelimter(SourceCode[j]))
                            break;
                    }
                    i = j;
                    this.lineCounter++;

                }


                // Is it a comment then ignore
                if (!isComment(CurrentLexeme/*.Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", "")*/))
                    FindTokenClass(CurrentLexeme);
            }

            HomeController.TokenStream = Tokens;



        }
        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;


            //Is it a reserved word?
            if (regulareExpression.matchKeywordsandOp(ReservedWords, Lex) != -1)
            {

                Tok.token_type = ReservedWords[Lex];
                Tok.tokenLine = this.lineCounter;
                Tokens.Add(Tok);
            }

            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identifier;
                Tok.tokenLine = this.lineCounter;
                Tokens.Add(Tok);

            }

            //Is it a Number?
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tok.tokenLine = this.lineCounter;
                Tokens.Add(Tok);

            }

            //Is it a String?
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tok.tokenLine = this.lineCounter;
                Tokens.Add(Tok);

            }

            //Is it an operator?
            else if (regulareExpression.matchKeywordsandOp(Operators, Lex) != -1)
            {
                Tok.token_type = Operators[Lex];
                Tok.tokenLine = this.lineCounter;
                Tokens.Add(Tok);

            }



            //Is it an Error?
            else
                Errors.Error_List.Add(Lex + " is undefined");
        }

        private bool isComment(string lex)
        {
            if (regulareExpression.matchMultipleComment(lex) || regulareExpression.matchSingleComment(lex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool isIdentifier(string lex)
        {
            bool isValid = true;
            // Check if the lex is an identifier or not.

            isValid = regulareExpression.matchIdentifier(lex);

            return isValid;
        }
        bool isNumber(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.

            isValid = regulareExpression.matchNumbers(lex);
            return isValid;
        }

        bool isString(string lex)
        {
            bool isValid = true;


            isValid = regulareExpression.matchString(lex);
            return isValid;
        }

        bool isItEmpty(char c)
        {
            return (c == ' ' || c == '\r' || c == '\t');
        }

        bool isSaparator(char c)
        {
            return (isItEmpty(c) || Operators.ContainsKey(c+"") || c == '|' || c == '&' || c == ':' || c == ';' || c == '\n');
        }
        public static bool isLineDelimter(char c)
        {
            return (c == ';' || c == '\n');
        }
        public static bool isDigit(char c)
        {
            if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isLetter(char c)
        {
            if (c == '_' || c == 'a' || c == 'A' || c == 'b' || c == 'B' || c == 'c' || c == 'C' || c == 'd' || c == 'D' || c == 'e' || c == 'E' || c == 'f' || c == 'F' || c == 'g' || c == 'G' || c == 'h' || c == 'H' || c == 'i' || c == 'I' || c == 'j' || c == 'J' || c == 'k' || c == 'K' || c == 'l' || c == 'L' || c == 'm' || c == 'M' || c == 'n' || c == 'N' || c == 'o' || c == 'O' || c == 'p' || c == 'P' || c == 'q' || c == 'Q' || c == 'r' || c == 'R' || c == 's' || c == 'S' || c == 't' || c == 'T' || c == 'u' || c == 'U' || c == 'v' || c == 'V' || c == 'w' || c == 'W' || c == 'x' || c == 'X' || c == 'y' || c == 'Y' || c == 'z' || c == 'Z')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    public class regulareExpression
    {
        public static bool matchIdentifier(String identifier)
        {
            if (!Lexer.isLetter(identifier[0]))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < identifier.Length; i++)
                {
                    if (!Lexer.isLetter(identifier[i]) && !Lexer.isDigit(identifier[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool matchNumbers(String number)
        {
            int dotCounter = 0;
            if (!Lexer.isDigit(number[0]))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < number.Length; i++)
                {
                    if (number[i] == '.')
                    {
                        dotCounter++;
                        if (i != number.Length - 1)
                            continue;
                    }
                    if (dotCounter >= 2 || !Lexer.isDigit(number[i]))
                        return false;

                }

            }
            return true;
        }
        public static bool matchMultipleComment(String comment)
        {
            int lastCharIndex = comment.Length - 1;
            if (comment[0] == '/' && comment[1] == '$' && comment[lastCharIndex - 1] == '$' && comment[lastCharIndex] == '/')
                return true;
            else
            {
                return false;
            }

        }
        public static bool matchSingleComment(String comment)
        {
            int lastcharindex = comment.Length - 1;

            if (comment[0] == '$' && comment[1] == '$' && comment[2] == '$' && Lexer.isLineDelimter(comment[lastcharindex]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool matchString(String str)
        {
            if (str[0] == '"' && str[str.Length - 1] == '"')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int matchKeywordsandOp(Dictionary<string, Token_Class> keywords, string lex)
        {
            string key;
            int index = -1;
            for (int i = 0; i < keywords.Count; i++)
            {
                key = keywords.ElementAt(i).Key;
                if (lex.Length != key.Length)
                {
                    continue;
                }
                if (lex == key)
                {
                    index = i;
                }
            }
            return index;

        }

    }
}


