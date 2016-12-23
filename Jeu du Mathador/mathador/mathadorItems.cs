using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathador
{
    class mathadorItems
    {
        private int Value1;
        private int Value2;
        private int Value3;
        private int Value4;
        private int Value5;

        private int ValueToFind;

        private List<mathadorOper> MathadorOperList = new List<mathadorOper>();

        public mathadorItems(int value1, int value2, int value3, int value4, int value5, int valueToFind)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
            Value5 = value5;
            ValueToFind = valueToFind;
        }
    }

    class mathadorOper
    {
        private int Value1;
        private int Value2;

        private string Operator;

        private int Result;

        /// <summary>
        /// Grâce à ca, on a juste à utiliser la variable et l'opération se fait toute seul
        /// En faite c'est con mais on n'en a pas besoin on veut juste stocker donc shit....
        /// </summary>
        /// <example>
        /// Func<int, int, int> plus = (a, b) => a + b;
        /// </example>
        //private Func<int, int, int> Operator;

        public mathadorOper(int value1, int value2, string _operator)
        {
            Value1 = value1;
            Value2 = value2;
            Operator = _operator;
        }
    }
}
