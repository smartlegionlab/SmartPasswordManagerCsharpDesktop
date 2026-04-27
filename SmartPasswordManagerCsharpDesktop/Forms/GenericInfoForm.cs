using System.Diagnostics;

namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class GenericInfoForm : Form
{
    private RichTextBox _contentBox = null!;
    private Button _closeButton = null!;

    public GenericInfoForm(string title, string content)
    {
        InitializeComponent(title, content);
    }

    private void InitializeComponent(string title, string content)
    {
        this.Text = title;
        this.Size = new Size(800, 650);
        this.StartPosition = FormStartPosition.CenterParent;
        this.MinimumSize = new Size(750, 550);
        this.BackColor = Color.FromArgb(28, 28, 35);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            Padding = new Padding(0)
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));

        var contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10),
            BackColor = Color.FromArgb(28, 28, 35)
        };

        _contentBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            BackColor = Color.FromArgb(35, 35, 42),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.None,
            Font = new Font("Segoe UI", 10),
            ScrollBars = RichTextBoxScrollBars.Vertical,
            WordWrap = true
        };

        _contentBox.Text = content;

        contentPanel.Controls.Add(_contentBox);

        var bottomPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(35, 35, 42),
            Padding = new Padding(10)
        };

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0),
            BackColor = Color.FromArgb(35, 35, 42),
            WrapContents = false
        };

        _closeButton = new Button
        {
            Text = "Close",
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Size = new Size(100, 38),
            TextAlign = ContentAlignment.MiddleCenter
        };
        _closeButton.FlatAppearance.BorderSize = 0;
        _closeButton.Click += (s, e) => this.Close();

        buttonPanel.Controls.Add(_closeButton);
        bottomPanel.Controls.Add(buttonPanel);

        mainLayout.Controls.Add(contentPanel, 0, 0);
        mainLayout.Controls.Add(bottomPanel, 0, 1);

        this.Controls.Add(mainLayout);
    }
}