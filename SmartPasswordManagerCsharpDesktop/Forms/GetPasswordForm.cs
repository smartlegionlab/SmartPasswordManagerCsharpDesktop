namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class GetPasswordForm : Form
{
    private TextBox _secretBox = null!;
    private Button _okButton = null!;
    private Button _cancelButton = null!;
    private Button _showHideButton = null!;
    private bool _isPasswordVisible = false;

    public string Secret => _secretBox.Text;

    public GetPasswordForm(string description, int length)
    {
        InitializeComponent(description, length);
    }

    private void InitializeComponent(string description, int length)
    {
        this.Text = "Get Password";
        this.Size = new Size(560, 380);
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
            Height = 65,
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

        var descriptionValueLabel = new Label
        {
            Text = description,
            Location = new Point(0, 28),
            Size = new Size(500, 25),
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Segoe UI", 11)
        };

        descPanel.Controls.Add(descLabel);
        descPanel.Controls.Add(descriptionValueLabel);
        flowLayout.Controls.Add(descPanel);

        var lengthPanel = new Panel
        {
            Width = 500,
            Height = 65,
            Margin = new Padding(0, 0, 0, 16),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        var lengthLabel = new Label
        {
            Text = "Password Length",
            Location = new Point(0, 0),
            Size = new Size(500, 25),
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        var lengthValueLabel = new Label
        {
            Text = $"{length} characters",
            Location = new Point(0, 28),
            Size = new Size(500, 25),
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Segoe UI", 11)
        };

        lengthPanel.Controls.Add(lengthLabel);
        lengthPanel.Controls.Add(lengthValueLabel);
        flowLayout.Controls.Add(lengthPanel);

        var secretPanel = new Panel
        {
            Width = 500,
            Height = 70,
            Margin = new Padding(0, 0, 0, 20),
            BackColor = Color.FromArgb(32, 32, 38)
        };

        var secretLabel = new Label
        {
            Text = "Secret phrase",
            Location = new Point(0, 0),
            Size = new Size(500, 25),
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        var secretInputPanel = new Panel
        {
            Location = new Point(0, 28),
            Size = new Size(500, 32),
            BackColor = Color.FromArgb(45, 45, 52)
        };

        _secretBox = new TextBox
        {
            Location = new Point(1, 1),
            Size = new Size(415, 30),
            BackColor = Color.FromArgb(45, 45, 52),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.None,
            UseSystemPasswordChar = true,
            Font = new Font("Segoe UI", 11)
        };

        _showHideButton = new Button
        {
            Text = "Show",
            Location = new Point(422, 1),
            Size = new Size(77, 30),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(60, 60, 68),
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
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
            Text = "Generate Password",
            Location = new Point(395, 0),
            Size = new Size(105, 42),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(40, 167, 69),
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