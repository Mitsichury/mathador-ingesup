using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathadorLibrary
{
    public class mathadorOper
    {
        public string Value1;
        public string Value2;

        public string Operator;

        public string Result;

        /// <summary>
        /// Grâce à ca, on a juste à utiliser la variable et l'opération se fait toute seul
        /// En faite c'est con mais on n'en a pas besoin on veut juste stocker donc shit....
        /// </summary>
        /// <example>
        /// Func<int, int, int> plus = (a, b) => a + b;
        /// </example>
        //private Func<int, int, int> Operator;

        public mathadorOper(string value1, string value2, string _operator)
        {
            Value1 = value1;
            Value2 = value2;
            Operator = _operator;
        }
    }
}
