namespace GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.possibleWordLabel = new System.Windows.Forms.Label();
            this.StepButton = new System.Windows.Forms.Button();
            this.RestartButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.entropyListLabel = new System.Windows.Forms.Label();
            this.possibleWordCountLabel = new System.Windows.Forms.Label();
            this.possibleWordListView = new System.Windows.Forms.ListView();
            this.recommendedWordListView = new System.Windows.Forms.ListView();
            this.firstCharTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(191, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(50, 23);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Word Length\r\n";
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(13, 85);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.Text = "Search\r\n";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(533, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Recommended Word :";
            // 
            // possibleWordLabel
            // 
            this.possibleWordLabel.AutoSize = true;
            this.possibleWordLabel.Location = new System.Drawing.Point(262, 42);
            this.possibleWordLabel.Name = "possibleWordLabel";
            this.possibleWordLabel.Size = new System.Drawing.Size(93, 15);
            this.possibleWordLabel.TabIndex = 6;
            this.possibleWordLabel.Text = "Possible Words :";
            // 
            // StepButton
            // 
            this.StepButton.Location = new System.Drawing.Point(12, 209);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(75, 23);
            this.StepButton.TabIndex = 7;
            this.StepButton.Text = "Next step";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new System.EventHandler(this.StepButtonClicked);
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(120, 209);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(75, 23);
            this.RestartButton.TabIndex = 8;
            this.RestartButton.Text = "Restart";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Pattern";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(94, 153);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(101, 23);
            this.textBox2.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "Entered Word";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(94, 124);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(101, 23);
            this.textBox3.TabIndex = 12;
            // 
            // entropyListLabel
            // 
            this.entropyListLabel.AutoSize = true;
            this.entropyListLabel.Location = new System.Drawing.Point(262, 85);
            this.entropyListLabel.Name = "entropyListLabel";
            this.entropyListLabel.Size = new System.Drawing.Size(63, 15);
            this.entropyListLabel.TabIndex = 13;
            this.entropyListLabel.Text = "Entropy : 0";
            // 
            // possibleWordCountLabel
            // 
            this.possibleWordCountLabel.AutoSize = true;
            this.possibleWordCountLabel.Location = new System.Drawing.Point(262, 70);
            this.possibleWordCountLabel.Name = "possibleWordCountLabel";
            this.possibleWordCountLabel.Size = new System.Drawing.Size(55, 15);
            this.possibleWordCountLabel.TabIndex = 14;
            this.possibleWordCountLabel.Text = "Count : 0";
            // 
            // possibleWordListView
            // 
            this.possibleWordListView.Location = new System.Drawing.Point(262, 109);
            this.possibleWordListView.Name = "possibleWordListView";
            this.possibleWordListView.Size = new System.Drawing.Size(235, 184);
            this.possibleWordListView.TabIndex = 15;
            this.possibleWordListView.UseCompatibleStateImageBehavior = false;
            this.possibleWordListView.SelectedIndexChanged += new System.EventHandler(this.possibleWordListView_SelectedIndexChanged);
            // 
            // recommendedWordListView
            // 
            this.recommendedWordListView.Location = new System.Drawing.Point(533, 109);
            this.recommendedWordListView.Name = "recommendedWordListView";
            this.recommendedWordListView.Size = new System.Drawing.Size(162, 184);
            this.recommendedWordListView.TabIndex = 16;
            this.recommendedWordListView.UseCompatibleStateImageBehavior = false;
            this.recommendedWordListView.SelectedIndexChanged += new System.EventHandler(this.recommendedWordListView_SelectedIndexChanged);
            // 
            // firstCharTextBox
            // 
            this.firstCharTextBox.Location = new System.Drawing.Point(191, 58);
            this.firstCharTextBox.Name = "firstCharTextBox";
            this.firstCharTextBox.Size = new System.Drawing.Size(50, 23);
            this.firstCharTextBox.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 15);
            this.label3.TabIndex = 18;
            this.label3.Text = "First Character (can be empty) :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 305);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.firstCharTextBox);
            this.Controls.Add(this.recommendedWordListView);
            this.Controls.Add(this.possibleWordListView);
            this.Controls.Add(this.possibleWordCountLabel);
            this.Controls.Add(this.entropyListLabel);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RestartButton);
            this.Controls.Add(this.StepButton);
            this.Controls.Add(this.possibleWordLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBox1;
        private Label label1;
        private Button SearchButton;
        private Label label2;
        private Label possibleWordLabel;
        private Button StepButton;
        private Button RestartButton;
        private Label label4;
        private TextBox textBox2;
        private Label label5;
        private TextBox textBox3;
        private Label entropyListLabel;
        private Label possibleWordCountLabel;
        private ListView possibleWordListView;
        private ListView recommendedWordListView;
        private TextBox firstCharTextBox;
        private Label label3;
    }
}