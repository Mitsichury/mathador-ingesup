using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathador
{
    public class mathadorItem
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }

        public string ValueToFind { get; set; }

        public List<mathadorOper> MathadorOperList = new List<mathadorOper>();

        #region constructeur
        /// <summary>
        /// Constructeur de MathadorItems prenant un string par valeur en parametre
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="value4"></param>
        /// <param name="value5"></param>
        /// <param name="valueToFind"></param>
        public mathadorItem(string value1, string value2, string value3, string value4, string value5, string valueToFind)
        {
            Value1 = value1.Trim();
            Value2 = value2.Trim();
            Value3 = value3.Trim();
            Value4 = value4.Trim();
            Value5 = value5.Trim();
            ValueToFind = valueToFind.Trim();
        }

        /// <summary>
        /// Constructeur de MathadorItems prenant un tableau string[] en parametre
        /// </summary>
        /// <param name="values"></param>
        public mathadorItem(string[] values)
        {
            Value1 = values[0].Trim();
            Value2 = values[1].Trim();
            Value3 = values[2].Trim();
            Value4 = values[3].Trim();
            Value5 = values[4].Trim();
            ValueToFind = values[5].Trim();
        }
        #endregion
    }

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
