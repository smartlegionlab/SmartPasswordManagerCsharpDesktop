# Smart Password Manager Desktop (C#) <sup>v1.1.1</sup>

**Desktop manager for deterministic smart passwords. Generate, manage, and retrieve passwords without storing them. Your secret phrase never leaves your device.**

---

[![GitHub top language](https://img.shields.io/github/languages/top/smartlegionlab/SmartPasswordManagerCsharpDesktop)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop)
[![GitHub license](https://img.shields.io/github/license/smartlegionlab/SmartPasswordManagerCsharpDesktop)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/LICENSE)
[![GitHub release](https://img.shields.io/github/v/release/smartlegionlab/SmartPasswordManagerCsharpDesktop)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/)
[![GitHub stars](https://img.shields.io/github/stars/smartlegionlab/SmartPasswordManagerCsharpDesktop?style=social)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/smartlegionlab/SmartPasswordManagerCsharpDesktop?style=social)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/network/members)

---

## ⚠️ Disclaimer

**By using this software, you agree to the full disclaimer terms.**

**Summary:** Software provided "AS IS" without warranty. You assume all risks.

**Full legal disclaimer:** See [DISCLAIMER.md](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/DISCLAIMER.md)

---

## Core Principles

- **Zero-Password Storage**: No passwords are ever stored or transmitted
- **Deterministic Regeneration**: Passwords are recreated identically from your secret phrase
- **Metadata Management**: Store only descriptions and verification keys
- **Client-Side Generation**: All cryptographic operations happen on your device
- **On-Demand Discovery**: Passwords exist only when you generate them

## Key Features

- **Smart Password Generation**: Deterministic from secret phrase
- **Public/Private Key System**: 30 iterations for private key, 60 for public key
- **Secret Verification**: Verify secret without exposing it
- **Dark Theme Interface**: Easy on the eyes during extended use
- **Full CRUD Operations**: Create, Read, Update, Delete
- **Search & Filter**: Quickly find your passwords
- **Export/Import**: Backup and restore your password metadata
- **Copy to Clipboard**: One-click password copying
- **Keyboard Shortcuts**: Full keyboard navigation
- **Context Menus**: Right-click for quick actions

## Security Model

- **Proof of Knowledge**: Public keys verify secrets without exposing them
- **Deterministic Security**: Same secret + length = same password, always
- **Metadata Separation**: Non-sensitive data stored in JSON file
- **Local Processing**: Secret and password never leave your device
- **No Recovery Backdoors**: Lost secret = permanently lost access (by design)

---

## Research Paradigms & Publications

- **[Pointer-Based Security Paradigm](https://doi.org/10.5281/zenodo.17204738)** - Architectural Shift from Data Protection to Data Non-Existence
- **[Local Data Regeneration Paradigm](https://doi.org/10.5281/zenodo.17264327)** - Ontological Shift from Data Transmission to Synchronous State Discovery

---

## Technical Foundation

Powered by **[smartpasslib-csharp](https://github.com/smartlegionlab/smartpasslib-csharp)** — C# implementation of deterministic password generation.

**Key derivation (same as Python/JS/Kotlin/Go versions):**

| Key Type    | Iterations | Purpose                                               |
|-------------|------------|-------------------------------------------------------|
| Private Key | 30         | Password generation (never stored, never transmitted) |
| Public Key  | 60         | Verification (stored on server)                       |

**Character Set:** `abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$&*-_`

## Download

### Pre-built Binary

| Platform        | Download                                                                                                                     |
|-----------------|------------------------------------------------------------------------------------------------------------------------------|
| **Windows x64** | [SmartPasswordManagerCsharpDesktop.exe](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/releases/latest) |

> Just download, run, and start using. No .NET installation required.

## Quick Start

### Run the Application

```cmd
SmartPasswordManagerCsharpDesktop.exe
```

### Main Interface

![Main Interface](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/raw/master/data/images/smart-password-manager-win-desktop.png)

### Keyboard Shortcuts

| Shortcut       | Action                            |
|----------------|-----------------------------------|
| `Ctrl+N`       | Add new password                  |
| `Ctrl+E`       | Edit selected                     |
| `Ctrl+G`       | Get password                      |
| `Ctrl+I`       | Import passwords                  |
| `Ctrl+X`       | Export passwords                  |
| `Ctrl+F`       | Focus search box                  |
| `Ctrl+/`       | Show shortcuts                    |
| `Ctrl+Shift+S` | Create desktop shortcut           |
| `Ctrl+D`       | Show disclaimer                   |
| `Ctrl+L`       | Show license                      |
| `Ctrl+A`       | Show about                        |
| `Ctrl+Q`       | Exit application                  |
| `Delete`       | Delete selected                   |
| `F5`           | Refresh list                      |
| `F1`           | Show help                         |
| `Enter`        | Get password (when item selected) |

## Storage Locations

| Platform | Path                                                          |
|----------|---------------------------------------------------------------|
| Windows  | `%USERPROFILE%\.config\smart_password_manager\passwords.json` |

**Export files** are saved to `%USERPROFILE%\SmartPasswordManager\` with timestamp: `passwords_export_[date].json`

## Security Requirements

### Secret Phrase
- **Minimum 12 characters** (enforced)
- Case-sensitive
- Use mix of: uppercase, lowercase, numbers, symbols, emoji, or Cyrillic
- Never store digitally
- **NEVER use your password description as secret phrase**

### Strong Secret Examples
```
✅ "MyCatHippo2026"          — mixed case + numbers
✅ "P@ssw0rd!LongSecret"     — special chars + numbers + length
✅ "КотБегемот2026НаДиете"   — Cyrillic + numbers
✅ "GitHubPersonal2026!"     — description + extra chars (not description alone)
```

### Weak Secret Examples (avoid)
```
❌ "GitHub Account"          — using description as secret (weak!)
❌ "password"                — dictionary word, too short
❌ "1234567890"              — only digits, too short
❌ "qwerty123"               — keyboard pattern
❌ Same as description       — never use the same value as password description
```

## Cross-Platform Compatibility

Smart Password Manager Desktop (C#) produces **identical passwords** to:

| Platform         | Application                                                                             |
|------------------|-----------------------------------------------------------------------------------------|
| Python CLI       | [CLI PassMan](https://github.com/smartlegionlab/clipassman)                             |
| Python CLI Gen   | [CLI PassGen](https://github.com/smartlegionlab/clipassgen)                             |
| Desktop Python   | [Desktop Manager](https://github.com/smartlegionlab/smart-password-manager-desktop)     |
| CLI C#           | [CLI Manager (C#)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpCli)     |
| CLI Generator C# | [CLI Generator (C#)](https://github.com/smartlegionlab/SmartPasswordGeneratorCsharpCli) |
| Web              | [Web Manager](https://github.com/smartlegionlab/smart-password-manager-web)             |
| Android          | [Android Manager](https://github.com/smartlegionlab/smart-password-manager-android)     |

## Ecosystem

**Core Libraries:**
- **[smartpasslib](https://github.com/smartlegionlab/smartpasslib)** - Python
- **[smartpasslib-js](https://github.com/smartlegionlab/smartpasslib-js)** - JavaScript
- **[smartpasslib-kotlin](https://github.com/smartlegionlab/smartpasslib-kotlin)** - Kotlin
- **[smartpasslib-go](https://github.com/smartlegionlab/smartpasslib-go)** - Go
- **[smartpasslib-csharp](https://github.com/smartlegionlab/smartpasslib-csharp)** - C#

**CLI Applications:**
- **[CLI PassMan (Python)](https://github.com/smartlegionlab/clipassman)**
- **[CLI PassGen (Python)](https://github.com/smartlegionlab/clipassgen)**
- **[CLI Manager (C#)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpCli)**
- **[CLI Generator (C#)](https://github.com/smartlegionlab/SmartPasswordGeneratorCsharpCli)**

**Desktop Applications:**
- **[Desktop Manager (Python)](https://github.com/smartlegionlab/smart-password-manager-desktop)**
- **[Desktop Manager (C#)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop)** (this)

**Other:**
- **[Web Manager](https://github.com/smartlegionlab/smart-password-manager-web)**
- **[Android Manager](https://github.com/smartlegionlab/smart-password-manager-android)**

## License

**[BSD 3-Clause License](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/LICENSE)**

Copyright (©) 2026, [Alexander Suvorov](https://github.com/smartlegionlab)

## Author

**Alexander Suvorov** - [GitHub](https://github.com/smartlegionlab)

---

## Support

- **Issues**: [GitHub Issues](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/issues)
- **Documentation**: This [README](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/README.md)

---

