using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyAscii;

namespace Wordle.SAL
{
    public class CsvReader
    {
        public Dictionary<string, float> GetAllWords(string path)
        {
            var wordsFreq = new Dictionary<string, float>();

            using var reader = new StreamReader(path);
            //skip header
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                wordsFreq = ParseLine(reader.ReadLine(), wordsFreq);
               
            }

            return wordsFreq;
        }

        public Dictionary<string, float>  ParseLine(string? line, Dictionary<string, float> wordsFreq)
        {
            if(string.IsNullOrWhiteSpace(line) || line.Contains(' ') || line.Contains('-')) return wordsFreq;
            var values = line.Split(';');
            if (wordsFreq.ContainsKey(values[0].Transliterate())) wordsFreq[values[0].Transliterate()] = AddTwoStrings(wordsFreq[values[0].Transliterate()], values[1]);

            else
            {
                var key = float.Parse(values[1], CultureInfo.InvariantCulture);
                wordsFreq.Add(values[0].Transliterate(), key);
            }
            return wordsFreq;
        }

        private float AddTwoStrings(float one, string two)
        {
            var key = float.Parse(two, CultureInfo.InvariantCulture);
            return one + key;
        }
    }
}
