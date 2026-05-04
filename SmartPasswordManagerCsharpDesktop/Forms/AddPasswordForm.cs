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
    private Label _descriptionCounterLabel = null!;
    private bool _isPasswordVisible = false;

    private const int MaxDescriptionLength = 255;

    public string Description => _descriptionBox.Text.Trim();
    public string Secret => _secretBox.Text;
    public int Length => (int)_lengthBox.Value;

    public AddPasswordForm()
    {
        InitializeComponent();
        SetupSecretStrengthCheck();
        SetupDescriptionCounter();
    }

    private void SetupDescriptionCounter()
    {
        _descriptionBox.TextChanged += (s, e) => UpdateDescriptionCounter();
        UpdateDescriptionCounter();
    }

    private void UpdateDescriptionCounter()
    {
        int currentLength = _descriptionBox.Text.Length;
        int remaining = MaxDescriptionLength - currentLength;

        if (currentLength > MaxDescriptionLength)
        {
            _descriptionBox.Text = _descriptionBox.Text.Substring(0, MaxDescriptionLength);
            _descriptionBox.SelectionStart = MaxDescriptionLength;
            return;
        }

        string counterText = $"{currentLength}/{MaxDescriptionLength}";

        if (remaining < 0)
        {
            _descriptionCounterLabel.Text = $"⚠️ {counterText} EXCEEDED!";
            _descriptionCounterLabel.ForeColor = Color.FromArgb(220, 53, 69);
        }
        else if (remaining < 20)
        {
            _descriptionCounterLabel.Text = $"⚠️ {counterText} - {remaining} chars left";
            _descriptionCounterLabel.ForeColor = Color.FromArgb(255, 193, 7);
        }
        else
        {
            _descriptionCounterLabel.Text = $"📝 {counterText}";
            _descriptionCounterLabel.ForeColor = Color.FromArgb(160, 160, 170);
        }
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
        this.Size = new Size(560, 500);
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

        var descPanel = new Panel { Width = 500, Height = 95, Margin = new Padding(0, 0, 0, 8), BackColor = Color.FromArgb(32, 32, 38) };
        var descLabel = new Label { Text = "Description", Location = new Point(0, 0), Size = new Size(500, 25), ForeColor = Color.FromArgb(0, 122, 204), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        _descriptionBox = new TextBox { Location = new Point(0, 28), Size = new Size(500, 32), BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.FromArgb(220, 220, 230), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 11), MaxLength = MaxDescriptionLength };
        _descriptionCounterLabel = new Label { Location = new Point(0, 65), Size = new Size(500, 22), Font = new Font("Segoe UI", 8), TextAlign = ContentAlignment.MiddleRight };
        descPanel.Controls.Add(descLabel);
        descPanel.Controls.Add(_descriptionBox);
        descPanel.Controls.Add(_descriptionCounterLabel);
        flowLayout.Controls.Add(descPanel);

        var secretPanel = new Panel { Width = 500, Height = 70, Margin = new Padding(0, 0, 0, 8), BackColor = Color.FromArgb(32, 32, 38) };
        var secretLabel = new Label { Text = "Secret phrase", Location = new Point(0, 0), Size = new Size(500, 25), ForeColor = Color.FromArgb(0, 122, 204), Font = new Font("Segoe UI", 10, FontStyle.Bold) };

        var secretInputPanel = new Panel { Location = new Point(0, 28), Size = new Size(500, 32), BackColor = Color.FromArgb(45, 45, 52) };
        _secretBox = new TextBox { Location = new Point(1, 1), Size = new Size(415, 30), BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.FromArgb(220, 220, 230), BorderStyle = BorderStyle.None, UseSystemPasswordChar = true, Font = new Font("Segoe UI", 11) };
        _showHideButton = new Button { Text = "Show", Location = new Point(422, 1), Size = new Size(77, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 68), ForeColor = Color.FromArgb(220, 220, 230), Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
        _showHideButton.FlatAppearance.BorderSize = 0;
        _showHideButton.Click += (s, e) =>
        {
            _isPasswordVisible = !_isPasswordVisible;
            _secretBox.UseSystemPasswordChar = !_isPasswordVisible;
            _showHideButton.Text = _isPasswordVisible ? "Hide" : "Show";
            _showHideButton.BackColor = _isPasswordVisible ? Color.FromArgb(0, 122, 204) : Color.FromArgb(60, 60, 68);
        };
        secretInputPanel.Controls.Add(_secretBox);
        secretInputPanel.Controls.Add(_showHideButton);
        secretPanel.Controls.Add(secretLabel);
        secretPanel.Controls.Add(secretInputPanel);
        flowLayout.Controls.Add(secretPanel);

        var strengthPanel = new Panel { Width = 500, Height = 35, Margin = new Padding(0, 0, 0, 4), BackColor = Color.FromArgb(32, 32, 38) };
        _strengthLabel = new Label { Text = "⚪ Enter secret phrase", Location = new Point(0, 5), Size = new Size(500, 25), ForeColor = Color.FromArgb(160, 160, 170), Font = new Font("Segoe UI", 9, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft };
        strengthPanel.Controls.Add(_strengthLabel);
        flowLayout.Controls.Add(strengthPanel);

        var tipsPanel = new Panel { Width = 500, Height = 35, Margin = new Padding(0, 0, 0, 12), BackColor = Color.FromArgb(32, 32, 38) };
        var tipsLabel = new Label { Text = "💡 Use mixed case, numbers, symbols, or emoji (12+ chars minimum)", Location = new Point(0, 5), Size = new Size(500, 25), ForeColor = Color.FromArgb(108, 117, 125), Font = new Font("Segoe UI", 8.5f, FontStyle.Italic), TextAlign = ContentAlignment.MiddleLeft };
        tipsPanel.Controls.Add(tipsLabel);
        flowLayout.Controls.Add(tipsPanel);

        var lengthPanel = new Panel { Width = 500, Height = 70, Margin = new Padding(0, 0, 0, 20), BackColor = Color.FromArgb(32, 32, 38) };
        var lengthLabel = new Label { Text = "Password length", Location = new Point(0, 0), Size = new Size(500, 25), ForeColor = Color.FromArgb(0, 122, 204), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        _lengthBox = new NumericUpDown { Location = new Point(0, 28), Size = new Size(120, 32), BackColor = Color.FromArgb(45, 45, 52), ForeColor = Color.FromArgb(220, 220, 230), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 11), Minimum = 12, Maximum = 100, Value = 16, TextAlign = HorizontalAlignment.Center };
        lengthPanel.Controls.Add(lengthLabel);
        lengthPanel.Controls.Add(_lengthBox);
        flowLayout.Controls.Add(lengthPanel);

        var buttonPanel = new Panel { Width = 500, Height = 50, Margin = new Padding(0, 0, 0, 0), BackColor = Color.FromArgb(32, 32, 38) };
        _cancelButton = new Button { Text = "Cancel", Location = new Point(290, 5), Size = new Size(100, 42), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 68), ForeColor = Color.FromArgb(220, 220, 230), Font = new Font("Segoe UI", 10), DialogResult = DialogResult.Cancel, Cursor = Cursors.Hand };
        _cancelButton.FlatAppearance.BorderSize = 0;
        _okButton = new Button { Text = "Add Password", Location = new Point(395, 5), Size = new Size(105, 42), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), DialogResult = DialogResult.OK, Cursor = Cursors.Hand };
        _okButton.FlatAppearance.BorderSize = 0;
        buttonPanel.Controls.Add(_cancelButton);
        buttonPanel.Controls.Add(_okButton);
        flowLayout.Controls.Add(buttonPanel);

        mainContainer.Controls.Add(flowLayout);
        this.Controls.Add(mainContainer);

        _okButton.Click += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Description cannot be empty!\n\nPlease enter a description for this password.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (Description.Length > MaxDescriptionLength)
            {
                MessageBox.Show($"Description is too long!\n\nMaximum {MaxDescriptionLength} characters allowed.\nCurrent: {Description.Length} characters.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (Description.Any(c => c == '\"' || c == '\\' || c == '\n' || c == '\r' || c == '\t'))
            {
                MessageBox.Show("Description contains invalid characters!\n\nInvalid characters: \\ \" \\n \\r \\t\nPlease remove them and try again.",
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