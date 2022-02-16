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

        //[TestMethod]
        //public void EntropyCalculationWhenPossiblyReducedBy2()
        //{
        //    var wordDictionary = new Dictionary<string, float> { { "abe", 0 }, { "aba", 0 } };
        //    WordleSolver _solver = new();

        //    var entropy = _solver.CalculateEntropy("abe", wordDictionary);
        //    Assert.AreEqual(1, entropy);
        //}

        //[TestMethod]
        //public void EntropyCalculationWhenPossiblyReducedBy4()
        //{
        //    var wordDictionary = new Dictionary<string, float> { { "abe", 0 }, { "aba", 0 }, { "cbe", 0 }, { "cba", 0 } };
        //    WordleSolver _solver = new();

        //    var entropy = _solver.CalculateEntropy("abe", wordDictionary);
        //    Assert.AreEqual(2, entropy);
        //}
    }
}