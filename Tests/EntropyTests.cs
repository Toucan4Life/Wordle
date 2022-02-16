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
            Entropy _solver = new();

            var entropy = _solver.Calculate(new List<List<Pattern>>
            {
                new() {Pattern.Correct, Pattern.Correct, Pattern.Correct},
                new() {Pattern.Correct, Pattern.Correct, Pattern.Incorrect}
            });
            Assert.AreEqual(1, entropy);
        }

        [TestMethod]
        public void EntropyCalculationWhenPossiblyReducedBy4()
        {
            Entropy _solver = new();

            var entropy = _solver.Calculate(new List<List<Pattern>>
            {
                new() {Pattern.Correct, Pattern.Correct, Pattern.Correct},
                new() {Pattern.Correct, Pattern.Correct, Pattern.Incorrect},
                new() {Pattern.Incorrect, Pattern.Correct, Pattern.Incorrect},
                new() {Pattern.Incorrect, Pattern.Correct, Pattern.Misplaced},
            });

            Assert.AreEqual(2, entropy);
        }
    }
}