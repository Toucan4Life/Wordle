using Wordle;
using Wordle.BLL;
using Wordle.SAL;

namespace GUI
{
    public partial class Form1 : Form
    {
        private IWordleSolver _wordSolver;
        public Form1()
        {
            InitializeComponent();
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            _wordSolver = new WordleSolver(int.Parse(textBox1.Text));

            var retrievePossibleWords = _wordSolver.RetrievePossibleWords();
            var count = retrievePossibleWords.Count();
            possibleWordCountLabel.Text = $"Count : {count} ";
            entropyListLabel.Text =
                $"Entropy : {_wordSolver.CalculateEntropy(retrievePossibleWords.Select(t => (float) 1 / count).ToList())} ";
            possibleWordListBox.DataSource = retrievePossibleWords
                .OrderByDescending(t => t.Value).Take(20).Select(t => $"{t.Key} {t.Value}").ToList();

            recommendedWordListBox.DataSource = _wordSolver.RetrieveRecommendedWords().OrderByDescending(t => t.Value).Take(20)
                .Select(t => $"{t.Key} {t.Value}").ToList();
        }

        private void StepButtonClicked(object sender, EventArgs e)
        {
            _wordSolver.ApplyWordPattern(textBox3.Text, textBox2.Text.Select(MapPattern).ToList());

            var retrievePossibleWords = _wordSolver.RetrievePossibleWords();
            var count = retrievePossibleWords.Count();
            possibleWordCountLabel.Text = $"Count : {count} ";
            entropyListLabel.Text =
                $"Entropy : {_wordSolver.CalculateEntropy(retrievePossibleWords.Select(t => (float)1 / count).ToList())} ";
            possibleWordListBox.DataSource = retrievePossibleWords
                .OrderByDescending(t => t.Value).Take(20).Select(t => $"{t.Key} {t.Value}").ToList();

            recommendedWordListBox.DataSource = _wordSolver.RetrieveRecommendedWords().OrderByDescending(t => t.Value).Take(20)
                .Select(t => $"{t.Key} {t.Value}").ToList();
            textBox2.Text = null;
        }

        private static Pattern MapPattern(char c)
        {
            return c switch
            {
                '0' => Pattern.Incorrect,
                '1' => Pattern.Misplaced,
                '2' => Pattern.Correct,
                _ => throw new ArgumentOutOfRangeException("Pattern not supported")
            };
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            possibleWordListBox.DataSource = null;
            recommendedWordListBox.DataSource = null;
            textBox3.Text = null;
            textBox1.Text = null;

        }

        private void recommendedWordListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = recommendedWordListBox.SelectedItem?.ToString()?.Split(' ')[0];
        }

        private void possibleWordListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = possibleWordListBox.SelectedItem?.ToString()?.Split(' ')[0];
        }
    }
}