using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solver;

namespace SolverTest
{
    [TestClass]
    public class MathadorSolverTest
    {
        [TestMethod]
        public void PlusTest()
        {
            string expected = "1+2";
            string result = MathadorSolver.Plus("1", "2");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MoinsTest()
        {
            string expected = "1-2";
            string result = MathadorSolver.Moins("1", "2");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MultiplyTest()
        {
            string expected = "1*2";
            string result = MathadorSolver.Multiply("1", "2");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DivideTest()
        {
            string expected = "1/2";
            string result = MathadorSolver.Divide("1", "2");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CreateListOfStringTest()
        {
            List<string> list = new List<string>{"1","2","3","4","5"};


            List<string> tmp = MathadorSolver.CreateListOfString(list);

            foreach (string s in tmp)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine(tmp.Count);

        }

        [TestMethod]
        public void IsMathadorTest()
        {
            List<string> list = new List<string> { "2", "4", "6", "8", "10" }; //echec pour 1,2,3,4,5
            string valueToFind = "20";

            bool isMathador = MathadorSolver.IsMathador(list, valueToFind);

            Assert.AreEqual(true, isMathador);
            
        }

        [TestMethod]
        public void IsNotMathadorTest()
        {
            List<string> list = new List<string> { "12", "12", "12", "12", "12" };
            string valueToFind = "15";

            bool isMathador = MathadorSolver.IsMathador(list, valueToFind);

            Assert.AreEqual(false, isMathador);

        }

        [TestMethod]
        public void testCalculation()
        {
            string s = "11+4*6*7/12";
            int result = 52;
            Assert.AreEqual(result, MathadorSolver.compute(s));
        }

        [TestMethod]
        public void GetPermutationsTest()
        {
            List<string> list = new List<string> { "1", "2", "3", "4", "5" };


            IEnumerable<IEnumerable<string>> tmp = Permute.GetPermutations(list, list.Count);

            foreach (IEnumerable<string> s in tmp)
            {

                Console.WriteLine(s.ToList());

            }

            int expected = 120;
            int result = tmp.Count();

            Assert.AreEqual(expected,result);

        }

    }
}