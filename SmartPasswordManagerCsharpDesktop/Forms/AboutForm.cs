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
        this.Size = new Size(640, 600);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.FromArgb(28, 28, 35);
        this.ForeColor = Color.FromArgb(220, 220, 230);

        var mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.FromArgb(28, 28, 35)
        };

        var contentPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Padding = new Padding(30, 20, 30, 20),
            BackColor = Color.FromArgb(28, 28, 35)
        };

        contentPanel.Controls.Add(new Label
        {
            Text = "🔐",
            Font = new Font("Segoe UI", 48),
            ForeColor = Color.FromArgb(0, 122, 204),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 80),
            Margin = new Padding(0, 0, 0, 10)
        });

        contentPanel.Controls.Add(new Label
        {
            Text = "Smart Password Manager",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 122, 204),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 40),
            Margin = new Padding(0, 0, 0, 5)
        });

        contentPanel.Controls.Add(new Label
        {
            Text = $"Version v1.1.3 | .NET 10.0 | Windows",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(160, 160, 170),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 25),
            Margin = new Padding(0, 0, 0, 15)
        });

        var descLabel = new Label
        {
            Text = "Deterministic password manager\nSame secret + same length = same password\nZero-password storage architecture\nDecentralized by design — no cloud, no database, no trust required",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.FromArgb(200, 200, 200),
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = false,
            Size = new Size(500, 75),
            Margin = new Padding(0, 0, 0, 15)
        };
        contentPanel.Controls.Add(descLabel);

        contentPanel.Controls.Add(new Label
        {
            Text = "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━",
            ForeColor = Color.FromArgb(60, 60, 68),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 20),
            Margin = new Padding(0, 0, 0, 15)
        });

        var infoPanel = new Panel
        {
            BackColor = Color.FromArgb(35, 35, 42),
            Size = new Size(500, 140),
            Margin = new Padding(0, 0, 0, 15),
            Padding = new Padding(15)
        };

        var infoLabel = new Label
        {
            Text = "🔒 Passwords never stored\n" +
                   "🔑 Deterministic generation\n" +
                   "🌐 Cross-platform compatible\n" +
                   "⚡ Lightweight & fast\n" +
                   "🛡️ Zero-knowledge architecture\n" +
                   "🏛️ Decentralized — no central servers",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.FromArgb(180, 180, 190),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
        infoPanel.Controls.Add(infoLabel);
        contentPanel.Controls.Add(infoPanel);

        contentPanel.Controls.Add(new Label
        {
            Text = "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━",
            ForeColor = Color.FromArgb(60, 60, 68),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 20),
            Margin = new Padding(0, 0, 0, 15)
        });

        contentPanel.Controls.Add(new Label
        {
            Text = "👨‍💻 Author: Alexander Suvorov",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(160, 160, 170),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 30),
            Margin = new Padding(0, 0, 0, 5)
        });

        contentPanel.Controls.Add(new Label
        {
            Text = "📄 License: BSD 3-Clause",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(160, 160, 170),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 30),
            Margin = new Padding(0, 0, 0, 5)
        });

        contentPanel.Controls.Add(new Label
        {
            Text = "⚠️ Disclaimer: Use at your own risk",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.FromArgb(140, 140, 150),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 25),
            Margin = new Padding(0, 0, 0, 5)
        });

        contentPanel.Controls.Add(new Label
        {
            Text = "🔒 Secret phrase: Minimum 12 characters",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.FromArgb(140, 140, 150),
            TextAlign = ContentAlignment.MiddleCenter,
            Size = new Size(500, 25),
            Margin = new Padding(0, 0, 0, 10)
        });

        mainPanel.Controls.Add(contentPanel);

        contentPanel.Location = new Point((mainPanel.Width - contentPanel.Width) / 2, 0);
        mainPanel.Resize += (s, e) =>
        {
            contentPanel.Location = new Point(Math.Max(0, (mainPanel.Width - contentPanel.Width) / 2), 0);
        };

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