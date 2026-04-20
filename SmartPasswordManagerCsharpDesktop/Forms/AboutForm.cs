using System.Diagnostics;

namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "About Smart Password Manager";
        this.Size = new Size(640, 550);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.FromArgb(28, 28, 35);
        this.ForeColor = Color.FromArgb(220, 220, 230);

        var mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30)
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 11,
            Padding = new Padding(0)
        };

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var iconLabel = new Label
        {
            Text = "🔐",
            Font = new Font("Segoe UI", 48),
            ForeColor = Color.FromArgb(0, 122, 204),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(iconLabel, 0, 0);

        var titleLabel = new Label
        {
            Text = "Smart Password Manager",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 122, 204),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(titleLabel, 0, 1);

        var versionLabel = new Label
        {
            Text = $"Version 1.0.0 | .NET 10.0 | Windows/Linux",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(160, 160, 170),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(versionLabel, 0, 2);

        var descLabel = new Label
        {
            Text = "Deterministic password manager\nSame secret + same length = same password\nZero-password storage architecture",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.FromArgb(200, 200, 200),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(descLabel, 0, 3);

        var separator = new Label
        {
            Text = "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━",
            ForeColor = Color.FromArgb(60, 60, 68),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(separator, 0, 4);

        var infoPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(35, 35, 42)
        };

        var infoLabel = new Label
        {
            Text = "🔒 Passwords never stored\n" +
                   "🔑 Deterministic generation\n" +
                   "🌐 Cross-platform compatible\n" +
                   "⚡ Lightweight & fast\n" +
                   "🛡️ Zero-knowledge architecture",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.FromArgb(180, 180, 190),
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Fill,
            Padding = new Padding(20, 10, 20, 10)
        };
        infoPanel.Controls.Add(infoLabel);
        layout.Controls.Add(infoPanel, 0, 5);

        var separator2 = new Label
        {
            Text = "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━",
            ForeColor = Color.FromArgb(60, 60, 68),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(separator2, 0, 6);

        var authorLabel = new Label
        {
            Text = "👨‍💻 Author: Alexander Suvorov",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(160, 160, 170),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(authorLabel, 0, 7);

        var licenseLabel = new Label
        {
            Text = "📄 License: BSD 3-Clause",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(160, 160, 170),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(licenseLabel, 0, 8);

        var disclaimerLabel = new Label
        {
            Text = "⚠️ Disclaimer: Use at your own risk",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.FromArgb(140, 140, 150),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(disclaimerLabel, 0, 9);

        var spacer = new Label
        {
            Dock = DockStyle.Fill,
            Text = ""
        };
        layout.Controls.Add(spacer, 0, 10);

        mainPanel.Controls.Add(layout);

        var buttonPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            BackColor = Color.FromArgb(35, 35, 42),
            Padding = new Padding(10)
        };

        var buttonsFlowPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0),
            BackColor = Color.FromArgb(35, 35, 42),
            WrapContents = false
        };

        var githubButton = CreateStyledButton("📁 GitHub", "https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop");
        var authorButton = CreateStyledButton("👨‍💻 Author", "https://github.com/smartlegionlab");
        var licenseButton = CreateStyledButton("📄 License", "https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/LICENSE");
        var disclaimerButton = CreateStyledButton("⚠️ Disclaimer", "https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/DISCLAIMER.md");
        var closeButton = CreateStyledButton("Close", null);
        closeButton.Click += (s, e) => this.Close();

        buttonsFlowPanel.Controls.Add(githubButton);
        buttonsFlowPanel.Controls.Add(authorButton);
        buttonsFlowPanel.Controls.Add(licenseButton);
        buttonsFlowPanel.Controls.Add(disclaimerButton);
        buttonsFlowPanel.Controls.Add(closeButton);

        buttonPanel.Controls.Add(buttonsFlowPanel);

        this.Controls.Add(mainPanel);
        this.Controls.Add(buttonPanel);
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