using SmartLegionLab.SmartPasswordLib;
using System.Diagnostics;
using System.Text.Json;

namespace SmartPasswordManagerCsharpDesktop.Forms;

public partial class MainForm : Form
{
    private SmartPasswordManager _manager = null!;
    private string _exportDir = null!;
    private List<SmartPassword> _allPasswords = null!;

    private MenuStrip _menuStrip = null!;
    private Panel _headerPanel = null!;
    private Panel _topButtonPanel = null!;
    private Panel _bottomButtonPanel = null!;
    private ListView _listView = null!;
    private StatusStrip _statusStrip = null!;
    private TableLayoutPanel _mainLayout = null!;
    private TextBox _searchBox = null!;
    private Label _searchLabel = null!;
    private Button _btnClearSearch = null!;

    public MainForm()
    {
        InitializeManager();
        InitializeComponent();
        LoadPasswords();
    }

    private void InitializeManager()
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _exportDir = Path.Combine(home, "SmartPasswordManager");
        Directory.CreateDirectory(_exportDir);
        _manager = new SmartPasswordManager();
        _allPasswords = new List<SmartPassword>();
    }

    private void InitializeComponent()
    {
        this.Text = "Smart Password Manager v1.0.0";
        this.Size = new Size(1100, 750);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.MinimumSize = new Size(800, 600);
        this.BackColor = Color.FromArgb(18, 18, 18);
        this.KeyPreview = true;
        this.KeyDown += MainForm_KeyDown;

        _mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6,
            BackColor = Color.FromArgb(18, 18, 18)
        };

        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

        _menuStrip = new MenuStrip
        {
            BackColor = Color.FromArgb(30, 30, 35),
            ForeColor = Color.FromArgb(220, 220, 230)
        };

        _menuStrip.RenderMode = ToolStripRenderMode.System;

        var fileMenu = new ToolStripMenuItem("File");
        fileMenu.DropDownItems.Add("Export", null, (s, e) => ExportPasswords());
        fileMenu.DropDownItems.Add("Import", null, (s, e) => ImportPasswords());
        fileMenu.DropDownItems.Add(new ToolStripSeparator());
        fileMenu.DropDownItems.Add("Exit", null, (s, e) => Application.Exit());

        var passwordsMenu = new ToolStripMenuItem("Passwords");
        passwordsMenu.DropDownItems.Add("Add", null, (s, e) => AddPassword());
        passwordsMenu.DropDownItems.Add("Edit", null, (s, e) => EditPassword());
        passwordsMenu.DropDownItems.Add("Get Password", null, (s, e) => GetPassword());
        passwordsMenu.DropDownItems.Add(new ToolStripSeparator());
        passwordsMenu.DropDownItems.Add("Delete", null, (s, e) => DeletePassword());
        passwordsMenu.DropDownItems.Add(new ToolStripSeparator());
        passwordsMenu.DropDownItems.Add("Refresh", null, (s, e) => LoadPasswords());

        var helpMenu = new ToolStripMenuItem("Help");
        helpMenu.DropDownItems.Add("📖 Help", null, (s, e) => ShowHelp());
        helpMenu.DropDownItems.Add("⌨️ Shortcuts", null, (s, e) => ShowShortcuts());
        helpMenu.DropDownItems.Add(new ToolStripSeparator());
        helpMenu.DropDownItems.Add("⚠️ Disclaimer", null, (s, e) => OpenDisclaimer());
        helpMenu.DropDownItems.Add(new ToolStripSeparator());
        helpMenu.DropDownItems.Add("ℹ️ About", null, (s, e) => ShowAbout());

        _menuStrip.Items.Add(fileMenu);
        _menuStrip.Items.Add(passwordsMenu);
        _menuStrip.Items.Add(helpMenu);

        _headerPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(25, 25, 30)
        };

        var titleLabel = new Label
        {
            Text = "Smart Password Manager",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 122, 204),
            Location = new Point(20, 10),
            Size = new Size(400, 35)
        };

        var subtitleLabel = new Label
        {
            Text = "Deterministic password manager - same secret + same length = same password",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.FromArgb(160, 160, 170),
            Location = new Point(20, 45),
            Size = new Size(700, 25)
        };

        _headerPanel.Controls.Add(titleLabel);
        _headerPanel.Controls.Add(subtitleLabel);

        _topButtonPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(35, 35, 42)
        };

        var btnAdd = new Button { Text = "+ Add", BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(15, 10), Cursor = Cursors.Hand };
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.Click += (s, e) => AddPassword();

        var btnImport = new Button { Text = "📥 Import", BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(125, 10), Cursor = Cursors.Hand };
        btnImport.FlatAppearance.BorderSize = 0;
        btnImport.Click += (s, e) => ImportPasswords();

        var btnExport = new Button { Text = "📤 Export", BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(235, 10), Cursor = Cursors.Hand };
        btnExport.FlatAppearance.BorderSize = 0;
        btnExport.Click += (s, e) => ExportPasswords();

        var btnRefresh = new Button { Text = "⟳ Refresh", BackColor = Color.FromArgb(52, 58, 64), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(345, 10), Cursor = Cursors.Hand };
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.Click += (s, e) => LoadPasswords();

        var btnAbout = new Button { Text = "ℹ️ About", BackColor = Color.FromArgb(111, 66, 193), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(455, 10), Cursor = Cursors.Hand };
        btnAbout.FlatAppearance.BorderSize = 0;
        btnAbout.Click += (s, e) => ShowAbout();

        _topButtonPanel.Controls.Add(btnAdd);
        _topButtonPanel.Controls.Add(btnImport);
        _topButtonPanel.Controls.Add(btnExport);
        _topButtonPanel.Controls.Add(btnRefresh);
        _topButtonPanel.Controls.Add(btnAbout);

        var searchPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(35, 35, 42),
            Padding = new Padding(20, 10, 20, 10)
        };

        _searchLabel = new Label
        {
            Text = "🔍",
            ForeColor = Color.FromArgb(0, 122, 204),
            Font = new Font("Segoe UI", 14),
            Location = new Point(20, 12),
            Size = new Size(30, 25),
            TextAlign = ContentAlignment.MiddleCenter
        };

        _searchBox = new TextBox
        {
            Location = new Point(55, 10),
            Width = searchPanel.Width - 150,
            Height = 30,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Color.FromArgb(45, 45, 52),
            ForeColor = Color.FromArgb(220, 220, 230),
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10)
        };
        _searchBox.TextChanged += (s, e) => ApplyFilter();

        _btnClearSearch = new Button
        {
            Text = "Clear",
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(70, 30),
            Location = new Point(searchPanel.Width - 85, 10),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };
        _btnClearSearch.FlatAppearance.BorderSize = 0;
        _btnClearSearch.Click += (s, e) =>
        {
            _searchBox.Text = "";
            _searchBox.Focus();
        };

        searchPanel.Controls.Add(_searchLabel);
        searchPanel.Controls.Add(_searchBox);
        searchPanel.Controls.Add(_btnClearSearch);

        searchPanel.Resize += (s, e) =>
        {
            if (_searchBox != null)
                _searchBox.Width = searchPanel.Width - 150;
            if (_btnClearSearch != null)
                _btnClearSearch.Location = new Point(searchPanel.Width - 85, 10);
        };

        _bottomButtonPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(35, 35, 42)
        };

        var btnGet = new Button { Text = "🔓 Get", BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(15, 10), Cursor = Cursors.Hand };
        btnGet.FlatAppearance.BorderSize = 0;
        btnGet.Click += (s, e) => GetPassword();

        var btnEdit = new Button { Text = "✎ Edit", BackColor = Color.FromArgb(255, 193, 7), ForeColor = Color.FromArgb(40, 40, 40), FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(125, 10), Cursor = Cursors.Hand };
        btnEdit.FlatAppearance.BorderSize = 0;
        btnEdit.Click += (s, e) => EditPassword();

        var btnDelete = new Button { Text = "🗑 Delete", BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(235, 10), Cursor = Cursors.Hand };
        btnDelete.FlatAppearance.BorderSize = 0;
        btnDelete.Click += (s, e) => DeletePassword();

        var btnHelp = new Button { Text = "❓ Help", BackColor = Color.FromArgb(111, 66, 193), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(345, 10), Cursor = Cursors.Hand };
        btnHelp.FlatAppearance.BorderSize = 0;
        btnHelp.Click += (s, e) => ShowHelp();

        _bottomButtonPanel.Controls.Add(btnGet);
        _bottomButtonPanel.Controls.Add(btnEdit);
        _bottomButtonPanel.Controls.Add(btnDelete);
        _bottomButtonPanel.Controls.Add(btnHelp);

        _listView = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = false,
            MultiSelect = false,
            HideSelection = false,
            BackColor = Color.FromArgb(25, 25, 30),
            ForeColor = Color.FromArgb(220, 220, 230),
            Font = new Font("Segoe UI", 11),
            BorderStyle = BorderStyle.FixedSingle,
            Scrollable = true
        };

        _listView.Columns.Clear();
        _listView.Columns.Add("Description", 400);
        _listView.Columns.Add("Length", 100, HorizontalAlignment.Center);
        _listView.Columns.Add("Public Key (short)", 450);

        _listView.DoubleClick += (s, e) => GetPassword();


        _listView.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter && _listView.SelectedItems.Count > 0)
            {
                GetPassword();
                e.Handled = true;
            }
        };

        var contextMenu = new ContextMenuStrip();
        contextMenu.BackColor = Color.FromArgb(35, 35, 42);
        contextMenu.ForeColor = Color.FromArgb(220, 220, 230);

        contextMenu.Items.Add("🔓 Get Password", null, (s, e) => GetPassword());
        contextMenu.Items.Add("✎ Edit", null, (s, e) => EditPassword());
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add("🗑 Delete", null, (s, e) => DeletePassword());

        contextMenu.RenderMode = ToolStripRenderMode.System;

        _listView.ContextMenuStrip = contextMenu;

        _statusStrip = new StatusStrip
        {
            BackColor = Color.FromArgb(30, 30, 35),
            ForeColor = Color.FromArgb(160, 160, 170),
            Dock = DockStyle.Bottom
        };

        _statusStrip.Items.Add(new ToolStripStatusLabel("Ready"));
        _statusStrip.Items.Add(new ToolStripStatusLabel(" • "));
        _statusStrip.Items.Add(new ToolStripStatusLabel("0 passwords"));
        _statusStrip.Items.Add(new ToolStripStatusLabel(" • "));
        _statusStrip.Items.Add(new ToolStripStatusLabel(""));

        _mainLayout.Controls.Add(_menuStrip, 0, 0);
        _mainLayout.Controls.Add(_headerPanel, 0, 1);
        _mainLayout.Controls.Add(_topButtonPanel, 0, 2);
        _mainLayout.Controls.Add(searchPanel, 0, 3);
        _mainLayout.Controls.Add(_listView, 0, 4);
        _mainLayout.Controls.Add(_bottomButtonPanel, 0, 5);

        this.Controls.Add(_mainLayout);
        this.Controls.Add(_statusStrip);
    }

    private void ApplyFilter()
    {
        var searchText = _searchBox.Text.Trim().ToLower();

        if (string.IsNullOrEmpty(searchText))
        {
            _listView.BeginUpdate();
            _listView.Items.Clear();
            foreach (var pwd in _allPasswords)
            {
                var item = new ListViewItem(pwd.Description);
                item.SubItems.Add(pwd.Length.ToString());
                var shortKey = pwd.PublicKey.Length > 30 ? pwd.PublicKey.Substring(0, 30) + "..." : pwd.PublicKey;
                item.SubItems.Add(shortKey);
                item.Tag = pwd.PublicKey;
                _listView.Items.Add(item);
            }
            _listView.EndUpdate();

            var countLabel = (ToolStripStatusLabel)_statusStrip.Items[2];
            countLabel.Text = $"{_allPasswords.Count} passwords";
        }
        else
        {
            var filtered = _allPasswords.Where(pwd =>
                pwd.Description.ToLower().Contains(searchText) ||
                pwd.PublicKey.ToLower().Contains(searchText)).ToList();

            _listView.BeginUpdate();
            _listView.Items.Clear();
            foreach (var pwd in filtered)
            {
                var item = new ListViewItem(pwd.Description);
                item.SubItems.Add(pwd.Length.ToString());
                var shortKey = pwd.PublicKey.Length > 30 ? pwd.PublicKey.Substring(0, 30) + "..." : pwd.PublicKey;
                item.SubItems.Add(shortKey);
                item.Tag = pwd.PublicKey;
                _listView.Items.Add(item);
            }
            _listView.EndUpdate();

            var countLabel = (ToolStripStatusLabel)_statusStrip.Items[2];
            countLabel.Text = $"{filtered.Count} / {_allPasswords.Count} passwords";
        }
    }

    private void LoadPasswords()
    {
        var storageLabel = (ToolStripStatusLabel)_statusStrip.Items[4];

        _allPasswords.Clear();
        foreach (var pwd in _manager.Passwords.Values)
        {
            _allPasswords.Add(pwd);
        }

        ApplyFilter();
        storageLabel.Text = $"Storage: {_manager.FilePath}";
    }

    private void AddPassword()
    {
        using (var dialog = new AddPasswordForm())
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var publicKey = SmartPasswordGenerator.GeneratePublicKey(dialog.Secret);

                    if (_manager.GetSmartPassword(publicKey) != null)
                    {
                        MessageBox.Show("Password with this secret already exists!", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var smartPassword = new SmartPassword(publicKey, dialog.Description, dialog.Length);
                    _manager.AddSmartPassword(smartPassword);
                    LoadPasswords();

                    MessageBox.Show("Password added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void EditPassword()
    {
        if (_listView.SelectedItems.Count == 0)
        {
            MessageBox.Show("Please select a password to edit", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedItem = _listView.SelectedItems[0];
        var publicKey = selectedItem.Tag?.ToString();

        if (string.IsNullOrEmpty(publicKey))
        {
            MessageBox.Show("Invalid selection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var currentPassword = _manager.GetSmartPassword(publicKey);

        if (currentPassword == null)
        {
            MessageBox.Show("Password not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        using (var dialog = new EditPasswordForm(currentPassword.Description, currentPassword.Length))
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _manager.UpdateSmartPassword(publicKey, dialog.Description, dialog.Length);
                LoadPasswords();
                MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    private void DeletePassword()
    {
        if (_listView.SelectedItems.Count == 0)
        {
            MessageBox.Show("Please select a password to delete", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedItem = _listView.SelectedItems[0];
        var publicKey = selectedItem.Tag?.ToString();

        if (string.IsNullOrEmpty(publicKey))
        {
            MessageBox.Show("Invalid selection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var description = selectedItem.Text;

        var result = MessageBox.Show($"Delete password for '{description}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            _manager.DeleteSmartPassword(publicKey);
            LoadPasswords();
            MessageBox.Show("Password deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void GetPassword()
    {
        if (_listView.SelectedItems.Count == 0)
        {
            MessageBox.Show("Please select a password to retrieve", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedItem = _listView.SelectedItems[0];
        var publicKey = selectedItem.Tag?.ToString();

        if (string.IsNullOrEmpty(publicKey))
        {
            MessageBox.Show("Invalid selection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var currentPassword = _manager.GetSmartPassword(publicKey);

        if (currentPassword == null)
        {
            MessageBox.Show("Password not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        using (var dialog = new PasswordInputDialog(currentPassword.Description, currentPassword.Length))
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!SmartPasswordGenerator.VerifySecret(dialog.Secret, publicKey))
                    {
                        MessageBox.Show("Invalid secret phrase!", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var password = SmartPasswordGenerator.GenerateSmartPassword(dialog.Secret, currentPassword.Length);
                    Clipboard.SetText(password);
                    MessageBox.Show("Password copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void ExportPasswords()
    {
        if (_manager.PasswordCount == 0)
        {
            MessageBox.Show("No passwords to export", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var filename = $"spm_export_{timestamp}.json";
        var filepath = Path.Combine(_exportDir, filename);

        try
        {
            var exportData = new Dictionary<string, object>();
            foreach (var kv in _manager.Passwords)
            {
                exportData[kv.Key] = new Dictionary<string, object>
                {
                    ["description"] = kv.Value.Description,
                    ["length"] = kv.Value.Length,
                    ["public_key"] = kv.Value.PublicKey
                };
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(exportData, options);
            File.WriteAllText(filepath, json);

            MessageBox.Show($"Exported {_manager.PasswordCount} passwords to:\n{filepath}", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ImportPasswords()
    {
        using (var dialog = new OpenFileDialog())
        {
            dialog.Title = "Import Passwords";
            dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var json = File.ReadAllText(dialog.FileName);
                    var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                    if (data == null)
                    {
                        MessageBox.Show("Invalid file format", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int imported = 0;
                    int skipped = 0;

                    foreach (var kv in data)
                    {
                        var publicKey = kv.Value.GetProperty("public_key").GetString();
                        var description = kv.Value.GetProperty("description").GetString();
                        var length = kv.Value.GetProperty("length").GetInt32();

                        if (publicKey == null || description == null)
                        {
                            skipped++;
                            continue;
                        }

                        if (_manager.GetSmartPassword(publicKey) == null)
                        {
                            var smartPassword = new SmartPassword(publicKey, description, length);
                            _manager.AddSmartPassword(smartPassword);
                            imported++;
                        }
                        else
                        {
                            skipped++;
                        }
                    }

                    LoadPasswords();
                    MessageBox.Show($"Import completed!\nImported: {imported}\nSkipped (duplicates): {skipped}", "Import Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Import failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void ShowHelp()
    {
        using (var helpForm = new HelpForm())
        {
            helpForm.ShowDialog();
        }
    }

    private void ShowShortcuts()
    {
        using (var shortcutsForm = new ShortcutsForm())
        {
            shortcutsForm.ShowDialog();
        }
    }

    private void OpenDisclaimer()
    {
        try
        {
            Process.Start(new ProcessStartInfo("https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/DISCLAIMER.md")
            { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Cannot open disclaimer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowAbout()
    {
        using (var aboutForm = new AboutForm())
        {
            aboutForm.ShowDialog();
        }
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.N)
        {
            AddPassword();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.E)
        {
            EditPassword();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.G)
        {
            GetPassword();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.I)
        {
            ImportPasswords();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.X)
        {
            ExportPasswords();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.F)
        {
            _searchBox.Focus();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.OemQuestion)
        {
            ShowShortcuts();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Delete)
        {
            DeletePassword();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.F5)
        {
            LoadPasswords();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.F1)
        {
            ShowHelp();
            e.Handled = true;
        }
    }
}