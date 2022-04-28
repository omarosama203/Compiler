using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myfirstcompilerproject
{
    public static class Program
    {
        public static Lexer Scanner = new Lexer();
        public static List<string> Lexemes = new List<string>();
        public static List<Token> TokenStream = new List<Token>();
        static void Main(string[] args)
        {
            string code = Console.ReadLine();
            Start_Compiling(code);
            for (int i = 0; i < Program.Scanner.Tokens.Count; i++)
            {
                Console.WriteLine(
                    Program.Scanner.Tokens.ElementAt(i).token_type );
                
            }
            Console.ReadKey();
        }

        public static void Start_Compiling(string SourceCode) 
        {
            //Scanner
            Scanner.StartScanning(SourceCode);
            
        }


        /*static void SplitLexemes(string SourceCode)
        {
            string[] Lexemes_arr = SourceCode.Split(' ');
            for (int i = 0; i < Lexemes_arr.Length; i++)
            {
                if (Lexemes_arr[i].Contains("\r\n"))
                {
                    Lexemes_arr[i] = Lexemes_arr[i].Replace("\r\n", string.Empty);
                }
                Lexemes.Add(Lexemes_arr[i]);
            }

        }*/


    }
}
