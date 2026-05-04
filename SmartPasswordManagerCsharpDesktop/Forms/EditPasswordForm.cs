namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class EditPasswordForm : Form
{
    private TextBox _descriptionBox = null!;
    private NumericUpDown _lengthBox = null!;
    private Button _okButton = null!;
    private Button _cancelButton = null!;
    private Label _descriptionCounterLabel = null!;

    private const int MaxDescriptionLength = 255;

    public string Description => _descriptionBox.Text.Trim();
    public int Length => (int)_lengthBox.Value;

    public EditPasswordForm(string currentDescription, int currentLength)
    {
        InitializeComponent();
        _descriptionBox.Text = currentDescription;
        _lengthBox.Value = currentLength;
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

    private void InitializeComponent()
    {
        this.Text = "Edit Smart Password";
        this.Size = new Size(560, 350);
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

        var descPanel = new Panel
        {
            Width = 500,
            Height = 95,
            Margin = new Padding(0, 0, 0, 16),
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

        _descriptionBox = new TextBox
        {
            Location = new Point(0, 28),
            Size = new Size(500, 32),
            BackColor = Color.FromArgb(45, 45, 52),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 11),
            MaxLength = MaxDescriptionLength
        };

        _descriptionCounterLabel = new Label
        {
            Location = new Point(0, 65),
            Size = new Size(500, 22),
            Font = new Font("Segoe UI", 8),
            TextAlign = ContentAlignment.MiddleRight
        };

        descPanel.Controls.Add(descLabel);
        descPanel.Controls.Add(_descriptionBox);
        descPanel.Controls.Add(_descriptionCounterLabel);
        flowLayout.Controls.Add(descPanel);

        var lengthPanel = new Panel
        {
            Width = 500,
            Height = 70,
            Margin = new Padding(0, 0, 0, 24),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        var lengthLabel = new Label
        {
            Text = "Password length",
            Location = new Point(0, 0),
            Size = new Size(500, 25),
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        _lengthBox = new NumericUpDown
        {
            Location = new Point(0, 28),
            Size = new Size(120, 32),
            BackColor = Color.FromArgb(45, 45, 52),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 11),
            Minimum = 12,
            Maximum = 100,
            Value = 16,
            TextAlign = HorizontalAlignment.Center
        };

        lengthPanel.Controls.Add(lengthLabel);
        lengthPanel.Controls.Add(_lengthBox);
        flowLayout.Controls.Add(lengthPanel);

        var buttonPanel = new Panel
        {
            Width = 500,
            Height = 42,
            Margin = new Padding(0, 0, 0, 0),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        _cancelButton = new Button
        {
            Text = "Cancel",
            Location = new Point(290, 0),
            Size = new Size(100, 42),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(60, 60, 68),
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Segoe UI", 10),
            DialogResult = DialogResult.Cancel,
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _cancelButton.FlatAppearance.BorderSize = 0;

        _okButton = new Button
        {
            Text = "Save Changes",
            Location = new Point(395, 0),
            Size = new Size(105, 42),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            DialogResult = DialogResult.OK,
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleCenter
        };
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
        };
    }
}