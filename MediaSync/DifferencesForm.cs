using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MediaSync
{
    public partial class DifferencesForm : Form
    {
        private RichTextBox richTextBox;

        public DifferencesForm(List<string> differences, int diffCount = 0)
        {
            richTextBox = new RichTextBox();
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.ReadOnly = true;
            richTextBox.Text = string.Join(Environment.NewLine, differences);

            this.Controls.Add(richTextBox);
            this.Text = "Differences "+diffCount.ToString();
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.MaximumSize = new System.Drawing.Size(800, 400);
            this.Size = new System.Drawing.Size(800, 400);
        }
    }
}