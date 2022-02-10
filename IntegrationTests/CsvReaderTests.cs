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
        public void DoesNotAddDataWhenNullLine()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            var response = reader.ParseLine(null, wordsFreq);
            Assert.AreEqual(0, response.Count);
        }

        [TestMethod]
        public void DoesNotAddDataWhenEmptyLine()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            var response = reader.ParseLine("", wordsFreq);
            Assert.AreEqual(0, response.Count);
        }

        [TestMethod]
        public void AddDataWhenEmptyDictionary()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            var response = reader.ParseLine("coucou;2", wordsFreq);
            Assert.AreEqual("coucou", response.First().Key);
            Assert.AreEqual(2, response.First().Value);
        }

        [TestMethod]
        public void AddDataWhenNotEmptyDictionaryAndNotKeyNotFound()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float> {{"toucan", 5}};
            var response = reader.ParseLine("coucou;2", wordsFreq);
            Assert.IsTrue( response.ContainsKey("toucan"));
            Assert.IsTrue( response.ContainsValue(5));
        }

        [TestMethod]
        public void AddDataWhenNotEmptyDictionaryAddValue()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float> {{"coucou", 5}};
            var response = reader.ParseLine("coucou;2", wordsFreq);
            Assert.AreEqual("coucou", response.First().Key);
            Assert.AreEqual(7, response.First().Value);
        }

        [TestMethod]
        public void AddFloatValue()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            var response = reader.ParseLine("coucou;5.5", wordsFreq);
            Assert.AreEqual("coucou", response.First().Key);
            Assert.AreEqual(5.5f, response.First().Value);
        }

        [TestMethod]
        public void DoesNotAddDataWithSpace()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            var response = reader.ParseLine("cou cou;5.5", wordsFreq);
            Assert.IsFalse(response.Any());
        }
        [TestMethod]
        public void DoesNotAddDataWithDash()
        {
            CsvReader reader = new();
            var wordsFreq = new Dictionary<string, float>();
            var response = reader.ParseLine("cou-cou;5.5", wordsFreq);
            Assert.IsFalse(response.Any());
        }
    }
}
