using System.Windows.Forms;
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
            _wordSolver = (string.IsNullOrWhiteSpace(firstCharTextBox.Text) || firstCharTextBox.Text.Length > 1)
                ? new WordleSolver(int.Parse(textBox1.Text))
                : new WordleSolver(int.Parse(textBox1.Text), firstCharTextBox.Text[0]);

            UpdateUiState();
        }

        private void UpdateUiState()
        {
            textBox2.Text = null;
            textBox3.Text = null;
            possibleWordListView.Items.Clear();
            recommendedWordListView.Items.Clear();
            possibleWordListView.Items.Clear();

            var retrieveRecommendedWords = _wordSolver.RetrieveRecommendedWords().ToList();
            var globalEntropy = _wordSolver.CalculateUniformEntropy(retrieveRecommendedWords.Count);

            possibleWordCountLabel.Text = $"Count : {retrieveRecommendedWords.Count} ";
            entropyListLabel.Text = $"Entropy : {globalEntropy} ";

            recommendedWordListView.Items.AddRange(retrieveRecommendedWords.OrderByDescending(t => t.Entropy).Take(20)
                .Select(item => new ListViewItem(new[] { item.Name, item.Entropy.ToString() })).ToArray());

            var listViewItems = from poss in retrieveRecommendedWords.OrderByDescending(t => t.Frequency).Take(20)
                select new ListViewItem(new[] { poss.Name, poss.Frequency.ToString(), poss.Entropy.ToString() });

            possibleWordListView.Items.AddRange(listViewItems.ToArray());
        }

        private void StepButtonClicked(object sender, EventArgs e)
        {
            _wordSolver.ApplyWordPattern(textBox3.Text, textBox2.Text);
            UpdateUiState();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            possibleWordListView.Items.Clear();
            recommendedWordListView.Items.Clear();
            textBox3.Text = null;
            textBox1.Text = null;
            possibleWordCountLabel.Text = $"Count : 0";
            entropyListLabel.Text = $"Entropy : 0";
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