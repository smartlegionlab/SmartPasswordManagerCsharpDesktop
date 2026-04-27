using SmartLegionLab.SmartPassLib;
using System.Diagnostics;
using System.Text.Json;
using SmartPasswordManagerCsharpDesktop.Helpers;

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
        this.Text = "Smart Password Manager v1.1.0";
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

        var exportItem = new ToolStripMenuItem("Export", null, (s, e) => ExportPasswords());
        exportItem.ShortcutKeys = Keys.Control | Keys.X;
        exportItem.ShowShortcutKeys = true;
        fileMenu.DropDownItems.Add(exportItem);

        var importItem = new ToolStripMenuItem("Import", null, (s, e) => ImportPasswords());
        importItem.ShortcutKeys = Keys.Control | Keys.I;
        importItem.ShowShortcutKeys = true;
        fileMenu.DropDownItems.Add(importItem);

        fileMenu.DropDownItems.Add(new ToolStripSeparator());

        var exitItem = new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit());
        exitItem.ShortcutKeys = Keys.Control | Keys.Q;
        exitItem.ShowShortcutKeys = true;
        fileMenu.DropDownItems.Add(exitItem);

        var passwordsMenu = new ToolStripMenuItem("Passwords");

        var addItem = new ToolStripMenuItem("Add", null, (s, e) => AddPassword());
        addItem.ShortcutKeys = Keys.Control | Keys.N;
        addItem.ShowShortcutKeys = true;
        passwordsMenu.DropDownItems.Add(addItem);

        var editItem = new ToolStripMenuItem("Edit", null, (s, e) => EditPassword());
        editItem.ShortcutKeys = Keys.Control | Keys.E;
        editItem.ShowShortcutKeys = true;
        passwordsMenu.DropDownItems.Add(editItem);

        var getItem = new ToolStripMenuItem("Get Password", null, (s, e) => GetPassword());
        getItem.ShortcutKeys = Keys.Control | Keys.G;
        getItem.ShowShortcutKeys = true;
        passwordsMenu.DropDownItems.Add(getItem);

        passwordsMenu.DropDownItems.Add(new ToolStripSeparator());

        var deleteItem = new ToolStripMenuItem("Delete", null, (s, e) => DeletePassword());
        deleteItem.ShortcutKeys = Keys.Delete;
        deleteItem.ShowShortcutKeys = true;
        passwordsMenu.DropDownItems.Add(deleteItem);

        passwordsMenu.DropDownItems.Add(new ToolStripSeparator());

        var refreshItem = new ToolStripMenuItem("Refresh", null, (s, e) => LoadPasswords());
        refreshItem.ShortcutKeys = Keys.F5;
        refreshItem.ShowShortcutKeys = true;
        passwordsMenu.DropDownItems.Add(refreshItem);

        var helpMenu = new ToolStripMenuItem("Help");

        var helpItem = new ToolStripMenuItem("📖 Help", null, (s, e) => ShowHelp());
        helpItem.ShortcutKeys = Keys.F1;
        helpItem.ShowShortcutKeys = true;
        helpMenu.DropDownItems.Add(helpItem);

        var shortcutsItem = new ToolStripMenuItem("⌨️ Shortcuts", null, (s, e) => ShowShortcuts());
        shortcutsItem.ShortcutKeys = Keys.Control | Keys.OemQuestion;
        shortcutsItem.ShortcutKeyDisplayString = "Ctrl+/";
        shortcutsItem.ShowShortcutKeys = true;
        helpMenu.DropDownItems.Add(shortcutsItem);

        helpMenu.DropDownItems.Add(new ToolStripSeparator());

        var disclaimerItem = new ToolStripMenuItem("⚠️ Disclaimer", null, (s, e) => ShowDisclaimer());
        disclaimerItem.ShortcutKeys = Keys.Control | Keys.D;
        disclaimerItem.ShowShortcutKeys = true;
        helpMenu.DropDownItems.Add(disclaimerItem);

        var licenseItem = new ToolStripMenuItem("📄 License", null, (s, e) => ShowLicense());
        licenseItem.ShortcutKeys = Keys.Control | Keys.L;
        licenseItem.ShowShortcutKeys = true;
        helpMenu.DropDownItems.Add(licenseItem);

        helpMenu.DropDownItems.Add(new ToolStripSeparator());

        var aboutItem = new ToolStripMenuItem("ℹ️ About", null, (s, e) => ShowAbout());
        aboutItem.ShortcutKeys = Keys.Control | Keys.A;
        aboutItem.ShowShortcutKeys = true;
        helpMenu.DropDownItems.Add(aboutItem);

        var toolsMenu = new ToolStripMenuItem("Tools");

        var createShortcutItem = new ToolStripMenuItem("Create Desktop Shortcut", null, (s, e) => CreateDesktopShortcut());
        createShortcutItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
        createShortcutItem.ShowShortcutKeys = true;
        toolsMenu.DropDownItems.Add(createShortcutItem);

        _menuStrip.Items.Add(fileMenu);
        _menuStrip.Items.Add(passwordsMenu);
        _menuStrip.Items.Add(toolsMenu);
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

        _topButtonPanel.Controls.Add(btnAdd);
        _topButtonPanel.Controls.Add(btnImport);

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

        var btnExport = new Button { Text = "📤 Export", BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(345, 10), Cursor = Cursors.Hand };
        btnExport.FlatAppearance.BorderSize = 0;
        btnExport.Click += (s, e) => ExportPasswords();

        _bottomButtonPanel.Controls.Add(btnGet);
        _bottomButtonPanel.Controls.Add(btnEdit);
        _bottomButtonPanel.Controls.Add(btnDelete);
        _bottomButtonPanel.Controls.Add(btnExport);

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

        try
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("SmartPasswordManagerCsharpDesktop.app.ico"))
            {
                if (stream != null)
                {
                    this.Icon = new Icon(stream);
                }
            }
        }
        catch
        {
            this.Icon = SystemIcons.Shield;
        }
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

        using (var dialog = new GetPasswordForm(currentPassword.Description, currentPassword.Length))
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
        using (var form = new GenericInfoForm("Help - Smart Password Manager", GetHelpText()))
        {
            form.ShowDialog();
        }
    }

    private void ShowDisclaimer()
    {
        using (var form = new GenericInfoForm("Disclaimer", GetDisclaimerText()))
        {
            form.ShowDialog();
        }
    }

    private void ShowLicense()
    {
        using (var form = new GenericInfoForm("License - BSD 3-Clause", GetLicenseText()))
        {
            form.ShowDialog();
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
        ShowDisclaimer();
    }

    private void ShowAbout()
    {
        using (var aboutForm = new AboutForm())
        {
            aboutForm.ShowDialog();
        }
    }

    private string GetHelpText()
    {
        return
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
"  Ctrl + Shift + S →  Create desktop shortcut\n" +
"  Ctrl + D     →  Show disclaimer\n" +
"  Ctrl + L     →  Show license\n" +
"  Ctrl + A     →  Show about\n" +
"  Alt + F4     →  Exit\n" +
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
"  Version v1.1.0 | Copyright © 2026 Alexander Suvorov\n" +
"  Licensed under BSD 3-Clause License\n" +
"══════════════════════════════════════════════════════════════════════════════";
    }

    private string GetDisclaimerText()
    {
        return
"══════════════════════════════════════════════════════════════════════════════\n" +
"  LEGAL DISCLAIMER\n" +
"══════════════════════════════════════════════════════════════════════════════\n\n" +

"COMPLETE AND ABSOLUTE RELEASE FROM ALL LIABILITY\n\n" +

"SOFTWARE PROVIDED \"AS IS\" WITHOUT ANY WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,\n" +
"INCLUDING BUT NOT LIMITED TO WARRANTIES OF MERCHANTABILITY, FITNESS FOR A\n" +
"PARTICULAR PURPOSE, AND NONINFRINGEMENT.\n\n" +

"The copyright holder, contributors, and any associated parties EXPLICITLY\n" +
"DISCLAIM AND DENY ALL RESPONSIBILITY AND LIABILITY for:\n\n" +

"1. ANY AND ALL DATA LOSS: Complete or partial loss of any data, files,\n" +
"   configuration, or information whatsoever\n\n" +

"2. ANY AND ALL SECURITY INCIDENTS: Unauthorized access, breaches, compromises,\n" +
"   theft, or exposure of any sensitive information\n\n" +

"3. ANY AND ALL FINANCIAL LOSSES: Direct, indirect, incidental, special,\n" +
"   consequential, or punitive damages of any kind\n\n" +

"4. ANY AND ALL OPERATIONAL DISRUPTIONS: Service interruptions, system failures,\n" +
"   authentication issues, or denial of service\n\n" +

"5. ANY AND ALL IMPLEMENTATION ISSUES: Bugs, errors, vulnerabilities,\n" +
"   misconfigurations, incorrect usage, or compatibility problems\n\n" +

"6. ANY AND ALL LEGAL OR REGULATORY CONSEQUENCES: Violations of laws,\n" +
"   regulations, compliance requirements, or third-party terms of service\n\n" +

"7. ANY AND ALL PERSONAL OR BUSINESS DAMAGES: Reputational harm, business\n" +
"   interruption, loss of revenue, lost profits, or any other damages\n\n" +

"8. ANY AND ALL THIRD-PARTY CLAIMS: Claims made by any other parties affected\n" +
"   by software usage\n\n" +

"9. ANY AND ALL SYSTEM DAMAGES: Hardware damage, software corruption,\n" +
"   operating system instability, or data corruption\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  USER ACCEPTS FULL AND UNCONDITIONAL RESPONSIBILITY\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +

"By installing, accessing, cloning, forking, or using this software in any\n" +
"manner, you irrevocably agree that:\n\n" +

"- You assume ALL risks associated with software usage\n" +
"- You bear SOLE responsibility for your data, credentials, and system security\n" +
"- You accept COMPLETE responsibility for all testing and validation before\n" +
"  production use\n" +
"- You are EXCLUSIVELY liable for compliance with all applicable laws and\n" +
"  regulations\n" +
"- You accept TOTAL responsibility for any and all consequences of usage\n" +
"- You PERMANENTLY AND IRREVOCABLY waive, release, and discharge all claims\n" +
"  against the copyright holder, contributors, distributors, and any associated\n" +
"  entities\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  NO WARRANTY OF ANY KIND\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +

"This software comes with ABSOLUTELY NO GUARANTEES regarding:\n\n" +

"- Security effectiveness or cryptographic strength\n" +
"- Reliability, availability, or uptime\n" +
"- Fitness for any particular purpose or use case\n" +
"- Accuracy, correctness, or completeness\n" +
"- Freedom from defects, vulnerabilities, or backdoors\n" +
"- Compatibility with any specific hardware, software, or environment\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  NOT A PROFESSIONAL OR CERTIFIED SOLUTION\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +

"This software is provided for educational and experimental purposes.\n" +
"It is not:\n\n" +

"- Professional advice or consultation of any kind\n" +
"- A certified, audited, or validated product\n" +
"- A guaranteed security solution\n" +
"- Enterprise-grade or production-ready software\n" +
"- Endorsed by any authority, organization, or standards body\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  FINAL AND BINDING AGREEMENT\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +

"Usage of this software constitutes your FULL AND UNCONDITIONAL ACCEPTANCE\n" +
"of this disclaimer. If you do not accept ALL terms and conditions,\n" +
"DO NOT USE, CLONE, FORK, OR DOWNLOAD THIS SOFTWARE.\n\n" +

"BY PROCEEDING, YOU ACKNOWLEDGE THAT YOU HAVE READ THIS DISCLAIMER IN ITS\n" +
"ENTIRETY, UNDERSTAND ITS TERMS COMPLETELY, AND ACCEPT THEM WITHOUT\n" +
"RESERVATION OR EXCEPTION.\n\n" +

"══════════════════════════════════════════════════════════════════════════════\n" +
"  Version v1.1.0 | Copyright © 2026 Alexander Suvorov\n" +
"══════════════════════════════════════════════════════════════════════════════";
    }

    private string GetLicenseText()
    {
        return
"══════════════════════════════════════════════════════════════════════════════\n" +
"  BSD 3-CLAUSE LICENSE\n" +
"══════════════════════════════════════════════════════════════════════════════\n\n" +

"Copyright (c) 2026, Alexander Suvorov\n" +
"All rights reserved.\n\n" +

"Redistribution and use in source and binary forms, with or without\n" +
"modification, are permitted provided that the following conditions are met:\n\n" +

"1. Redistributions of source code must retain the above copyright notice,\n" +
"   this list of conditions and the following disclaimer.\n\n" +

"2. Redistributions in binary form must reproduce the above copyright notice,\n" +
"   this list of conditions and the following disclaimer in the documentation\n" +
"   and/or other materials provided with the distribution.\n\n" +

"3. Neither the name of the copyright holder nor the names of its\n" +
"   contributors may be used to endorse or promote products derived from\n" +
"   this software without specific prior written permission.\n\n" +

"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
"  DISCLAIMER OF WARRANTY\n" +
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +

"THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS \"AS IS\"\n" +
"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE\n" +
"IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE\n" +
"ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE\n" +
"LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR\n" +
"CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF\n" +
"SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS\n" +
"INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN\n" +
"CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)\n" +
"ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE\n" +
"POSSIBILITY OF SUCH DAMAGE.\n\n" +

"══════════════════════════════════════════════════════════════════════════════\n" +
"  Version v1.1.0 | Copyright © 2026 Alexander Suvorov\n" +
"══════════════════════════════════════════════════════════════════════════════";
    }

    private void CreateDesktopShortcut()
    {
        ShellShortcut.CreateDesktopShortcut();
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.F)
        {
            _searchBox.Focus();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.OemQuestion)
        {
            ShowShortcuts();
            e.Handled = true;
        }
        else if (e.Control && e.Shift && e.KeyCode == Keys.S)
        {
            CreateDesktopShortcut();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.D)
        {
            ShowDisclaimer();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.L)
        {
            ShowLicense();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.A)
        {
            ShowAbout();
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.Q)
        {
            Application.Exit();
            e.Handled = true;
        }
    }
}