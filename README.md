# Smart Password Manager Desktop (C#) <sup>v4.0.0</sup>

---

**Desktop manager for deterministic smart passwords. Generate, manage, and retrieve passwords without storing them. Your secret phrase never leaves your device.**

**Decentralized by Design**: Unlike traditional password managers that store encrypted vaults on central servers, 
Smart Password Manager stores nothing. Your secrets never leave your device. Passwords are regenerated on-demand — 
**no cloud, no database, no trust required**.

---

[![GitHub top language](https://img.shields.io/github/languages/top/smartlegionlab/SmartPasswordManagerCsharpDesktop)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop)
[![GitHub license](https://img.shields.io/github/license/smartlegionlab/SmartPasswordManagerCsharpDesktop)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/LICENSE)
[![GitHub release](https://img.shields.io/github/v/release/smartlegionlab/SmartPasswordManagerCsharpDesktop)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/)
[![GitHub stars](https://img.shields.io/github/stars/smartlegionlab/SmartPasswordManagerCsharpDesktop?style=social)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/smartlegionlab/SmartPasswordManagerCsharpDesktop?style=social)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/network/members)
![Platform](https://img.shields.io/badge/platform-windows-lightgrey)

---

## ⚠️ Disclaimer

**By using this software, you agree to the full disclaimer terms.**

**Summary:** Software provided "AS IS" without warranty. You assume all risks.

**Full legal disclaimer:** See [DISCLAIMER.md](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/DISCLAIMER.md)

---

## 🔄 Breaking Change (v4.0.0)

> **⚠️ This version uses [smartpasslib-csharp](https://github.com/smartlegionlab/smartpasslib-csharp) v4.0.0, which is NOT backward compatible with v1.x.x**

Smart passwords created with older versions **cannot be regenerated** with v4.0.0.

**What changed:**
- Dynamic iterations: private key 15-30 steps (was fixed 30), public key 45-60 steps (was fixed 60)
- Expanded Google-compatible character set
- Secret phrases now require minimum 12 characters (was 4)
- Password length now limited to 100 characters (was 1000)

📖 **Full migration instructions** → see [MIGRATION.md](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop/blob/master/MIGRATION.md)

---

## Core Principles

- **Zero-Storage Security**: No passwords or secret phrases are ever stored or transmitted
- **Decentralized Architecture**: No central servers, no cloud dependency, no third-party trust required
- **Deterministic Regeneration**: Passwords are recreated identically from your secret phrase
- **Metadata Only**: Store only descriptions and verification keys
- **Client-Side Generation**: All cryptographic operations happen on your device
- **On-Demand Discovery**: Passwords exist only when you generate them

## Key Features

- **Decentralized & Serverless**: No central database, no cloud lock-in, complete user sovereignty
- **Smart Password Generation**: Deterministic from secret phrase
- **Dynamic Key Derivation**: 15-30 iterations for private key, 45-60 for public key (deterministic per secret)
- **Secret Verification**: Verify secret without exposing it
- **Dark Theme Interface**: Easy on the eyes during extended use
- **Full CRUD Operations**: Create, Read, Update, Delete
- **Search & Filter**: Quickly find your passwords
- **Export/Import**: Backup and restore your password metadata
- **Copy to Clipboard**: One-click password copying
- **Keyboard Shortcuts**: Full keyboard navigation
- **Context Menus**: Right-click for quick actions
- **QR Code Export**: Transfer password metadata to Android app via QR code

## Security Model

- **Proof of Knowledge**: Public keys verify secrets without exposing them
- **Decentralized Trust**: No third party needed — you control your secrets completely
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

**Key derivation (same as Python/JS/Kotlin/Go versions v4.0.0):**

| Key Type    | Iterations              | Purpose                                               |
|-------------|-------------------------|-------------------------------------------------------|
| Private Key | 15-30 (dynamic)         | Password generation (never stored, never transmitted) |
| Public Key  | 45-60 (dynamic)         | Verification (stored locally)                         |

**Character Set (Google-compatible):**
```
!@#$%^&*()_+-=[]{};:,.<>?/ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz
```

**Validation Rules:**
- Secret phrase: minimum 12 characters
- Password length: 12-100 characters

**Decentralized Architecture**:
- No central authority required
- Metadata can be synced via any channel
- Your security depends only on your secret phrase
- Works offline — no internet connection required

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

### QR Code Export to Mobile

The application allows you to export password metadata to the **Smart Password Manager Android** app via QR code:

**How to use:**
1. Select any password in the list
2. Right-click and choose **QR Code**, press **Ctrl+R**, or go to **Passwords → QR Code**
3. A dialog with QR code will appear containing the password metadata
4. Open the Android app and scan the QR code
5. The password entry will be automatically added to your mobile device

**What's included in QR:**
- Password length
- Public verification key

**What's NOT included:**
- Password description (shown in dialog for reference, but not embedded in QR)
- Your secret phrase (never leaves your device)
- The actual password

**Security Note:** QR codes contain only metadata that is already stored locally. Your secret phrase and actual passwords are never embedded in QR codes.

### Keyboard Shortcuts

| Shortcut       | Action                              |
|----------------|-------------------------------------|
| `Ctrl+N`       | Add new password                    |
| `Ctrl+E`       | Edit selected                       |
| `Ctrl+R`       | Show QR code for selected password  |
| `Ctrl+G`       | Get password                        |
| `Ctrl+I`       | Import passwords                    |
| `Ctrl+X`       | Export passwords                    |
| `Ctrl+F`       | Focus search box                    |
| `Ctrl+/`       | Show shortcuts                      |
| `Ctrl+Shift+S` | Create desktop shortcut             |
| `Ctrl+D`       | Show disclaimer                     |
| `Ctrl+L`       | Show license                        |
| `Ctrl+A`       | Show about                          |
| `Ctrl+Q`       | Exit application                    |
| `Delete`       | Delete selected                     |
| `F5`           | Refresh list                        |
| `F1`           | Show help                           |
| `Enter`        | Get password (when item selected)   |

## Storage Locations

| Platform | Path                                                          |
|----------|---------------------------------------------------------------|
| Windows  | `%USERPROFILE%\.config\smart_password_manager\passwords.json` |

**Export files** are saved to `%USERPROFILE%\SmartPasswordManager\` with timestamp: `passwords_export_[date].json`

## Security Requirements

### Secret Phrase
- **Minimum 12 characters** (enforced)
- Case-sensitive
- Use mix of: uppercase, lowercase, numbers, symbols
- Never store digitally
- **NEVER use your password description as secret phrase**

### Password Length Requirements
- **Smart passwords**: 12-100 characters

### Strong Secret Examples
```
✅ "MyStrongSecretPhrase2026!"   — mixed case + numbers + symbols
✅ "P@ssw0rd!LongSecret"         — special chars + numbers + length
✅ "КотБегемот2026НаДиете"       — Cyrillic + numbers
```

### Weak Secret Examples (avoid)
```
❌ "short"                       — too short, rejected
❌ "GitHub Account"              — using description as secret (weak!)
❌ "password"                    — dictionary word, too short
❌ "1234567890"                  — only digits, too short
❌ "qwerty123"                   — keyboard pattern
❌ Same as description           — never use the same value as description
```

### Decentralized Nature

**There is no "forgot password" button.** This is by design:

- No central server can reset your passwords
- No support team can recover your access
- Your secret phrase is the ONLY key

**This is the price of true decentralization** — you are completely in control.

## Cross-Platform Data Transfer

Smart Password Manager Desktop (C#) supports seamless metadata transfer across all platforms:

**Methods:**
- **QR codes** - Transfer password metadata from Desktop to Android app instantly
- **Export/Import** - JSON export/import works across all platforms (Desktop Python, Web, CLI, Android)

**Cross-platform compatibility:**
- Same JSON format for export/import on all platforms
- QR code format is identical across Desktop (C# and Python) and Web versions
- Metadata sync without cloud dependency

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

**Data transfer:** Use QR codes or Export/Import to sync metadata across all platforms.

## Ecosystem

**Core Libraries:**
- **[smartpasslib](https://github.com/smartlegionlab/smartpasslib)** - Python
- **[smartpasslib-js](https://github.com/smartlegionlab/smartpasslib-js)** - JavaScript
- **[smartpasslib-kotlin](https://github.com/smartlegionlab/smartpasslib-kotlin)** - Kotlin
- **[smartpasslib-go](https://github.com/smartlegionlab/smartpasslib-go)** - Go
- **[smartpasslib-csharp](https://github.com/smartlegionlab/smartpasslib-csharp)** - C#

**CLI Applications:**
- **[CLI Smart Password Manager (Python)](https://github.com/smartlegionlab/clipassman)**
- **[CLI Smart Password Generator (Python)](https://github.com/smartlegionlab/clipassgen)**
- **[CLI Smart Password Manager (C#)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpCli)**
- **[CLI Smart Password Generator (C#)](https://github.com/smartlegionlab/SmartPasswordGeneratorCsharpCli)**

**Desktop Applications:**
- **[Desktop Smart Password Manager (Python)](https://github.com/smartlegionlab/smart-password-manager-desktop)**
- **[Desktop Smart Password Manager (C#)](https://github.com/smartlegionlab/SmartPasswordManagerCsharpDesktop)** (this)

**Other:**
- **[Web Smart Password Manager](https://github.com/smartlegionlab/smart-password-manager-web)**
- **[Android Smart Password Manager](https://github.com/smartlegionlab/smart-password-manager-android)**

## Version History

| Version          | smartpasslib-csharp | Status                   | Migration Required     |
|------------------|---------------------|--------------------------|------------------------|
| v1.x.x and below | v1.x.x              | ❌ Deprecated/Unsupported | Must migrate to v4.x.x |
| **v4.0.0+**      | **v4.0.0+**         | ✅ Current                | N/A                    |

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

