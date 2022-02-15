using Wordle;
using Wordle.BLL;
using Wordle.SAL;

namespace GUI
{
    public partial class Form1 : Form
    {
        private WordSearcher wordSearcher;
        public Form1()
        {
            InitializeComponent();
            wordSearcher = new WordSearcher(new CsvReader().GetAllWords("SAL/Lexique381.csv"));
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            wordSearcher.WordLength = int.Parse(textBox1.Text); ;
            possibleWordListBox.DataSource = wordSearcher.Search().OrderByDescending(t => t.Value).Take(20).Select(t => $"{t.Key} {t.Value}").ToList();
            recommendedWordListBox.DataSource = new Solver().GetEntropy(wordSearcher).OrderByDescending(t=>t.Value).Take(20).Select(t=>$"{t.Key} {t.Value}").ToList();
        }

        private void StepButtonClicked(object sender, EventArgs e)
        {
            new Rule().Filter(textBox3.Text, textBox2.Text.Select(MapPattern).ToList(), wordSearcher).Search();

            possibleWordListBox.DataSource = wordSearcher.Search().OrderByDescending(t=>t.Value).Take(20).Select(t => $"{t.Key} {t.Value}").ToList();
            recommendedWordListBox.DataSource = new Solver().GetEntropy(wordSearcher).OrderByDescending(t => t.Value).Take(20).Select(t => $"{t.Key} {t.Value}").ToList();
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
            wordSearcher = new WordSearcher(new CsvReader().GetAllWords("SAL/Lexique381.csv"));
            possibleWordListBox.DataSource = null;
            recommendedWordListBox.DataSource = null;
            textBox3.Text = null;
            textBox1.Text = null;

        }

        private void recommendedWordListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = recommendedWordListBox.SelectedItem?.ToString();
        }

        private void possibleWordListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = possibleWordListBox.SelectedItem?.ToString();
        }
    }
}