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


    }

    class operation
    {
        private int Value1;
        private int Value2;

        private Func<int, int> Operator;

        public operation(int value1, int value2, Func<int, int> _operator)
        {
            
        }
    }
}
