using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using Myfirstcompilerproject;

namespace CompilingCompiler.Controllers
{

    public class HomeController : Controller
    {
        public static Lexer Scanner = new Lexer();
        public static Errors errors = new Errors();
        public static List<string> Lexemes = new List<string>();
        public static List<Token> TokenStream = new List<Token>();
        List<string> tokens = new List<string>();
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Scan(string gib)
        {
         
                TempData.Remove("result");
            


            string code = gib;
            String tmpStr = "";

            List<String> employees = new List<String>();
            Start_Compiling(code);
 /*           for (int i = 0; i < HomeController.Scanner.Tokens.Count; i++)
            {
                TempData[i.ToString()];


                //      r2 = r2+  HomeController.Scanner.Tokens.ElementAt(i).tokenLine;
            } */
            for (int i = 0; i < HomeController.Scanner.Tokens.Count; i++)
            {
                TempData[i.ToString()] = " Line #" + HomeController.Scanner.Tokens.ElementAt(i).tokenLine + " Contains " + HomeController.Scanner.Tokens.ElementAt(i).token_type ;// " Errors :" + HomeController.errors;

                
          //      r2 = r2+  HomeController.Scanner.Tokens.ElementAt(i).tokenLine;
            }
            //TempData["result"]= tmpStr;//r1 + "----" + r2;
            code = "";
            ViewBag.Message = string.Format("Your Added/Updated details has been Saved successfully.");
            return RedirectToAction("Index", "Home");




        }
        public static void Start_Compiling(string SourceCode)
        {
            //Scanner
            Scanner.StartScanning(SourceCode);

        }





        [HttpPost]
        public ActionResult Function2(string gib)
        {
            //apply f2 here and replace the gib below
            TempData["result"] = gib;
            return RedirectToAction("Index", "Home");
        }

    }
}