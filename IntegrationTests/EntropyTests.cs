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
        public void CartesianProductWith1D()
        {
            Pattern[][] items = {
                new[] { Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced }
            };

            WordleSolver _solver = new(new Dictionary<string, float>());
            var cartesianProduct = _solver.CartesianProduct(items).ToList();
            Assert.AreEqual(3, cartesianProduct.Count);
        }

        [TestMethod]
        public void CartesianProductWith2D()
        {
            Pattern[][] items = {
                new[] { Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced },
                new[] { Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced }
            };

            WordleSolver _solver = new(new Dictionary<string, float>());
            var cartesianProduct = _solver.CartesianProduct(items).ToList();
            Assert.AreEqual(9,cartesianProduct.Count);
        }

        [TestMethod]
        public void EntropyCalculationWhenOnly2Elements()
        {
            var wordDictionary = new Dictionary<string, float> { { "abe", 0 }, { "aba", 0 } };
            WordleSolver _solver = new(wordDictionary);

            var wordSearcher = new WordSearcher(wordDictionary);

            var entropy = _solver.CalculateEntropy("abe",wordSearcher);
            Assert.AreEqual(1, entropy);
        }

        [TestMethod]
        public void EntropyCalculationDoesnotAlterSearcherState()
        {
            var wordDictionary = new Dictionary<string, float> { { "abe", 0 }, { "aba", 0 } };
            WordleSolver _solver = new(wordDictionary);

            var wordSearcher = new WordSearcher(wordDictionary);
            //wordSearcher._wordLength = 0;
            _solver.CalculateEntropy("abe", wordSearcher);

            Assert.AreEqual(0, wordSearcher._charPosToMatch.Count);
        }

        [TestMethod]
        public void EntropyCalculationDoesnotAlterSearcherState1()
        {
            var wordDictionary = new Dictionary<string, float> { { "ab", 0 }, { "ac", 0 } };
            WordleSolver _solver = new(wordDictionary);

            var wordSearcher = new WordSearcher(wordDictionary);
            //wordSearcher._wordLength = 0;
            var entropy = _solver.CalculateEntropy("ab", wordSearcher);
            var entropy2 = _solver.CalculateEntropy("ac", wordSearcher);

            Assert.AreEqual(1, entropy);
            Assert.AreEqual(1, entropy2);
        }
    }
}