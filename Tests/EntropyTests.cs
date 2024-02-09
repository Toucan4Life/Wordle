using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordle;
using Wordle.BLL;

namespace IntegrationTests
{
    [TestClass]
    public class EntropyTests
    {

        [TestMethod]
        public void EntropyCalculationWhenPossiblyReducedBy2()
        {
            var entropy = WordleSolver.GetEntropy(new List<float> { (float).5, (float).5 });
            Assert.AreEqual(1, entropy);
        }

        [TestMethod]
        public void EntropyCalculationWhenPossiblyReducedBy4()
        {
            var entropy = WordleSolver.GetEntropy(new List<float> { (float).25, (float).25, (float).25, (float).25 });
            Assert.AreEqual(2, entropy);
        }
    }
}