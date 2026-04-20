using System.Diagnostics;

namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class ShortcutsForm : Form
{
    public ShortcutsForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Keyboard Shortcuts";
        this.Size = new Size(500, 450);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.FromArgb(28, 28, 35);

        var mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10)
        };

        var titleLabel = new Label
        {
            Text = "⌨️ KEYBOARD SHORTCUTS",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 122, 204),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 45,
            BackColor = Color.FromArgb(35, 35, 42)
        };

        var listBox = new ListBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(32, 32, 38),
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Consolas", 11),
            BorderStyle = BorderStyle.None,
            IntegralHeight = false,
            DrawMode = DrawMode.OwnerDrawFixed,
            ItemHeight = 30
        };

        listBox.DrawItem += (s, e) =>
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                var text = listBox.Items[e.Index].ToString();
                var textColor = e.State.HasFlag(DrawItemState.Selected) ?
                    Color.White : Color.FromArgb(220, 220, 230);

                using (var brush = new SolidBrush(textColor))
                {
                    e.Graphics.DrawString(text, listBox.Font, brush, e.Bounds.X + 5, e.Bounds.Y + 5);
                }
            }
            e.DrawFocusRectangle();
        };

        listBox.Items.Add("Ctrl + N     →  Add new password");
        listBox.Items.Add("Ctrl + E     →  Edit selected password");
        listBox.Items.Add("Ctrl + G     →  Get password (copy to clipboard)");
        listBox.Items.Add("Ctrl + I     →  Import passwords");
        listBox.Items.Add("Ctrl + X     →  Export passwords");
        listBox.Items.Add("Ctrl + F     →  Focus search box");
        listBox.Items.Add("Ctrl + /     →  Show this shortcuts");
        listBox.Items.Add("Delete       →  Delete selected password");
        listBox.Items.Add("F5           →  Refresh list");
        listBox.Items.Add("F1           →  Show help");
        listBox.Items.Add("Enter        →  Get password (when item selected)");

        mainPanel.Controls.Add(listBox);
        mainPanel.Controls.Add(titleLabel);

        var buttonPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            BackColor = Color.FromArgb(35, 35, 42),
            Padding = new Padding(10)
        };

        var closeButton = new Button
        {
            Text = "Close",
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(120, 38),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            TextAlign = ContentAlignment.MiddleCenter
        };
        closeButton.FlatAppearance.BorderSize = 0;
        closeButton.Click += (s, e) => this.Close();

        buttonPanel.Controls.Add(closeButton);

        void PositionButton()
        {
            closeButton.Location = new Point(buttonPanel.ClientSize.Width - closeButton.Width - 10,
                                            (buttonPanel.ClientSize.Height - closeButton.Height) / 2);
        }

        buttonPanel.Resize += (s, e) => PositionButton();
        this.Shown += (s, e) => PositionButton();
        PositionButton();

        this.Controls.Add(mainPanel);
        this.Controls.Add(buttonPanel);
    }
}