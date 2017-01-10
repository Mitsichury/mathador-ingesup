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
        public void IsMathadorTest()
        {
            List<string> list = new List<string>{"1","2","3","4","5","20"};


            List<string> tmp = MathadorSolver.CreateListOfString(list);

            foreach (string s in tmp)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine(tmp.Count);

        }

        [TestMethod]
        public void ReturnArrayNbToCalcTest()
        {
            List<int> listTest = new List<int> { 1, 2, 3, 4, 5, 20 };
            List<int> expected = new List<int> { 1, 2, 3, 4, 5 };


            List<int> result = MathadorSolver.ReturnArrayNbToCalc(listTest);

            CollectionAssert.AreEqual(result, expected);


        }


        [TestMethod]
        public void PermuteTest()
        {
            int[] list = new int[] {1,2,3,4,5};
            List<int[]> listePermute = Permute.Commencer(list);

 
            Console.WriteLine(listePermute.Count.ToString());
            Console.WriteLine(listePermute.ToString());

            int expected = 120;
            int result = listePermute.Count;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ChercheCombinaisonsTest()
        {
            
        }

    }
}