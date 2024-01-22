using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AnyAscii;
using static System.Net.Mime.MediaTypeNames;

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
                ParseLine(reader.ReadLine(), wordsFreq);
            }

            return wordsFreq;
        }

        public void ParseLine(string? line, Dictionary<string, float> wordsFreq)
        {
            var values = line.Split(';');

            var transliterate = values[0].Transliterate();

            if (transliterate.Any(t => !char.IsLetter(t))) return;

            if (wordsFreq.ContainsKey(transliterate)) wordsFreq[transliterate] = AddFrequency(wordsFreq[transliterate], values[1]);

            else
                wordsFreq.Add(transliterate, float.Parse(values[1], CultureInfo.InvariantCulture));
        }

        private float AddFrequency(float one, string two)
        {
            var key = float.Parse(two, CultureInfo.InvariantCulture);
            return one + key;
        }
    }
}
