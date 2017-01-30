using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using MathadorLibrary;

namespace Solver
{
    public class MathadorSolver
    {
        public static bool IsMathador(List<string> listeNb, string valueToFind)
        {
            return GetMathadors(listeNb, valueToFind).Count > 0;
        }

        public static List<string> GetMathadors(List<string> list, string valueTofind)
        {
            List<string> listOfCalculs = GetResults(list, valueTofind);
            List<string> mathadors = new List<string>();

            foreach (string s in listOfCalculs)
            {
                if (s.Contains("+") && s.Contains("-") && s.Contains("*") && s.Contains("/"))
                {
                    mathadors.Add(s);
                }
            }
            return mathadors;
        }

        private static List<string> GetResults(List<string> listeNb, string valueToFind)
        {
            IEnumerable<IEnumerable<string>> listePermute = Permute.GetPermutations(listeNb, listeNb.Count);


            List<List<string>> listeOperation = new List<List<string>>();

            foreach (IEnumerable<string> tab in listePermute)
            {
                List<string> s = new List<string>();
                s = tab.ToList();
                listeOperation.Add(CreateListOfString(s));
            }

            List<string> result = new List<string>();
            

            int i = 0;

            foreach (List<string> list in listeOperation)
            {

                listeOperation[i].RemoveRange(0, 85);
                foreach (string s in list)
                {
                    string resultat = compute(s).ToString();
                    
                    if (resultat == valueToFind)
                    {
                        //Console.WriteLine(s + "=" + resultat);
                        result.Add(s);
                    }
                }
                i++;
            }

            return result;
        }

        public static List<string> CreateListOfString(List<string> tab)
        {

            if (tab.Count == 1)
            {
                return tab;
            }
            else
            {
                List<string> operation = new List<string>();

                operation.Add( tab[0]);
                tab.RemoveAt(0);

                List<string> result = CreateListOfString(tab);

                foreach (string s in result)
                {
                    operation.Add(Plus(operation[0], s));
                    operation.Add(Moins(operation[0], s));
                    operation.Add(Multiply(operation[0], s));
                    operation.Add(Divide(operation[0], s));
                }

                return operation;
            }
            
        }


        public static string Plus(string a, string b)
        {
            string str = a + "+" + b;
            return str;
        }

        public static string Moins(string a, string b)
        {
            string str = a + "-" + b;
            return str;
        }

        public static string Multiply(string a, string b)
        {
            string str = a + "*" + b;
            return str;
        }

        public static string Divide(string a, string b)
        {
            string str = a + "/" + b;
            return str;
        }

        public static int compute(string s)
        {
            bool isFirst = true;
            int state = 0;
            char _operator = '0';

            int lastIndex = 0;            
            int i = 0;

            foreach (char c in s)
            {
                if (!Char.IsDigit(c))
                {
                    if (isFirst)
                    {
                        state = Int32.Parse(s.Substring(lastIndex, i - lastIndex));
                        _operator = c;
                        isFirst = false;
                        lastIndex = i + 1;
                    }
                    else
                    {
                        state = exec(state, _operator, Int32.Parse(s.Substring(lastIndex, i - lastIndex)));
                        _operator = c;
                        lastIndex = i + 1;
                    }
                }
                i++;
            }

            state = exec(state, _operator, Int32.Parse(s.Substring(lastIndex, i - lastIndex)));

            return state;
        }

        public static int exec(int state, char _operator, int value)
        {
            int result = state;
            switch (_operator)
            {
                case '*':
                    result = state * value;
                    break;
                case '+':
                    result = state + value;                    
                    break;
                case '-':
                    result = state - value;
                    break;
                case '/':
                    result = state / value;
                    break;
                default:
                    throw new Exception("bad operator");
            }

            return result;
        }

    }

    public class Permute
    {

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

    }

    

}

