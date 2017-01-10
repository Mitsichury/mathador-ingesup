using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public class MathadorSolver
    {
        public static bool IsMathador(List<int> listeNb) // liste v1,v2,v2,v4,v5,valueToFind
        {
            //recup de la valeur a trouver...............................
            int valueToFind = listeNb[5];

            //recup liste des 5 valeurs pour les calcules....................................
            listeNb.RemoveAt(5);
            int[] nbToCalc = listeNb.ToArray();

            //création d'une liste de tableau avec les 5 valeurs permutées.......................
            List<int[]> listePermute = new List<int[]>();
            listePermute = Permute.Commencer(nbToCalc);

            List<List<string>> listeOperation = new List<List<string>>();

            foreach (var tab in listePermute)
            {
                //listeOperation.Add(CreateListOfString(tab)); 
            }
            


            return true;
        }

        public static List<string> CreateListOfString(List<string> tab)
        {
            //List<string> operation = new List<string>();

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



        public static List<int> ReturnArrayNbToCalc(List<int> listeNb)
        {
            listeNb.RemoveAt(5);       
            return listeNb;
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



        public static void Main()
        {
 


        }
    }

    public class Permute
    {
        public static List<int[]> Commencer(int[] list)
        {
            List<int[]> listePermute = new List<int[]>();
            listePermute = Travaille(list, 0, list.Length - 1, listePermute);
            return listePermute;
        }


        private static void Echange(ref int a, ref int b)
        {
            if (a == b) return;
            a ^= b;
            b ^= a;
            a ^= b;
        }


        private static List<int[]> Travaille(int[] list, int current, int max, List<int[]> listePermute)
        {
            
            int[] tab = new int[5];
            

            int j;
            int i;
            if (current == max)
            {
                j = 0;
                foreach (var item in list)
                {
                    Console.Write(Convert.ToString(item));
                    
                    tab[j] = (item);
                    j++;

                }
                Console.WriteLine(" ");
                listePermute.Add(tab);
            }
            else
                for (i = current; i <= max; i++)
                {
                    Echange(ref list[current], ref list[i]);
                    Travaille(list, current + 1, max, listePermute); // on descend
                    Echange(ref list[current], ref list[i]);
                }
            return listePermute;
        }
    }

}

