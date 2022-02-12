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
        public void EntropyCalculationWhenOnly2Elements()
        {
            var wordDictionary = new Dictionary<string, float> { { "abe", 0 }, { "aba", 0 } };
            WordleSolver _solver = new();

            var entropy = _solver.CalculateEntropy("abe", wordDictionary);
            Assert.AreEqual(1, entropy);
        }

        [TestMethod]
        public void EntropyCalculationDoesnotAlterSearcherState()
        {
            var wordDictionary = new Dictionary<string, float> { { "abe", 0 }, { "aba", 0 } };
            WordleSolver _solver = new();

            var wordSearcher = new WordSearcher(wordDictionary);
            //wordSearcher._wordLength = 0;
            _solver.CalculateEntropy("abe", wordDictionary);

            Assert.AreEqual(0, wordSearcher._charPosToMatch.Count);
        }

        [TestMethod]
        public void EntropyCalculationDoesnotAlterSearcherState1()
        {
            var wordDictionary = new Dictionary<string, float> { { "ab", 0 }, { "ac", 0 } };
            WordleSolver _solver = new();
            
            //wordSearcher._wordLength = 0;
            var entropy = _solver.CalculateEntropy("ab", wordDictionary);
            var entropy2 = _solver.CalculateEntropy("ac", wordDictionary);

            Assert.AreEqual(1, entropy);
            Assert.AreEqual(1, entropy2);
        }

        [TestMethod]
        public void EntropyCalculationDoesnotAlterSearcherState2()
        {
            var wordDictionary = new Dictionary<string, float> { { "abe", 0 }, { "aba", 0 } };
            WordleSolver _solver = new();

            var entropy = _solver.CalculateEntropy("abe", wordDictionary);
            Assert.AreEqual(1, entropy);
        }
    }
}