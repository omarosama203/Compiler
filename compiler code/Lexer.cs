using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
public enum Token_Class
{
    // DatatTypes
    Integer, SInteger, Character, String, Float, SFloat,

    // reserved
    Void, Loop, Return, Break, Struct, Condition,  Endline, End, Inclusion,


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
		public  int tokenLine;
        public string lex;
        public Token_Class token_type;
    }
    public static class Errors
    {
        public static List<string> Error_List = new List<string>();
    }
    public class Lexer
    {
        public List<Token> Tokens = new List<Token>();
		public int lineCounter=1;
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
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (isItEmpty(CurrentChar))
                    continue;
                if (isLineDelimter(CurrentChar))
                {
                    this.lineCounter += 1;
                    continue;
                }
                // if it starts with a char it can be a reserevedword or an identifier (allow  only digits and letters to the lexeme) 
                if (char.IsLetter(CurrentChar)) //if you read a character
                {

                    while (j + 1 < SourceCode.Length && !isSaparator(SourceCode[j + 1]))
                    {
                        j++;
                        CurrentLexeme += SourceCode[j].ToString();
                    }

                    i = j;

                }

                // if it starts with a digit it can only be a number
                else if (char.IsDigit(CurrentChar))
                {

                    while (j + 1 < SourceCode.Length && !isSaparator(SourceCode[j + 1]))
                    {
                        j++;
                        CurrentLexeme += SourceCode[j].ToString();
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
                        CurrentLexeme += SourceCode[j].ToString();
                    }
                    if (j + 1 < SourceCode.Length)
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                    }
                    i = j;

                }
     
                // if it starts with ! if the next is = then it's a notEqualOp or it can be just not
                else if (CurrentChar == '!')
                {
                    if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '=')
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
                        j++;
                    }
                    j--;

                    i = j;


                }
               

                // Is it a comment then ignore
                if (!isComment(CurrentLexeme.Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", "")))
                    FindTokenClass(CurrentLexeme);
            }

            Program.TokenStream = Tokens;



        }
        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;


            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
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
            else if (Operators.ContainsKey(Lex))
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
            Regex regex = new Regex(@"^((\/\$)(.*)(\$\/))$");
            return regex.IsMatch(lex);
        }

        bool isIdentifier(string lex)
        {
            bool isValid = true;
            // Check if the lex is an identifier or not.
            Regex regex = new Regex("^([a-zA-Z][a-zA-Z0-9]*)$");
            isValid = regex.IsMatch(lex);

            return isValid;
        }
        bool isNumber(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            Regex regex = new Regex("^([0-9]+(\\.[0-9]+)?)$");
            isValid = regex.IsMatch(lex);
            return isValid;
        }

        bool isString(string lex)
        {
            bool isValid = true;
            Regex regex = new Regex("^(\"(.*)\")$");
            // Regex regex = new Regex("^((.*))$");
            isValid = regex.IsMatch(lex);
            return isValid;
        }

        bool isItEmpty(char c)
        {
            return (c == ' ' || c == '\r' || c == '\n' || c == '\t');
        }

        bool isSaparator(char c)
        {
            return (isItEmpty(c) || Operators.ContainsKey(c.ToString()) || c == '|' || c == '&' || c == ':' || c ==';');
        }
		bool isLineDelimter(char c)
        {
            return (c==';');
        }
    }
}
