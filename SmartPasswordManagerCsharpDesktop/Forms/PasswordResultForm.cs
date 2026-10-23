namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class PasswordResultForm : Form
{
    private TextBox _passwordBox = null!;
    private Button _copyButton = null!;
    private Button _closeButton = null!;

    public PasswordResultForm(string password, string description, int length)
    {
        InitializeComponent(password, description, length);
    }

    private void InitializeComponent(string password, string description, int length)
    {
        this.Text = "Generated Password";
        this.Size = new Size(560, 400);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.FromArgb(32, 32, 38);
        this.ForeColor = Color.FromArgb(220, 220, 230);

        var mainContainer = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        var flowLayout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Color.FromArgb(32, 32, 38)
        };

        var successPanel = new Panel
        {
            Width = 500,
            Height = 45,
            Margin = new Padding(0, 0, 0, 16),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        var successLabel = new Label
        {
            Text = "Password generated successfully",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(40, 167, 69),
            Location = new Point(0, 10),
            Size = new Size(500, 30),
            TextAlign = ContentAlignment.MiddleCenter
        };
        successPanel.Controls.Add(successLabel);
        flowLayout.Controls.Add(successPanel);

        var descPanel = new Panel
        {
            Width = 500,
            Height = 60,
            Margin = new Padding(0, 0, 0, 12),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        var descLabel = new Label
        {
            Text = "Description",
            Location = new Point(0, 0),
            Size = new Size(500, 25),
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        var descriptionBox = new TextBox
        {
            Location = new Point(0, 28),
            Size = new Size(500, 32),
            BackColor = Color.FromArgb(45, 45, 52),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 11),
            Text = description,
            ReadOnly = true
        };

        descPanel.Controls.Add(descLabel);
        descPanel.Controls.Add(descriptionBox);
        flowLayout.Controls.Add(descPanel);

        var passwordPanel = new Panel
        {
            Width = 500,
            Height = 70,
            Margin = new Padding(0, 0, 0, 12),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        var passwordLabel = new Label
        {
            Text = $"Password ({length} characters)",
            Location = new Point(0, 0),
            Size = new Size(500, 25),
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        _passwordBox = new TextBox
        {
            Location = new Point(0, 28),
            Size = new Size(500, 32),
            BackColor = Color.FromArgb(45, 45, 52),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Consolas", 12),
            Text = password,
            ReadOnly = true
        };

        passwordPanel.Controls.Add(passwordLabel);
        passwordPanel.Controls.Add(_passwordBox);
        flowLayout.Controls.Add(passwordPanel);

        var hintPanel = new Panel
        {
            Width = 500,
            Height = 35,
            Margin = new Padding(0, 0, 0, 20),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        var hintLabel = new Label
        {
            Text = "You can select the password with mouse and copy it manually",
            Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
            ForeColor = Color.FromArgb(140, 140, 150),
            Location = new Point(0, 5),
            Size = new Size(500, 25),
            TextAlign = ContentAlignment.MiddleCenter
        };
        hintPanel.Controls.Add(hintLabel);
        flowLayout.Controls.Add(hintPanel);

        var buttonPanel = new Panel
        {
            Width = 500,
            Height = 50,
            Margin = new Padding(0, 0, 0, 0),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        _closeButton = new Button
        {
            Text = "Close",
            Location = new Point(230, 5),
            Size = new Size(120, 42),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(60, 60, 68),
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Segoe UI", 10),
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _closeButton.FlatAppearance.BorderSize = 0;
        _closeButton.Click += (s, e) => this.Close();

        _copyButton = new Button
        {
            Text = "Copy",
            Location = new Point(360, 5),
            Size = new Size(140, 42),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _copyButton.FlatAppearance.BorderSize = 0;
        _copyButton.Click += (s, e) => CopyToClipboard();

        buttonPanel.Controls.Add(_closeButton);
        buttonPanel.Controls.Add(_copyButton);
        flowLayout.Controls.Add(buttonPanel);

        mainContainer.Controls.Add(flowLayout);
        this.Controls.Add(mainContainer);

        this.Shown += (s, e) =>
        {
            _passwordBox.Focus();
            _passwordBox.SelectAll();
        };
    }

    private void CopyToClipboard()
    {
        try
        {
            Clipboard.SetText(_passwordBox.Text);

            var originalText = _copyButton.Text;
            _copyButton.Text = "Copied!";
            _copyButton.BackColor = Color.FromArgb(0, 122, 204);

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1500;
            timer.Tick += (s, e) =>
            {
                _copyButton.Text = originalText;
                _copyButton.BackColor = Color.FromArgb(40, 167, 69);
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to copy: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}