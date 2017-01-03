using System;
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
            string result = MathadorSolver.Plus(1, 2);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MoinsTest()
        {
            string expected = "1-2";
            string result = MathadorSolver.Moins(1, 2);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MultiplyTest()
        {
            string expected = "1*2";
            string result = MathadorSolver.Multiply(1, 2);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DivideTest()
        {
            string expected = "1/2";
            string result = MathadorSolver.Divide(1, 2);

            Assert.AreEqual(expected, result);
        }
    }
}