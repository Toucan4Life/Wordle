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
            possibleWordListView.View = View.Details;
            possibleWordListView.Columns.Add("Word");
            possibleWordListView.Columns.Add("Frequency");
            possibleWordListView.Columns.Add("Entropy");
            recommendedWordListView.View = View.Details;
            recommendedWordListView.Columns.Add("Word");
            recommendedWordListView.Columns.Add("Entropy");
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            _wordSolver = new WordleSolver(int.Parse(textBox1.Text));

            var retrievePossibleWords = _wordSolver.RetrievePossibleWords().ToList();
            var count = retrievePossibleWords.Count;
            possibleWordCountLabel.Text = $"Count : {count} ";
            entropyListLabel.Text =
                $"Entropy : {_wordSolver.CalculateEntropy(retrievePossibleWords.Select(t => (float) 1 / count).ToList())} ";

            possibleWordListView.Items.Clear();
            foreach (var (key, value) in retrievePossibleWords
                         .OrderByDescending(t => t.Value).Take(20))
            {
                possibleWordListView.Items.Add(new ListViewItem(new[] { key, value.ToString() }));
            }


            recommendedWordListView.Items.Clear();
            var retrieveRecommendedWords = _wordSolver.RetrieveRecommendedWords().ToList();
            foreach (var (key, value) in retrieveRecommendedWords.OrderByDescending(t => t.Value).Take(20))
            {
                recommendedWordListView.Items.Add(new ListViewItem(new []{key,value.ToString()}));
            }

            possibleWordListView.Items.Clear();
            foreach (var (key, value) in retrievePossibleWords
                         .OrderByDescending(t => t.Value).Take(20))
            {
                possibleWordListView.Items.Add(new ListViewItem(new[] { key, value.ToString(), retrieveRecommendedWords.Single(t=>t.Key==key).Value.ToString() }));
            }
        }

        private void StepButtonClicked(object sender, EventArgs e)
        {
            _wordSolver.ApplyWordPattern(textBox3.Text, textBox2.Text.Select(MapPattern).ToList());

            var retrievePossibleWords = _wordSolver.RetrievePossibleWords().ToList();
            var count = retrievePossibleWords.Count;
            possibleWordCountLabel.Text = $"Count : {count} ";
            entropyListLabel.Text =
                $"Entropy : {_wordSolver.CalculateEntropy(retrievePossibleWords.Select(t => (float)1 / count).ToList())} ";
            possibleWordListView.Items.Clear();
            foreach (var (key, value) in retrievePossibleWords
                         .OrderByDescending(t => t.Value).Take(20))
            {
                possibleWordListView.Items.Add(new ListViewItem(new[] { key, value.ToString() }));
            }


            recommendedWordListView.Items.Clear();
            var retrieveRecommendedWords = _wordSolver.RetrieveRecommendedWords().ToList();
            foreach (var (key, value) in retrieveRecommendedWords.OrderByDescending(t => t.Value).Take(20))
            {
                recommendedWordListView.Items.Add(new ListViewItem(new[] { key, value.ToString() }));
            }
            textBox2.Text = null;
            textBox3.Text = null;

            possibleWordListView.Items.Clear();
            foreach (var (key, value) in retrievePossibleWords
                         .OrderByDescending(t => t.Value).Take(20))
            {
                possibleWordListView.Items.Add(new ListViewItem(new[] { key, value.ToString(), retrieveRecommendedWords.Single(t => t.Key == key).Value.ToString() }));
            }
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
            possibleWordListView.Items.Clear();
            recommendedWordListView.Items.Clear();
            textBox3.Text = null;
            textBox1.Text = null;

        }

        private void recommendedWordListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (recommendedWordListView.SelectedItems.Count == 0)
                return;
            textBox3.Text = recommendedWordListView.SelectedItems[0].Text;
        }

        private void possibleWordListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (possibleWordListView.SelectedItems.Count == 0)
                return;
            textBox3.Text = possibleWordListView.SelectedItems[0].Text;
        }
    }
}