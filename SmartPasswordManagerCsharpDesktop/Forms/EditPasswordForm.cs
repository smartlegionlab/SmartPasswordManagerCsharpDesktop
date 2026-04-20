namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class EditPasswordForm : Form
{
    private TextBox _descriptionBox = null!;
    private NumericUpDown _lengthBox = null!;
    private Button _okButton = null!;
    private Button _cancelButton = null!;

    public string Description => _descriptionBox.Text.Trim();
    public int Length => (int)_lengthBox.Value;

    public EditPasswordForm(string currentDescription, int currentLength)
    {
        InitializeComponent();
        _descriptionBox.Text = currentDescription;
        _lengthBox.Value = currentLength;
    }

    private void InitializeComponent()
    {
        this.Text = "Edit Smart Password";
        this.Size = new Size(520, 260);
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
            RowCount = 4
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
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

        var lengthLabel = new Label
        {
            Text = "Password length:",
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(lengthLabel, 0, 1);

        _lengthBox = new NumericUpDown
        {
            Minimum = 12,
            Maximum = 100,
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(40, 40, 48),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10)
        };
        layout.Controls.Add(_lengthBox, 1, 1);

        var spacerLabel = new Label
        {
            Dock = DockStyle.Fill,
            Text = ""
        };
        layout.Controls.Add(spacerLabel, 0, 2);
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
            Text = "Save",
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
        layout.Controls.Add(buttonPanel, 1, 3);

        this.Controls.Add(layout);
    }
}