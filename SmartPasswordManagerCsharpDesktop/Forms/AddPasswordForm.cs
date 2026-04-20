using System.Diagnostics;

namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class AddPasswordForm : Form
{
    private TextBox _descriptionBox = null!;
    private TextBox _secretBox = null!;
    private NumericUpDown _lengthBox = null!;
    private Button _okButton = null!;
    private Button _cancelButton = null!;
    private Button _showHideButton = null!;
    private Label _strengthLabel = null!;
    private bool _isPasswordVisible = false;

    public string Description => _descriptionBox.Text.Trim();
    public string Secret => _secretBox.Text;
    public int Length => (int)_lengthBox.Value;

    public AddPasswordForm()
    {
        InitializeComponent();
        SetupSecretStrengthCheck();
    }

    private void SetupSecretStrengthCheck()
    {
        _secretBox.TextChanged += (s, e) => UpdateStrengthIndicator();
        UpdateStrengthIndicator();
    }

    private void UpdateStrengthIndicator()
    {
        var secret = _secretBox.Text;
        var length = secret.Length;

        string strengthText;
        Color strengthColor;

        if (length == 0)
        {
            strengthText = "⚪ Enter secret phrase";
            strengthColor = Color.FromArgb(160, 160, 170);
        }
        else if (length < 12)
        {
            strengthText = $"🔴 TOO SHORT ({length}/12) - Must be at least 12 characters!";
            strengthColor = Color.FromArgb(220, 53, 69);
        }
        else if (length < 16)
        {
            strengthText = $"🟡 WEAK ({length} chars) - Use longer phrase (16+ recommended)";
            strengthColor = Color.FromArgb(255, 193, 7);
        }
        else if (length < 24)
        {
            strengthText = $"🟢 GOOD ({length} chars) - Acceptable strength";
            strengthColor = Color.FromArgb(40, 167, 69);
        }
        else
        {
            strengthText = $"💪 STRONG ({length} chars) - Excellent!";
            strengthColor = Color.FromArgb(0, 122, 204);
        }

        if (length >= 12)
        {
            bool hasUpper = secret.Any(char.IsUpper);
            bool hasLower = secret.Any(char.IsLower);
            bool hasDigit = secret.Any(char.IsDigit);
            bool hasNonLatin = secret.Any(ch => ch > 0x7F);

            if (!hasUpper || !hasLower || !hasDigit)
            {
                strengthText += " ⚠️ Add uppercase, lowercase and numbers";
                strengthColor = Color.FromArgb(255, 193, 7);
            }
            else if (hasNonLatin)
            {
                strengthText += " ✨ Good! Using extended characters";
            }
        }

        _strengthLabel.Text = strengthText;
        _strengthLabel.ForeColor = strengthColor;
    }

    private void InitializeComponent()
    {
        this.Text = "Add Smart Password";
        this.Size = new Size(550, 420);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.FromArgb(28, 28, 35);
        this.ForeColor = Color.FromArgb(220, 220, 230);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(25),
            ColumnCount = 2,
            RowCount = 7
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

        var descLabel = new Label
        {
            Text = "Description:",
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(descLabel, 0, 0);

        _descriptionBox = new TextBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(40, 40, 48),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10)
        };
        layout.Controls.Add(_descriptionBox, 1, 0);

        var secretLabel = new Label
        {
            Text = "Secret phrase:",
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(secretLabel, 0, 1);

        var secretPanel = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            Padding = new Padding(0),
            Margin = new Padding(0)
        };
        secretPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        secretPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));

        _secretBox = new TextBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(40, 40, 48),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            UseSystemPasswordChar = true,
            Font = new Font("Segoe UI", 10)
        };

        _showHideButton = new Button
        {
            Text = "Show",
            Dock = DockStyle.Fill,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(60, 60, 68),
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _showHideButton.FlatAppearance.BorderSize = 0;
        _showHideButton.Click += (s, e) =>
        {
            _isPasswordVisible = !_isPasswordVisible;
            _secretBox.UseSystemPasswordChar = !_isPasswordVisible;
            _showHideButton.Text = _isPasswordVisible ? "Hide" : "Show";
            _showHideButton.BackColor = _isPasswordVisible ? Color.FromArgb(0, 122, 204) : Color.FromArgb(60, 60, 68);
        };

        secretPanel.Controls.Add(_secretBox, 0, 0);
        secretPanel.Controls.Add(_showHideButton, 1, 0);
        layout.Controls.Add(secretPanel, 1, 1);

        _strengthLabel = new Label
        {
            Text = "⚪ Enter secret phrase",
            ForeColor = Color.FromArgb(160, 160, 170),
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
        layout.Controls.Add(_strengthLabel, 1, 2);

        var tipsLabel = new Label
        {
            Text = "💡 Use mixed case, numbers, symbols, or emoji (12+ chars minimum)",
            ForeColor = Color.FromArgb(108, 117, 125),
            Font = new Font("Segoe UI", 8, FontStyle.Italic),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
        layout.Controls.Add(tipsLabel, 1, 3);

        var lengthLabel = new Label
        {
            Text = "Password length:",
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(lengthLabel, 0, 4);

        _lengthBox = new NumericUpDown
        {
            Minimum = 12,
            Maximum = 100,
            Value = 16,
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(40, 40, 48),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10)
        };
        layout.Controls.Add(_lengthBox, 1, 4);

        var spacerLabel = new Label
        {
            Dock = DockStyle.Fill,
            Text = ""
        };
        layout.Controls.Add(spacerLabel, 0, 5);
        layout.SetColumnSpan(spacerLabel, 2);

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0),
            Margin = new Padding(0)
        };

        _okButton = new Button
        {
            Text = "Add",
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(110, 38),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            DialogResult = DialogResult.OK,
            Cursor = Cursors.Hand
        };
        _okButton.FlatAppearance.BorderSize = 0;

        _cancelButton = new Button
        {
            Text = "Cancel",
            BackColor = Color.FromArgb(60, 60, 68),
            ForeColor = Color.FromArgb(220, 220, 230),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(110, 38),
            Font = new Font("Segoe UI", 10),
            DialogResult = DialogResult.Cancel,
            Cursor = Cursors.Hand
        };
        _cancelButton.FlatAppearance.BorderSize = 0;

        buttonPanel.Controls.Add(_okButton);
        buttonPanel.Controls.Add(_cancelButton);
        layout.Controls.Add(buttonPanel, 1, 6);

        this.Controls.Add(layout);

        _okButton.Click += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Description cannot be empty!\n\nPlease enter a description for this password.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(Secret))
            {
                MessageBox.Show("Secret phrase is required!\n\nPlease enter your secret phrase.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (Secret.Length < 12)
            {
                MessageBox.Show(
                    "SECURITY ERROR!\n\n" +
                    $"Your secret phrase is only {Secret.Length} characters long.\n" +
                    "Secret phrase must be at least 12 characters.\n" +
                    "Short phrases are vulnerable to brute force attacks.\n\n" +
                    "Please use a longer, more complex secret phrase.",
                    "Security Violation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (Secret.Length < 16)
            {
                var result = MessageBox.Show(
                    "SECURITY WARNING!\n\n" +
                    $"Your secret phrase is only {Secret.Length} characters long.\n" +
                    "For better security, we strongly recommend using 16+ characters\n" +
                    "with mixed case, numbers, and symbols.\n\n" +
                    "Are you sure you want to continue with this secret phrase?",
                    "Weak Secret Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            if (Secret.Equals(Description, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "SECURITY ERROR!\n\n" +
                    "Your secret phrase is the same as the description!\n" +
                    "This is EXTREMELY UNSAFE!\n\n" +
                    "Please use a different, more complex secret phrase.",
                    "Security Violation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
        };
    }
}