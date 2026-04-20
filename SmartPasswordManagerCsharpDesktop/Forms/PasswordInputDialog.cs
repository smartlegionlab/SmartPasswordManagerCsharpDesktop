namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class PasswordInputDialog : Form
{
    private TextBox _secretBox = null!;
    private Button _okButton = null!;
    private Button _cancelButton = null!;
    private Button _showHideButton = null!;
    private bool _isPasswordVisible = false;

    public string Secret => _secretBox.Text;

    public PasswordInputDialog(string description, int length)
    {
        InitializeComponent(description, length);
    }

    private void InitializeComponent(string description, int length)
    {
        this.Text = "Enter Secret Phrase";
        this.Size = new Size(520, 300);
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
            RowCount = 5
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

        var descLabel = new Label
        {
            Text = "Description:",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        layout.Controls.Add(descLabel, 0, 0);

        var descriptionValueLabel = new Label
        {
            Text = description,
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Segoe UI", 11),
            TextAlign = ContentAlignment.MiddleLeft
        };
        layout.Controls.Add(descriptionValueLabel, 1, 0);

        var lengthLabel = new Label
        {
            Text = "Password Length:",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        layout.Controls.Add(lengthLabel, 0, 1);

        var lengthValueLabel = new Label
        {
            Text = $"{length} characters",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Segoe UI", 11),
            TextAlign = ContentAlignment.MiddleLeft
        };
        layout.Controls.Add(lengthValueLabel, 1, 1);

        var secretLabel = new Label
        {
            Text = "Secret phrase:",
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(secretLabel, 0, 2);

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
            Font = new Font("Segoe UI", 10),
            Height = 35
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
        layout.Controls.Add(secretPanel, 1, 2);

        var spacerLabel = new Label
        {
            Dock = DockStyle.Fill,
            Text = ""
        };
        layout.Controls.Add(spacerLabel, 0, 3);
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
            Text = "Generate",
            BackColor = Color.FromArgb(40, 167, 69),
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
        layout.Controls.Add(buttonPanel, 1, 4);

        this.Controls.Add(layout);

        _okButton.Click += (s, e) =>
        {
            if (string.IsNullOrEmpty(Secret))
            {
                MessageBox.Show("Secret phrase is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (Secret.Length < 12)
            {
                MessageBox.Show("Secret phrase must be at least 12 characters", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
            }
        };
    }
}