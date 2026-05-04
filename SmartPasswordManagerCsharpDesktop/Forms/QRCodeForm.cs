using QRCoder;
using SmartLegionLab.SmartPassLib;
using System.Diagnostics;

namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class QRCodeForm : Form
{
    private SmartPassword _password = null!;
    private PictureBox _qrPictureBox = null!;
    private TextBox _descriptionTextBox = null!;
    private Label _lengthLabel = null!;
    private TextBox _publicKeyTextBox = null!;
    private Button _copyButton = null!;
    private Button _closeButton = null!;
    private Button _androidButton = null!;

    public QRCodeForm(SmartPassword password)
    {
        _password = password ?? throw new ArgumentNullException(nameof(password));
        InitializeComponent();
        GenerateQRCode();
    }

    private void InitializeComponent()
    {
        this.Text = "QR Code - Smart Password";
        this.Size = new Size(550, 700);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.FromArgb(28, 28, 35);
        this.ForeColor = Color.FromArgb(220, 220, 230);

        var bottomPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            BackColor = Color.FromArgb(35, 35, 42),
            Padding = new Padding(10)
        };

        var buttonsFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0),
            BackColor = Color.FromArgb(35, 35, 42),
            WrapContents = false
        };

        _closeButton = new Button
        {
            Text = "Close",
            BackColor = Color.FromArgb(60, 60, 68),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Size = new Size(100, 38),
            Margin = new Padding(5, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleCenter
        };
        _closeButton.FlatAppearance.BorderSize = 0;
        _closeButton.Click += (s, e) => this.Close();

        _copyButton = new Button
        {
            Text = "📋 Copy JSON",
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Size = new Size(120, 38),
            Margin = new Padding(5, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleCenter
        };
        _copyButton.FlatAppearance.BorderSize = 0;
        _copyButton.Click += (s, e) => CopyJSON();

        _androidButton = new Button
        {
            Text = "📲 Android App",
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Size = new Size(130, 38),
            Margin = new Padding(5, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleCenter
        };
        _androidButton.FlatAppearance.BorderSize = 0;
        _androidButton.Click += (s, e) => OpenAndroidUrl();

        buttonsFlow.Controls.Add(_closeButton);
        buttonsFlow.Controls.Add(_copyButton);
        buttonsFlow.Controls.Add(_androidButton);
        bottomPanel.Controls.Add(buttonsFlow);

        var scrollPanel = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.FromArgb(28, 28, 35)
        };

        var contentPanel = new Panel
        {
            Size = new Size(510, 600),
            Location = new Point(0, 0),
            BackColor = Color.FromArgb(28, 28, 35)
        };

        int y = 20;

        var titleLabel = new Label
        {
            Text = "📱 QR Code for Smart Password",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 122, 204),
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(0, y),
            Size = new Size(510, 40)
        };
        contentPanel.Controls.Add(titleLabel);
        y += 50;

        var descPanel = new Panel
        {
            BackColor = Color.FromArgb(35, 35, 42),
            Padding = new Padding(15),
            Location = new Point(15, y),
            Size = new Size(480, 90)
        };

        var descTitleLabel = new Label
        {
            Text = "Description:",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 122, 204),
            Location = new Point(0, 0),
            Size = new Size(450, 25)
        };

        _descriptionTextBox = new TextBox
        {
            Location = new Point(0, 28),
            Size = new Size(450, 50),
            BackColor = Color.FromArgb(45, 45, 52),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10),
            Text = _password.Description,
            ReadOnly = true,
            Multiline = true,
            WordWrap = true,
            ScrollBars = ScrollBars.Vertical
        };

        descPanel.Controls.Add(descTitleLabel);
        descPanel.Controls.Add(_descriptionTextBox);
        contentPanel.Controls.Add(descPanel);
        y += 100;

        var lengthPanel = new Panel
        {
            BackColor = Color.FromArgb(35, 35, 42),
            Padding = new Padding(15),
            Location = new Point(15, y),
            Size = new Size(480, 50)
        };

        var lengthTitleLabel = new Label
        {
            Text = "Password Length:",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 122, 204),
            Location = new Point(0, 0),
            Size = new Size(120, 25)
        };

        _lengthLabel = new Label
        {
            Text = $"{_password.Length} characters",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(220, 220, 230),
            Location = new Point(125, 0),
            Size = new Size(325, 25),
            TextAlign = ContentAlignment.MiddleLeft
        };

        lengthPanel.Controls.Add(lengthTitleLabel);
        lengthPanel.Controls.Add(_lengthLabel);
        contentPanel.Controls.Add(lengthPanel);
        y += 65;

        var qrPanel = new Panel
        {
            BackColor = Color.White,
            Padding = new Padding(15),
            Location = new Point(15, y),
            Size = new Size(480, 320)
        };

        _qrPictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom
        };
        qrPanel.Controls.Add(_qrPictureBox);
        contentPanel.Controls.Add(qrPanel);
        y += 335;

        var keyPanel = new Panel
        {
            BackColor = Color.FromArgb(35, 35, 42),
            Padding = new Padding(15),
            Location = new Point(15, y),
            Size = new Size(480, 120)
        };

        var keyTitleLabel = new Label
        {
            Text = "Public Key:",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 122, 204),
            Location = new Point(0, 0),
            Size = new Size(450, 25)
        };

        _publicKeyTextBox = new TextBox
        {
            Location = new Point(0, 28),
            Size = new Size(450, 75),
            BackColor = Color.FromArgb(45, 45, 52),
            ForeColor = Color.FromArgb(180, 180, 190),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Consolas", 9),
            Text = _password.PublicKey,
            ReadOnly = true,
            Multiline = true,
            WordWrap = true,
            ScrollBars = ScrollBars.Vertical
        };

        keyPanel.Controls.Add(keyTitleLabel);
        keyPanel.Controls.Add(_publicKeyTextBox);
        contentPanel.Controls.Add(keyPanel);
        y += 135;

        var noteLabel = new Label
        {
            Text = "📲 Scan this QR code with Smart Password Manager Android app\nto quickly access this password on your mobile device\n\n💡 Tip: Select and copy text from description or public key fields",
            Font = new Font("Segoe UI", 8),
            ForeColor = Color.FromArgb(140, 140, 150),
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(15, y),
            Size = new Size(480, 55)
        };
        contentPanel.Controls.Add(noteLabel);
        y += 65;

        contentPanel.Height = y;

        scrollPanel.Controls.Add(contentPanel);

        this.Controls.Add(bottomPanel);
        this.Controls.Add(scrollPanel);
    }

    private void GenerateQRCode()
    {
        try
        {
            var qrData = new
            {
                l = _password.Length,
                k = _password.PublicKey
            };

            string jsonData = System.Text.Json.JsonSerializer.Serialize(qrData);

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(jsonData, QRCodeGenerator.ECCLevel.H);
                using (var qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] qrCodeBytes = qrCode.GetGraphic(20);
                    using (var ms = new MemoryStream(qrCodeBytes))
                    {
                        var image = Image.FromStream(ms);
                        _qrPictureBox.Image = new Bitmap(image);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to generate QR code: {ex.Message}",
                "QR Code Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CopyJSON()
    {
        try
        {
            var qrData = new
            {
                l = _password.Length,
                k = _password.PublicKey
            };

            string jsonData = System.Text.Json.JsonSerializer.Serialize(qrData);
            Clipboard.SetText(jsonData);

            var originalText = _copyButton.Text;
            _copyButton.Text = "✅ Copied!";
            _copyButton.BackColor = Color.FromArgb(40, 167, 69);

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1500;
            timer.Tick += (s, e) =>
            {
                _copyButton.Text = originalText;
                _copyButton.BackColor = Color.FromArgb(0, 122, 204);
                timer.Stop();
            };
            timer.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to copy JSON: {ex.Message}",
                "Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OpenAndroidUrl()
    {
        try
        {
            string url = "https://github.com/smartlegionlab/smart-password-manager-android/releases";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Cannot open link: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}