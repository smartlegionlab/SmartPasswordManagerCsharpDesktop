using System.Diagnostics;

namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class HelpForm : Form
{
    private RichTextBox _contentBox = null!;
    private Button _closeButton = null!;
    private FlowLayoutPanel _buttonsPanel = null!;

    public HelpForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Help - Smart Password Manager";
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

        _contentBox.Text =
"══════════════════════════════════════════════════════════════════════════════\n" +
"  SMART PASSWORD MANAGER\n" +
"══════════════════════════════════════════════════════════════════════════════\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  HOW IT WORKS\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
"  • Add a password entry with description and secret phrase\n" +
"  • System generates a public key from your secret\n" +
"  • Password is generated deterministically from your secret\n" +
"  • Same secret + same length = same password every time\n\n" +

"  To retrieve a password:\n" +
"  → Select the entry → Click 'Get' → Enter secret phrase\n" +
"  → Password is copied to clipboard automatically\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  SECURITY NOTES\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
"  ✓ Passwords are NEVER stored anywhere\n" +
"  ✓ Secret phrase never leaves your device\n" +
"  ✓ Only metadata (description, length, public key) is stored\n" +
"  ✓ Lost secret phrase = permanently lost passwords\n" +
"  ✓ Secret phrase must be at least 12 characters\n" +
"  ⚠ Never use the description as your secret phrase!\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  KEYBOARD SHORTCUTS\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
"  Ctrl + N     →  Add new password\n" +
"  Ctrl + E     →  Edit selected password\n" +
"  Ctrl + G     →  Get password (copy to clipboard)\n" +
"  Ctrl + I     →  Import passwords\n" +
"  Ctrl + X     →  Export passwords\n" +
"  Ctrl + F     →  Focus search box\n" +
"  Ctrl + /     →  Show shortcuts\n" +
"  Delete       →  Delete selected password\n" +
"  F5           →  Refresh list\n" +
"  F1           →  Show this help\n" +
"  Enter        →  Get password (when item selected)\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  STORAGE LOCATIONS\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
"  🐧 Linux:   ~/.config/smart_password_manager/passwords.json\n" +
"  🪟 Windows: %USERPROFILE%\\.config\\smart_password_manager\\passwords.json\n" +
"  📦 Exports: ~/SmartPasswordManager/spm_export_*.json\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  REQUIREMENTS\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
"  • Secret phrase: Minimum 12 characters\n" +
"  • Password length: 12-100 characters\n" +
"  • .NET Runtime: Not required for pre-built binaries\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  PRO TIPS\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
"  • Use different secret phrases for different security levels\n" +
"  • Backup your passwords.json file regularly\n" +
"  • Export your metadata to JSON for safekeeping\n" +
"  • Test new secret phrases before deleting old ones\n" +
"  • Use emoji or non-Latin characters for stronger secrets\n\n" +

"══════════════════════════════════════════════════════════════════════════════\n" +
"  Version 1.0.1 | Copyright © 2026 Alexander Suvorov\n" +
"  Licensed under BSD 3-Clause License\n" +
"══════════════════════════════════════════════════════════════════════════════";

        contentPanel.Controls.Add(_contentBox);

        var bottomPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(35, 35, 42),
            Padding = new Padding(10)
        };

        _buttonsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0),
            BackColor = Color.FromArgb(35, 35, 42),
            WrapContents = false
        };

        var githubButton = CreateStyledButton("📁 GitHub", "https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop");
        var disclaimerButton = CreateStyledButton("⚠️ Disclaimer", "https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/DISCLAIMER.md");
        var licenseButton = CreateStyledButton("📄 License", "https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/LICENSE");
        var coreLibButton = CreateStyledButton("🔧 Core Lib", "https://github.com/smartlegionlab/smartpasslib-csharp");

        _closeButton = CreateStyledButton("Close", null);
        _closeButton.Click += (s, e) => this.Close();

        _buttonsPanel.Controls.Add(githubButton);
        _buttonsPanel.Controls.Add(disclaimerButton);
        _buttonsPanel.Controls.Add(licenseButton);
        _buttonsPanel.Controls.Add(coreLibButton);
        _buttonsPanel.Controls.Add(_closeButton);

        bottomPanel.Controls.Add(_buttonsPanel);

        mainLayout.Controls.Add(contentPanel, 0, 0);
        mainLayout.Controls.Add(bottomPanel, 0, 1);

        this.Controls.Add(mainLayout);
    }

    private Button CreateStyledButton(string text, string? url)
    {
        var button = new Button
        {
            Text = text,
            BackColor = Color.FromArgb(45, 45, 52),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Size = new Size(110, 38),
            Margin = new Padding(0, 0, 10, 0),
            TextAlign = ContentAlignment.MiddleCenter
        };

        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);

        if (url != null)
        {
            button.Click += (s, e) => OpenUrl(url);
        }

        button.MouseEnter += (s, e) =>
        {
            button.BackColor = Color.FromArgb(0, 122, 204);
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
        };

        button.MouseLeave += (s, e) =>
        {
            button.BackColor = Color.FromArgb(45, 45, 52);
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
        };

        return button;
    }

    private void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Cannot open link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}