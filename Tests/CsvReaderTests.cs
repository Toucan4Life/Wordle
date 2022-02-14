using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordle;
using Wordle.SAL;

namespace IntegrationTests
{
    [TestClass]
    public class CsvReaderTests
    {
        [TestMethod]
        public void AddDataWhenEmptyDictionary()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            reader.ParseLine("coucou;2", wordsFreq);
            Assert.AreEqual("coucou", wordsFreq.First().Key);
            Assert.AreEqual(2, wordsFreq.First().Value);
        }

        [TestMethod]
        public void AddDataWhenNotEmptyDictionaryAndNotKeyNotFound()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float> {{"toucan", 5}};
            reader.ParseLine("coucou;2", wordsFreq);
            Assert.IsTrue(wordsFreq.ContainsKey("toucan"));
            Assert.IsTrue(wordsFreq.ContainsValue(5));
        }

        [TestMethod]
        public void AddDataWhenNotEmptyDictionaryAddValue()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float> {{"coucou", 5}};
            reader.ParseLine("coucou;2", wordsFreq);
            Assert.AreEqual("coucou", wordsFreq.First().Key);
            Assert.AreEqual(7, wordsFreq.First().Value);
        }

        [TestMethod]
        public void AddFloatValue()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            reader.ParseLine("coucou;5.5", wordsFreq);
            Assert.AreEqual("coucou", wordsFreq.First().Key);
            Assert.AreEqual(5.5f, wordsFreq.First().Value);
        }

        [TestMethod]
        public void DoesNotAddDataWithSpace()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            reader.ParseLine("cou cou;5.5", wordsFreq);
            Assert.IsFalse(wordsFreq.Any());
        }
        [TestMethod]
        public void DoesNotAddDataWithDash()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            reader.ParseLine("cou-cou;5.5", wordsFreq);
            Assert.IsFalse(wordsFreq.Any());
        }

        [TestMethod]
        public void DoesNotAddDataWithApostrophe()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            reader.ParseLine("cou'cou;5.5", wordsFreq);
            Assert.IsFalse(wordsFreq.Any());
        }
    }
}
